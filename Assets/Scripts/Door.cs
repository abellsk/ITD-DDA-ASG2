using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Rigidbody collide;

    // Start is called before the first frame update
    void Start()
    {
        collide.isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
