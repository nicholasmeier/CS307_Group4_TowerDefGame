using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Wave{
    public List<GameObject> monsters;
}

[System.Serializable]
public class Position
{
    public int i;
    public int j;
    public Position(int i, int j)
    {
        this.i = i;
        this.j = j;
    }
}

public class MapController_script : MonoBehaviour {

    //for prototyping purposes
    public List<GameObject> monsterHolder;  //The list that holds all the live monsters on the map
    public List<Wave> waves;                //The list containing the wave design
    public List<GameObject> gridMap;        //The list holding all the grids on the map
    public int mapWidth;                
    public int mapHeight;
    public Text wave_display;
    public GameObject player;
    public GameObject gridPrefab;           //The grid used to build the battleground
    private bool[,] gridArray;              //Int version of gridMap, used to increase performance,
                                            //False in entry meaning the grid is not available(occupied).

    public Position entry;
    public Position exit;
    public List<Position> route;
    //for bottom display
    public Text bot_hp;
    public Text bot_type;
    public Button sell;
    public Button upgrade;
    // private int monster_counter;
    private float player_current_resource;
    private int waveNumber;



    // Use this for initialization
    void Start () {
        route = new List<Position>();
        StartCoroutine(SpawnMap());
        StartCoroutine(SpawnWave(waves[0]));
	}
	
	// Update is called once per frame
	void Update () {
	    //for testing 
        /*if(Input.GetMouseButtonDown(0)){
            Debug.Log(Input.mousePosition.ToString());
        }*/
	}

