using UnityEngine;

public class RoadTile : CityTile {

    //Buffer variables to stabilise the false/true water and power
    public bool nextPower = false;
    public bool nextWater = false;

    public GameObject[] connectionList = new GameObject[4];
    public GameObject[] destinationList = new GameObject[4];
    [SerializeField]
    private GameObject[] roadPieces = new GameObject[4];

    public void Start()
    {
        tileType = 7;
    }

    // Use this for initialization
    public override void Initialise()
    {
        base.Initialise();
        cost = 50;
        Recheck();
    }

    /// <summary>
    /// Confirm neighbours, connections, and road pieces. Called when a tile change happens nearby.
    /// </summary>
    public override void Recheck()
    {
        AddAllNeighbours();
        AddConnection();
        RoadPieces();
    }

    /// <summary>
    /// Find ROAD TILES only to connect with
    /// </summary>
    public void AddConnection()
    {
        for (int i = 0; i < 4; i++)
        {
            if(neighbourList[i] != null)
            {
                if (neighbourList[i].GetComponent<RoadTile>() != null)
                {
                    connectionList[i] = neighbourList[i];
                }
                else if (neighbourList[i].GetComponent<ProductionTile>() != null)
                {
                    destinationList[i] = neighbourList[i];
                }
            }
        }
    }

    /// <summary>
    /// Activate "roadpieces" to link up the road network visually
    /// </summary>
    private void RoadPieces()
    {
        for(int i = 0; i<4; i++)
        {
            if(connectionList[i] != null)
            {
                roadPieces[i].SetActive(true);
                roadPieces[i].GetComponent<RoadPiece>().PowerWater(power, water);
            }
            else
            {
                roadPieces[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Empty and reset buffer variables at 0 count, then set tiles to reflect the updated variables 
    /// </summary>
    /// <param name="count"></param>
    public override void SmallUpdate(int count)
    {
        if(count == 0) //Reset tiles at step 0, right after big update
        {
            power = nextPower;
            water = nextWater;
            nextWater = false;
            nextPower = false;

            foreach (GameObject p in roadPieces)
            {
                p.GetComponent<RoadPiece>().PowerWater(power, water);
            }
        }
    }

    public override void BigUpdate()
    {
        base.BigUpdate();
    }
}
