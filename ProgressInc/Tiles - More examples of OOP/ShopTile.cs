using UnityEngine;

public class ShopTile : ProductionTile{

    //Inherited Lists are for incoming industrial goods, and outgoing shop goods
    int maxIncomingPeople = 2;

    //Additional vars for second agent type
    public int peopleUnitsAcquired = 0;
    public int incomingPeopleNo = 0;

    public void Start()
    {
        tileType = 2;
    }

    /// <summary>
    /// Adds functionality for second agent type
    /// </summary>
    public override void BigUpdate()
    {
        base.BigUpdate();

        while (peopleUnitsAcquired >= unitsRequired)
        {
            peopleUnitsAcquired -= unitsRequired;
            StaticValues.cityMoney += tax;
            Instantiate(dollar, gameObject.transform.position + new Vector3(0, 0.1f, 0), gameObject.transform.rotation);
        }
    }

    /// <summary>
    /// Checks for incoming and outgoing slots and adds them to the static lists if necessary.
    /// </summary>
    protected override void outgoingIncoming()
    {
        if (!destroying)
        {
            if (incomingNo < maxIncoming)
            {
                StaticValues.openIncomingIndustryList.Add(this); //Add to home list because it needs goods from shops
            }

            if (incomingPeopleNo < maxIncomingPeople)
            {
                StaticValues.openIncomingHomeList.Add(this);
            }

            if (outgoing.Count < maxOutgoing)
            {
                StaticValues.openOutgoingShopList.Add(this); //Add to home list because it gives out people
            }
        }
    }

    /// <summary>
    /// Recieves two types of incoming agents
    /// </summary>
    /// <param name="type"></param>
    public override void ReceiveGoods(int type)
    {
        if(type == (int)EnumClass.BID.House)
        {
            peopleUnitsAcquired++;
        }
        else if(type == (int)EnumClass.BID.Factory)
        {
            unitsAcquired++;
        }
    }

    /// <summary>
    /// if new incoming link, checks the link and increments accordingly.
    /// </summary>
    /// <param name="g"></param>
    public override  void IncomingList(GameObject g)
    {
        if (g.GetComponent<HomeTile>() != null)
        {
            incomingPeopleNo++;
        }
        else
        {
            incomingNo++;
        }
    }

    /// <summary>
    /// if a link is broken, checks the link and reduces the incoming number accordingly
    /// </summary>
    /// <param name="g"></param>
    public override void SearchAndRemoveIncomingGameObject(GameObject g)
    {
        if(g.GetComponent<HomeTile>() != null)
        {
            incomingPeopleNo--;
        }
        else
        {
            incomingNo--;
        }
    }
}
