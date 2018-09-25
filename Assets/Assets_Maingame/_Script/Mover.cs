using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
    public float speed;

    private Rigidbody rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.velocity = speed * new Vector3(-1, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
