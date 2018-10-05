using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;

public class signincontroller : MonoBehaviour {
    public string f1;
    public string f2;
    public string f3;
    public string f4;
    public void formatted1(string formatted) {
        f1 = formatted;
    }
    public void formatted2(string formatted2)
    {
        f2 = formatted2;
    }
    public void formatted3(string formatted3)
    {
        f3 = formatted3;
    }
    public void formatted4(string formatted4)
    {
        f4 = formatted4;
    }
    public void conf() {
        string total = f1 + f2 + f3 + f4;
        Debug.Log(total);
        
    }
    public void conf2(int index) {
        SceneManager.LoadScene(index);
    }
}
