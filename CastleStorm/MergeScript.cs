using UnityEngine;
using System.Collections;

public class MergeScript : MonoBehaviour
{
    public void ButtonMerge(int unit)
    {
        GameObject changeUnit = GameObject.FindGameObjectWithTag("Changing");

        switch (unit)
        {
            case 1: // Range
                changeUnit.GetComponent<UnitStats>().BaseToRange();

                if (TurnState.playerOneTurn)
                { UIController.playerOneResourceValue--; }
                else
                { UIController.playerTwoResourceValue--; }

                break;

            case 2: // Speed
                changeUnit.GetComponent<UnitStats>().BaseToSpeed();

                if (TurnState.playerOneTurn)
                { UIController.playerOneResourceValue--; }
                else
                { UIController.playerTwoResourceValue--; }

                break;

            case 3: // Health
                changeUnit.GetComponent<UnitStats>().BaseToHealth();

                if (TurnState.playerOneTurn)
                { UIController.playerOneResourceValue--; }
                else
                { UIController.playerTwoResourceValue--; }

                break;

            default:
                Debug.LogError("NO NUMBER SPECIFIED");
                break;
        }


        changeUnit.GetComponent<MovementScript>().transformed = true;
    }
}