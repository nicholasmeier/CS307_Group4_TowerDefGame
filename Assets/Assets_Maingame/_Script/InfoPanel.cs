using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour {

    //private bool mouseflag;
    private GameObject BottomInfoPanel;
    private GameObject Parent;
    private GameObject currentClicked;
    public Text type_display;
    public Text Hp_display;
    public Image Image_display;
    
    
    //below is for testing
    private int counter;
	// Use this for initialization
	void Start () {
        
        //BottomInfoPanel = Parent.transform.Find("BottomInfoUI").gameObject;
        //mouseflag = false;
        type_display.text = "";
        Hp_display.text = "";
        Image_display.gameObject.SetActive(true);

	}

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                currentClicked = hit.transform.gameObject;
                if (currentClicked.tag == "Monster")
                {
                    type_display.text = "Monster";
                    string clickedHP = currentClicked.GetComponent<Monster_script>().hp.ToString();
                    Hp_display.text = clickedHP;
                 
                }
         
            }
            
        }
        

    }
   

    // Update is called once per frame
    //void OnGUI() {
    //    if (mouseflag)
    //    {
    //        BottomInfoPanel.SetActive(true);
    //        Debug.Log("Live");
    //    }
    //}
}
