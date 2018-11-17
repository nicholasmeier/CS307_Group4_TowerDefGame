using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface Tower_script{

    void TowerUpgrade(int type);

    void Sell();

    void shoot(GameObject target);

    float getPrice();

    List<GameObject> GetMonsters();

    //Pass Controllers
    void SetPlayer(GameObject player);

    void SetMapController(GameObject mapController);

    void SetGrid(GameObject grid);

    GameObject Prefab();
}
