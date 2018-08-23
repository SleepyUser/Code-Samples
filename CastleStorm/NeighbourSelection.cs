using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// A collection of methods which will select various tiles
/// </summary>
public class NeighbourSelection : MonoBehaviour
{
    public static void SelectTiles(GameObject currentTile, GameObject currentUnit)
    {
        currentTile.GetComponent<HexStats>().SelectNeighbours();

        #region Two Tile Select
        if (currentUnit.tag == "Speed") //Speed can move two tiles
        {
            for (int i = 0; i < 6; i++) // iterate through neighbors
            {
                if (currentTile.GetComponent<HexStats>().neighbours[i] != null)
                {
                    if (currentTile.GetComponent<HexStats>().neighbours[i].tag == "sHex")
                    {
                        currentTile.GetComponent<HexStats>().neighbours[i].GetComponent<HexStats>().SelectNeighbours();
                    }
                }
            }
        }
        else if (currentUnit.tag == "Base") //Needs to be able to move ONTO other allied base units
        {
            for (int i = 0; i < 6; i++) // iterate through neighbors
            {
                if (currentTile.GetComponent<HexStats>().neighbours[i] != null)
                {
                    if (currentTile.GetComponent<HexStats>().neighbours[i].GetComponent<HexStats>().occupier != null)
                    {
                        if (currentTile.GetComponent<HexStats>().neighbours[i].GetComponent<HexStats>().occupier.tag == "Base" && currentTile.GetComponent<HexStats>().neighbours[i].GetComponent<HexStats>().occupier.GetComponent<UnitStats>().playerOneUnit == currentUnit.GetComponent<UnitStats>().playerOneUnit)
                        {
                            currentTile.GetComponent<HexStats>().neighbours[i].GetComponent<HexStats>().SelectHex();
                        }
                    }
                }
            }
        }
        currentTile.GetComponent<HexStats>().DeselectHex();
        #endregion
    }

    public static void RangeSelect(GameObject currentTile, GameObject currentUnit, string unitTag, bool currentTurn)
    {
        //bool neighbours = false;
        if (currentUnit.tag == unitTag)
        {
            for (int i = 0; i < 6; i++) // iterate through neighbors
            {
                if (currentTile.GetComponent<HexStats>().neighbours[i] != null) //Null check
                {
                    for (int j = 0; j < 6; j++) //Iterates through six because tiles are hexagonal
                    {
                        GameObject neighbour = currentTile.GetComponent<HexStats>().neighbours[i].GetComponent<HexStats>().neighbours[j];
                        if (neighbour != null)
                        {
                            if (neighbour.GetComponent<HexStats>().occupier != null) //Null check
                            {
                                if (neighbour.GetComponent<HexStats>().occupier.GetComponent<UnitStats>() != null) //Null check
                                {
                                    if (neighbour.GetComponent<HexStats>().occupier.GetComponent<UnitStats>().playerOneUnit != currentTurn) //if the owner of the occupier is not the current player
                                    {
                                        neighbour.GetComponent<HexStats>().SelectHex();
                                    }
                                }
                                else if (neighbour.GetComponent<HexStats>().occupier.GetComponent<CoreScript>() != null) //Null check
                                {
                                    if (neighbour.GetComponent<HexStats>().occupier.GetComponent<CoreScript>().playerOne != currentTurn) //if the owner of the occupier is not the current player
                                    {
                                        neighbour.GetComponent<HexStats>().SelectHex();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        currentTile.GetComponent<HexStats>().DeselectNeighbours();
    }

    /// <summary>
    /// Finds an intermediate tile between two tiles
    /// </summary>
    /// <param name="currentTile"> current tile (from) </param>
    /// <param name="clickedObj"> tile two hexes away from the current tile (to) </param>
    /// <returns></returns>
    public static GameObject PathToTile(GameObject currentTile, GameObject clickedObj)
    {
        GameObject interObj = null;

        var intersection = currentTile.GetComponent<HexStats>().neighbours.Intersect(clickedObj.transform.GetComponent<HexStats>().neighbours);
        foreach (GameObject value in intersection)
        {
            if (value != null && value.tag == "sHex") //Look for a valid tile that is both existent and not occupied (checked using whether it would be selectable)
            {
                interObj = value;
                break;
            }
        }
        return interObj;
    }
}