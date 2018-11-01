using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textBtn : MonoBehaviour
{

    public Text mytext = null;
    public int counter = 0;
    public void changeText()
    {
        counter++;
        if (counter % 2 == 1)
        {
            mytext.text = "Pause";
        }
        else
        {
            mytext.text = "Start";
        }
    }
}