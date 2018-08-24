using System;

public class EmptyTile : CityTile {

    public int typeToBecome;

    public void Start()
    {
        tileType = 0;
    }

    /// <summary>
    /// "destroys" the tile by building it into the typeToBecome
    /// </summary>
    public override void BigUpdate()
    {
        if (destroying)
        {
            destroyCount++;
            if (destroyCount >= 2)
            {
                StaticValues.cityMoney -= destroyCost;
                //Change tile to typeToBecome number
                ChangeTile(gameObject.GetComponentInParent<CityGrid>().ReturnObject((string)Enum.GetName(typeof(EnumClass.BID),typeToBecome)));
            }
        }
        else
        {
            destroyCount = 0;
        }
    }

    public override void SmallUpdate(int count)
    {
        //No need
    }
}
