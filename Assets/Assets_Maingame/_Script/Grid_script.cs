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
    public PlayerController_script playerController;

    public bool availability;
    

	// Use this for initialization
	void Start () {
        availability = true;
        playerController = mapController.player.GetComponent<PlayerController_script>();
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
    public void setAvailability(bool availability) {
        mapController.SetAvailability(this.gameObject, availability);
        this.availability = availability;
    }

    void OnMouseDown()
    {
        if(playerController.GetSelectionStatus() == SelectionStatus.iconSelected){
            if (availability)
            {
                mapController.BuildTower(playerController.GetSelectedIcon(), this.gameObject);
                playerController.SetSelectionStatus(SelectionStatus.none);
                //TODO:BUILD TOWER
            }
            //TODO:else display a text
        }
        else{
            playerController.SetSelectionStatus(SelectionStatus.none);
        }
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
