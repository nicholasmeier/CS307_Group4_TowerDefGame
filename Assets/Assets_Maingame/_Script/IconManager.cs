using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour {

    public Button baseTower;
    public GameObject playerController;
    public GameObject tower_base_prefab;
    Tower_script tower_base;

    PlayerController_script ps;

	// Use this for initialization
	void Start () {
        ps = playerController.GetComponent<PlayerController_script>();
        //Instantiate tower scripts for each type of tower
        tower_base = new Tower_base();
        baseTower.onClick.AddListener(delegate { selectIcon(tower_base, tower_base_prefab); });
	}

    void selectIcon(Tower_script tower_script, GameObject tower){
        //TODO:If the player is currently selecting the same type of tower, deselect.

        //Check the player's current gold
        if (ps.getCurrentResource() > tower_script.getPrice())
        {
            //towerBuilder.GetComponent<TowerBuild_script>().BuildTower();

            //Change the selection status
            ps.SetSelectionStatus(SelectionStatus.iconSelected);
            ps.SetSelectedIcon(tower);
        }
    }
}
