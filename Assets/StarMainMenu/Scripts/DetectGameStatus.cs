using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectGameStatus : MonoBehaviour {

    GameObject sceneM;
    public GameObject info;
	// Use this for initialization
	void Start () {
        sceneM = GameObject.Find("SceneManager");
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(sceneM.GetComponent<SceneManager_script>().isOver());
        if (sceneM.GetComponent<SceneManager_script>().isOver())
        {
            info.SetActive(true);
        }
        else {
            info.SetActive(false);
        }
	}
}
