using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface Tower_script{

    void TowerUpgrade(int type);

    void TowerSell();

    void shoot(GameObject target);

    float getPrice();

    List<GameObject> GetMonsters();

    //To be deleted
    Text GetBot_atk_display();

    Text GetBot_type_display();

    void SetBot_atk_display(Text bot_atk_display);

    void SetBot_type_display(Text bot_type_display);

    void SetSell(Button sell);

    void SetUpgrade(Button upgrade);


    //Pass Controllers
    void SetPlayer(GameObject player);

    void SetMapController(GameObject mapController);

    void SetGrid(GameObject grid);

    GameObject Prefab();
}
