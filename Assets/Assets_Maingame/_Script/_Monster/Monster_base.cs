using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Monster_base : MonoBehaviour, Monster_script {
    //Attributes of this monster
    public float fullHp;
    public float originalSpeed;
    public int reward;
    public AudioSource explo;

    //Current attributes of this monster
    public float speed;
    public float hp;
    float routePosition;

    //References of the controllers
    public GameObject player;
    public GameObject mapcontroller;
    public List<GameObject> towers;

    void Start () {
        routePosition = 0;
        hp = fullHp;

    }
    private void Update()
    {
    }

    // Update is called once per frame
    void FixedUpdate () {
        Move();
    }
















    //Copy the following functions without change
    public void OnMouseDown()
    {

    }
    public void Move(){
        MapController_script m = mapcontroller.GetComponent<MapController_script>();
        routePosition += speed * Time.deltaTime / 100;
        int currentRoutePositionInt = (int)Mathf.Floor(routePosition);
        int nextRoutePositionInt = currentRoutePositionInt + 1;


        if (nextRoutePositionInt >= m.route.Count)
        {


            foreach (GameObject tower in towers)
            {
                if (this.gameObject)
                {
                    tower.GetComponent<Tower_script>().GetMonsters().Remove(this.gameObject);
                }
            }
            Destroy(this.gameObject);
            player.GetComponent<PlayerController_script>().addCurrentHP(-1);
        }
        else
        {
            Vector3 current = m.GetMapPosition(m.route[currentRoutePositionInt].i, m.route[currentRoutePositionInt].j, 0.5f);
            Vector3 next = m.GetMapPosition(m.route[nextRoutePositionInt].i, m.route[nextRoutePositionInt].j, 0.5f);
            float dec = routePosition - currentRoutePositionInt;
            Vector3 newPosition = next * dec + current * (1 - dec);
            transform.Find("Model").LookAt(next);
            this.transform.position = newPosition;
        }

        if (hp.Equals(0))
        {
            //explo.Play();
            foreach (GameObject tower in towers)
            {
                tower.GetComponent<Tower_script>().GetMonsters().Remove(this.gameObject);
            }
            //Debug.Log("123");
            Destroy(this.gameObject);
            player.GetComponent<PlayerController_script>().addResource(reward);
        }
    }
    public void damage(float val){
        hp = hp - val;
        if(hp < 0){
            hp = 0;
        }
    }

    public void setPlayer(GameObject player) {
        this.player = player;
    }

    public void setMapContoller(GameObject mc)
    {
        this.mapcontroller = mc;
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
    public void addToTowers( GameObject tower)
    {
        towers.Add(tower);
    }

    public void removeFromTowers(GameObject tower)
    {
        towers.Remove(tower);
    }
    public void setSpeed(float newSpeed)
    {
        this.speed = newSpeed;
        //Debug.Log("after slow speed is");
        //Debug.Log(speed);
    }

    public float getSpeed()
    {
        return this.speed;
    }
}
