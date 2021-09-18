using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    //Variable to store AudioSource component
    private AudioSource music;

    //this creates a static version of the script a gives us this instance
    private static Music instance = null;
    public static Music Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        //Get the AudioSource component from the object this script is attached to
        music = GetComponent<AudioSource>();

        //This checks if another instance of the script exists and destroys itself if its not the first instance
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        //When loading between scenes this object will not be destroyed
        DontDestroyOnLoad(this.gameObject);
    }
}
