public class IndustryTile : ProductionTile {

    public void Start()
    {
        tileType = 3;
    }

    /// <summary>
    /// Checks for outgoing industry slots and incoming people slots.
    /// </summary>
    protected override void outgoingIncoming()
    {
        if (!destroying)
        {
            if (incomingNo < maxIncoming)
            {
                StaticValues.openIncomingHomeList.Add(this); //Add to home list because it needs people from homes
            }

            if (outgoing.Count < maxOutgoing)
            {
                StaticValues.openOutgoingIndustryList.Add(this); //Add to home list because it gives out industrial goods
            }
        }
    }
}
