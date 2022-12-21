using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairCounter : MonoBehaviour
{
    static int Counter = 0;
    public GameObject CheckedBox;
    


    public void CounterChair()
    {
        Counter++;
        Debug.Log("Chair Counter : " + Counter);
        if (Counter == 11)
        {
            CheckedBox.SetActive(true);
            GameManager.instance.TaskTracker();
        }
    } 

    
}
