using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileInfoGenerator : MonoBehaviour {

    GameObject previousObject;
    [SerializeField]
    Text field1;
    [SerializeField]
    Text field2;
    [SerializeField]
    Text field3;
    [SerializeField]
    Text field4;
    [SerializeField]
    Text field5;
    [SerializeField]
    Text field6;

    /// <summary>
    /// Gets data from the tiles and outputs it to the Text fields
    /// </summary>
    /// <param name="hover"></param>
    public void UpdateInfo(GameObject hover)
    {
        if(hover != previousObject) //Save computation if no change is found
        {
            previousObject = hover;
            CityTile city = hover.GetComponent<CityTile>();
            switch (city.tileType)
            {
                case 0: //Empty
                    field1.text = "Empty";
                    field2.text = "";
                    field3.text = "";
                    field4.text = "";
                    field5.text = "";
                    field6.text = "";
                    break;
                case 1: //House
                    HomeTile h = city.GetComponent<HomeTile>();
                    field1.text = "House";
                    field2.text = "Cost $" + city.cost;
                    field3.text = "Income: $" + city.tax;
                    field4.text = "Resources: " + h.unitsAcquired + "/" + h.unitsRequired;
                    field5.text = "Power: " + TrueFalse(city.power);
                    field6.text = "Water: " + TrueFalse(city.water);
                    break;
                case 2: //Shop
                    ShopTile s = city.GetComponent<ShopTile>();
                    field1.text = "Shop";
                    field2.text = "Cost: $" + city.cost;
                    field3.text = "Income: $" + city.tax;
                    field4.text = "Resources:" + s.unitsAcquired + "/" + s.unitsRequired + " " + s.peopleUnitsAcquired + "/" + s.unitsRequired;
                    field5.text = "Power: " + TrueFalse(city.power);
                    field6.text = "Water: " + TrueFalse(city.water);
                    break;
                case 3: //Factory
                    IndustryTile n = city.GetComponent<IndustryTile>();
                    field1.text = "Factory";
                    field2.text = "Cost: $" + city.cost;
                    field3.text = "Income: $" + city.tax;
                    field4.text = "Resources: "+ n.unitsAcquired +"/" + n.unitsRequired;
                    field5.text = "Power: " + TrueFalse(city.power);
                    field6.text = "Water: " + TrueFalse(city.water);
                    break;
                case 4: //Destroyed?
                    field1.text = "NULL";
                    field2.text = "";
                    field3.text = "";
                    field4.text = "";
                    field5.text = "";
                    field6.text = "";
                    break;
                case 5: //Power
                    field1.text = "PowerPlant";
                    field2.text = "Cost: $" + city.cost;
                    field3.text = "";
                    field4.text = "Powering city...";
                    field5.text = "";
                    field6.text = "";
                    break;
                case 6: //Water
                    field1.text = "WaterWorks";
                    field2.text = "Cost: $" + city.cost;
                    field3.text = "";
                    field4.text = "Pumping water...";
                    field5.text = "";
                    field6.text = "";
                    break;
                case 7: //Road
                    field1.text = "Road";
                    field2.text = "Cost: $" + city.cost;
                    field3.text = "";
                    field4.text = "";
                    field5.text = "Power: " + TrueFalse(city.power);
                    field6.text = "Water: " + TrueFalse(city.water);
                    break;
            }
        }
    }

    /// <summary>
    /// Transforms a bool into "yes" or "no" string
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    private string TrueFalse(bool b)
    {
        if(b)
        {
            return "Yes";
        }
        else
        {
            return "No";
        }
    }
}
