using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CityGrid : MonoBehaviour {

    public GameObject[,] city;

    public int width = 10;
    public int height = 10; 
    [SerializeField]
    private GameObject[] buildings = new GameObject[10];

    private int timeInterval = 10;
    private int agentCalls = 4;

    private float timeElapsed = 0;
    public float timeSmallInterval = 0;
    private int counter = 0;

    [SerializeField]
    public MenuButtons levelChanges;

    public delegate void SmallUpdate(int c);
    public delegate void BigUpdate();
    public static SmallUpdate smallUpdateTrigger;
    public static BigUpdate bigUpdateTrigger;

    private SoundControls soundManager;

    /// <summary>
    /// Input a tilename string and return the ID from the enum
    /// </summary>
    /// <param name="tileName"></param>
    /// <returns></returns>
    public int ReturnID(string tileName)
    {
        return (int)Enum.Parse(typeof(EnumClass.BID), tileName, true);
    }

    /// <summary>
    /// Input a tilename string and return the prefab gameObject from the building array
    /// </summary>
    /// <param name="tileName"></param>
    /// <returns></returns>
    public GameObject ReturnObject(string tileName)
    {
        return buildings[(int)Enum.Parse(typeof(EnumClass.BID), tileName, true)];
    }

    /// <summary>
    /// Initialises several variables & city array.
    /// </summary>
    void Awake()
    {
        soundManager = Camera.main.GetComponent<SoundControls>();
        timeSmallInterval = timeInterval / agentCalls;
        city = new GameObject[width,height]; //Create a grid for the city.

        for (int i = 0; i < width; i++) //Begin city initialisation.
        {
            for(int j = 0; j < height; j++)
            {
                GameObject temp;

                temp = Instantiate(buildings[(int)EnumClass.BID.Empty]);

                city[i, j] = temp;
                temp.GetComponent<CityTile>().location = new int[2] { i, j };

                Transform temp2 = temp.transform;

                temp2.parent = gameObject.transform;
                temp2.position = temp2.position + new Vector3(2 * j, 0, 2 * i);
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        timeElapsed += Time.deltaTime; //Timer
        if(counter >= agentCalls) //Every X smallUpdates
        {
            StaticValues.weekNo++;
            bigUpdateTrigger(); //Call all tiles bigUpdates
            counter = 0; //Reset counter

            if (StaticValues.cityMoney < 0) //If the player is bankrupt
            {
                StaticValues.winStrikes = 0; //Reset winning counter
                if (StaticValues.loseStrikes == 3)
                {
                    levelChanges.LoadLevelNumber(2); //Game over
                }
                else
                {
                    StaticValues.loseStrikes++; //Closer to game over
                    soundManager.PlayingSound(10,0.75f); //Warning
                }
            }
            else if (StaticValues.cityMoney > StaticValues.goalMoney)
            {
                StaticValues.loseStrikes = 0;
                if (StaticValues.winStrikes == 3) //Reached target for 3 months?
                {
                    levelChanges.LoadLevelNumber(1); //Level Complete!
                }
                else
                {
                    StaticValues.winStrikes++;
                    soundManager.PlayingSound(5,0.75f);
                }
            }
            else //Reset both win and lose
            {
                StaticValues.winStrikes = 0;
                StaticValues.loseStrikes = 0;
            }
        }

        if(timeElapsed >= timeSmallInterval) //Call smallupdate if enough time has elapsed
        {
            smallUpdateTrigger(counter); //Call all subscribed methods
            counter++;//count the number of small updates so bigupdate can be triggered
            timeElapsed = 0; //reset timer
            StaticValues.AssignIncomingOutgoing(); //Complete incoming outgoing lists
        }
    }
}
