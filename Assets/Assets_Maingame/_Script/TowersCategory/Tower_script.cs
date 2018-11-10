using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Tower_script{

    void TowerUpgrade(int type);

    void TowerSell();

    void shoot(GameObject target);

    float getPrice();

    List<GameObject> GetMonsters();


}
