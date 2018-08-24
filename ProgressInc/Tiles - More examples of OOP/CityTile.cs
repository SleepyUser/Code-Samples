using System.Collections;
using UnityEngine;

public abstract class CityTile : MonoBehaviour {

    public enum compass { N, E, S, W };
    public int tileType; //See: enum class. Corresponding number to enum to denote tile type 
    public int[] location; //Grid location
    public int cost = 0; //Money to subtract from total
    public int tax = 0; //Money to return to bank
    public bool destroying =false; //Is this tile marked for destruction?
    public int destroyCost = 50; //Demolition/Construction cost
    protected int destroyCount = 0; //Counter until demolition

    public GameObject[] neighbourList = new GameObject[4];

    [SerializeField]
    protected GameObject construction;//Prefab of construction symbol

    protected SoundControls soundManager; //Soundmanager to use sound effects

    //Access to utilities?
    public bool power = false;
    public bool water = false;


    /// <summary>
    /// Initialise variables
    /// </summary>
    public void Awake()
    {
        construction.SetActive(false);
        soundManager = Camera.main.GetComponent<SoundControls>();
    }

    /// <summary>
    /// Sign up for updates
    /// </summary>
    public void OnEnable()
    {
        CityGrid.smallUpdateTrigger += SmallUpdate;
        CityGrid.bigUpdateTrigger += BigUpdate;
    }

    /// <summary>
    /// Remove signups for updates
    /// </summary>
    public void OnDisable()
    {
        CityGrid.smallUpdateTrigger -= SmallUpdate;
        CityGrid.bigUpdateTrigger -= BigUpdate;
    }

    /// <summary>
    /// Things to complete first time only
    /// </summary>
    public virtual void Initialise()
    {
        soundManager.PlayingSound(1,0.05f);
        Recheck();
        foreach (GameObject g in neighbourList)
        {
            if (g != null)
            {
                g.GetComponent<CityTile>().Recheck(); //Call recheck on all neighbours to update tile neighbours/connections
            }
        }
        StartCoroutine(RoadFrameWait());//Needs a delay to properly show road tiles connecting
    }

    /// <summary>
    /// Things to complete whenever a neighbour is changed
    /// </summary>
    public virtual void Recheck()
    {
        AddAllNeighbours();
    }

    /// <summary>
    /// Adds all neighbour tiles to array of neighbours
    /// </summary>
    protected void AddAllNeighbours()
    {
        CityGrid cityScript = GetComponentInParent<CityGrid>();
        int width = cityScript.width;
        int height = cityScript.height;
        //Add neighbour in every direction
        AddNeighbour(cityScript, height, 1, 0, (int)compass.N);
        AddNeighbour(cityScript, width, 1, 1, (int)compass.E);
        AddNeighbour(cityScript, height, -1, 0, (int)compass.S);
        AddNeighbour(cityScript, width, -1, 1, (int)compass.W);
    }

    /// <summary>
    /// Adds neighbour to the array in heightwidth direction, by the modifier number
    /// </summary>
    /// <param name="cityScript">city spawner script</param>
    /// <param name="heightWidth">X or Y axis</param>
    /// <param name="modifier">+1 or -1 along axis axis</param>
    /// <param name="direction">0 for height, 1 for width</param>
    /// <param name="compassDirection">where to place in array. CompassDirection enum can be used</param>
    protected void AddNeighbour(CityGrid cityScript, int heightWidth, int modifier, int direction, int compassDirection)
    {
        if (heightWidth > (location[direction] + modifier)) //check to see if tile to be searched is out of bounds
        {
            if (0 <= (location[direction] + modifier)) //checked to see if tile to be searched is out of bounds
            {
                GameObject temp;
                if (direction == 0) //Varies which axis to check
                {
                    temp = cityScript.city[location[0] + modifier, location[1]];
                }
                else
                {
                    temp = cityScript.city[location[0], location[1] + modifier];
                }
                if (temp.GetComponent<CityTile>() != null) //Leaves it null if it's already null, adds to neighbourlist if not
                {
                    neighbourList[compassDirection] = temp;
                }
            }
        }
    }

    /// <summary>
    /// Update that happens every few smallUpdates.
    /// </summary>
    public virtual void BigUpdate()
    {
        if(destroying) //Checks for destruction
        {
            destroyCount++;
            if(destroyCount >= 2) //if it has been destroying for two bigupdates
            {
                StaticValues.cityMoney -= destroyCost;
                ChangeTile(gameObject.GetComponentInParent<CityGrid>().ReturnObject("Empty"));
            }
        }
        else
        {
            destroyCount = 0;
        }
        StaticValues.cityMoney -= cost;//Pay cost
    }

    public abstract void SmallUpdate(int count);

    /// <summary>
    /// Toggle if construction sprite is showing
    /// </summary>
    public void ConstructionToggle()
    {
        construction.SetActive(!construction.activeSelf);
    }


    /// <summary>
    /// Function to be called when a new tile will replace this one.
    /// </summary>
    /// <param name="desiredTile">Tile type compared to enum</param>
    protected void ChangeTile(GameObject desiredTile)
    {
        GameObject newTile = Instantiate(desiredTile, this.transform.position, this.transform.rotation, transform.parent);
        newTile.GetComponent<CityTile>().location = location;
        gameObject.GetComponentInParent<CityGrid>().city[location[0], location[1]] = newTile;
        newTile.GetComponent<CityTile>().Initialise();
        Destroy(gameObject);
    }

    /// <summary>
    /// Coroutine to buffer the road pieces updating until the end of the frame.
    /// Else roads do not update correctly.
    /// </summary>
    /// <returns></returns>
    protected IEnumerator RoadFrameWait() 
    {
        yield return new WaitForEndOfFrame();
        foreach (GameObject g in neighbourList)
        {
            if (g)
            {
                if(g.tag == "TRoad")
                {
                    g.GetComponent<RoadTile>().Recheck();
                }
            }
        }
    }
}
