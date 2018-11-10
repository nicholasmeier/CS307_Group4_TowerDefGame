using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower_base: MonoBehaviour, Tower_script {
    //public float range;
    //for prototyping
    //public GameObject monster;
    //public MapController
    //for bottom display information
    public float projectileSpeed;
    public float gold;
    public float coolDown; 
    GameObject target;
    float lastShot;

    public List<GameObject> monsters;
    public AudioSource shootAud;
    public GameObject mapcontroller;
    public GameObject projectilePrefab;
    public GameObject player;
    public Button sell;
    public Button upgrade;
    public Text bot_atk_display;
    public Text bot_type_display;
    bool display_flag;

    float attack;
    //counter for set bool mouse

    public GameObject baseGrid;
    int TowerIndex;
    public Material IceMaterial;
    //Tower Types

    // Use this for initialization
    void Start () {
        monsters.Clear();
        target = null;
        lastShot = -coolDown;
        display_flag = false;
        attack = projectilePrefab.GetComponent<projectile_script>().damage;

        upgrade.onClick.AddListener(delegate { TowerUpgrade(1); });
        sell.onClick.AddListener(TowerSell);
    }

    public void TowerUpgrade(int TowerTypeIndex)
    {
        if (TowerTypeIndex == 1)
        {

            //Debug.Log("Lost");
            projectilePrefab.GetComponent<projectile_script>().damage = 20000;
            this.gameObject.GetComponent<Renderer>().material = IceMaterial;
            player.GetComponent<PlayerController_script>().addCurrentResource(-30);
            

        }
    }
    
    public void TowerSell()
    { 
        player.GetComponent<PlayerController_script>().addCurrentResource(5);
        baseGrid.GetComponent<Grid_script>().availability = true;
        mapcontroller.GetComponent<MapController_script>().SetAvailability(baseGrid, true);
        Destroy(this.gameObject);
        mapcontroller.GetComponent<MapController_script>().UpdatePath();
    }

    // Update is called once per frame
    void Update()
    {
        //determine the target
        if (display_flag)
        {
            attack = projectilePrefab.GetComponent<projectile_script>().damage;
            bot_type_display.text = "Type: " + this.tag.ToString();
            bot_atk_display.text = "ATK: " + attack.ToString();
            sell.gameObject.SetActive(true);
            upgrade.gameObject.SetActive(true);
            this.transform.Find("Cylinder").GetComponent<MeshRenderer>().enabled = true;

        }



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
    public void OnMouseDown()
    {
        display_flag = true;
    }

    private Vector3 getRelativePosition(GameObject a, GameObject b){
        return a.transform.position - b.transform.position;
    }

    public float getPrice() {
        return gold;
    }

    public void shoot(GameObject t){
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

    //Getters
    public List<GameObject> GetMonsters(){
        return monsters;
    }

    public Text GetBot_atk_display(){
        return bot_atk_display;
    }

    public void SetBot_atk_display(Text bot_atk_display){
        this.bot_atk_display = bot_atk_display;
    }

    public Text GetBot_type_display(){
        return bot_type_display;
    }

    public void SetBot_type_display(Text bot_type_display){
        this.bot_type_display = bot_type_display;
    }

    public void SetSell(Button sell){
        this.sell = sell;
    }

    public void SetUpgrade(Button upgrade){
        this.upgrade = upgrade;
    }

    public void SetPlayer(GameObject player){
        this.player = player;
    }

    public void SetMapController(GameObject mapController){
        this.mapcontroller = mapController;
    }

    public void SetGrid(GameObject grid){
        this.baseGrid = grid;
    }
    /*public void slowDown(int speed)
    {

    }*/
}

