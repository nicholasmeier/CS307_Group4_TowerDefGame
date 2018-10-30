using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterVolume : MonoBehaviour {

    private AudioSource audioSource;
    private float musicVolume = 1f;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }
    private void Update()
    {
        audioSource.volume = musicVolume;
    }
    public void setVolume(float vol){
        musicVolume = vol;
    }
}
