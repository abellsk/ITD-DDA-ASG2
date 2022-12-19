using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeCounter : MonoBehaviour
{
    static int Counter = 0;
    public GameObject CheckedBox;

    public void CounterCoffee()
    {
        Counter++;
        if (Counter == 1)
        {
            CheckedBox.SetActive(true);
        }
    }
}
