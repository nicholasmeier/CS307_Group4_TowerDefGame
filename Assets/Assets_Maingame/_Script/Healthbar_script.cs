﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar_script : MonoBehaviour
{


    public Vector3 offset;
    public Vector3 scale;
    Monster_script monster; 
    // Use this for initialization
    void Start()
    {
        monster = GetComponentInParent<Monster_script>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        //Destroy in parent class.
        //set length
        this.transform.localScale = new Vector3(monster.hp / monster.fullHp, scale.y, scale.z);
        //set left alignment
        float moveLeft = (monster.fullHp - monster.hp) / monster.fullHp / 2;
        offset = new Vector3(-moveLeft, offset.y, offset.z);

        this.transform.position = monster.transform.position + offset;
    }
}
