using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbishCounter : MonoBehaviour
{
    static int Counter = 3;
    public GameObject CheckedBox;
   

    public void CounterRubbish()
    {
        Counter++;
        Debug.Log("Rubbish Counter : " + Counter);
        if (Counter == 3)
        {
            CheckedBox.SetActive(true);
            GameManager.instance.gameOver.SetActive(true);
        }
    }
}
