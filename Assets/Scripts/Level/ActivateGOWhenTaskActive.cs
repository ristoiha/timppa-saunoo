using ProjectEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RoboRyanTron.SearchableEnum;

public class ActivateGOWhenTaskActive : MonoBehaviour {

	[SearchableEnum] public LocID relatedTask;

	private void Awake() {
		SubscribeEvents();
		CheckActivate(TaskManager.instance.currentTask);
	}

	private void OnDestroy() {
		UnsubscribeEvents();
	}

	private void SubscribeEvents() {
		LocIDVariable.RegisterListener(LocID.Action_ChangeNextTask, CheckActivate);
	}

	private void UnsubscribeEvents() {
		LocIDVariable.UnregisterListener(LocID.Action_ChangeNextTask, CheckActivate);
	}

	private void CheckActivate(LocID currentTask) {
		if (relatedTask == currentTask) {
			gameObject.SetActive(true);
		}
		else {
			gameObject.SetActive(false);
		}
	}

}
