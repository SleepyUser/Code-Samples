using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICamFilters : MonoBehaviour {

    [SerializeField]
    Toggle[] toggleArray = new Toggle[6];

    public void CalcFilter()
    {
        List<string> layersToShow = new List<string>() { "Default", "TransparentFX", "IgnoreRaycast", "Water", "UI" };

        for(int i = 0; i<6; i++)
        {
            if(i!=3)
            {
                if(toggleArray[i].isOn)
                {
                    layersToShow.Add(LayerMask.LayerToName(i + 8));
                }
            }
        }
        ApplyFilter(layersToShow);
    }

    private void ApplyFilter(List<string> layers)
    {
        Camera.main.cullingMask = LayerMask.GetMask(layers.ToArray());
    }
}
