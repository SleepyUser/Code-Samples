using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTracker : MonoBehaviour {

    public int previousLevel = 0;
    public int currentLevel = 0;

    /// <summary>
    /// Make this object eternal to track levels
    /// </summary>
	void Awake ()
    {
        DontDestroyOnLoad(transform.gameObject);
	}
    
    //Recieve notification when scene is loaded
    void OnEnable()
    {
        SceneManager.sceneLoaded += Loadedscene;
    }

    //Unsubscribes
    void OnDisable()
    {
        SceneManager.sceneLoaded -= Loadedscene;
    }

    /// <summary>
    /// Called when a scene is loaded, buffers the level number to allow references to previous level.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void Loadedscene(Scene scene, LoadSceneMode mode)
    {
        scene = SceneManager.GetActiveScene();
        previousLevel = currentLevel;
        currentLevel = scene.buildIndex;
    }

}
