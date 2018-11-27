using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower_resource : MonoBehaviour, Tower_script
{
    //attributes of tower
    //public float projectileSpeed;
    public float price;
    public float coolDown;
    public AudioSource shootAud;
    public GameObject prefab;
    public float attack;
    //public GameObject projectilePrefab;
    //public float range;

    //references of the control system passed when created
    GameObject mapcontroller;
    GameObject player;
    GameObject baseGrid;

    //helper fields
    public GameObject target;
    public float lastShot;
    public List<GameObject> monsters;


    int TowerIndex;
    int TowerType;
    Material IceMaterial;
    string _name;
    bool able_upgrade;

    void Start()
    {

        monsters.Clear();
        _name = "Resource Tower";
        able_upgrade = false;
        target = null;
        lastShot = -coolDown;
        //projectilePrefab.GetComponent<projectile_script>().SetDamage(attack);
        //transform.Find("Range").GetComponent<MonsterAdder>().SetRange(range);
        TowerIndex = 1;
        TowerType = 1;
    }

    public void TowerUpgrade()
    {
        /*
        if (TowerIndex == 1)
        {
            //Debug.Log("Lost");
            attack *= 2;
            //this.gameObject.GetComponent<Renderer>().material = IceMaterial;
            player.GetComponent<PlayerController_script>().addCurrentResource(-30);
            TowerIndex = 2;
        }
        else if (TowerIndex == 2)
        {
            transform.Find("Range").GetComponent<MonsterAdder>().SetRange(2 * range);
            player.GetComponent<PlayerController_script>().addCurrentResource(-30);
            TowerIndex = 3;
        }
        */
    }
    //
    


    public void shoot(GameObject t)
    {

        if (Time.time > lastShot + coolDown)
        {

            player.GetComponent<PlayerController_script>().addResource(10);
            shootAud.Play();
            lastShot = Time.time;

        }
    }

















    //Copy these functions without change
    public string getName()
    {
        return _name;
    }

    public bool can_be_upgrade()
    {
        return able_upgrade;
    }
    public float getAtk()
    {
        return this.attack;
    }
    public int getType()
    {
        return TowerType;
    }
    public void Sell()
    {
        player.GetComponent<PlayerController_script>().addCurrentResource(5);
        baseGrid.GetComponent<Grid_script>().availability = true;
        mapcontroller.GetComponent<MapController_script>().SetAvailability(baseGrid, true);
        foreach (GameObject monster in monsters)
        {

            monster.GetComponent<Monster_script>().removeFromTowers(this.gameObject);
        }
        monsters.Clear();
        Destroy(this.gameObject);
        mapcontroller.GetComponent<MapController_script>().UpdatePath();
    }

    // Update is called once per frame
    void Update()
    {
        shoot(null);
    }
    public void OnMouseDown()
    {
        PlayerController_script ps = player.GetComponent<PlayerController_script>();
        ps.SetSelectionStatus(SelectionStatus.towerSelected);
        ps.SetSelectedTower(this.gameObject);
    }

    private Vector3 getRelativePosition(GameObject a, GameObject b)
    {
        return a.transform.position - b.transform.position;
    }
    public List<GameObject> GetMonsters()
    {
        return monsters;
    }

    public float getPrice()
    {
        return price;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public void SetMapController(GameObject mapController)
    {
        this.mapcontroller = mapController;
    }

    public void SetGrid(GameObject grid)
    {
        this.baseGrid = grid;
    }

    public GameObject Prefab()
    {
        return prefab;
    }

}


