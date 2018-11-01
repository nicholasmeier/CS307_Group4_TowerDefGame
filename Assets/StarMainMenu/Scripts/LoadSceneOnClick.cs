using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {
    public static int ind = 1;

    public void loadByIndex(int index){
        SceneManager.LoadScene(index);
    }

    public void loadToMainMenu(){
        ind = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync(ind);
        if (ind == -1) print("wrong scene ");
    }

    public void loadBack(){
        SceneManager.UnloadSceneAsync(0);
        SceneManager.LoadScene(ind);
    }
}
