using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Monster_base : MonoBehaviour, Monster_script {
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
    //for bottom infor bar
    private bool display_flag;
    public Text bot_hp_display;
    public Text bot_type_display;
    public Button sell;
    public Button upgrade;
    //public AudioSource explo;

    private Rigidbody rb;
    // Use this for initialization
    void Start () {
        //rb = GetComponent<Rigidbody>();
        //rb.velocity = speed * new Vector3(-1, 0, 0);
        routePosition = 0;
        hp = fullHp;
        display_flag = false;


    }
    private void Update()
    {
       
            if (display_flag)
            {
                bot_type_display.text = "Type: " + this.tag.ToString();
                bot_hp_display.text = "HP: " + hp.ToString();
                sell.gameObject.SetActive(false);
                upgrade.gameObject.SetActive(false);
            }
        
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
        
        if (nextRoutePositionInt >= m.route.Count)
        {
            Destroy(this.gameObject);
            player.GetComponent<PlayerController_script>().addCurrentHP(-1);
        }
        else
        {
            Vector3 current = m.GetMapPosition(m.route[currentRoutePositionInt].i, m.route[currentRoutePositionInt].j, 0.5f);
            Vector3 next = m.GetMapPosition(m.route[nextRoutePositionInt].i, m.route[nextRoutePositionInt].j, 0.5f);
            float dec = routePosition - currentRoutePositionInt;
            Vector3 newPosition = next * dec + current * (1 - dec);
            this.transform.position = newPosition;
        }
        /*
        float dec = routePosition - currentRoutePositionInt;
        Vector3 newPosition = next * dec + current * (1 - dec);
        this.transform.position = newPosition;
        */
        //Debug.Log(newPosition.y);
        
        if (hp.Equals(0)){
            //explo.Play();
            Destroy(this.gameObject);
            player.GetComponent<PlayerController_script>().addResource(getgold);
            bot_type_display.text = "";
            bot_hp_display.text = "";
        }
    }

    public void OnMouseDown()
    {
        display_flag = true;

    }


    public void damage(float val){
        hp = hp - val;
        if(hp < 0){
            hp = 0;
        }
        //Debug.Log("Current hp: " + hp);
    }

    public void setPlayer(GameObject player) {
        this.player = player;
    }

    public void setMapContoller(GameObject mc)
    {
        this.mapcontroller = mc;
    }

    public void setHptext(Text hp) {
        this.bot_hp_display = hp;
    }

    public void setTypetext(Text type) {
        this.bot_type_display = type;
    }
    public void setSellButton(Button sell) {
        this.sell = sell;
    }

    public void setUpgradeButton(Button upgrade) {
        this.upgrade = upgrade;
    }

    public float getHp() {
        return this.hp;
    }

    public float getFullHp() {
        return this.fullHp;
    }

    public Vector3 getPos() {
        return this.transform.position;
    }
}
