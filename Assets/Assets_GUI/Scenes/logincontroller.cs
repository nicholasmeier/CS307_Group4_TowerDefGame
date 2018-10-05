using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;

public class logincontroller : MonoBehaviour {

    public string formattedinfo1;
    public string formattedinfo2;

    public void getInput(string uname) {
        //formattedinfo1 += "uname:";
        formattedinfo1 += uname;
        formattedinfo1 += "\n";
    }
    public void getInput2(string upswd) {
        //formattedinfo2 += "upswd:";
        
        formattedinfo2 += upswd;
        formattedinfo2 += "\n";
    }
    public void pushlogin() {
        string newstring = formattedinfo1 + formattedinfo2;
        Debug.Log(newstring);
    }

    public void scenetransfer(int index) {
        SceneManager.LoadScene(index);
    }
}
