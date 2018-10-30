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
        if(other.CompareTag("Monster")){
            GetComponentInParent<Tower_script>().monsters.Add(other.gameObject);
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Monster")){
            GetComponentInParent<Tower_script>().monsters.Remove(other.gameObject);
        }
    }
}
