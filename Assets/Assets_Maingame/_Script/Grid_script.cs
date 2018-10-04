using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_script : MonoBehaviour {

    public Material selectedMaterial;
    public Material normalMaterial;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    //For testing purposes
    //Use Physics.Raycast instead
    private void OnMouseOver()
    {
        this.gameObject.GetComponent<Renderer>().material = selectedMaterial;
        Debug.Log("over");
    }

    private void OnMouseExit()
    {
        this.gameObject.GetComponent<Renderer>().material = normalMaterial;
        Debug.Log("exit");
    }
}
