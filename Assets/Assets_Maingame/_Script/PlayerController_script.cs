using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SelectionStatus
{
    towerSelected, iconSelected, monsterSelected, none
}
public class PlayerController_script : MonoBehaviour {
    //Resources
    public int init_hp;
    public float init_resource;
    float current_resource;
    bool mouse;
    int current_hp;

    //Related game objects
    public Text resourceText;
    public Text type_display;
    public Text hp_atk_display;
    public Text HPtext;
    public Text Gameover;

    //Selection status
    public SelectionStatus selectionStatus;
    public GameObject selectedIcon;
    public GameObject selectedTower;
    GameObject selectedMonster;

    // Use this for initialization
    void Start () {
        current_resource = init_resource;
        current_hp = init_hp;
        resourceText.text = "Gold: " + current_resource.ToString();
        HPtext.text = "HP: " + current_hp.ToString();
        type_display.text = "";
        hp_atk_display.text = "";
        Gameover.text = "";
        mouse = false;
        selectionStatus = SelectionStatus.none;
	}

    public void addCurrentHP(int tobeadd) {
        current_hp += tobeadd;
        HPtext.text = "HP: " + current_hp.ToString();
    }

    public float getCurrentResource() {
        return current_resource;
    }

    public void addCurrentResource(float tobeadd) {
        current_resource += tobeadd;
        resourceText.text = "Gold: " + current_resource.ToString();
    }

    public void addResource(int delta) {
        current_resource += delta;
        resourceText.text = "Gold: " + current_resource.ToString();
    }
    public int getUserHP() {
        return current_hp;
    }
	// Update is called once per frame
	void Update () {
        if (current_hp <= 0) {
            Gameover.text = "GGWP!!";
        }
        /*
        if (mouse) {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    GameObject currentClicked = hit.transform.gameObject;
                    if (currentClicked.tag == "Monster")
                    {
                        Debug.Log("HIT!");
                        type_display.text = "Monster";
                        hp_atk_display.text ="HP: " + currentClicked.GetComponent<Monster_script>().hp.ToString();
                        

                    }

                }

            }
        }
        */
	}

    public SelectionStatus GetSelectionStatus(){
        return selectionStatus;
    }
    public void SetSelectionStatus(SelectionStatus selectionStatus){
        this.selectionStatus = selectionStatus;
    }
    public GameObject GetSelectedIcon(){
        return selectedIcon;
    }
    public void SetSelectedIcon(GameObject tower){
        this.selectedIcon = tower;
    }
    public GameObject GetSelectedTower(){
        return this.selectedTower;
    }
    public void SetSelectedTower(GameObject tower){
        this.selectedTower = tower;
    }
    private void OnMouseDown()
    {
        mouse = true;
    }
}
