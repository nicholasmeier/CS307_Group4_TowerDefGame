using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomInfoBar_Controller : MonoBehaviour {
    public GameObject player;
    public Text type;
    public Text attribute;
    public Button sell;
    public Button upgrade;
    //current selected
    GameObject current_selected_tower;
    GameObject current_selected_monster;

	// Use this for initialization
	void Start () {
        clear();
        sell.onClick.AddListener(delegate { sellTower(); });
        upgrade.onClick.AddListener(delegate { current_selected_tower.GetComponent<Tower_script>().TowerUpgrade(); });
    }

    void Monster_display() {
        clear();
        type.text = "Monster";
        attribute.text = "HP: " + current_selected_monster.GetComponent<Monster_script>().getHp().ToString();
    }

    void clear() {
        sell.gameObject.SetActive(false);
        upgrade.gameObject.SetActive(false);
        type.text = "";
        attribute.text = "";
    }

    void Tower_display() {
        type.text = "Tower";
        attribute.text = "Basic";
        upgrade.gameObject.SetActive(true);
        sell.gameObject.SetActive(true);
    }


    void setTower(GameObject tower) {
        this.current_selected_tower = tower;
    }

    void setMonster(GameObject monster) {
        this.current_selected_monster = monster;
    }
	
	// Update is called once per frame
	void Update () {
        if (player.GetComponent<PlayerController_script>().GetSelectionStatus() == SelectionStatus.none) {
            clear();
        }
        else if (player.GetComponent<PlayerController_script>().GetSelectionStatus() == SelectionStatus.towerSelected)
        {
            setTower(player.GetComponent<PlayerController_script>().GetSelectedTower());
            Tower_display();
        }
        else if (player.GetComponent<PlayerController_script>().GetSelectionStatus() == SelectionStatus.monsterSelected)
        {
            Monster_display();
        }
    }

    void sellTower(){
        current_selected_tower.GetComponent<Tower_script>().Sell();
        player.GetComponent<PlayerController_script>().SetSelectionStatus(SelectionStatus.none);
    }
}
