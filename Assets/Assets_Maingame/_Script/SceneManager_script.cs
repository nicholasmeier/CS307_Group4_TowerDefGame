using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager_script : MonoBehaviour {
    public GameObject player;
    // Use this for initialization
    bool isGameover;
	void Start () {
        if (!player) {
            player = GameObject.Find("PlayerController");
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        int current_index = SceneManager.GetActiveScene().buildIndex;
        if (current_index == 1)
        {
            if (!player)
            {
                player = GameObject.Find("PlayerController");
            }
            if (player.GetComponent<PlayerController_script>().getUserHP() <= 0 && player)
            {
                Time.timeScale = 1;
                Debug.Log("Die");
                isGameover = true;
                loadByIndex(0);
            }
            else
            {
                isGameover = false;
            }
        }
	}

    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
		Application.Quit();
        #endif
    }
    public void loadByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
    public bool isOver() {
        return isGameover;
    }
}
