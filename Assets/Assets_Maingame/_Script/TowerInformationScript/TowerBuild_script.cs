using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBuild_script : MonoBehaviour {
    bool TowerOnMouse = false;
    public GameObject Tower;
    public GameObject InsTower;
    public GameObject player;
    public GameObject mapcontroller;
    public Text display_info;
    public Button sell;
    public Button upgrade;
    public Text bot_atk_display;
    public Text bot_type_display;
    private float gold;
    private float price;
	// Use this for initialization
	void Start () {
        gold = player.GetComponent<PlayerController_script>().getCurrentResource();
        price = Tower.GetComponent<Tower_script>().getPrice();

    }
	
	// Update is called once per frame
	void Update () {

        /*RaycastHit[] allHit = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
        foreach(RaycastHit hit in allHit)
        {
            if (hit.collider.gameObject.name == "TowerIcon1")
            {
                Debug.Log("Click");
            }
        }*/
        //Debug.Log(price);
        gold = player.GetComponent<PlayerController_script>().getCurrentResource();
        if (TowerOnMouse == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 200))
            {
                Collider gridhit= hitInfo.collider;
                GameObject gridhitObject = gridhit.gameObject;
                if (Input.GetMouseButtonDown(0) )
                {
                    if (gold < 10) 
                    {
                        display_info.text = "You don't have enough gold!!";
                        TowerOnMouse = false;
                    }
                    else
                    {
                        if (mapcontroller.GetComponent<MapController_script>().getInwave())
                        {
                            bool flag = false;
                            for (int i = 0; i < mapcontroller.GetComponent<MapController_script>().route.Count; i++)
                            {
                                GameObject gs = mapcontroller.GetComponent<MapController_script>().GetGrid(mapcontroller.GetComponent<MapController_script>().route[i]);
                                if (gridhit.gameObject.Equals(gs))
                                {
                                    TowerOnMouse = false;
                                    display_info.text = "Can't build";
                                    flag = true;

                                }

                            }
                            if (!flag) {
                                if (gridhit.GetComponent<Grid_script>().availability == true)
                                {
                                    display_info.text = "";
                                    InsTower = GameObject.Instantiate(Tower, null, true);
                                    Tower_script ts = InsTower.GetComponent<Tower_script>();
                                    ts.SetBot_atk_display(bot_atk_display);
                                    ts.SetBot_type_display(bot_type_display);
                                    ts.SetSell(sell);
                                    ts.SetUpgrade(upgrade);
                                    ts.SetPlayer(player);
                                    ts.SetMapController(mapcontroller);

                                    InsTower.GetComponent<Tower_script>().SetGrid(gridhitObject);

                                    InsTower.transform.position = gridhitObject.transform.position + new Vector3(0, 1F, 0);
                                    gridhit.gameObject.GetComponent<Grid_script>().availability = false;
                                    mapcontroller.GetComponent<MapController_script>().SetAvailability(gridhitObject, false);
                                    TowerOnMouse = false;
                                    player.GetComponent<PlayerController_script>().addCurrentResource(-10);
                                    //mapcontroller.GetComponent<MapController_script>().UpdatePath();

                                }
                            }

                        }
                        else
                        {
                            if (gridhit.GetComponent<Grid_script>().availability == true)
                            {
                                display_info.text = "";
                                InsTower = GameObject.Instantiate(Tower, null, true);
                                Tower_script ts = InsTower.GetComponent<Tower_script>();
                                ts.SetBot_atk_display(bot_atk_display);
                                ts.SetBot_type_display(bot_type_display);
                                ts.SetSell(sell);
                                ts.SetUpgrade(upgrade);
                                ts.SetPlayer(player);
                                ts.SetMapController(mapcontroller);

                                InsTower.GetComponent<Tower_script>().SetGrid(gridhitObject);

                                InsTower.transform.position = gridhitObject.transform.position + new Vector3(0, 1F, 0);
                                gridhit.gameObject.GetComponent<Grid_script>().availability = false;
                                mapcontroller.GetComponent<MapController_script>().SetAvailability(gridhitObject, false);
                                TowerOnMouse = false;
                                player.GetComponent<PlayerController_script>().addCurrentResource(-10);
                                mapcontroller.GetComponent<MapController_script>().UpdatePath();

                            }
                        }
                    }
                    
                    //Debug.Log("shoot");
                }
                else if(Input.GetMouseButtonDown(1))
                {
                    TowerOnMouse = false;
                }
            }
         

        }
    }

    /*private void OnMouseDown()
    {
        TowerOnMouse = true;
        //Debug.Log("Click");
    }*/

    public void BuildTower()
    {
        TowerOnMouse = true;
    }
    /*
        if (TowerOnMouse == true)
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 200))
            {
                
                Collider gridhit = hitInfo.collider;
                GameObject gridthitObject = gridhit.gameObject;
                //Debug.Log("baidu");
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("baidu");
                    InsTower = GameObject.Instantiate(Tower, null, true);
                    if (gridhit.GetComponent<Grid_script>().availability == true)
                    {
                        InsTower.transform.position = gridthitObject.transform.position;
                        gridhit.GetComponent<Grid_script>().availability = false;
                        TowerOnMouse = false;
                    }

                    //Debug.Log("shoot");
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    TowerOnMouse = false;
                }
            }

        }
    }*/
}
