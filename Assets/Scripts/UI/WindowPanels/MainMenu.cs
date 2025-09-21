using MEC;
using ProjectEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class MainMenu : WindowBase {

	public ContentPanelAsset vrIntroContent;
	public Transform exitGameButton;

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		base.Init(parameters, windowStack);
		if (PlayerPrefs.HasKey("LAST_PROFILE") == false) {
			Timing.RunCoroutine(FirstStartUp());
		}
		else {
			Timing.RunCoroutine(NormalStartUp());
		}
		Cursor.lockState = CursorLockMode.None;
#if UNITY_EDITOR || UNITY_WEBGL
		exitGameButton.gameObject.SetActive(false);
#endif
	}

	public override void UpdateUI() {

	}

	public void SelectLanguage(int langId) {
		if ((Language)langId != LocalizationManager.instance.selectedLanguage) {
			PlayerPrefs.SetInt("CURRENT_LANGUAGE", langId);
			LocalizationManager.instance.ChangeLanguage((Language)langId);
			LocalizedObject.UpdateLocalization();
		}
	}

	protected override void OpeningAnimationFinished() {

	}

	protected override void Closing() {

	}

	protected override void Destroying() {

	}

	private IEnumerator<float> FirstStartUp() {
		// Show ToS if not agreed already
		CheckPermissions();
		ShowInfoScreen();
		yield return Timing.WaitForOneFrame;

	}

	private IEnumerator<float> NormalStartUp() {
		CheckPermissions();
		ShowInfoScreen();
		yield return Timing.WaitForOneFrame;
	}

	private void ShowInfoScreen() {
#if VR
        if (BoolVariable.GetRuntimeValue(LocID.Value_InstructionsShown) == false) {
			CommandVariable commandVariable = new CommandVariable();
			commandVariable.buttonActionType = ButtonActionType.ContentPanel;
			commandVariable.contentPanelAction = ContentPanelAction.SwitchToWindow;
			commandVariable.contentPanelAsset = vrIntroContent;
			UIEventManager.instance.TriggerEvent(commandVariable, this.transform);
			BoolVariable.SetRuntimeValue(LocID.Value_InstructionsShown, true);
		}
#endif
		
	}

	private void CheckPermissions() {
#if UNITY_ANDROID && !VR && !UNITY_EDITOR
		if (Permission.HasUserAuthorizedPermission(Permission.Camera) == false) {
			Permission.RequestUserPermission(Permission.Camera);
		}
#endif
	}

}
