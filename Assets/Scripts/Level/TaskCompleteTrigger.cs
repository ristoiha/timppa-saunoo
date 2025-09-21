using ProjectEnums;
using RoboRyanTron.SearchableEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskCompleteTrigger : MonoBehaviour {

	public static List<TaskCompleteTrigger> taskCompleteTriggers = new List<TaskCompleteTrigger>();
	[SearchableEnum] public LocID triggeringLocID;

	public abstract void TriggerActions();

	protected virtual void Awake() {
		taskCompleteTriggers.Add(this);
	}

	public static void TaskCompleted(LocID locID) {
		for (int i = 0; i < taskCompleteTriggers.Count; i++) {
			if (taskCompleteTriggers[i].triggeringLocID == locID) {
				taskCompleteTriggers[i].TriggerActions();
			}
		}
	}

	private void OnDestroy() {
		taskCompleteTriggers.Remove(this);
	}


}
