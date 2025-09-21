using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectEnums;
using RoboRyanTron.SearchableEnum;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class ButtonMessenger : MonoBehaviour {

	public bool playSound = true;
	public AudioID overrideAudioID;
	public List<CommandVariable> buttonActions = new List<CommandVariable>();

	private AudioID audioID = AudioID.Effect_ButtonClick2;

	private void Awake() {
		if (overrideAudioID != AudioID.None) {
			audioID = overrideAudioID;
		}
	}

	public void ButtonClicked() {
		for (int i = 0; i < buttonActions.Count; i++) {
			UIEventManager.instance.TriggerEvent(buttonActions[i], this.transform);
		}
		PlayAudio();
	}

	public void ButtonClicked(float val) {
		for (int i = 0; i < buttonActions.Count; i++) {
			if (buttonActions[i].variableType == VariableType.Dynamic) {
				buttonActions[i].floatValue = val;
			}
			UIEventManager.instance.TriggerEvent(buttonActions[i], this.transform);
		}
		PlayAudio();
	}

	public void ButtonClicked(int val) {
		for (int i = 0; i < buttonActions.Count; i++) {
			if (buttonActions[i].variableType == VariableType.Dynamic) {
				buttonActions[i].intValue = val;
			}
			UIEventManager.instance.TriggerEvent(buttonActions[i], this.transform);
		}
		PlayAudio();
	}

	public void ButtonClicked(string val) {
		for (int i = 0; i < buttonActions.Count; i++) {
			if (buttonActions[i].variableType == VariableType.Dynamic) {
				buttonActions[i].stringValue = val;
			}
			UIEventManager.instance.TriggerEvent(buttonActions[i], this.transform);
		}
		PlayAudio();
	}

	public void ButtonClicked(bool val) {
		for (int i = 0; i < buttonActions.Count; i++) {
			if (buttonActions[i].variableType == VariableType.Dynamic) {
				buttonActions[i].boolValue = val;
			}
			UIEventManager.instance.TriggerEvent(buttonActions[i], this.transform);
		}
		PlayAudio();
	}

	public void ButtonClicked(LocID val) {
		for (int i = 0; i < buttonActions.Count; i++) {
			if (buttonActions[i].variableType == VariableType.Dynamic) {
				buttonActions[i].locIDValue = val;
			}
			UIEventManager.instance.TriggerEvent(buttonActions[i], this.transform);
		}
		PlayAudio();
	}

	public void ButtonReleased() {
		PlayReleaseAudio();
	}

	public void PlayAudio() {
		if (playSound == true) {
			AudioManager.PlayAudio(audioID);
		}
	}

	public void PlayReleaseAudio() {
		if (playSound == true) {
			AudioIDVariable.SetSceneValue(LocID.Action_EndAudio, audioID);
		}
	}

}
