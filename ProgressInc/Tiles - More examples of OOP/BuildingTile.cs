using UnityEngine;

public abstract class BuildingTile : CityTile {

    public GameObject entranceRoad; //All buildings require an entrance road to "hook in" to the road network.

    /// <summary>
    /// No additional functionality at this level.
    /// </summary>
    public override void Initialise()
    {
        base.Initialise();
    }

    /// <summary>
    /// Adds functionality to base select an entrance road
    /// </summary>
    public override void Recheck()
    {
        base.Recheck();
        foreach (GameObject g in neighbourList)
        {
            if (g != null && g.GetComponent<RoadTile>())
            {
                entranceRoad = g;
                break;
            }
        }
    }
}
