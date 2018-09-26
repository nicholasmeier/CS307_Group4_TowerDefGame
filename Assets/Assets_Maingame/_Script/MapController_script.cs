using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is a test
[System.Serializable]
public class Wave{
    public List<GameObject> monsters;

}

public class MapController_script : MonoBehaviour {

    //for prototyping purposes
    public List<GameObject> monsterHolder;
    public List<Wave> waves;
    public float speed;

	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnWave(waves[0]));
	}
	
	// Update is called once per frame
	void Update () {
	    
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
