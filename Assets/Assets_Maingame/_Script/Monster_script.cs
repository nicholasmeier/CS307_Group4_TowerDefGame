using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Monster_script : MonoBehaviour {
    public float speed;
    public float hp;
    public float fullHp;
    public int getgold;
    public GameObject player;
    //public AudioSource explo;

    private Rigidbody rb;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        rb.velocity = speed * new Vector3(-1, 0, 0);
        hp = fullHp;
        
    }
    
    // Update is called once per frame
    void FixedUpdate () {
        if(hp.Equals(0)){
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
        Debug.Log("Current hp: " + hp);
    }
}
