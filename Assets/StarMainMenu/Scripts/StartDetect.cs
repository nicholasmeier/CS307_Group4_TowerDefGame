using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartDetect : MonoBehaviour {

    bool first_start;
    bool changed;
    bool gameover;
    GameObject player;
	// Use this for initialization
	void Start () {
        gameover = false;
        changed = false;
        first_start = true;
        Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
        if (!changed && SceneManager.GetActiveScene().buildIndex == 1)
        {
            player = GameObject.Find("PlayerController");
            if (player.GetComponent<PlayerController_script>().getUserHP() <= 0)
            {
                gameover = true;
            }
            else {
                gameover = false;
            }
            changed = true;
            first_start = false;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1) {
            player = GameObject.Find("PlayerController");
            if (player.GetComponent<PlayerController_script>().getUserHP() <= 0)
            {
                gameover = true;
            }
            else
            {
                gameover = false;
            }
        }
	}

    public bool isfirst() {
        return first_start;
    }
    public bool isOver() {
        return gameover;
    }
}
