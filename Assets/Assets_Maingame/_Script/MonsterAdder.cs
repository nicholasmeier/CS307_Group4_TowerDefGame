using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAdder : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster") || other.CompareTag("Boss"))
        {
            GetComponentInParent<Tower_script>().GetMonsters().Add(other.gameObject);
            other.GetComponent<Monster_script>().addToTowers(transform.parent.gameObject);
            //Debug.Log(GetComponentInParent<Tower_script>().getType());
            if (GetComponentInParent<Tower_script>().getType()==2)
            {
                //Debug.Log("get slow tower");
                foreach (GameObject ob in GetComponentInParent<Tower_script>().GetMonsters())
                {
                    
                    GameObject.Find("Buff_controller").GetComponent<Buff_Controller>().slowSpeedbyPercent(ob, 0.5F);
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Monster") || other.CompareTag("Boss"))
        {
            GetComponentInParent<Tower_script>().GetMonsters().Remove(other.gameObject);
            other.GetComponent<Monster_script>().removeFromTowers(transform.parent.gameObject);
            if (GetComponentInParent<Tower_script>().getType()==2)
            {
                    GameObject.Find("Buff_controller").GetComponent<Buff_Controller>().backtoOriginalSpeed(other.gameObject);
            }
        }
    }

    public void SetRange(float range){
        transform.localScale = new Vector3(2 * range / transform.parent.localScale.x, 1 / transform.parent.localScale.y, 2 * range / transform.parent.localScale.z);
    }
}
