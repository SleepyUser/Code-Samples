/*http://stackoverflow.com/questions/767821/is-else-if-faster-than-switch-case*/
/*Glenn Clarke, Max Topan*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AttackScript : MonoBehaviour
{
    //public bool playerOneTurnTemp;

    GameObject currentTile = null;
    int hexLayer = 0;
    private const string range = "Range", speed = "Speed", health = "Health", baseUnit = "Base", core = "Core";

    void OnEnable()
    {
        currentTile = gameObject.GetComponent<UnitStats>().CheckTile();
        currentTile.GetComponent<HexStats>().DeselectAll();
        currentTile.GetComponent<HexStats>().SelectHex();

        var layer = 1 << 8;
        hexLayer = layer;

        if (gameObject.tag == range)
        {
            NeighbourSelection.RangeSelect(currentTile, gameObject, range, TurnState.playerOneTurn);
        }
        else
        {
            currentTile.GetComponent<HexStats>().SelectEnemyNeighbours(TurnState.playerOneTurn); // check for neighbour hexes occupied by enemy units
        }
    }

    void OnDisable()
    {
        currentTile.GetComponent<HexStats>().DeselectAll();
        PercentageDisplay.ReturnText();
		TurnState.SelectAllies();
        TurnState.usingScript = false; // set using script to false
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit clickedObj;
        Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        PercentageDisplay.SetText(gameObject);
        if (Input.GetMouseButtonUp(0) && Physics.Raycast(clickRay, out clickedObj, Mathf.Infinity, hexLayer))   // if player clicks a hex
        {
            if (clickedObj.transform.gameObject == currentTile) //Click the current tile to cancel the attack
            {
                CancelScript(true);
            }
            else
            {
                if (clickedObj.transform.tag == "sHex") // if the hex is selectable
                {
                    
                    // get the occupier of the clicked hex
                    GameObject enemy = clickedObj.transform.gameObject.GetComponent<HexStats>().occupier;

                    // compare tags of self and enemy
                    if (enemy.tag != gameObject.tag)
                    {
                        #region SwitchAttackChances
                        switch (enemy.tag)
                        {
                            case baseUnit:
                                if (AttackChance(75) == true) // 75% chance
                                {
                                    Destroy(enemy);
                                }
                                break;

                            case core:
                                if (gameObject.tag != speed) //If the attacker is NOT a speed unit
                                {
                                    if (clickedObj.transform.GetComponent<HexStats>().occupier.GetComponent<CoreScript>().ReduceHealth())
                                    {
                                        if (TurnState.playerOneTurn == true)
                                        {
                                            Destroy(clickedObj.transform.GetComponent<HexStats>().occupier.transform.parent.gameObject);
                                            Debug.Log("Winner is player one");
                                        }
                                        else
                                        {
                                            Destroy(clickedObj.transform.GetComponent<HexStats>().occupier.transform.parent.gameObject);
                                            Debug.Log("Winner is player two");
                                        }
                                    }
                                }
                                else
                                {
                                    AttackChance(0);
                                }

                                break;

                            case health:
                                if (gameObject.tag != speed && enemy.GetComponent<UnitStats>().shield == true) //If the attacking unit is not speed, and the shield is up
                                {
                                    enemy.GetComponent<UnitStats>().shield = false; //Turn shield off
                                    // play animation, turns off shield when attacked
                                    enemy.GetComponent<UnitStats>().shieldSprite.SetActive(false);
                                }
                                else if (enemy.GetComponent<UnitStats>().shield == false) //If shield is not up
                                {
                                    if (gameObject.tag == speed)
                                    {
                                        if (AttackChance(75)) // 75% chance
                                        {
                                            Destroy(enemy);
                                        }
                                    }
                                    else // tag is mage
                                    {
                                        if (AttackChance(75))
                                        {
                                            Destroy(enemy);
                                        }
                                    }
                                    
                                    break;
                                }
                                break;

                            case speed:
                                if (AttackChance(75)) // 75% chance
                                {
                                    Destroy(enemy);
                                }
                                break;

                            case range:
                                if (gameObject.tag == speed)
                                {
                                    if (AttackChance(75)) // 75% chance
                                    {
                                        Destroy(enemy);
                                    }
                                }
                                else // if you are a health
                                {
                                    if (AttackChance(75)) //75% chance
                                    {
                                        Destroy(enemy);
                                    }
                                }
                                break;

                            default:
                                Debug.LogError("Enemy is untagged");
                                break;
                        }
                        #endregion
                    }
                    else
                    {
                        if (enemy.tag == health && enemy.GetComponent<UnitStats>().shield == true)
                        {
                            enemy.GetComponent<UnitStats>().shield = false;
                            enemy.GetComponent<UnitStats>().shieldSprite.SetActive(false);
                        }
                        else if (enemy.tag == speed && AttackChance(75) == true)
                        {
                            Destroy(enemy);
                        }
                        else if (AttackChance(40) == true) // 40% chance to kill
                        {
                                Destroy(enemy);
                        }
                    }
                    gameObject.GetComponent<UnitStats>().currentTile = clickedObj.transform.gameObject; // set unit stats current tile to the new tile
                    gameObject.GetComponent<UnitStats>().actionPoint = false;                           // use up the unit's action point for the turn
                    currentTile.GetComponent<HexStats>().DeselectAll();                                 // Deselect all selected tiles
                    enabled = false;                                                                    // Disable self
                }
            }
        }
    }

    /// <summary>
    /// Enter a % chance to hit, and this will return true if it rolls a hit
    /// </summary>
    /// <param name="percentage"></param>
    /// <returns></returns>
    bool AttackChance(int percentage)
    {
        bool hit = false;
        int rand = Random.Range(1, 101);
        if (rand <= percentage)
        {
            hit = true;
        }
        return hit;
    }

    public void CancelScript(bool actionPoint)
    {
        gameObject.GetComponent<UnitStats>().actionPoint = actionPoint;
        currentTile.GetComponent<HexStats>().DeselectAll();
        enabled = false;
    }
}