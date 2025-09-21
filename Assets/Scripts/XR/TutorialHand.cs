using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.SearchableEnum;
using ProjectEnums;

public class TutorialHand : MonoBehaviour {

	public Animator anim;
	[SearchableEnum] public List<LocID> taskWhenActive = new List<LocID>();
	[SearchableEnum] public AnimationName animationName;

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

	private void CheckActivate(LocID task) {
#if VR
		if (taskWhenActive.Contains(task) == true) {
			gameObject.SetActive(true);
			anim.Play(animationName.ToString(), -1, 0F);
		}
		else {
			gameObject.SetActive(false);
		}
#else
		gameObject.SetActive(false);
#endif
	}

}
