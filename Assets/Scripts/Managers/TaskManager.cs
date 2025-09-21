using MEC;
using ProjectEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour {

	public static TaskManager instance;

	[System.NonSerialized] public TaskAsset currentTaskAsset;
	[System.NonSerialized] public LocID currentTask;
	[System.NonSerialized] public int currentTaskIndex = 0;

	private LocID queuedTaskComplete = LocID.None;

	private CoroutineHandle changeNextTaskRoutine;

	private void Awake() {
		if (instance == null) {
			instance = this;
			SubscribeEvents();
		}
		else {
			Destroy(gameObject);
		}
	}

	private void OnDestroy() {
		if (instance != this) {
			instance = null;
			UnsubscribeEvents();
		}
	}

	private void SubscribeEvents() {
		LocIDVariable.RegisterListener(LocID.TaskComplete, CompleteTask);
	}

	private void UnsubscribeEvents() {
		LocIDVariable.UnregisterListener(LocID.TaskComplete, CompleteTask);
	}

	public static bool TaskIsComplete(LocID locID) {
		bool taskComplete = LocIDVariable.SceneValueExists(locID);
		return taskComplete;
	}

	public void ChangeCurrentTaskAsset(TaskAsset task) {
		currentTaskAsset = Instantiate(task);
		currentTaskAsset.taskList = new List<LocID>();
		currentTaskAsset.taskList.Add(task.taskList[task.taskList.Count - 1]);
		currentTask = currentTaskAsset.taskList[0];
		currentTaskIndex = 0;
	}

	public static bool TaskIsActive(LocID task) {
		bool isActive = false;
		if (TaskManager.instance.currentTaskAsset.showAllTasks == true) {
			for (int i = 0; i < TaskManager.instance.currentTaskAsset.taskList.Count; i++) {
				if (TaskManager.instance.currentTaskAsset.taskList[i] == task) {
					isActive = true;
					break;
				}
			}
		}
		else {
			if (TaskManager.instance.currentTask == task) {
				isActive = true;
			}
		}
		return isActive;
	}

	public bool CheckIfCurrentTaskGroupComplete() {
		for (int i = 0; i < currentTaskAsset.taskList.Count; i++) {
			if (TaskManager.TaskIsComplete(currentTaskAsset.taskList[i]) == false) {
				return false;
			}
		}
		return true;
	}

	public void CompleteTask(LocID locID) {
		if (currentTask == locID && changeNextTaskRoutine.IsValid == false) {
			CompleteTask(locID, false);
		}
	}

	public void CompleteTask(LocID locID, bool queueTaskChangeRoutine = false) {
		if (TaskManager.TaskIsComplete(locID) == false) {
			TaskCompletedFirstTime(locID);
		}
		if (CheckIfCurrentTaskGroupComplete() == true) {
			TaskGroupComplete();
		}
		if (queueTaskChangeRoutine == false) {
			AnnounceManager.Announce(LocalizationManager.instance.GetString(LocID.TaskComplete), 1F);
			changeNextTaskRoutine = Timing.RunCoroutine(ChangeNextTaskRoutine(locID, 0.8F));
		}
		else {
			queuedTaskComplete = locID;
		}
	}

	private IEnumerator<float> ChangeNextTaskRoutine(LocID locID, float delay = 0F) {
		if (delay > 0F) {
			yield return Timing.WaitForSeconds(delay);
		}
		if (TaskPanel.instance != null) {
			TaskPanel.instance.CompleteTask(locID);
		}
		ChangeNextTask();
		changeNextTaskRoutine = new CoroutineHandle(); // Resets isValid status
	}

	private void ChangeNextTask() {
		currentTaskIndex++;
		if (currentTaskIndex != -1 && currentTaskIndex < currentTaskAsset.taskList.Count) {
			currentTask = currentTaskAsset.taskList[currentTaskIndex];
			if (currentTaskAsset.CheckTaskSkip(currentTask) == true) {
				LocIDVariable.SetSceneValue(currentTask, LocID.Ok);
				ChangeNextTask();
				return;
			}
			//TaskPanel.instance.InitializeInfoButton();
		}
		else {
			currentTask = LocID.None;
		}
		LocIDVariable.SetSceneValue(LocID.Action_ChangeNextTask, currentTask);
	}

	public void CompleteCurrentTask(bool queueTaskCompletion = false) {
		CompleteTask(currentTask, queueTaskCompletion);
	}

	public void ShowQueuedChangeTask() {
		if (queuedTaskComplete != LocID.None) {
			AnnounceManager.Announce(LocalizationManager.instance.GetString(LocID.TaskComplete), 1F);
			if (TaskPanel.instance != null) {
				TaskPanel.instance.CompleteTask(queuedTaskComplete);
			}
			ChangeNextTask();
			queuedTaskComplete = LocID.None;
		}
	}

	private void TaskCompletedFirstTime(LocID locID) {
		LocIDVariable.SetSceneValue(currentTask, LocID.Ok);
		TaskCompleteTrigger.TaskCompleted(locID);
	}

	public void TaskGroupComplete() {
		if (TaskManager.TaskIsComplete(currentTaskAsset.taskGroupLocID) == false) {
			LocIDVariable.SetSceneValue(currentTaskAsset.taskGroupLocID, LocID.Ok);
			//TaskCompleteTrigger.TaskCompleted(currentTaskAsset.taskGroupLocID);
			//WindowManager.instance.CloseWindowsInLocation(WindowLocation.Console);
			//WindowManager.instance.OpenWindow(WindowPanel.EndScreen);
		}
	}

	public bool AllTasksCompleted() {
		//if (UserProfile.CurrentProfile.completedTasks.Contains(LocID.Task_GameComplete) == true) {
		//	WindowManager.instance.CloseWindow(WindowPanel.TaskPanel);
		//	return true;
		//}
		return false;
	}

	public void BlinkTaskText(Color color) {
		Timing.RunCoroutine(BlinkTaskTextRoutine(color));
	}

	private IEnumerator<float> BlinkTaskTextRoutine(Color color) {
		yield return Timing.WaitUntilDone(Timing.RunCoroutine(Tools.ChangeTextColor(TaskPanel.instance.taskText, color, 0.1F)));
		yield return Timing.WaitUntilDone(Timing.RunCoroutine(Tools.ChangeTextColor(TaskPanel.instance.taskText, GameManager.instance.settings.taskTextNormalColor, 0.1F)));
	}

}
