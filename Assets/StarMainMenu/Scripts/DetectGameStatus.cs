using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectGameStatus : MonoBehaviour {

    GameObject sceneM;
    GameObject Detecter;
    public GameObject info;
    bool ready;
    
	// Use this for initialization
	void Start () {
        //sceneM = GameObject.Find("SceneManager");
        Detecter = GameObject.Find("StartDetecter");
        ready = false;
        
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(sceneM.GetComponent<SceneManager_script>().isOver());
        if (!Detecter.GetComponent<StartDetect>().isfirst()) {
            ready = true;
        }
        int current_index = SceneManager.GetActiveScene().buildIndex;
        if (current_index == 0 && ready)
        {
            if (!Detecter) {
                Detecter = GameObject.Find("StartDetecter");
            }
            if (Detecter.GetComponent<StartDetect>().isOver())
            {
                info.SetActive(true);
            }
            else
            {
                info.SetActive(false);
            }
        }
	}
}
