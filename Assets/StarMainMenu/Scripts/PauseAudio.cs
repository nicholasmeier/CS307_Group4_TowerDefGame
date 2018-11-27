using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseAudio : MonoBehaviour {

    // Use this for initialization
    public void Pause () {
        DontDestroy.Instance.gameObject.GetComponent<AudioSource>().Pause();
	}
    public void Play() {
        DontDestroy.Instance.gameObject.GetComponent<AudioSource>().Play();
    }
	// Update is called once per frame
	void Update () {
		
	}
}
