using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

// NOTE: TWO CLASSES IN THIS SCRIPT.

public class Pathfinder : MonoBehaviour
{

    public void Pathfind(GameObject destination, GameObject start)
    {
        StartCoroutine(Algorithm(GameObject.FindGameObjectWithTag("CitySpawn").GetComponent<CityGrid>().city, destination.GetComponent<ProductionTile>().entranceRoad, start.GetComponent<ProductionTile>().entranceRoad, start, destination));
    }


    /// <summary>
    /// Pathfinding Algorithm
    /// </summary>
    /// <param name="roadNetworks"></param>
    /// <param name="finRoad">The entranceroad of the fin tile.</param>
    /// <param name="startRoad">The entranceroad of the start tile.</param>
    /// <param name="startTile"></param>
    /// <param name="finTile"></param>
    /// <returns></returns>
    IEnumerator Algorithm(GameObject[,] roadNetworks, GameObject finRoad, GameObject startRoad, GameObject startTile, GameObject finTile)
    {

        List<GameObject> route = new List<GameObject>();
        List<RoadTileProxy> open = new List<RoadTileProxy>();
        List<RoadTileProxy> closed = new List<RoadTileProxy>();
        List<RoadTileProxy> neighbours;
        int[] finLoc = finTile.GetComponent<ProductionTile>().location;
        GameObject[] connections;
        RoadTileProxy destination = new RoadTileProxy(finLoc, finLoc, open.Count(), null, finRoad);
        RoadTileProxy current;
        bool found = false;


        open.Add(new RoadTileProxy(startRoad.GetComponent<CityTile>().location, finRoad.GetComponent<CityTile>().location, closed.Count, null, startRoad));//Add the first RoadTileProxy to the open list

        do
        {
            current = open.Find(RoadTileProxy => (RoadTileProxy.F == (open.Min(RoadTileProxy2 => RoadTileProxy2.F)))); //Find the lowest F valued RoadTileProxy in the open list
            closed.Add(current);
            open.Remove(current);

            if (closed.Find(RoadTileProxy => (RoadTileProxy.inGrid == destination.inGrid)) != null) //If the destination has been found, end the loop
            {
                found = true;
                break;
            }

            //use the connection list to find all roads.
            neighbours = new List<RoadTileProxy>();
            connections = current.inGrid.GetComponent<RoadTile>().connectionList;

            foreach (GameObject g in connections)
            {
                if (g != null && !g.GetComponent<RoadTile>().destroying)
                {
                    neighbours.Add(new RoadTileProxy(g.GetComponent<RoadTile>().location, finLoc, closed.Count, current, g));
                }
            }

            for (int i = 0; i < neighbours.Count(); i++)
            {
                if (closed.FindIndex(RoadTileProxy => ((RoadTileProxy.inGrid == neighbours[i].inGrid))) >= 0)//skip the RoadTileProxy if it's already in the closed list
                {
                    continue;
                }
                else if (open.FindIndex(RoadTileProxy => (RoadTileProxy.inGrid == neighbours[i].inGrid)) > 0) //If the tile is in the open list already
                {
                    int tempInt = open.FindIndex(RoadTileProxy => (RoadTileProxy.inGrid == neighbours[i].inGrid)); //Find the index of the neighbour in the open list.

                    if (neighbours[i].F < open[tempInt].F) //Comparison of the old and new tiles F values.
                    {
                        open[tempInt] = neighbours[i]; //Use the new tile instead of the old, if the new one is more direct.
                    }
                }
                else
                {
                    open.Add(neighbours[i]);
                }
            }
            yield return null; //Reduce load on a single frame by splitting up loops over multiple frames
        }
        while (open.Count > 0);

        if (found) //If the destination is reached
        {
            var temp2 = closed.Last(); //temp is the last thing in the list (destination)
            do
            {
                route.Add(temp2.inGrid);
                temp2 = temp2.parent;
            }
            while (temp2 != null && temp2.parent != null); //Adds all the gameObjects to a list
            route.Reverse();
            route.Insert(0, startRoad); //Adds the second tile to the start.
            route.Insert(0, startTile); //Adds the first tile to the start, pushing the previous tile into the correct second place.
            route.Add(finTile); //Adds the final tile to the end.
            startTile.GetComponent<ProductionTile>().RouteFound(found, route.ToArray(), finTile);
        }
        else
        {
            startTile.GetComponent<ProductionTile>().RouteFound(found, null, finTile); //Returns not found
        }

        Destroy(this);
    }
}


/// <summary>
/// Class used only within this script.
/// Allows
/// </summary>
public class RoadTileProxy
{
    public int[] index;
    public int F;
    public RoadTileProxy parent;
    public GameObject inGrid;

    /// <summary>
    /// Initialisation
    /// </summary>
    /// <param name="i"></param>
    /// <param name="fin"></param>
    /// <param name="count"></param>
    /// <param name="parentRoadTileProxy"></param>
    /// <param name="realTile"></param>
    public RoadTileProxy(int[] i, int[] fin, int count, RoadTileProxy parentRoadTileProxy, GameObject realTile)
    {
        index = i;
        CalculateFValue(count, fin);
        parent = parentRoadTileProxy;
        inGrid = realTile;
    }

    /// <summary>
    /// Calculates the F value of the tile
    /// This shows how "far" this is away from the goal, including the path so far.
    /// </summary>
    /// <param name="count"></param>
    /// <param name="fin"></param>
    private void CalculateFValue(int count, int[] fin)
    {
        F = CalculateGValue() + CalcH(fin); 
    }

    private int CalculateGValue() //G Value => number of tiles travelled to get to this point.
    {
        RoadTileProxy temp = this;
        int count = 0;
        while(temp.parent != null)
        {
            count++;
        }
        return count;
    }

    private int CalcH(int[] fin) //Distance to go around city blocks - Manhattan Distance
    {
        int temp;
        temp = (index[0] - fin[0]) + (index[1] - fin[1]);
        Math.Abs(temp);
        return temp;
    }
}