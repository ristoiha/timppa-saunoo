using ProjectEnums;
using RoboRyanTron.SearchableEnum;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutscenePlayer : MonoBehaviour {

	[SearchableEnum] public LocID cutsceneID;
	//public LevelID nextLevelID;
	public PlayableDirector timeline;

	public Collider triggerCollider;
	public bool disableTriggerAfterActivation = true;
	public WindowPanel[] closePanelsOnStart;
	public WindowPanel[] openPanelsOnEnd;
	public bool restoreClosedPanels;
	public bool isSkippableCutscene;

	private List<WindowPanel> closedPanels = new List<WindowPanel>();

	public void SkipCutscene() {
		AudioManager.cutsceneSkippedFrame = Time.frameCount;
		timeline.time = timeline.duration;
		if (SubtitlePanel.instance != null) {
			SubtitlePanel.instance.StopPreviousSubtitles();
		}
		AudioManager.instance.StopSpeechAudioSource();
	}

	public void PlayCutscene() {
		//if (UserProfile.CurrentProfile.cutscenes.Contains(cutsceneID) == false) {
			//UserProfile.CurrentProfile.cutscenes.Add(cutsceneID);
			//UserProfile.SaveCurrent();
			timeline.Play();
			if (isSkippableCutscene) {
				WindowManager.instance.OpenWindow(WindowPanel.SkipCutsceneWindow, this);
			}
			timeline.stopped += DisableUI;
			for (int i = 0; i < closePanelsOnStart.Length; i++) {
				bool closed = WindowManager.instance.CloseWindow(closePanelsOnStart[i]);
				if (closed == true) {
					closedPanels.Add(closePanelsOnStart[i]);
				}
			}
			
			if (disableTriggerAfterActivation == true) {
				gameObject.SetActive(false);
				triggerCollider.enabled = false;
			}
		//}
	}

	private void OnTriggerEnter() {
		PlayCutscene();
	}

	// Update is called once per frame
	public void DisableUI(PlayableDirector director) {
		
		if (restoreClosedPanels == true) {
			for (int i = 0; i < closedPanels.Count; i++) {
				WindowManager.instance.OpenWindow(closedPanels[i]);
			}
		}
		for (int i = 0; i < openPanelsOnEnd.Length; i++) {
			WindowManager.instance.OpenWindow(openPanelsOnEnd[i]);
		}
		TaskManager.instance.ShowQueuedChangeTask();
		TaskManager.instance.AllTasksCompleted();
		WindowManager.instance.CloseWindow(WindowPanel.SkipCutsceneWindow);
	}
}
