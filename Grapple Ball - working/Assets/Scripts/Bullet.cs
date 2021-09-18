using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Variables for the bullet
    public float bulletSpeed = 20f;
    public float maxDistance = 0f;

    // Update is called once per frame
    void Update()
    {
        //Makes the bullet move
        transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);

        //Starts a counter from when the bullet is created
        maxDistance += 1 * Time.deltaTime;

        //Once counter reaches 10 destroy this instance of the bullet
        if (maxDistance >= 10)
        {
            Destroy(this.gameObject);
        }
    }

    //Old damage code used for testing, may be helpful later

    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Hurt")
    //    {
    //        Destroy(gameObject);
    //    }
    //}

}

