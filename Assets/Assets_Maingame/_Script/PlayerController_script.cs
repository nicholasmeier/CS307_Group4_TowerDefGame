using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController_script : MonoBehaviour {
    public Text resourceText;
    public Text HPtext;
    public Text Gameover;
    public int init_hp;
    private int current_hp;
    public float init_resource;
    private float current_resource;
	// Use this for initialization
	void Start () {
        current_resource = init_resource;
        current_hp = init_hp;
        resourceText.text = "Gold: " + current_resource.ToString();
        HPtext.text = "HP: " + current_hp.ToString();
        Gameover.text = "";
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
	}
}
