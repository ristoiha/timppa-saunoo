using TMPro;
using ProjectEnums;
using UnityEngine;
using System.Collections.Generic;

public class CheckPasswordWindow : WindowBase {

	public TMP_InputField passwordField;

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		base.Init(parameters, windowStack);
	}

	public override void UpdateUI() {

	}

	public void JoinPasswordLobby() {
		ServiceManager.instance.JoinToLobby(ServiceManager.instance.selectedLobby, passwordField.text);
		WindowManager.instance.CloseWindow(WindowPanel.CheckPasswordWindow);
	}

	protected override void OpeningAnimationFinished() {

	}

	protected override void Closing() {

	}

	protected override void Destroying() {

	}

}
