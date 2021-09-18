using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class CameraMultiplayer : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
   
    }
    public override void OnStartClient()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (!hasAuthority)
        {
            //I also disabled this component so it also doesn't move the other player

            var camera = transform.Find("Camera");
           
            var canvas = transform.Find("Canvas");
            camera.GetComponent<Camera>().enabled = false;
            camera.GetComponent<AudioListener>().enabled = false;
            canvas.GetComponent<Canvas>().enabled = false;
        }
       
   }
}
