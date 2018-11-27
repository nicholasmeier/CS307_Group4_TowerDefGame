using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Price{
    public float baseTower;
    public float snipeTower;
    public float barrier;
    public float resourceTower;
    public float slowTower;

    public float GetPrice(GameObject tower){
        if (tower.name.Equals("BaseTower"))
        {
            return baseTower;
        }
        if(tower.name.Equals("SnipeTower")){
            return snipeTower;
        }
        if(tower.name.Equals("Barrier")){
            return barrier;
        }
        if(tower.name.Equals("ResourceTower")){
            return resourceTower;
        }
        if(tower.name.Equals("SlowTower")){
            return slowTower;
        }

        return 10000;
    }
}

public class IconManager : MonoBehaviour {

    public GameObject playerController;
    public GameObject mapController;
    public Price price;

    //Towers
    public Button baseTower;
    public GameObject tower_base_prefab;
    //Tower_script tower_base;

    public Button snipeTower;
    public GameObject tower_snipe_prefab;
    //Tower_script tower_snipe;

    public Button barrier;
    public GameObject barrier_prefab;
    //Tower_script barrier_script;

    public Button resourceTower;
    public GameObject tower_resource_prefab;
    //Tower_script tower_resource;

    public Button slowTower;
    public GameObject tower_slow_prefab;
    //Tower_script tower_slow;

    PlayerController_script ps;

	// Use this for initialization
	void Start () {
        ps = playerController.GetComponent<PlayerController_script>();
        //Instantiate tower scripts for each type of tower
        //tower_base = new Tower_base();
        baseTower.onClick.AddListener(delegate { selectIcon(price.baseTower, tower_base_prefab); });
        //tower_snipe = new Tower_snipe();
        snipeTower.onClick.AddListener(delegate { selectIcon(price.snipeTower, tower_snipe_prefab); });
        //barrier_script = new Tower_barrier();
        barrier.onClick.AddListener(delegate { selectIcon(price.barrier, barrier_prefab); });
        //tower_resource = new Tower_resource();
        resourceTower.onClick.AddListener(delegate { selectIcon(price.resourceTower, tower_resource_prefab); });
        //tower_slow = new Tower_slow();
        slowTower.onClick.AddListener(delegate { selectIcon(price.slowTower, tower_slow_prefab); });

    }

    void selectIcon(float price, GameObject tower){
        //TODO:If the player is currently selecting the same type of tower, deselect.

        //Check the player's current gold
        if (ps.getCurrentResource() > price)
        {
            mapController.GetComponent<MapController_script>().display_info.text = "";
            //Change the selection status
            ps.SetSelectionStatus(SelectionStatus.iconSelected);
            ps.SetSelectedIcon(tower);
        }
        else{
            mapController.GetComponent<MapController_script>().display_info.text = "Not enough gold!";
        }
    }
}
