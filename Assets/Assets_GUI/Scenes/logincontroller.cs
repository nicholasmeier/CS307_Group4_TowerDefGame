using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;

public class logincontroller : MonoBehaviour {

    public string userInfo;
    private string userPass;
    private string formattedInfo;
    public void getInput(string uname) {
        //formattedinfo1 += "uname:";
        userInfo = uname;
    }
    public void getInput2(string upswd) {
        //formattedinfo2 += "upswd:";
        char[] c = upswd.ToCharArray();
        //Simple ShiftCypher for storing info. Will change to more secure cypher later
        for (int i = 0; i < upswd.Length; i++)
        {
            char a = c[i];
            a++;    
            c[i] = a;
        }
        userPass = new string(c);
    }
    public void pushlogin() {
        formattedInfo = userInfo + ":" + userPass;
        string[] lines = System.IO.File.ReadAllLines("infoTemp.txt");
        foreach (string line in lines)
            Debug.Log(line);
    }

    public void scenetransfer(int index) {
        
        SceneManager.LoadScene(index);
    }
}
