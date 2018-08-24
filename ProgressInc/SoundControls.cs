using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundControls : MonoBehaviour {

    [SerializeField]
    AudioClip[] clips = new AudioClip[5]; //Set of audio clips to play

    AudioSource source; 

    void Awake()
    {
        source = GetComponent<AudioSource>(); //Finds audiosource on gameobject
    }

    public void PlayingSound(int index, float volume)
    {
        source.PlayOneShot(clips[index], volume); //plays given audio clip
    }

}
