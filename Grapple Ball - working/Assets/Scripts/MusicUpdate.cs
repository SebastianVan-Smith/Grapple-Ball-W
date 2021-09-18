using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicUpdate : MonoBehaviour
{
    //Defines the object to be used in the script, this is done in the inspector
    public AudioSource music;

    //Variable to store Slider component
    private Slider musicSlider;

    // Start is called before the first frame update
    void Start()
    {
        //Finds a component with the tag "music" and gets the AudioSource script  
        music = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>();

        //Gets the slider component from the object this script is attached to
        musicSlider = GetComponent<UnityEngine.UI.Slider>();
    }

    //A funcion to be called in the inspector, this makes the music volume equal to the slider value "vol"
    public void SetVolume(float vol)
    {
        music.volume = vol;
    }
}
