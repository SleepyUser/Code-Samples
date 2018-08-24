using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {

    int type;
    public bool recalc = false;
    public bool recalcDest = false;
    public GameObject[] route;
    public float totalTravelTime;
    private float timeBetweenNodes;
    [SerializeField]
    private List<Material> colours;

    public void Initialise(int typeTemp, GameObject[] routeArray)
    {
        type = typeTemp;
        SetAgentType();
        route = routeArray;
        totalTravelTime = GameObject.FindGameObjectWithTag("CitySpawn").GetComponent<CityGrid>().timeSmallInterval;
        timeBetweenNodes = totalTravelTime / (float)route.Length;
        StartCoroutine(MoveObjectToDest(timeBetweenNodes));
    }


    /// <summary>
    /// Sets the colour of the agent based on the type of tile it comes from
    /// </summary>
    void SetAgentType()
    {
        Material tempMat;

        switch (type) //Include all types that use agents
        {
            case (int)EnumClass.BID.House:
                tempMat = colours[0];
                gameObject.layer = (int)EnumClass.BID.House + 7;
                break;
            case (int)EnumClass.BID.Shop:
                tempMat = colours[1];
                gameObject.layer = (int)EnumClass.BID.Shop + 7;
                break;
            case (int)EnumClass.BID.Factory:
                tempMat = colours[2];
                gameObject.layer = (int)EnumClass.BID.Factory + 7;
                break;
            case (int)EnumClass.BID.Power:
                tempMat = colours[3];
                gameObject.layer = (int)EnumClass.BID.Power + 7;
                break;
            case (int)EnumClass.BID.Water:
                tempMat = colours[4];
                gameObject.layer = (int)EnumClass.BID.Water + 7;
                break;
            default: //Should never be used
                tempMat = colours[5];
                break;
        }
        GetComponent<Renderer>().material = tempMat;
    }

    /// <summary>
    /// Moves the agent to the destination along the route. 
    /// Also checks for if parts of the route will be destroyed soon.
    /// Calls the recievegoods on the destination tile.
    /// Activates the return to home subroutine.
    /// </summary>
    /// <param name="duration">the length of time to move between squares</param>
    /// <returns></returns>
    IEnumerator MoveObjectToDest(float duration)
    {
        float startTime = Time.time;
        for (int i = 0; i<route.Length; i++)
        {
            if (route[i]!= null  && route[i].GetComponent<CityTile>().destroying == true ) //Sets recalc to true if any tiles are going to be destroyed
            {
                recalc = true;
            }

            Vector3 positionTemp;
            positionTemp = transform.position;
            while (Time.time < startTime + duration)
            {
                this.transform.position = Vector3.Lerp(positionTemp, route[i].transform.position+new Vector3(0.0f,0.1f,0.0f), (Time.time - startTime) / duration);
                yield return null;
            }
            startTime += duration;
            transform.position = route[i].transform.position + new Vector3(0.0f, 0.1f, 0.0f);
        }
        recalcDest = route[route.Length - 1].GetComponent<ProductionTile>().destroying;
        route[route.Length - 1].GetComponent<ProductionTile>().ReceiveGoods(type);
        StartCoroutine(MoveObjectToHome(timeBetweenNodes));
    }
    
    /// <summary>
    /// Reverse of the above, moves the object back along the "route" array.
    /// Calls the recieveagent on the home tile.
    /// Destroys the game object.
    /// </summary>
    /// <param name="duration">the length of time to move between squares</param>
    /// <returns></returns>
    IEnumerator MoveObjectToHome(float duration)
    {
        float startTime = Time.time;
        for (int i = route.Length-1; i >= 0; i--)
        {
            Vector3 positionTemp;
            positionTemp = transform.position;
            while (Time.time < startTime + duration)
            {
                this.transform.position = Vector3.Lerp(positionTemp, route[i].transform.position + new Vector3(0.0f, 0.1f, 0.0f), (Time.time - startTime) / duration);
                yield return null;
            }
            startTime += duration;
            transform.position = route[i].transform.position + new Vector3(0.0f, 0.1f, 0.0f);
        }
        route[0].GetComponent<ProductionTile>().RecieveAgent(recalc, recalcDest, route);
        Destroy(gameObject);
    }
}
