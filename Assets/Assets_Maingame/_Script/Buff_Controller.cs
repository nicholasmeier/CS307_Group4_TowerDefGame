using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Buff_Controller : MonoBehaviour {

    int count = 0;
    float fixedSpeed = 0;
    

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public void slowSpeedbyPercent(GameObject monster, float percent)
    {
        if (monster)
        {
                float updatedSpeed = monster.GetComponent<Monster_script>().getSpeed();
                if (count == 0)
                {
                    fixedSpeed = updatedSpeed;
                    count = 1;
                }
                //Debug.Log("time now is");
                //Debug.Log(fixedSpeed);
            if (updatedSpeed != fixedSpeed * percent)
            {
                monster.GetComponent<Monster_script>().setSpeed(updatedSpeed * percent);
            }
        }
    }

    public void backtoOriginalSpeed(GameObject monster)
    {
        float originalSpeed = monster.GetComponent<Monster_script>().getOriginSpeed();
        monster.GetComponent<Monster_script>().setSpeed(originalSpeed);
    }
}
