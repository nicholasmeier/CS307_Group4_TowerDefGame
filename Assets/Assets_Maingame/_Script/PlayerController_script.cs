using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController_script : MonoBehaviour {
    public Text resourceText;
    public Text type_display;
    public Text hp_atk_display;
    public Text HPtext;
    public Text Gameover;
    public int init_hp;
    private int current_hp;
    public float init_resource;
    private float current_resource;
    private bool mouse;
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
	}

    public int GetCurrentResource() {
        return current_hp;
    }

    public void addResource(int delta) {
        current_resource += delta;
        resourceText.text = "Gold: " + current_resource.ToString();
    }

	// Update is called once per frame
	void Update () {
        if (current_hp <= 0) {
            Gameover.text = "GGWP!!";
        }
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
                        type_display.text = "Monster";
                        hp_atk_display.text ="HP: " + currentClicked.GetComponent<Monster_script>().hp.ToString();
                        

                    }

                }

            }
        }
	}

    private void OnMouseDown()
    {
        mouse = true;
    }
}
