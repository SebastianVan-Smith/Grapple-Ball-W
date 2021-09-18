using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Teams : NetworkBehaviour
{
    [SyncVar]
    public int team = 0;
    private int team0tot = 0;
    private int team1tot = 0;
    public Material[] material;
    public GameObject[] players;
    Renderer rend;
    bool cunt = false;
    private Teams tt;
    private float randtime;
    private float timesincespawn = 0;
    // Start is called before the first frame update
    [Client]
    void Start()
    {
        randtime = Random.Range(1f, 4f);
        rend = GetComponentInChildren<MeshRenderer>();
        rend.enabled = true;

        team = 0;


        Debug.Log(team);
        base.OnStartClient();
    }

    // Update is called once per frame
    void Update()
    {

        if (timesincespawn < randtime)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                Debug.Log(players.Length);
                tt = player.GetComponent<Teams>();
                if (tt.team == 0)
                {
                    team0tot++;
                }
                if (tt.team == 1)
                {
                    team1tot++;
                }
                if (team0tot > team1tot)
                {
                    team = 1;
                }
                else
                {
                    team = 0;
                }
            }
            timesincespawn += Time.deltaTime;
        }
        if (team == 0)
        {
            rend.sharedMaterial = material[0];
        }
        if (team == 1)
        {
            rend.sharedMaterial = material[1];
        }


    }
    public override void OnStartClient()
    {

    }
}