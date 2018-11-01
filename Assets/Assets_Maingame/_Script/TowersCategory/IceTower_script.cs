using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceTower_script : MonoBehaviour
{
    //public float range;
    public float coolDown;
    public GameObject projectilePrefab;
    //for prototyping
    public GameObject monster;
    public float projectileSpeed;
    public float gold;
    public List<GameObject> monsters;
    public AudioSource shootAud;
    //public MapController
    //for bottom display information
    public Button sell;
    public Button upgrade;
    public Text bot_atk_display;
    public Text bot_type_display;
    private GameObject target;
    private float lastShot;
    private bool display_flag;
    private float attack;
    //counter for set bool mouse

    public GameObject UpgradedTower;
    //Tower Types


    // Use this for initialization
    void Start()
    {
        monsters.Clear();
        target = null;
        lastShot = -coolDown;
        display_flag = false;
        attack = projectilePrefab.GetComponent<projectile_script>().damage;

    }

    public void TowerUpgrade(int TowerTypeIndex)
    {
        Vector3 OriginPosition = this.gameObject.transform.position;
        Destroy(this.gameObject);
        /*if (TowerTypeIndex == 1)
        {

            //Debug.Log("Lost");
            UpgradedTower = (GameObject)Instantiate(IceTower, null, true);
            UpgradedTower.transform.position = OriginPosition;

        }*/
    }

    // Update is called once per frame
    void Update()
    {
        //determine the target
        if (display_flag)
        {
            bot_type_display.text = "Type: " + this.tag.ToString();
            bot_atk_display.text = "ATK: " + attack.ToString();
            sell.gameObject.SetActive(true);
            upgrade.gameObject.SetActive(true);

        }




        if (target != null && target.GetComponent<Monster_script>().hp.Equals(0))
        {
            monsters.Remove(target);
            target = null;
        }

        //Debug.Log("Monster count= " + monsters.Count);
        if (target == null && monsters.Count > 0)
        {
            target = monsters[0];
        }

        shoot(target);
    }
    public void OnMouseDown()
    {
        display_flag = true;
    }

    private Vector3 getRelativePosition(GameObject a, GameObject b)
    {
        return a.transform.position - b.transform.position;
    }

    public float getPrice()
    {
        return gold;
    }

    private void shoot(GameObject t)
    {
        if (Time.time > lastShot + coolDown && t != null)
        {
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
}

