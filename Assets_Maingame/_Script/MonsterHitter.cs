using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitter : MonoBehaviour {
    public float range;
    public float coolDown; 
    public Rigidbody projectilePrefab;
    //for prototyping
    public GameObject monster;
    public float projectileSpeed;


    //public MapController 
    private GameObject target;
    private float lastShot; 
	// Use this for initialization
    void Start () {
        target = null;
        lastShot = -coolDown;
	}

    // Update is called once per frame
    void Update()
    {
        Vector3 position = getRelativePosition(monster, this.gameObject);
        
        if (position.magnitude <= range)
        {
            target = monster;
        }
        else
        {
            target = null;
        }
        shoot(target);
    }

    private Vector3 getRelativePosition(GameObject a, GameObject b){
        return a.transform.position - b.transform.position;
    }

    private void shoot(GameObject t){
        if(Time.time > lastShot + coolDown && t != null){
            Vector3 position = getRelativePosition(t, this.gameObject);
            Debug.Log("Bang");
            Rigidbody bulletInstance;
            bulletInstance = Instantiate(projectilePrefab, this.transform.position, this.transform.rotation) as Rigidbody;
            bulletInstance.velocity = projectileSpeed * position.normalized;
            lastShot = Time.time;
        }
    }
}

