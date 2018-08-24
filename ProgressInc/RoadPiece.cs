using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPiece : MonoBehaviour {
    [SerializeField]
    private GameObject power;
    [SerializeField]
    private GameObject water;


    /// <summary>
    /// Initialise water and power off
    /// </summary>
    public void Awake()
    {
        power.SetActive(false);
        water.SetActive(false);
    }

    /// <summary>
    /// Set the active vars of power and water
    /// </summary>
    /// <param name="powerOn"></param>
    /// <param name="waterOn"></param>
    public void PowerWater(bool powerOn, bool waterOn)
    {
        power.SetActive(powerOn);
        water.SetActive(waterOn);
    }
}
