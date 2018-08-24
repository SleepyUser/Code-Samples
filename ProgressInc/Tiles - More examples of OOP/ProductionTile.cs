using System.Collections.Generic;
using UnityEngine;

public abstract class ProductionTile : BuildingTile {

    //Units before money is paid out
    public int unitsRequired = 3;
    public int unitsAcquired = 0; //Counter

    //Max incoming and outgoing connections
    public int maxOutgoing = 2;
    public int maxIncoming = 2;

    public int incomingNo = 0; //Counter

    [SerializeField]
    protected GameObject agent; //Agent prefab

    [SerializeField]
    protected GameObject dollar; //Dollar Sign prefab

    //These lists are a triad and the index of one, should correspond to the other indexes.
    [SerializeField]
    protected List<ProductionTile> outgoing = new List<ProductionTile>(); // Reference of script on tile to go to
    protected List<GameObject[]> routes = new List<GameObject[]>(); //Reference to list to get to above tile
    [SerializeField]
    protected List<GameObject> agents = new List<GameObject>(); //Reference of agent going to the above tile

    protected abstract void outgoingIncoming();

    /// <summary>
    /// Calls the pathfinder script to plot a route if possible.
    /// </summary>
    /// <param name="i"></param>
    protected void RouteCalc(int i)
    {
        if (GetComponent<Pathfinder>() == null && entranceRoad != null && outgoing[i].entranceRoad != null) //Check both ends have an adjoining road
        {
            Pathfinder temp = gameObject.AddComponent<Pathfinder>();
            temp.Pathfind(outgoing[i].gameObject, gameObject);
        }
    }

    /// <summary>
    /// Sends agents along routes or calls for calculation of routes if they do not have a route.
    /// </summary>
    protected void SendAgent()
    {
        if (!destroying)
        {
            for (int i = 0; i < routes.Count; i++)
            {
                if (routes[i] != null)
                {
                    if (agents[i] == null)
                    {
                        GameObject temp = Instantiate(agent, gameObject.transform.position + new Vector3(0.0f, 0.1f, 0.0f), Quaternion.Euler(new Vector3(0, 90, 0)), gameObject.transform);
                        temp.GetComponent<Agent>().Initialise(tileType, routes[i]);
                        agents[i] = temp.gameObject;
                    }
                }
                else
                {
                    RouteCalc(i);
                }
            }
        }
    }
    

    /// <summary>
    /// To be called when an agent returns home from its route.
    /// </summary>
    /// <param name="recalcRoute">is any part of the route being destroyed</param>
    /// <param name="destDestroying">is the destination tile being destroyed?</param>
    /// <param name="route">the route array</param>
    public void RecieveAgent(bool recalcRoute, bool destDestroying, GameObject[] route)
    {
        if (destDestroying)
        {
            if (routes.IndexOf(route) >= 0)
            {
                RemoveFromLists(routes.IndexOf(route));
            }
        }
        else if (recalcRoute)
        {
            if (routes.IndexOf(route)>=0)
            { 
                routes[routes.IndexOf(route)] = null;
            }
        }

    }

    /// <summary>
    /// Add extra functionality to the cityUpdate for classes derived from this.
    /// Large interval tasks
    /// </summary>
    public override void BigUpdate()
    {
        base.BigUpdate();

        outgoingIncoming();
        while(unitsAcquired >= unitsRequired)
        {
            unitsAcquired -= unitsRequired;
            StaticValues.cityMoney += tax;
            soundManager.PlayingSound(6,0.1f);
            Instantiate(dollar, gameObject.transform.position + new Vector3(0, 0.1f, 0), gameObject.transform.rotation);
        }
    }
    
    /// <summary>
    /// Small interval tasks
    /// </summary>
    public override void SmallUpdate(int count)
    {
        if (count == 1)
        {
            DrawPower();
        }
        if ((water && power))
        {
            SendAgent();
        }
    }

    /// <summary>
    /// Checks for utilities on the entranceroad tile.
    /// </summary>
    protected void DrawPower()
    {
        power = false;
        water = false;
        if (entranceRoad!=null)
        {
            RoadTile r = entranceRoad.GetComponent<RoadTile>();
            power = r.power;
            water = r.water;
        }
    }

    /// <summary>
    /// To be called when an agent arrives here as its destination (not home)
    /// </summary>
    /// <param name="incomingType"></param>
    public virtual void ReceiveGoods(int incomingType)
    {
        unitsAcquired++;
    }

    /// <summary>
    /// Removes from triad of lists at the same time to ensure the whole record is removed.
    /// </summary>
    /// <param name="index">index of record to remove</param>
    public void RemoveFromLists(int index)
    {
        outgoing[index].SearchAndRemoveIncomingGameObject(gameObject);
        outgoing.Remove(outgoing[index]);
        routes.Remove(routes[index]);
        agents.Remove(agents[index]);
    }

    /// <summary>
    /// To keep lists synced, adds to all at once
    /// </summary>
    /// <param name="destination"></param>
    public void AddToLists(GameObject destination)
    {
        outgoing.Add(destination.GetComponent<ProductionTile>());
        destination.GetComponent<ProductionTile>().IncomingList(gameObject);
        routes.Add(null);
        agents.Add(null);
    }

    /// <summary>
    /// Adds incoming connection
    /// </summary>
    /// <param name="g"></param>
    public virtual void IncomingList(GameObject g)
    {
        incomingNo++;
    }

    /// <summary>
    /// Remove incoming connection
    /// </summary>
    /// <param name="g">here for</param>
    public virtual void SearchAndRemoveIncomingGameObject(GameObject g)
    {
        incomingNo--;
    }

    /// <summary>
    /// Has the route from the pathfinding algorithm been found?
    /// </summary>
    /// <param name="found"></param>
    /// <param name="route"></param>
    /// <param name="destination"></param>
    public void RouteFound(bool found, GameObject[] route, GameObject destination)
    {
        for (int i = 0; i < outgoing.Count; i++)
        {
            if(outgoing[i] == destination.GetComponent<ProductionTile>())
            {
                if (found)
                {
                    routes[i] = route;
                }
                else
                {
                    RemoveFromLists(i);
                    i--;
                }
            }
        }
    }
}
