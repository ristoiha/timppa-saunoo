using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ProjectEnums;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using MEC;

public class TaskPanel : WindowBase {

	public static TaskPanel instance;

	public TextMeshProUGUI taskText;
	public TextMeshProUGUI headerText;

	[System.NonSerialized]
	public CoroutineHandle changeTaskRoutine;

	private TaskAsset currentTaskAsset;
	private bool disabledOnce = false;

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		if (instance == null) {
			instance = this;
			base.Init(parameters, windowStack);
			//ChangeTaskGroup(TaskManager.instance.currentTaskAsset);
		}
		else {
			Destroy(gameObject);
		}
	}

	protected override void OnDestroy() {
		base.OnDestroy();
		if (changeTaskRoutine != null ) {
			Timing.KillCoroutines(changeTaskRoutine);
		}
		if (instance == this) {
			instance = null;
		}
	}

	private void OnEnable() {
		if (TaskManager.instance.currentTask != LocID.None) {
			UpdateUI();
		}
		if (disabledOnce == true) {
			//LocIDVariable.SetSceneValue(LocID.TaskComplete, LocID.Task_LookAtWrist);
		}
	}

	private void OnDisable() {
		disabledOnce = true;
	}

	public void CompleteTask(LocID locID, bool showSuccess = true) {
		if (gameObject.activeInHierarchy == true) {
			changeTaskRoutine = Timing.RunCoroutine(ChangeTaskRoutine(showSuccess));
		}
	}

	public void ChangeTaskGroup(TaskAsset taskAsset) {
		currentTaskAsset = taskAsset;
		headerText.text = LocalizationManager.instance.GetString(taskAsset.taskGroupLocID);
		taskText.color = GameManager.instance.settings.taskTextNormalColor;
		UpdateUI();
	}

	public override void UpdateUI() {
		taskText.text = "";
		taskText.color = GameManager.instance.settings.taskTextNormalColor;
		if (canvasGroup != null) {
			canvasGroup.alpha = 1F;
		}
		if (currentTaskAsset != null) {
			if (currentTaskAsset.showAllTasks == true) {
				for (int i = 0; i < currentTaskAsset.taskList.Count; i++) {
					bool isComplete = TaskManager.TaskIsComplete(currentTaskAsset.taskList[i]);
					if (isComplete == true) {
						string color = ColorUtility.ToHtmlStringRGB(GameManager.instance.settings.taskTextSuccessColor);
						taskText.text += "<color=#" + color + ">";
					}
					taskText.text += GetTaskString(currentTaskAsset.taskList[i]);
					if (isComplete == true) {
						taskText.text += "</color>";
					}
					if (i != currentTaskAsset.taskList.Count - 1) {
						taskText.text += "<br>";
					}
				}
			}
			else {
				headerText.text = LocalizationManager.instance.GetString(currentTaskAsset.taskGroupLocID) + 
					" ( " + (TaskManager.instance.currentTaskIndex + 1).ToString("0") + " / " + 
					currentTaskAsset.taskList.Count.ToString() + " )";
				bool isComplete = TaskManager.TaskIsComplete(TaskManager.instance.currentTask);
				if (isComplete == true) {
					string color = ColorUtility.ToHtmlStringRGB(GameManager.instance.settings.taskTextSuccessColor);
					taskText.text += "<color=#" + color + ">";
				}
				taskText.text += GetTaskString(TaskManager.instance.currentTask);
				if (isComplete == true) {
					taskText.text += "</color>";
				}
			}
		}
		else {
			taskText.text = "";
		}
	}

	private string GetTaskString(LocID taskLocID) {
		string returnString;
		//if (taskLocID == LocID.Task_SelectCategory) {
		//	returnString = LocalizationManager.GetFormattedString(taskLocID, new List<string>() { LocalizationManager.instance.GetString(TaskManager.instance.currentTaskAsset.programCategory) });
		//}
		//else {
			returnString = LocalizationManager.instance.GetString(taskLocID);
		//}
		return returnString;
	}

	private IEnumerator<float> ChangeTaskRoutine(bool showSuccess = false) {
		if (showSuccess == true) {
			yield return Timing.WaitUntilDone(Timing.RunCoroutine(Tools.ChangeTextColor(TaskPanel.instance.taskText, GameManager.instance.settings.taskTextSuccessColor, 0.3F)));
			yield return Timing.WaitForSeconds(2F);
		}
		float fadeDuration = 0.2F;
		yield return Timing.WaitUntilDone(WindowManager.instance.HideWindow(WindowPanel.TaskPanel, fadeDuration));
		if (TaskManager.instance.currentTask != LocID.None) {
			UpdateUI(); // updates current task text
			taskText.color = GameManager.instance.settings.taskTextNormalColor;
			yield return Timing.WaitUntilDone(WindowManager.instance.ShowWindow(WindowPanel.TaskPanel, fadeDuration));
		}
	}

	protected override void OpeningAnimationFinished() {

	}

	protected override void Closing() {
		if (instance == this) {
			instance = null;
		}
	}

	protected override void Destroying() {

	}

}
