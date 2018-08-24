public class HomeTile : ProductionTile {

    public void Start()
    {
        tileType = 1;
    }

    /// <summary>
    /// checks for incoming shop agent slots, and any open outgoing slots
    /// </summary>
    protected override void outgoingIncoming()
    {
        if (!destroying)
        {
            if (incomingNo < maxIncoming)
            {
                StaticValues.openIncomingShopList.Add(this); //Add to home list because it needs goods from shops
            }

            if (outgoing.Count < maxOutgoing)
            {
                StaticValues.openOutgoingHomeList.Add(this); //Add to home list because it gives out people
            }
        }
    }
}
