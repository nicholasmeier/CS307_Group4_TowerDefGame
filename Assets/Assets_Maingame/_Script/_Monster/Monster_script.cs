using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface Monster_script
{

    void damage(float val);

    void OnMouseDown();

    void setPlayer(GameObject player);

    void setMapContoller(GameObject mc);

    float getHp();

    float getFullHp();

    Vector3 getPos();

}
