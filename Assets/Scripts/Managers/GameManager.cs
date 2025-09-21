using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using ProjectEnums;
using System.Collections.Generic;
using MEC;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public SettingsAsset settings;
	public LocID selectedDifficulty = LocID.None;

	private void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
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

	private void Start() {
#if UNITY_ANDROID && !UNITY_EDITOR
		// On mobile platforms the default frame rate is less than the maximum achievable frame rate due to need to conserve battery power.
		// Typically on mobile platforms the default frame rate is 30 frames per second.
		// So because of that the framerate has to be set manually.
		Application.targetFrameRate = 60;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
#endif
		Timing.RunCoroutine(StartUpRoutine());
	}

	private IEnumerator<float> StartUpRoutine() {
		LoadLocalizations();
		UserProfile.GetLatestProfileAtStartup();
		AudioManager.LoadAudioSettings();
		bool showSplashScreen = false;
		if (showSplashScreen == true) {
			WindowManager.instance.OpenWindow(WindowPanel.SplashScreen);
			while (WindowManager.instance.WindowIsOpen(WindowPanel.SplashScreen) == true) {
				yield return Timing.WaitForOneFrame;
			}
		}
		WindowManager.instance.OpenWindow(WindowPanel.LoadingScreen);
		// Update loading screen progress bar if there is one
		// Load other systems here
		// etc.
		SceneLoader.instance.LoadScene("MainMenu");
		yield return Timing.WaitForOneFrame;
	}


	private void LoadLocalizations() {
		if (PlayerPrefs.HasKey("CURRENT_LANGUAGE") == false) {
			PlayerPrefs.SetInt("CURRENT_LANGUAGE", 1);
		}
		int langId = PlayerPrefs.GetInt("CURRENT_LANGUAGE");
		LocalizationManager.instance.ChangeLanguage((Language)langId);
	}

}
