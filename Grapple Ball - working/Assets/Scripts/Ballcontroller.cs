using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Ballcontroller : NetworkBehaviour
{
    
    [SyncVar]
    public float heldFor=0;
    [SyncVar]
    public bool isHeld = false;
    [SyncVar]
    public GameObject heldBy=null;
    private Vector3 offset = new Vector3(0, 1, 0);

    //while held variables
    [SerializeField]
    private Collider sphererCol;
    private Rigidbody rb;
    private PlayerBallController ballcont;
    [SyncVar]
    private float delay;
    [SyncVar]
    private NetworkIdentity networkid;
    
    // Start is called before the first frame update

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        
        if (!isHeld)
        {
            delay += Time.deltaTime;
            heldFor = 0;
            WhileNotHeld();
        }
        else
        {
            heldFor += Time.deltaTime;
            delay = 0;
            WhileHeld();
        }

        //Debug.Log(rb.velocity);

    }

    public void Pickup(GameObject holder,NetworkIdentity nett)
    {
        if (delay > 0.3)
        {
            if (!isHeld)
            {
                networkid = nett;
                sphererCol.enabled = false;
                transform.position = holder.transform.position + offset;
                
                heldBy = holder;
               // ballcont = heldBy.GetComponent<PlayerBallController>();
               // ballcont.pickupp(netIdentity);
                isHeld = true;
                
            }
          
            else if (heldFor > 1)
            {
                
                ballcont.droppp(netIdentity);
                sphererCol.enabled = false;
                transform.position = holder.transform.position + offset;
                
                heldBy = holder;
               // ballcont = heldBy.GetComponent<PlayerBallController>();
               // ballcont.pickupp(netIdentity);
                isHeld = true;
                networkid = nett;


            }
        }
        
        
    }
    private void WhileHeld()
    {
        sphererCol.enabled = false;
        transform.position = heldBy.transform.position + offset;
        Vector3 tempvel = heldBy.GetComponent<Rigidbody>().velocity;
        rb.velocity = tempvel;

    }
    private void WhileNotHeld()
    {
      
            sphererCol.enabled = true;
        
      
        
        
    }
    //[Command]
    public void Drop(Vector3 direction, Vector3 pos, NetworkIdentity NetworkConn)
    {
        Debug.Log("dropping");
        if (isHeld && (networkid == NetworkConn))
        {
            Debug.Log("dropping2");
            // ballcont.droppp(netIdentity);
            networkid = null;
            //direction = new Vector3(0, 10, 0);           

            Vector3 tempvel = heldBy.GetComponent<Rigidbody>().velocity;
            // ballcont = heldBy.GetComponent<PlayerBallController>();
            isHeld = false;
            heldBy = null;
            // ballcont.droppp(netIdentity);
        

            Debug.DrawLine(pos, pos + (Vector3.up * 2.5f), new Color(0, 0, 0, 1), 10000);
            transform.position = pos + ((Vector3.up + new Vector3(direction.x, 0, direction.z) * -1).normalized * 2.5f);
            rb.velocity = tempvel;
            rb.AddForce(Vector3.up * 2.5f, ForceMode.Impulse);

        }

    }
    [Command(ignoreAuthority =true)]
    public void Throw(Vector3 direction, Vector3 pos,NetworkIdentity NetworkConn)
    {
        if (isHeld && (networkid == NetworkConn))
        {
            networkid = null;
            //direction = new Vector3(0, 10, 0);           
           
            Vector3 tempvel = heldBy.GetComponent<Rigidbody>().velocity;
           // ballcont = heldBy.GetComponent<PlayerBallController>();
            isHeld = false;
            heldBy = null;
            // ballcont.droppp(netIdentity);
          

            Debug.DrawLine(pos, pos + (direction.normalized * 2.5f), new Color(0, 0, 0, 1), 10000);
            transform.position = pos + (direction.normalized * 2.5f);
            rb.velocity = tempvel;
            rb.AddForce(direction.normalized * 25f, ForceMode.Impulse);
           
          
        }

    }
    public void Dropscore(Vector3 pos)
    {
        // ballcont.droppp(netIdentity);
        networkid = null;
            //direction = new Vector3(0, 10, 0);           

            Vector3 tempvel = Vector3.up;
            // ballcont = heldBy.GetComponent<PlayerBallController>();
            isHeld = false;
            heldBy = null;
            // ballcont.droppp(netIdentity);


            Debug.DrawLine(pos, pos + (Vector3.up * 2.5f), new Color(0, 0, 0, 1), 10000);
            transform.position = pos + (Vector3.up * 2.5f);
            rb.velocity = tempvel;
            rb.AddForce(Vector3.up * 2.5f, ForceMode.Impulse);

        

    }
    public void Dropscoree( NetworkIdentity NetworkConn, Vector3 direction)
    {
        if (isHeld && (networkid != NetworkConn))
        {
            // ballcont.droppp(netIdentity);
            networkid = null;
            //direction = new Vector3(0, 10, 0);           

            Vector3 tempvel = Vector3.up;
            // ballcont = heldBy.GetComponent<PlayerBallController>();
            isHeld = false;
            heldBy = null;
            // ballcont.droppp(netIdentity);


            Debug.DrawLine(transform.position, transform.position + (Vector3.up * 2.5f), new Color(0, 0, 0, 1), 10000);
            transform.position = transform.position + ((Vector3.up + new Vector3(direction.x, 0, direction.z) * -1).normalized * 2.5f);
            rb.velocity = tempvel;
            rb.AddForce(Vector3.up * 2.5f, ForceMode.Impulse);

        }

    }
}
