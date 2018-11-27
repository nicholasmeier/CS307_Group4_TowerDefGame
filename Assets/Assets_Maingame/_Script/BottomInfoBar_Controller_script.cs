using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomInfoBar_Controller_script : MonoBehaviour {
    public GameObject player;
    public Text type;
    public Text attribute;
    public Text gameInfo;
    public Button sell;
    public Button upgrade;
    public Button speed;
    public Button pause;
    //current selected
    GameObject current_selected_tower;
    GameObject current_selected_monster;
    float tower_attack;

	// Use this for initialization
	void Start () {
        clear();
        pause.onClick.AddListener(delegate { pauseGame(); });
        sell.onClick.AddListener(delegate { sellTower(); });
        speed.onClick.AddListener(delegate { speedChange(); });
        upgrade.onClick.AddListener(delegate { current_selected_tower.GetComponent<Tower_script>().TowerUpgrade(); });
    }

    void pauseGame() {

        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
            speed.GetComponentInChildren<Text>().text = ">";
            pause.GetComponentInChildren<Text>().text = "G O";
        }
        else {
            Time.timeScale = 1;
            speed.GetComponentInChildren<Text>().text = ">";
            pause.GetComponentInChildren<Text>().text = "|  |";
        }
    }

    void speedChange() {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 3;
            speed.GetComponentInChildren<Text>().text = ">>";
            pause.GetComponentInChildren<Text>().text = "|  |";
        }
        else {
            Time.timeScale = 1;
            speed.GetComponentInChildren<Text>().text = ">";
            pause.GetComponentInChildren<Text>().text = "|  |";
        }
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
        gameInfo.text = "";
    }

    void Tower_display() {
        type.text = current_selected_tower.GetComponent<Tower_script>().getName();
        attribute.text = "ATK: " + current_selected_tower.GetComponent<Tower_script>().getAtk().ToString();
        if (current_selected_tower.GetComponent<Tower_script>().can_be_upgrade() == true)
        {
            upgrade.gameObject.SetActive(true);
            gameInfo.text = "";
        }
        else {
            upgrade.gameObject.SetActive(false);
            gameInfo.text = "This tower cannot be upgraded now!";
        }
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
