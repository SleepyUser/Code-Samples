using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticValues : MonoBehaviour {
    //Static for ease of use as only one will exist per scene.
    public static int cityMoney = 4000;
    public int startingMoney = 3000;
    public int levelGoal = 5000;
    public static int goalMoney = 99999999;
    public static int loseStrikes = 0;
    public static int weekNo = 0;
    public static int winStrikes = 0;

    //Lists for what tiles are needing
    public static List<ProductionTile> openIncomingHomeList = new List<ProductionTile>();
    public static List<ProductionTile> openIncomingShopList = new List<ProductionTile>();
    public static List<ProductionTile> openIncomingIndustryList = new List<ProductionTile>();


    //Lists for what tiles are offering
    public static List<ProductionTile> openOutgoingHomeList = new List<ProductionTile>();
    public static List<ProductionTile> openOutgoingShopList = new List<ProductionTile>();
    public static List<ProductionTile> openOutgoingIndustryList = new List<ProductionTile>();

    /// <summary>
    /// Reset & Initialise the static variables
    /// </summary>
    public void Start()
    {
        cityMoney = startingMoney;
        goalMoney = levelGoal;
        loseStrikes = 0;
        winStrikes = 0;
        weekNo = 0;
    }


    /// <summary>
    /// Assigns members of each list to each other to test if they can be linked.
    /// </summary>
    public static void AssignIncomingOutgoing()
    {
        foreach(ProductionTile p in StaticValues.openOutgoingHomeList)
        {
            if (openIncomingHomeList.Count > 0)
            {
                int temp = Random.Range(0, openIncomingHomeList.Count - 1);
                p.AddToLists(openIncomingHomeList[temp].gameObject);
                openIncomingHomeList.RemoveAt(temp);
            }
        }
        foreach (ProductionTile p in StaticValues.openOutgoingIndustryList)
        {
            if (openIncomingIndustryList.Count > 0)
            {
                int temp = Random.Range(0, openIncomingIndustryList.Count - 1);
                p.AddToLists(openIncomingIndustryList[temp].gameObject);
                openIncomingIndustryList.RemoveAt(temp);
            }
        }
        foreach (ProductionTile p in StaticValues.openOutgoingShopList)
        {
            if (openIncomingShopList.Count > 0)
            {
                int temp = Random.Range(0, openIncomingShopList.Count - 1);
                p.AddToLists(openIncomingShopList[temp].gameObject);
                openIncomingShopList.RemoveAt(temp);
            }
        }
        ClearLists();
    }

    /// <summary>
    /// Resets lists for the next big update
    /// </summary>
    public static void ClearLists()
    {
            openIncomingHomeList.Clear();
            openIncomingShopList.Clear();
            openIncomingIndustryList.Clear();
            openOutgoingHomeList.Clear();
            openOutgoingShopList.Clear();
            openOutgoingIndustryList.Clear();
    }
}
