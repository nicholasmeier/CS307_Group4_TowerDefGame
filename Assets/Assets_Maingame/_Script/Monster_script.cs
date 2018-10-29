using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Monster_script : MonoBehaviour {
    public float speed;
    public float hp;
    public float fullHp;
    //for route finding
    public float originalSpeed;
    private float routePosition;
    //for information display
    public int getgold;
    public GameObject player;
    public GameObject mapcontroller;
    //public AudioSource explo;

    private Rigidbody rb;
    // Use this for initialization
    void Start () {
        //rb = GetComponent<Rigidbody>();
        //rb.velocity = speed * new Vector3(-1, 0, 0);
        routePosition = 0;
        hp = fullHp;
        
    }
    
    // Update is called once per frame
    void FixedUpdate () {
        //move
        MapController_script m = mapcontroller.GetComponent<MapController_script>();
        routePosition += speed * Time.deltaTime / 100;
        int currentRoutePositionInt = (int)Mathf.Floor(routePosition);
        int nextRoutePositionInt = currentRoutePositionInt + 1;
        if (nextRoutePositionInt >= m.route.Count)
        {
            //This monster instance has reached the exit.
            Debug.Log("haha");
        }
        Vector3 current = m.GetMapPosition(m.route[currentRoutePositionInt].i, m.route[currentRoutePositionInt].j, 0.5f);
        Vector3 next = m.GetMapPosition(m.route[nextRoutePositionInt].i, m.route[nextRoutePositionInt].j, 0.5f);
        float dec = routePosition - currentRoutePositionInt;
        Vector3 newPosition = next * dec + current * (1 - dec);
        this.transform.position = newPosition;
        //Debug.Log(newPosition.y);
        if (hp.Equals(0)){
            //explo.Play();
            Destroy(this.gameObject);
            player.GetComponent<PlayerController_script>().addResource(getgold);
        }
    }



    public void damage(float val){
        hp = hp - val;
        if(hp < 0){
            hp = 0;
        }
        //Debug.Log("Current hp: " + hp);
    }
}
