using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutChecker : MonoBehaviour
{
    public GameObject SetDonut;
    public GameObject Donut;

    public void DeleteDonut()
    {
        Destroy(Donut);
        SetDonut.SetActive(true);
    }
}
