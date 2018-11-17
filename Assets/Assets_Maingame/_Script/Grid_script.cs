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
        ChangeMaterial(normalMaterial);
	}
	
	void Update () {
	    
	}

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
            }
            //TODO:else display a text
        }
        else{
            playerController.SetSelectionStatus(SelectionStatus.none);
        }
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
