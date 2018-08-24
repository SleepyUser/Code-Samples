using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(DestroySelfCoroutine());
	}
	
	IEnumerator DestroySelfCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
    
}
