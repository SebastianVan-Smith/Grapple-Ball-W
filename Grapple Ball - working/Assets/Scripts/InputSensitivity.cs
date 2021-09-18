using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputSensitivity : MonoBehaviour
{
    //Defines the object to be used in the script, this is done in the inspector
    private InputField SenInput;

    //Variable to store reference to script
    private PlayerController playerC;

    // Start is called before the first frame update
    void Start()
    {
        //Gets the input field componenet from the defined input field object
        SenInput = GetComponent<InputField>();

        //Finds a component with the tag "player" and gets the PlayerController script  
        playerC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    //This function is called on the Input field object and will make the sensitivity from the player controller equal to the input float
    public void Text_Changed(string newText)
    {
        float temp = float.Parse(newText);
        playerC.lookSensitivity = temp;
    }

}
