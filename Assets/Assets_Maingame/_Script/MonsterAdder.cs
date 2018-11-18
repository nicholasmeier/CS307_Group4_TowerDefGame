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
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Monster") || other.CompareTag("Boss"))
        {
            GetComponentInParent<Tower_script>().GetMonsters().Remove(other.gameObject);
            other.GetComponent<Monster_script>().removeFromTowers(transform.parent.gameObject);
        }
    }

    public void SetRange(float range){
        transform.localScale = new Vector3(2 * range, 1, 2 * range);
    }
}
