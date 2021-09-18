using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
public class PlayerScoreUi : NetworkBehaviour
{
    public Text Score0;
    public Text Score1;
    public TMP_Text winner;
    private GameObject pp;
    public ScoreManager man;
    public int winscore;
    public Canvas ingame;
    public Canvas gameOver;
    private GameObject lmao;
    private NetworkRoomManager god;
    [Scene]
    public string RoomScene;
    // Start is called before the first frame update
    void Start()
    {
        pp = GameObject.FindGameObjectWithTag("Ball");
        man = pp.GetComponent<ScoreManager>();
        lmao = GameObject.FindGameObjectWithTag("Room");
        god = lmao.GetComponent<NetworkRoomManager>();
    }

    // Update is called once per frame
    void Update()
    {
       
        Score0.text = man.team0Score.ToString();
        Score1.text = man.team1Score.ToString();
        if (man.team1Score>=winscore || man.team0Score >= winscore)
        {
               
            if (man.team0Score > man.team1Score)
            {
                winner.text = "Blue Team Won";
                gameOver.gameObject.SetActive(true);
                ingame.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                winner.text = "Red Team Won";
                gameOver.gameObject.SetActive(true);
                ingame.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
    public void exittomenu()
    {
        god.OnRoomStopClient();
        //god.OnRoomStopHost();

        god.ServerChangeScene(RoomScene);
    }
}
