using UnityEngine;
using System.Collections;

public class zhizhu : MonoBehaviour {
	private float h;
	private float v;
	public float speed=0.05f;
	//private Animation ani;
	// Use this for initialization
	void Start () {
		//ani=transform.GetComponent<"Animation">();
	}
	
	// Update is called once per frame
	void Update () {
		h = Input.GetAxis ("Horizontal")*speed;
		v = Input.GetAxis ("Vertical")*speed;
		if (Input.GetKeyDown ("a") || Input.GetKeyDown ("d") || Input.GetKeyDown ("w") || Input.GetKeyDown ("s")) {	transform.GetComponent<Animation>().Play("Run");}
		if (Input.GetKeyUp ("a") || Input.GetKeyUp ("d") || Input.GetKeyUp ("w") || Input.GetKeyUp ("s")) {	transform.GetComponent<Animation>().Play("Idle");}
		/*if (h != 0 || v != 0) {
			transform.animation.Play("Run");
				} else {
					transform.animation.Play("Idle");
		}*/
		if (Input.GetKeyDown ("p")) {
			transform.GetComponent<Animation>().Play ("Attack");
		} else {
			//transform.animation.Play ("Attack");
		}
		transform.Translate (h,0,v);

	
	}
}
