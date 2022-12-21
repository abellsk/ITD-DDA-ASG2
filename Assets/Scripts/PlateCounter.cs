using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounter : MonoBehaviour
{
    static int Counter = 0;
    public GameObject CheckedBox;

    public void CounterPlate()
    {
        Counter++;
        if (Counter == 1)
        {
            CheckedBox.SetActive(true);
            GameManager.instance.TaskTracker();
            
        }
    }
}
