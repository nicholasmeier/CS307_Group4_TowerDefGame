using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile_script : MonoBehaviour {
    public float damage;

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
            Destroy(this.gameObject);
            other.gameObject.GetComponent<Monster_script>().damage(damage);
            //Debug.Log("bang");
        }
    }
}
