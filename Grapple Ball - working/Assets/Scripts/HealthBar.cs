using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour 
{
    //Defines the object to be used in the script, this is done in the inspector
    public UnityEngine.UI.Image barImage;

    private void Awake()
    {
        //Gets the image component from the object
        barImage = GetComponent<UnityEngine.UI.Image>();
    }

    //Function that will update the bar fill amount, by a float input when the function is called
    public void HandleHealthChanged(float pct)
    {
        barImage.fillAmount = pct;
    }

}
