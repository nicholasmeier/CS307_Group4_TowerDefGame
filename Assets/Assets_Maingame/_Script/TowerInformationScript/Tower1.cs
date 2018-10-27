using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower1 : MonoBehaviour {
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
                Vector3 target = hitInfo.point;
                if (Input.GetMouseButtonDown(0))
                {
                    
                    InsTower = GameObject.Instantiate(Tower, null, true);
                    InsTower.transform.position = hitInfo.point;
                    Debug.Log("shoot");
                }
            }

        }
    }

    private void OnMouseDown()
    {
        TowerOnMouse = true;
        //Debug.Log("Click");
    }
}
