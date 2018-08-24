using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAlternate : MonoBehaviour {
    [SerializeField]
    int timeDelay = 1;
    [SerializeField]
    string first;
    [SerializeField]
    string second;
    [SerializeField]
    Text text;

    float timePassed; 

    bool alternate = false;

	void Update () {
        timePassed += Time.deltaTime; //Timer
        if(timePassed > timeDelay) //If enough time has passed, switch the text
        {
            timePassed = 0;
            AlternateText();
        }
	}

    /// <summary>
    /// Switches the text between the two strings
    /// </summary>
    public void AlternateText()
    {
        if(alternate)
        {
            text.text = second;
        }
        else
        {
            text.text = first;
        }
        alternate = !alternate;
    }
}
