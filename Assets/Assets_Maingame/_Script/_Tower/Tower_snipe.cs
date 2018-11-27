﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower_snipe: MonoBehaviour, Tower_script {
    //attributes of tower
    public float projectileSpeed;
    public float price;
    public float coolDown;
    public AudioSource shootAud;
    public GameObject prefab;
    public float attack;
    public GameObject projectilePrefab;
    public float range;

    //references of the control system passed when created
    GameObject mapcontroller;
    GameObject player;
    GameObject baseGrid;

    //helper fields
    public GameObject target;
    public float lastShot;
    public List<GameObject> monsters;

    int TowerIndex;
    Material IceMaterial;
    //
    bool able_upgrade;
    string _name;

    void Start () {
        monsters.Clear();
        _name = "Snipe Tower";
        able_upgrade = true;
        target = null;
        lastShot = -coolDown;
        projectilePrefab.GetComponent<Projectile_script>().SetDamage(attack);
        transform.Find("Range").GetComponent<MonsterAdder>().SetRange(range);
        TowerIndex = 1;
    }

    public void TowerUpgrade()
    {
        if (TowerIndex == 1 && player.GetComponent<PlayerController_script>().getCurrentResource() >= 30)
        {
            //Debug.Log("Lost");
            attack *= 2;
            //this.gameObject.GetComponent<Renderer>().material = IceMaterial;
            player.GetComponent<PlayerController_script>().addCurrentResource(-30);
            TowerIndex = 2;
        }
        else if(TowerIndex == 2 && player.GetComponent<PlayerController_script>().getCurrentResource() >= 30)
        {
            transform.Find("Range").GetComponent<MonsterAdder>().SetRange(2*range);
            player.GetComponent<PlayerController_script>().addCurrentResource(-30);
            TowerIndex = 3;
            able_upgrade = false;
        }
    }
    

    public void shoot(GameObject t){
        if (Time.time > lastShot + coolDown && t != null)
        {
            Vector3 position = getRelativePosition(t, this.gameObject);
            //Rotate the tower
            this.gameObject.transform.LookAt(t.transform.position);
            this.gameObject.transform.Rotate(new Vector3(-90, 180, 0));

            shootAud.Play();
            //Debug.Log("Bang");
            GameObject bulletInstance;
            bulletInstance = Instantiate(projectilePrefab, this.transform.position, this.transform.rotation) as GameObject;
            bulletInstance.transform.LookAt(t.transform.position);
            bulletInstance.GetComponent<Rigidbody>().velocity = projectileSpeed * position.normalized;
            lastShot = Time.time;
        }
    }
















    //Copy these functions without change
    //
    public string getName()
    {
        return _name;
    }

    public bool can_be_upgrade()
    {
        return able_upgrade;
    }
    public void Sell()
    {
        MapController_script mc = mapcontroller.GetComponent<MapController_script>();
        player.GetComponent<PlayerController_script>().addCurrentResource(5);
        baseGrid.GetComponent<Grid_script>().availability = true;
        mc.SetAvailability(baseGrid, true);
        foreach (GameObject monster in mc.monsterHolder)
        {
            monster.GetComponent<Monster_script>().removeFromTowers(this.gameObject);
        }
        Destroy(this.gameObject);
        if (!mc.getInwave())
        {
            mc.UpdatePath();
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController_script ps = player.GetComponent<PlayerController_script>();
        if (ps.GetSelectionStatus() == SelectionStatus.towerSelected && ps.GetSelectedTower().Equals(this.gameObject))
        {
            transform.Find("Range").GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            transform.Find("Range").GetComponent<MeshRenderer>().enabled = false;
        }

        if (!monsters.Contains(target))
        {
            target = null;
        }


        //Debug.Log("Monster count= " + monsters.Count);
        if (target == null && monsters.Count > 0)
        {
            target = monsters[0];
        }

        shoot(target);
    }

    public float getAtk()
    {
        return this.attack;
    }
    public int getType()
    {
        return 2;
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
    public List<GameObject> GetMonsters(){
        return monsters;
    }

    public float getPrice()
    {
        return price;
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

    public GameObject Prefab(){
        return prefab;
    }
}

