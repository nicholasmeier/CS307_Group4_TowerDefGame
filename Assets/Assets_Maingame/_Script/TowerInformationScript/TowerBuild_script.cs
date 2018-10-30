using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBuild_script : MonoBehaviour {
    bool TowerOnMouse = false;
    public GameObject Tower;
    public GameObject InsTower;
	// Use this for initialization
	void Start () {
		
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
        if(TowerOnMouse == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 200))
            {
                Collider gridhit= hitInfo.collider;
                GameObject gridthitObject = gridhit.gameObject;
                if (Input.GetMouseButtonDown(0) )
                {
                    
                    InsTower = GameObject.Instantiate(Tower, null, true);
                    if (gridhit.GetComponent<Grid_script>().availability == true)
                    {
                        InsTower.transform.position = gridthitObject.transform.position;
                        gridhit.GetComponent<Grid_script>().availability = false;
                        TowerOnMouse = false;
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
