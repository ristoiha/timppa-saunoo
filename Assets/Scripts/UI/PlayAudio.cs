using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectEnums;
using RoboRyanTron.SearchableEnum;

public class PlayAudio : MonoBehaviour {

	public bool playSound = true;
	public AudioID overrideAudioID;
	
	private AudioID audioID = AudioID.Effect_ButtonClick2;

	private void Awake() {
		if (overrideAudioID != AudioID.None) {
			audioID = overrideAudioID;
		}
	}

	public void ButtonClicked() {
		if (playSound == true) {
			AudioManager.PlayAudio(audioID);
		}
	}

	public void ToggleClicked(bool val) {
		if (playSound == true) {
			AudioManager.PlayAudio(audioID);
		}
	}
	public void ButtonReleased() {
		if (playSound == true) {
			AudioIDVariable.SetSceneValue(LocID.Action_EndAudio, audioID);
		}
	}

}
