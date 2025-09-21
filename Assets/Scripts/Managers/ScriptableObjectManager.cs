using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectEnums;
using UnityEngine.SceneManagement;
using System;

public class ScriptableObjectManager : MonoBehaviour {

	public static ScriptableObjectManager instance;

	public TextAudioPairAsset[] textAudioPairs;
	public WindowStyleAsset[] windowStyles;
	public VariableContent[] variableOptions;
	public TaskAsset[] tasks;
	//public TaskAsset tutorialTask;
	//public TaskAsset tutorialTaskMobile;

	private bool debug = false;

	private void Awake() {
		if (instance == null) {
			instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}


	private void OnDestroy() {
		if (instance == this) {
			instance = null;
		}
	}
	public void OnSceneChange() {
		LocIDVariable.sceneValues.Clear();
		FloatVariable.sceneValues.Clear();
		IntVariable.sceneValues.Clear();
		BoolVariable.sceneValues.Clear();
		StringVariable.sceneValues.Clear();
	}

	public TextAudioPairAsset GetTextAudioPairBySpeechID(SpeechID speechID) {
		for (int i = 0; i < textAudioPairs.Length; i++) {
			if (textAudioPairs[i].speechID == speechID) {
				return textAudioPairs[i];
			}
		}
		if (debug == true) Debug.LogError("TextAudioPair asset not found by " + speechID.ToString());
		return null;
	}

	public WindowStyleAsset GetWindowStyle(WindowStyle windowStyle) {
		for (int i = 0; i < windowStyles.Length; i++) {
			if (windowStyles[i].style == windowStyle) {
				return windowStyles[i];
			}
		}
		if (debug == true) Debug.LogError("WindowStyle asset not found by " + windowStyle.ToString());
		return null;
	}

	public VariableContent GetVariableOption(LocID option) {
		for (int i = 0; i < variableOptions.Length; i++) {
			if (variableOptions[i].locID == option) {
				variableOptions[i].Initialize(option);
				return variableOptions[i];
			}
		}
		if (debug == true) Debug.LogError("VariableOption asset not found by " + option.ToString());
		return null;
	}

	public TaskAsset GetTask(LocID task) {
		for (int i = 0; i < tasks.Length; i++) {
			if (tasks[i].taskGroupLocID == task) {
				return tasks[i];
			}
		}
		if (debug == true) Debug.LogError("Task asset not found by " + task.ToString());
		return null;
	}

}
