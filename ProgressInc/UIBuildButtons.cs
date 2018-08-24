using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildButtons : MonoBehaviour
{

    public List<Button> button = new List<Button>();
    public List<Image> iconButtons = new List<Image>();

    public GameObject buildingPanel;

    Color normal = new Color(255, 255, 255);
    Color selected = new Color(150, 150, 150);

    /// <summary>
    /// Changes all buttons in array back to normal colours
    /// See: SelectButton for reverse
    /// </summary>
    public void ClearButtons()
    {
        var colors = button[0].colors;
        colors.normalColor = normal;
        for (int i = 0; i < button.Count; i++)
        {
            button[i].colors = colors;
        }
    }

    /// <summary>
    /// Changes colour of button when it is selected.
    /// See: ClearButtons for reverse
    /// </summary>
    public void SelectButton()
    {
        var colors = GetComponent<Button>().colors;
        colors.normalColor = selected;
        GetComponent<Button>().colors = colors;
    }

    /// <summary>
    /// Toggles the "active" of entered panel
    /// </summary>
    /// <param name="panel"></param>
    public void TogglePanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
    }

    /// <summary>
    /// Used to highlight one out of many buttons if it is in the image array
    /// Deselects all other buttons
    /// </summary>
    /// <param name="g">clicked button image</param>
    public void ShowSelected(Image g)
    {
        for (int i = 0; i < iconButtons.Count; i++)
        {
            if (g == iconButtons[i])
            {

                iconButtons[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                iconButtons[i].color = new Color(1, 1, 1, 0.5f);
            }
        }
    }
}
