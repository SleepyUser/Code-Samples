using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpawnScript : MonoBehaviour
{
    public GameObject baseUnit;
    int playerTurn;
    int hexLayer = 0;
    int spawnNumber = 4;

    public void Awake() //Things to only be done once
    {
        var layer = 1 << 8;
        hexLayer = layer;
    }

    public void OnEnable() //Things to be done each time the spawn phase takes place
    {
        spawnNumber = 4; 
        EnableSpawn();
    }

    public void EnableSpawn()
    {
        if (TurnState.playerOneTurn == true) //Which turn is it?
        {
            playerTurn = 0;
        }
        else
        {
            playerTurn = 1;
        }

        for (int i = 0; i < gameObject.GetComponent<HexSpawner>().width; i++) //For each hex in the outermost rows
        {
            if ((HexSpawner.spawnRows[playerTurn, i].GetComponent<HexStats>().occupier == null)) //If the hex doesn't contain anything
            {
                HexSpawner.spawnRows[playerTurn, i].GetComponent<HexStats>().SelectHex(); //Make the hex selectable
            }
        }
    }

    public void OnDisable()
    {
        HexSpawner.spawnRows[playerTurn, 0].GetComponent<HexStats>().DeselectAll(); //Clear up all selectable tiles
    }

    public void Update()
    {
        Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit clickedObj;

        if (Input.GetMouseButtonUp(0) && Physics.Raycast(clickRay, out clickedObj, hexLayer))
        {
            if (clickedObj.transform.tag == "sHex" &&  spawnNumber > 0) //If the clicked object is a selectable hex, and the player has resources left
            {
                if (TurnState.playerOneTurn == true && UIController.playerOneResourceValue > 0) //If it is player one's turn and they have resources left
                {
                    Instantiate(baseUnit, new Vector3(clickedObj.transform.position.x, 1, clickedObj.transform.position.z), Quaternion.Euler(0, 0, 0)); //Spawn a unit in the hex
                }
                else if (TurnState.playerOneTurn == false && UIController.playerTwoResourceValue > 0) //If it is player two's turn and they have resources left
                {
                    Instantiate(baseUnit, new Vector3(clickedObj.transform.position.x, 1, clickedObj.transform.position.z), Quaternion.Euler(0, 180, 0)); //Spawn a unit in the hex
                }
                clickedObj.transform.GetComponent<HexStats>().DeselectHex(); //Deselect the clicked hex
                spawnNumber--;

                //Updates resources
                UIController spawnUnit = new UIController();
                spawnUnit.SpawnBaseUnit();
            }
        }

        if (spawnNumber <= 0) //If the player has no more alotted spawns
        {
            enabled = false;
        }
        else if (TurnState.playerOneTurn == true && UIController.playerOneResourceValue <= 0) //If it is player one's turn, and they have no resources left
        {
            enabled = false;
        }
        else if (TurnState.playerOneTurn == false && UIController.playerTwoResourceValue <= 0) //If it is player two's turn, and they have no resources left
        {
            enabled = false;
        }
    }
}
