using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave{
    public List<GameObject> monsters;
}

public class MapController_script : MonoBehaviour {

    //for prototyping purposes
    public List<GameObject> monsterHolder;  //The list that holds all the live monsters on the map
    public List<Wave> waves;                //The list containing the wave design
    public List<GameObject> gridMap;        //The list holding all the grids on the map
    public int mapWidth;                
    public int mapLength;
    public GameObject gridPrefab;           //The grid used to build the battleground
    private bool[,] gridArray;              //Int version of gridMap, used to increase performance,
                                            //False in entry meaning the grid is not available(occupied).

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
                float mapX = i - mapWidth / 2.0f + 0.5f;
                float mapZ = j - mapLength / 2.0f + 0.5f;
                GameObject gridInstance;
                gridInstance = Instantiate(gridPrefab, new Vector3(mapX, -0.5f, mapZ), Quaternion.identity);
                gridMap.Add(gridInstance);
            }
        }
        yield return 0;
    }

    IEnumerator SpawnWave(Wave wave){
        foreach(GameObject monster in wave.monsters){
            GameObject monsterInstance;
            monsterInstance = Instantiate(monster);
            monsterHolder.Add(monsterInstance);
            yield return new WaitForSeconds(1);
        }
    }
}
