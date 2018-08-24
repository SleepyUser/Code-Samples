using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeTile : MonoBehaviour {

    int tileType;
    public int clickType = -1; //-1 means clicks before UI is clicked do nothing
    [SerializeField]
    TileInfoGenerator tileInfo;
    [SerializeField]
    GameObject InfoPanel;

    /// <summary>
    /// UI Interaction function, allows changes to interactions with tiles
    /// </summary>
    /// <param name="clickTypeChange"> see switch statement in changetile script for numbers</param>
    public void ChangeButton(int clickTypeChange)
    {
        clickType = clickTypeChange;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || InfoPanel.activeSelf) //saves on computation if infopanel is closed, and mouse is not clicked
        {
            if (!EventSystem.current.IsPointerOverGameObject()) //Does not work over UI
            {
                Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition); //Casts ray to point on screen from cursor
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    GameObject hitObject = hit.transform.gameObject;

                    if(InfoPanel.activeSelf) //If info panel is active
                    {
                        tileInfo.UpdateInfo(hitObject);
                    }

                    if (Input.GetMouseButtonDown(0)) //If clicked
                    {
                        switch (clickType) //Switch to detemine what to do with the click. Clicktype is changed by UI buttons
                        {
                            case 0: //Destroy
                                if (hitObject.GetComponent<CityTile>() != null && hitObject.tag != "TEmpty")
                                {
                                    CityTile temp = hitObject.GetComponent<CityTile>();
                                    temp.destroying = !hitObject.GetComponent<CityTile>().destroying;
                                    temp.ConstructionToggle();
                                }
                                break;
                            case 1: //Build Home
                            case 2: //Build Shop
                            case 3: //Build Factory
                                    //case 4: //Build Destroyed - Not possible
                            case 5: //Build Power
                            case 6: //Build Water
                            case 7: //Build Road
                                if (hitObject.GetComponent<EmptyTile>() != null)
                                {
                                    EmptyTile temp = hitObject.GetComponent<EmptyTile>();
                                    temp.destroying = !temp.destroying;
                                    temp.typeToBecome = clickType;
                                    temp.ConstructionToggle();
                                }
                                break;
                        };
                    }
                }
            }
        }
    }
}
