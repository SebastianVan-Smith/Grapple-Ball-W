using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class ScoreManager : NetworkBehaviour
{
    [SyncVar]
    public int team0Score = 0;
    [SyncVar]
    public int team1Score = 0;
    [SerializeField]
    public Vector3[] startpos;
    private Rigidbody rb;
    private float wait;
    private Ballcontroller ballControl;
    private GameObject ball;
    System.Random rnd;
    // Start is called before the first frame update
    void Start()
    {
        rnd =  new System.Random();
        rb = GetComponent<Rigidbody>();
        ball = GameObject.FindGameObjectWithTag("Ball");
        ballControl = ball.GetComponent<Ballcontroller>();
    }

    // Update is called once per frame
    void Update()
    {
        wait += Time.deltaTime;
    }
    [Server]
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other);
        if (wait > 1)
        {


            if (other.gameObject.tag == ("Team0Hoop"))
            {
                team0Score++;
                if (ballControl.isHeld)
                {
                    ballControl.Dropscore(transform.position);
                }
                
                transform.position = startpos[rnd.Next(0, startpos.Length - 1)];
                rb.velocity = Vector3.zero;
                wait = 0;

                Debug.Log("Team0score");

            }
            if (other.gameObject.tag == ("Team1Hoop"))
            {
                team1Score++;
                if (ballControl.isHeld)
                {
                    ballControl.Dropscore(transform.position);
                }

                transform.position = startpos[rnd.Next(0, startpos.Length-1)];
                rb.velocity = Vector3.zero;
                wait = 0;
                
            }
        }
        //Debug.Log(team0Score);
        //Debug.Log(team1Score);
    }
    
}
