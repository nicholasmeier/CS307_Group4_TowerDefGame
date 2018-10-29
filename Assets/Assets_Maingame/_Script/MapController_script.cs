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
    public int mapLength;
    public Text wave_display;
    public GameObject player;
    public GameObject gridPrefab;           //The grid used to build the battleground
    private bool[,] gridArray;              //Int version of gridMap, used to increase performance,
                                            //False in entry meaning the grid is not available(occupied).

    public Position entry;
    public Position exit;
    public List<Position> route;

    private float player_current_resource;
    private int waveNumber;



    // Use this for initialization
    void Start () {
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
        gridArray = new bool[mapWidth,mapLength];
        for (int i = 0; i < mapWidth; i++){
            for (int j = 0; j < mapLength; j++){
                gridArray[i, j] = true;
            }
        }

        //Initialize gridMap
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapLength; j++)
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
        

        //A sample wave
        waveNumber++;
        wave_display.text = "Wave: " + waveNumber.ToString();
        yield return new WaitForSeconds(5);
        foreach (GameObject monster in wave.monsters){
            GameObject monsterInstance;
            monsterInstance = Instantiate(monster);
            monsterInstance.GetComponent<Monster_script>().player = player;
            monsterInstance.GetComponent<Monster_script>().mapcontroller = this.gameObject;
            monsterHolder.Add(monsterInstance);
            yield return new WaitForSeconds(1);
        }
    }

    //For testing:show the grids available for build/pass
    public void ShowPath(){
        int count = 0;
        for (int i = 0; i < mapWidth; i++){
            for (int j = 0; j < mapLength; j++){
                if(gridArray[i,j]){
                    Debug.DrawLine(GetMapPosition(i, j, 3) - new Vector3(0.2f, 0, 0),
                                   GetMapPosition(i, j, 3) + new Vector3(0.2f, 0, 0), Color.black, 1);
                    count++;
                }
            }
        }
        //Debug.Log("Available grids: " + count);
        //Debug.DrawLine(new Vector3(-3,3, 0.5f), new Vector3(3,3,0.5f), Color.black, 1);
    }

    //helper function to return a map position of corresponding gridArray indices with y coordinator set to y.
    public Vector3 GetMapPosition(int i, int j, float y){
        float mapX = i - mapWidth / 2.0f + 0.5f;
        float mapZ = j - mapLength / 2.0f + 0.5f;
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
        int j = (int)(grid.transform.position.z - 0.5 + mapLength / 2.0f);
        return j;
    }
}
