using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch 
{

    public Light lightToToggle;

    public void ToggleLight()
    {
        lightToToggle.enabled = !lightToToggle.enabled;
    }
}
