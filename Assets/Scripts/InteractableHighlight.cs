using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableHighlight : MonoBehaviour
{
    
    public void OnHover()
    {

        // Look through all children and store their mesh renderers in the array
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        // Iterate through the mest renderer array
        foreach(MeshRenderer renderer in meshRenderers)
        {
            //For every renderer in the array, enable emission
            renderer.material.EnableKeyword("_EMISSION");
        }
    }

    public void ExitHover()
    {
        // Look through all children and store their mesh renderers in the array
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        // Iterate through the mest renderer array
        foreach (Renderer renderer in meshRenderers)
        {
            //For every renderer in the array, disable emission
            renderer.material.DisableKeyword("_EMISSION");
        }
    }
}