    IEnumerator SpawnMap(){
        //Initialize gridArray
        gridArray = new bool[mapWidth, mapHeight];
        for (int i = 0; i < mapWidth; i++){
            for (int j = 0; j < mapHeight; j++){
                gridArray[i, j] = true;
            }
        }

        //Initialize gridMap
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                //converting array pos to map pos
                GameObject gridInstance;
                //instantiate the grid instance
                gridInstance = Instantiate(gridPrefab, GetMapPosition(i, j, -0.5f), Quaternion.identity);
                gridInstance.GetComponent<Grid_script>().mapController = this;

                gridMap.Add(gridInstance);
            }
        }
        yield return 0;
    }

    IEnumerator SpawnWave(Wave wave){

        //monster_counter = 0;
        //A sample wave
        //waveNumber++;
        //wave_display.text = "Wave: " + waveNumber.ToString();
        FindPath();
        foreach (Wave w in waves)
        {
            yield return new WaitForSeconds(20);
            waveNumber++;
            wave_display.text = "Wave: " + waveNumber.ToString();
            player.GetComponent<PlayerController_script>().addCurrentResource(100);
            ShowPath();
            foreach (GameObject monster in wave.monsters)
            {
                GameObject monsterInstance;
                //monster_counter++;
                monsterInstance = Instantiate(monster);
                monsterInstance.GetComponent<Monster_script>().player = player;
                monsterInstance.GetComponent<Monster_script>().mapcontroller = this.gameObject;
                monsterInstance.GetComponent<Monster_script>().bot_hp_display = bot_hp;
                monsterInstance.GetComponent<Monster_script>().bot_type_display = bot_type;
                monsterInstance.GetComponent<Monster_script>().sell = sell;
                monsterInstance.GetComponent<Monster_script>().upgrade = upgrade;
                monsterHolder.Add(monsterInstance);
                yield return new WaitForSeconds(1);
            }
            
            
        }
    }

    //For testing:show the grids available for build/pass
    public void ShowPath(){
        for (int i = 0; i < route.Count; i++)
        {
            Grid_script gs = GetGrid(route[i]).GetComponent<Grid_script>();
            gs.ChangeMaterial(gs.highlightMaterial);
        }
        /*int count = 0;
        for (int i = 0; i < mapWidth; i++){
            for (int j = 0; j < mapHeight; j++){
                if(gridArray[i,j]){
                    Debug.DrawLine(GetMapPosition(i, j, 3) - new Vector3(0.2f, 0, 0),
                                   GetMapPosition(i, j, 3) + new Vector3(0.2f, 0, 0), Color.black, 1);
                    count++;
                }
            }
        }
        */
        //Debug.Log("Available grids: " + count);
        //Debug.DrawLine(new Vector3(-3,3, 0.5f), new Vector3(3,3,0.5f), Color.black, 1);
    }


    public GameObject GetGrid(Position position)
    {
        foreach (GameObject grid in gridMap)
        {
            if (GetGridI(grid) == position.i && GetGridJ(grid) == position.j)
            {
                return grid;
            }
        }
        return null;
    }
    //helper function to return a map position of corresponding gridArray indices with y coordinator set to y.
    public Vector3 GetMapPosition(int i, int j, float y){
        float mapX = i - mapWidth / 2.0f + 0.5f;
        float mapZ = j - mapHeight / 2.0f + 0.5f;
        return new Vector3(mapX, y, mapZ);
    }
    //helper function to set the availability of a grid
    public void SetAvailability(GameObject grid, bool availability){
        int i = GetGridI(grid);
        int j = GetGridJ(grid);
        gridArray[i, j] = availability;
    }


    //getting i and j indices of the grid
    public int GetGridI(GameObject grid){
        int i = (int)(grid.transform.position.x - 0.5 + mapWidth / 2.0f);
        return i;
    }
    public int GetGridJ(GameObject grid)
    {
        int j = (int)(grid.transform.position.z - 0.5 + mapHeight / 2.0f);
        return j;
    }
    bool FindPath()
    {
        List<Position> newFrontier = new List<Position>();
        List<Position> oldFrontier = new List<Position>();
        List<Position> temp;
        List<Position> explored = new List<Position>();
        Dictionary<Position, Position> previous = new Dictionary<Position, Position>();

        oldFrontier.Add(entry);
        while (!Contains(oldFrontier, exit))
        {
            foreach (Position p in oldFrontier)
            {
                foreach (Position q in Neighbor(p))
                {
                    //check for availability and if q is explored
                    if (!Contains(explored, q))
                    {
                        explored.Add(q);
                        //GetGrid(q).GetComponent<Grid_script>().ChangeMaterial(GetGrid(q).GetComponent<Grid_script>().selectedMaterial);

                        if (gridArray[q.i, q.j])
                        {
                            newFrontier.Add(q);
                            previous.Add(q, p);
                        }
                    }
                }
            }
            oldFrontier.Clear();
            temp = oldFrontier;
            oldFrontier = newFrontier;
            newFrontier = temp;
            if (oldFrontier.Count == 0)
            {
                break;
            }
        }
        if (!Contains(oldFrontier, exit))
        {
            return false;
        }
        else
        {
            Position newExit = null;
            //Get the new exit in oldFrontier
            foreach (Position q in oldFrontier)
            {
                if (q.i == exit.i && q.j == exit.j)
                {
                    newExit = q;
                }
            }
            Position p = newExit;
            explored.Clear();
            while (previous[p] != entry)
            {
                explored.Add(p);
                p = previous[p];
            }
            explored.Add(entry);

            //flip order
            route.Clear();
            for (int i = explored.Count - 1; i >= 0; i--)
            {
                route.Add(explored[i]);
            }
            return true;
        }

    }

    bool Contains(List<Position> l, Position p)
    {
        foreach (Position q in l)
        {
            if (q.i == p.i && q.j == p.j)
            {
                return true;
            }
        }
        return false;
    }


    List<Position> Neighbor(Position p)
    {
        List<Position> dest = new List<Position>();
        if (p.i > 0)
        {
            dest.Add(new Position(p.i - 1, p.j));
        }
        if (p.i < mapWidth - 1)
        {
            dest.Add(new Position(p.i + 1, p.j));
        }
        if (p.j > 0)
        {
            dest.Add(new Position(p.i, p.j - 1));
        }
        if (p.j < mapHeight - 1)
        {
            dest.Add(new Position(p.i, p.j + 1));
        }
        return dest;
    }
}
