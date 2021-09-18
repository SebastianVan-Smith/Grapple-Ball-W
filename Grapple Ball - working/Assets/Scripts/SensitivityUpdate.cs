using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityUpdate : MonoBehaviour
{
    //Defines the object to be used in the script, this is done in the inspector
    private InputField senInput;

    //Variable to store reference to script
    private PlayerController playerC;

    public float temp = 5f;

    //This creates a static version of the script a gives us this instance
    private static SensitivityUpdate instance = null;
    public static SensitivityUpdate Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        //Get the AudioSource component from the object this script is attached to

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

    void Start()
    {
        //Gets the input field componenet from the defined input field object
        senInput = GameObject.FindGameObjectWithTag("InputField").GetComponent<InputField>();

        //Finds a component with the tag "player" and gets the PlayerController script  
        playerC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        playerC.lookSensitivity = temp;
    }

    //This function is called on the Input field object and will make the sensitivity from the player controller equal to the input float
    public void Text_Changed(string newText)
    {
        temp = float.Parse(newText);
    }
}
