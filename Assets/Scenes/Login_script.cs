using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login_script : MonoBehaviour {

    public InputField userField;
    public InputField passField;
    public static string username;
    public static string pass;

    public Button loginButton;

	// Use this for initialization
	void Start () {
        Button lgn_btn = loginButton.GetComponent<Button>();

        lgn_btn.onClick.AddListener(login_check);

	}
	
    void login_check()
    {
        username = userField.text.ToString();
        Debug.Log("df;lahjfka");
    }

	// Update is called once per frame
	void Update () {
		
	}
}
