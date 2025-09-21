using ProjectEnums;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnnounceManager : MonoBehaviour {

	public static AnnounceManager instance;

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

	public static void Announce(string message, float duration = 1F) {
		AnnounceScreen announceScreen = (AnnounceScreen)WindowManager.instance.GetWindow(WindowPanel.AnnounceScreen);
		if (announceScreen == null) {
			announceScreen = (AnnounceScreen)WindowManager.instance.OpenWindow(WindowPanel.AnnounceScreen);
		}
		announceScreen.Announce(message, duration);
	}

}
