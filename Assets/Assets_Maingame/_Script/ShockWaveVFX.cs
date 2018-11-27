using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveVFX : MonoBehaviour {
    public float aoe;
    float lastAnimated;
    float animateRate = 0.05f;
    int counter;
	// Use this for initialization
	void Start () {
        counter = 0;
        lastAnimated = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > lastAnimated + animateRate && counter < 20)
        {
            gameObject.transform.localScale += new Vector3(aoe / 20, aoe / 20, aoe / 20);
            lastAnimated = Time.time;
            counter++;
        }
        if(counter == 20){
            Destroy(this.gameObject);
        }
    }

    public void SetAoe(float aoe){
        this.aoe = aoe;
    }
}
