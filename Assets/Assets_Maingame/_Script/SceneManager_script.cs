using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager_script : MonoBehaviour {
    public GameObject player;
    // Use this for initialization
    bool isGameover;
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (player.GetComponent<PlayerController_script>().getUserHP() <= 0 && player)
        {
            isGameover = true;
            loadByIndex(0);
        }
        else {
            isGameover = false;
        }
	}

    public void Quit()
    {
        Application.Quit();
    }
    public void loadByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
    public bool isOver() {
        return isGameover;
    }
}
