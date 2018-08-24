using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour {

    Text button;

    /// <summary>
    /// Load given level number
    /// </summary>
    /// <param name="levelNo"></param>
    public void LoadLevelNumber(int levelNo)
    {
        SceneManager.LoadScene(levelNo);
    }

    /// <summary>
    /// if button is assigned in the scene, and it's the max level change the button text to finish.
    /// </summary>
    void Start()
    {
        if(button != null)
        {
            LevelTracker l = GameObject.FindGameObjectWithTag("Eternal").GetComponent<LevelTracker>();
            if (l.previousLevel != SceneManager.sceneCountInBuildSettings - 1)
            {
                button.text = "Finish";
            }
        }
    }


    /// <summary>
    /// Used on victory screen. Loads "next" level. Previous level's index + 1.
    /// Allows use of only one victory screen.
    /// </summary>
    public void LoadNextLevel()
    {
        LevelTracker l = GameObject.FindGameObjectWithTag("Eternal").GetComponent<LevelTracker>();
        if (l.previousLevel != SceneManager.sceneCountInBuildSettings-1)
        {
            SceneManager.LoadScene(l.previousLevel + 1);
        }
        else
        {
            LoadLevelNumber(0);
        }
    }

    /// <summary>
    /// Quit game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
