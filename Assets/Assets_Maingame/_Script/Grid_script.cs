using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_script : MonoBehaviour {

    public Material selectedMaterial;
    public Material normalMaterial;
    public MapController_script mapController;

    public bool availability;
    

	// Use this for initialization
	void Start () {
        availability = true;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public bool getAvailability() {
        return availability;
    }

    //For testing purposes
    //Use Physics.Raycast instead
    private void OnMouseOver()
    {
        if(availability){
            ChangeMaterial(selectedMaterial);
        }
    }

    private void OnMouseDown()
    {
        /*if(availability){
            availability = false;
            mapController.SetAvailability(this.gameObject, false);
            ChangeMaterial(selectedMaterial);
        }else{
            availability = true;
            mapController.SetAvailability(this.gameObject, true);
            ChangeMaterial(normalMaterial);
        }
        mapController.ShowPath();*/
    }

    private void OnMouseExit()
    {
        if(availability){
            ChangeMaterial(normalMaterial);
        }
    }

    private void ChangeMaterial(Material material){
        this.gameObject.GetComponent<Renderer>().material = material;
    }
}
