using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectGameStatus : MonoBehaviour {

    GameObject sceneM;
    public GameObject info;
    bool first_load;
	// Use this for initialization
	void Start () {
        sceneM = GameObject.Find("SceneManager");
        first_load = true;
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(sceneM.GetComponent<SceneManager_script>().isOver());
        int current_index = SceneManager.GetActiveScene().buildIndex;
        
        if (current_index == 0 && !first_load)
        {
            if (!sceneM) {
                sceneM = GameObject.Find("SceneManager");
            }
            if (sceneM.GetComponent<SceneManager_script>().isOver() && sceneM)
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
