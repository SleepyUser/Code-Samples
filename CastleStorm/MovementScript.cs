/* Max Topan, Glenn Clarke */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MovementScript : MonoBehaviour
{
    float moveTime = 0.5f;                                                  // time duration of move animation
    RaycastHit onTile;                                                      // used to store info for current tile hit
    const string speed = "Speed";                                           // used to store speed class tag
    public bool transformed = false;                                        // stores a transform bool used to pause code until unit is transformed
    private bool singleMoveSpeed = false;

    [SerializeField]
    GameObject currentTile = null;                                          // stores tile unit is currently on
    int hexLayer = 0;                                                       // initialise layer
    public GameObject mergeUI;                                              // UI game object
    private GameObject mergeObj = null;
    private GameObject clickedObjStore;

    void OnEnable()
    {
        var layer = 1 << 8;                                                 // bit-shifted layer for hexes
        hexLayer = layer;
        currentTile = gameObject.GetComponent<UnitStats>().CheckTile();     // set current tile to whatever unit is on
        currentTile.GetComponent<HexStats>().DeselectAll();                 // set all tiles to unselectable

        NeighbourSelection.SelectTiles(currentTile, gameObject);            // make all neighbours selectable, two rows if tag is speed
        currentTile.GetComponent<HexStats>().SelectHex();

        foreach (GameObject hex in GameObject.FindGameObjectsWithTag("sHex"))
        {
            if (TurnState.playerOneTurn)
            {
                for (int i = 0; i < HexSpawner.spawnRows.Length / 2; i++)
                {
                    if (hex == HexSpawner.spawnRows[1, i])
                    {
                        hex.GetComponent<HexStats>().DeselectHex();
                        break;
                    }
                }
            }
            else if (TurnState.playerOneTurn == false)
            {
                for (int i = 0; i < HexSpawner.spawnRows.Length / 2; i++)
                {
                    if (hex == HexSpawner.spawnRows[0, i])
                    {
                        hex.GetComponent<HexStats>().DeselectHex();
                        break;
                    }
                }
            }
        }
    }

    void OnDisable()
    {
        TurnState.usingScript = false;                                      // set using script to false
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit clickedObj;                                                                                                      // raycast info for the hex selected
        Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);                                                           // raycast for detecting new hexes
        
        if (!transformed)
        {
            if (Input.GetMouseButtonUp(0) && Physics.Raycast(clickRay, out clickedObj, Mathf.Infinity, hexLayer))                       // if player clicks a hex
            {
                if (clickedObj.transform.tag == "sHex")                                                                                 // if the hex is selectable
                {
                    
                    clickedObjStore = clickedObj.transform.gameObject;
                    if (clickedObj.transform.gameObject == currentTile)
                    {
                        CancelScript();
                    }
                    else
                    {
                        if (clickedObj.transform.GetComponent<HexStats>().occupier != null)
                        {
                            gameObject.tag = "Changing";
                            mergeUI = GameObject.Find("ButtonHolder");
                            mergeUI.transform.position = Camera.main.WorldToScreenPoint(clickedObj.transform.position);
                            //Destroy(clickedObj.transform.GetComponent<HexStats>().occupier);
                            mergeObj = clickedObj.transform.gameObject;

                        }
                        else if (gameObject.tag != speed)                                                                                   // if unit is not a speed unit
                        {
                            StartCoroutine(MoveObject(transform.position, new Vector3(clickedObj.transform.position.x, transform.position.y, clickedObj.transform.position.z), moveTime)); // move to clicked tile
                        }
                        else                                                                                                                // if unit is speed class
                        {
                            for (int i = 0; i < 6; i++)                                                                                     // iterate through hex sides
                            {
                                if (clickedObj.transform.GetComponent<HexStats>().neighbours[i] == currentTile)                             // if clicked tile is one tile away
                                {
                                    StartCoroutine(MoveObject(transform.position, new Vector3(clickedObj.transform.position.x, transform.position.y, clickedObj.transform.position.z), moveTime)); // move to clicked tile
                                    singleMoveSpeed = true;
                                    break; // break out of loop
                                }
                                else if (i == 5)                                                                                            // if clicked tile is not adjacent
                                {
                                    GameObject interObj = NeighbourSelection.PathToTile(currentTile, clickedObj.transform.gameObject);      // find a free path to the clicked tile
                                    StartCoroutine(MoveSpeedUnit(                                                                           // move to the clicked tile via an available adjacent tile
                                        (
                                        new Vector3(interObj.transform.position.x, transform.position.y, interObj.transform.position.z)),
                                        new Vector3(clickedObj.transform.position.x, transform.position.y, clickedObj.transform.position.z))
                                        );
                                }
                            }
                        }
                        gameObject.GetComponent<UnitStats>().currentTile = clickedObj.transform.gameObject;                                 // set unit stats current tile to the new tile
                        currentTile.GetComponent<HexStats>().DeselectAll();                                                                 // deselect all selected tiles           
                        if (gameObject.tag != "Changing")
                        {
                            gameObject.GetComponent<UnitStats>().actionPoint = false;                                                       // use up the unit's action point for the turn
                            enabled = false;                                                                                                // disable movement script
                        }
                        else
                        {
                            currentTile.GetComponent<HexStats>().SelectHex();
                        }
                    }
                }
            }
        }
        else if (transformed == true)
        {
            Destroy(clickedObjStore.GetComponent<HexStats>().occupier);
            mergeUI.transform.position = new Vector3(-500, 0, 0);
            StartCoroutine(MoveObject(transform.position, new Vector3(mergeObj.transform.position.x, transform.position.y, mergeObj.transform.position.z), moveTime)); // move to clicked tile
            gameObject.GetComponent<UnitStats>().currentTile = mergeObj.transform.gameObject;
            //mergeObj = null;
            transformed = false;
            enabled = false;
        }
    }

    #region Movement Coroutines
    IEnumerator MoveSpeedUnit(Vector3 interDest, Vector3 finalDest)
    {
        StartCoroutine(MoveObject(transform.position, interDest, moveTime));                        // move unit from start to intermediate tile
        yield return new WaitForSeconds(moveTime);                                                  // wait for the time movement takes
        StartCoroutine(MoveObject(transform.position, finalDest, moveTime));                        // move unit from intermediate tile to clicked tile
        yield return new WaitForSeconds(moveTime);                                                  // wait for the time movement takes
        TurnState.SelectAllies();                                                                   // select all allies that have not yet moved
    }

    /* http://answers.unity3d.com/questions/63060/vector3lerp-works-outside-of-update.html */
    IEnumerator MoveObject(Vector3 source, Vector3 target, float overTime)
    {
        currentTile.GetComponent<HexStats>().DeselectAll();
        float startTime = Time.time;                                                                // creates float to store when movement started
        while (Time.time < startTime + overTime)                                                    // runs from start time until moveTime has elapsed
        {
            transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);  // lerp from initial hex to next hex in amount of time moveTime
            yield return null;
        }
        transform.position = target;                                                                // set position to targetted position
        currentTile.GetComponent<HexStats>().CheckOccupier();                                       // call occupency check on old hex
        currentTile = gameObject.GetComponent<UnitStats>().CheckTile();                             // set current tile to tile landed on
        currentTile.GetComponent<HexStats>().CheckOccupier();                                       // call occupency check on new currentTile
        if (gameObject.tag != speed || mergeObj != null || singleMoveSpeed == true)
        {
            TurnState.SelectAllies();                                                               // select all allies that have not yet moved
            mergeObj = null;
            singleMoveSpeed = false;
        }
    }
    #endregion

    public void CancelScript()
    {
        gameObject.GetComponent<UnitStats>().actionPoint = true;
        currentTile.GetComponent<HexStats>().DeselectAll();
        if (gameObject.tag == "Changing")
        {
            gameObject.tag = "Base";
            mergeObj = null;
            mergeUI.transform.position = new Vector3(-500, 0, 0);
        }
        TurnState.SelectAllies();    
        enabled = false;
    }
}