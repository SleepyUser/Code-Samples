using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUpdate : MonoBehaviour {

    [SerializeField]
    Text moneyText;
    [SerializeField]
    Text timeText;
    Color limeGreen = new Color(0, 255, 0);


	void Update () {
        moneyText.text = "$" + StaticValues.cityMoney + " / $" + StaticValues.goalMoney; //Updates the text in the moneyText box
        if (StaticValues.cityMoney < 0) //Red or black if balance is negative or not
        {
            moneyText.color = Color.red;
        }
        else
        {
            moneyText.color = limeGreen;
        }

        timeText.text = "Week: " + StaticValues.weekNo; //Update week number in timetext
    }
}
