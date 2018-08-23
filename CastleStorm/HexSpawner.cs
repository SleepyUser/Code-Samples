/*Based on: http://catlikecoding.com/unity/tutorials/hex-map-1/*/
/* Max Topan, Glenn Clarke */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexSpawner : MonoBehaviour
{
    public GameObject hex;                                                      // hex prefab to be spawned
    public GameObject core;                                                     // core prefab to be spawned
    public GameObject cameraHinge;                                              // camera is rotated around this
    public int height = 6, width = 6;                                           // height and with of hex grid
    public static GameObject[,] spawnRows;

    void Awake()
    {
        float hexScale = hex.transform.localScale.x;                            // even out scale of hexes
        hex.transform.localScale = new Vector3(hexScale, hexScale, hexScale);   // even out scale of hexes

        HexMetrics.outerRadius *= hex.transform.localScale.x;                   // calculate new outer radius based on hex scale
        HexMetrics.innerRadius *= hex.transform.localScale.x;                   // calculate new inner radius based on hex scale

        spawnRows = new GameObject[2, width];
        SpawnGrid(height, width);                                               // calls SpawnGrid function

        PositionCamera(height, width);
    }

    void Start()
    {
        StartCoroutine(SpawnCore(spawnRows));
    }

    /// <summary>
    /// spawns hexes based on width and height
    /// </summary>
    /// <param name="height"></param>
    /// <param name="width"></param>
    void SpawnGrid(int height, int width)
    {
        float offset = 0;                                                       // stores how far hexes must be offset every other row
        GameObject spawnedHex;                                                  // stores hex just spawned
        int coordOffset = -1;                                                   // stores amount needed to offset coords for hexes

        for (int i = 0; i < height; i++)                                        // iterates through rows (height)
        {
            if ((i + 1) % 2 != 1)                                               // if row number is divisible by two (every other row)
            {
                offset = HexMetrics.innerRadius;                                // offset current row by half a hex (inner radius)
            }
            else                                                                // if row number is not divisible by 2 (starts here)
            {
                offset = 0;                                                     // don't offset current row
                coordOffset++;                                                  // increment coord offset by 1
            }

            for (int j = 0; j < width; j++)                                     // iterates through columns (width)
            {
                // V spawn a hex based on coords and hex scale (inner and outer radius)
                spawnedHex = Instantiate(hex, new Vector3(offset + (j * (HexMetrics.innerRadius * 2)), transform.position.y, (i * 1.5f) * HexMetrics.outerRadius), transform.rotation) as GameObject;
                spawnedHex.GetComponent<HexStats>().DeselectHex();

                spawnedHex.GetComponent<HexStats>().z = i;                      // set hex z coords
                spawnedHex.GetComponent<HexStats>().y = -(j - coordOffset) - i; // set hex y coords
                spawnedHex.GetComponent<HexStats>().x = j - coordOffset;        // set hex x coords

                if (i == 0)
                {
                    spawnRows[0, j] = spawnedHex;
                }
                else if (i == height - 1)
                {
                    spawnRows[1, j] = spawnedHex;
                }
                spawnedHex.transform.parent = gameObject.transform;
            }
        }
    }

    IEnumerator SpawnCore(GameObject[,] spawnRows)
    {
        int lowerSpawn = Mathf.RoundToInt(spawnRows.Length / 4);
        int upperSpawn = Mathf.RoundToInt((spawnRows.Length / 4) - 1);

        GameObject playerOneCore = Instantiate(core, spawnRows[0, lowerSpawn].transform.position, Quaternion.Euler(0, -60, 0)) as GameObject;
        playerOneCore.GetComponent<CoreScript>().playerOne = true;
        foreach (Transform child in playerOneCore.transform)
        {
            child.GetComponent<CoreScript>().playerOne = true;
        }

        GameObject playerTwoCore = Instantiate(core, spawnRows[1, upperSpawn].transform.position, Quaternion.Euler(0, 120, 0)) as GameObject;

        yield return new WaitForEndOfFrame();
        AssignCores(lowerSpawn, upperSpawn, playerOneCore, playerTwoCore);
    }

    void PositionCamera(int height, int width)
    {
        float midHeight = (HexMetrics.outerRadius * (height / 2)) + HexMetrics.outerRadius * 1.5f;
        float midWidth = (HexMetrics.innerRadius * (width / 2)) + HexMetrics.innerRadius * 2;

        cameraHinge.transform.position = new Vector3(midWidth, 0, midHeight);
    }

    /// <summary>
    /// Max Topan
    /// Assigns the hex occupiers to their corresponding core
    /// </summary>
    /// <param name="lowerSpawn"></param>
    /// <param name="upperSpawn"></param>
    /// <param name="playerOneCore"></param>
    /// <param name="playerTwoCore"></param>
    void AssignCores(int lowerSpawn, int upperSpawn, GameObject playerOneCore, GameObject playerTwoCore)
    {
        GameObject coreHex = spawnRows[0, lowerSpawn];
        coreHex.GetComponent<HexStats>().occupier = playerOneCore.transform.FindChild("castle_med").gameObject;
        coreHex.GetComponent<HexStats>().DeselectHex();

        coreHex = spawnRows[0, lowerSpawn].GetComponent<HexStats>().neighbours[4];
        coreHex.GetComponent<HexStats>().occupier = playerOneCore.transform.FindChild("castle_tall").gameObject;
        coreHex.GetComponent<HexStats>().DeselectHex();

        coreHex = spawnRows[0, lowerSpawn].GetComponent<HexStats>().neighbours[5];
        coreHex.GetComponent<HexStats>().occupier = playerOneCore.transform.FindChild("castle_short").gameObject;
        coreHex.GetComponent<HexStats>().DeselectHex();

        coreHex = spawnRows[1, upperSpawn];
        coreHex.GetComponent<HexStats>().occupier = playerTwoCore.transform.FindChild("castle_med").gameObject;
        coreHex.GetComponent<HexStats>().DeselectHex();

        coreHex = spawnRows[1, upperSpawn].GetComponent<HexStats>().neighbours[1];
        coreHex.GetComponent<HexStats>().occupier = playerTwoCore.transform.FindChild("castle_tall").gameObject;
        coreHex.GetComponent<HexStats>().DeselectHex();

        coreHex = spawnRows[1, upperSpawn].GetComponent<HexStats>().neighbours[2];
        coreHex.GetComponent<HexStats>().occupier = playerTwoCore.transform.FindChild("castle_short").gameObject;
        coreHex.GetComponent<HexStats>().DeselectHex();
    }
}