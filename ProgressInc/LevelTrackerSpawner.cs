using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrackerSpawner : MonoBehaviour {

    [SerializeField]
    GameObject prefab;

	// Use this for initialization
	void Start () {
		if(GameObject.FindGameObjectWithTag("Eternal") == null) //If there's not already a level tracker, instantiate one
        {
            Instantiate(prefab);
        }
	}
}
