using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnOff : MonoBehaviour
{

    public Light Lights;


    // Start is called before the first frame update
    void Start()
    {
        Lights = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        Lights.enabled = !Lights.enabled;
    }
}
