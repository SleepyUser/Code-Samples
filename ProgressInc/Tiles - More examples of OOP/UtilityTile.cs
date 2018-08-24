using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UtilityTile : BuildingTile
{
    
    public int tilesCovered = 4; //No of tiles away to provide resource to

    /// <summary>
    /// Small updates
    /// </summary>
    /// <param name="count"></param>
    public override void SmallUpdate(int count)
    {
        //Not currently used
    }

    /// <summary>
    /// Big update intervals.
    /// Adds providing resource, if there is an entrance road.
    /// </summary>
    public override void BigUpdate()
    {
        base.BigUpdate();
        if (entranceRoad != null)
        {
            ProvideResource();
        }
    }

    /// <summary>
    /// Function finds the next n road tiles and gives them a resource
    /// </summary>
    protected void ProvideResource()
    {
        int tiles = 0;
        List<GameObject> currentTiles;
        List<GameObject> nextTiles = new List<GameObject>();
        nextTiles.Add(entranceRoad);
        while (tiles < tilesCovered && nextTiles.Count != 0)
        {
            tiles++;
            currentTiles = nextTiles;
            nextTiles = new List<GameObject>();

            foreach (GameObject g in currentTiles) //Check the whole list
            {
                ActivateResource(g);
                foreach (GameObject o in g.GetComponent<CityTile>().neighbourList) //Check the neighbours of each item on the list 
                {
                    if (o != null)
                    {
                        if (o.GetComponent<RoadTile>() != null && !currentTiles.Contains(o) && !nextTiles.Contains(o)) //If it's a road tile, and it hasn't already been added to either list 
                        {
                            nextTiles.Add(o);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Selects the correct resource and activates it within the target game object
    /// </summary>
    /// <param name="g"></param>
    protected void ActivateResource(GameObject g)
    {
        switch (tileType) //Activate the relevant utility flag in the tile.
        {
            case 5:
                g.GetComponent<RoadTile>().nextPower = true;
                break;
            case 6:
                g.GetComponent<RoadTile>().nextWater = true;
                break;
        };
    }
}