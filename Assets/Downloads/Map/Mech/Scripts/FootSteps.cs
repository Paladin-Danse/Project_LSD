using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour {

	public AudioClip audioFootStep;

	AudioSource ASFootStep;

	void Start () {
		ASFootStep = GetComponent<AudioSource> ();
        ASFootStep.outputAudioMixerGroup = SoundManager.instance.UISound.outputAudioMixerGroup;
        ASFootStep.clip = audioFootStep;
	}
	
	void FootStep() {
		ASFootStep.Play ();
	}
}
