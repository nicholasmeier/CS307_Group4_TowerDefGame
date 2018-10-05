using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_script: MonoBehaviour {
    //public float range;
    public float coolDown; 
    public GameObject projectilePrefab;
    //for prototyping
    public GameObject monster;
    public float projectileSpeed;
    public List<GameObject> monsters;
    public AudioSource shootAud;
    //public MapController 

    private GameObject target;
    private float lastShot; 
	// Use this for initialization
    void Start () {
        monsters.Clear();
        target = null;
        lastShot = -coolDown;
	}

    // Update is called once per frame
    void Update()
    {
        //determine the target
        if (target != null && target.GetComponent<Monster_script>().hp.Equals(0))
        {
            monsters.Remove(target);
            target = null;
        }

        //Debug.Log("Monster count= " + monsters.Count);
        if(target == null && monsters.Count > 0){
            target = monsters[0];
        }

        shoot(target);
    }

    private Vector3 getRelativePosition(GameObject a, GameObject b){
        return a.transform.position - b.transform.position;
    }

    private void shoot(GameObject t){
        if(Time.time > lastShot + coolDown && t != null){
            Vector3 position = getRelativePosition(t, this.gameObject);
            //Rotate the tower
            this.gameObject.transform.LookAt(t.transform.position);
            shootAud.Play();
            //Debug.Log("Bang");
            GameObject bulletInstance;
            bulletInstance = Instantiate(projectilePrefab, this.transform.position, this.transform.rotation) as GameObject;
            bulletInstance.transform.LookAt(t.transform.position);
            bulletInstance.GetComponent<Rigidbody>().velocity = projectileSpeed * position.normalized;
            lastShot = Time.time;
        }
    }
}

