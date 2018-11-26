using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour {

    public GameObject playerController; 

    //Towers
    public Button baseTower;
    public GameObject tower_base_prefab;
    Tower_script tower_base;

    public Button snipeTower;
    public GameObject tower_snipe_prefab;
    Tower_script tower_snipe;

    public Button barrier;
    public GameObject barrier_prefab;
    Tower_script barrier_script;

    public Button resourceTower;
    public GameObject tower_resource_prefab;
    Tower_script tower_resource;

    public Button slowTower;
    public GameObject tower_slow_prefab;
    Tower_script tower_slow;

    PlayerController_script ps;

	// Use this for initialization
	void Start () {
        ps = playerController.GetComponent<PlayerController_script>();
        //Instantiate tower scripts for each type of tower
        tower_base = new Tower_base();
        baseTower.onClick.AddListener(delegate { selectIcon(tower_base, tower_base_prefab); });
        tower_snipe = new Tower_snipe();
        snipeTower.onClick.AddListener(delegate { selectIcon(tower_snipe, tower_snipe_prefab); });
        barrier_script = new Tower_barrier();
        barrier.onClick.AddListener(delegate { selectIcon(barrier_script, barrier_prefab); });
        tower_resource = new Tower_resource();
        resourceTower.onClick.AddListener(delegate { selectIcon(tower_resource, tower_resource_prefab); });
        tower_slow = new Tower_slow();
        slowTower.onClick.AddListener(delegate { selectIcon(tower_slow, tower_slow_prefab); });

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
