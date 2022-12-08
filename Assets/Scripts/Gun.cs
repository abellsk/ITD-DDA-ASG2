using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public Transform bulletOrigin;

    public GameObject bulletPrefab;


    public void Fire()
    {

        // Spawn a new bullet at the position of bulletOrigin
        GameObject newBullet = Instantiate(bulletPrefab, bulletOrigin.position, transform.rotation);


        newBullet.GetComponent<Rigidbody>().AddForce(bulletOrigin.forward * 50f);
    }



}
