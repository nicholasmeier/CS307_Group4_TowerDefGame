using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_script : MonoBehaviour {

    public Material selectedMaterial;
    public Material normalMaterial;
    public Material highlightMaterial;
    public Material entryMaterial;
    public Material exitMaterial;
    public MapController_script mapController;

    public bool availability;
    

	// Use this for initialization
	void Start () {
        availability = true;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    

    //For testing purposes
    //Use Physics.Raycast instead
    private void OnMouseOver()
    {
        Material currentM = this.gameObject.GetComponent<Renderer>().material;
        if (availability){
            if (currentM.Equals(highlightMaterial) && currentM.Equals(entryMaterial) && currentM.Equals(exitMaterial))
            {
                ChangeMaterial(selectedMaterial);
            }
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
        Material currentM = this.gameObject.GetComponent<Renderer>().material;
        if (availability){

            if (currentM.Equals(highlightMaterial) && currentM.Equals(entryMaterial) && currentM.Equals(exitMaterial))
            {
                ChangeMaterial(normalMaterial);
            }
        }
    }

    public void ChangeMaterial(Material material){
        this.gameObject.GetComponent<Renderer>().material = material;
    }
}
