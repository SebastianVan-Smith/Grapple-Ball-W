using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
public class PlayerBallController : NetworkBehaviour
{
    private Ballcontroller ballControl;
    private GameObject ball;
    public LayerMask ballLayer;
    private PlayerMotor playerMotor;
    public GameObject coll;

    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        ballControl = ball.GetComponent<Ballcontroller>();
        playerMotor = this.GetComponent<PlayerMotor>();
        //ballControl.Pickup(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        input();
    }
 
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("ball"))
        {
            if (this.gameObject!=coll)
            {
                var bob = GetComponent<NetworkIdentity>();
                ballControl.Pickup(this.gameObject, bob);

            }
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("charater"))
        {
            Debug.Log("pp");
            var bob = GetComponent<NetworkIdentity>();
            ballControl.Dropscoree(bob, playerMotor.cam_forward);
        }
    }


    private void input()
    {
        if (hasAuthority)
        {
            if (Input.GetKeyDown("q"))
            {
                var bob = GetComponent<NetworkIdentity>();
                ballControl.Drop(playerMotor.cam_forward, transform.position, bob);



            }
            if (Input.GetButton("Fire2"))
            {
                var bob = GetComponent<NetworkIdentity>();
                ballControl.Throw(playerMotor.cam_forward, transform.position,bob);
                



            }
        }
        
        
    }
    
    public void pickupp(NetworkIdentity item)
    {
        item.AssignClientAuthority(connectionToClient);
    }
  
    public void droppp(NetworkIdentity item)
    {
        item.RemoveClientAuthority();
    }
}
