using TMPro;
using ProjectEnums;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SendEmailWindow : WindowBase {

	public TMP_InputField emailField;

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		base.Init(parameters, windowStack);
		EventSystem.current.SetSelectedGameObject(emailField.gameObject);
	}

	public override void UpdateUI() {

	}

	public void SetEmail() {
		string recipients = "";
		//if (ServiceManager.instance.onlineMode == true) {
		//	ServiceManager.instance.SaveData(); // Save data to cloud
		//	if (Tools.CheckEmailAddressValidity(ServiceManager.instance.GetLobbyEmail()) == true) {
		//		recipients += ServiceManager.instance.GetLobbyEmail();
		//	}
		//}
		if (Tools.CheckEmailAddressValidity(emailField.text) == true) {
			if (recipients != "") {
				recipients += ",";
			}
			recipients += emailField.text;
		}
		//ServiceManager.instance.SendMail(recipients, "Tarinatalo kirjoitus", UserProfile.CurrentProfile.storyText);
		WindowManager.instance.CloseWindow(WindowPanel.SendEmailWindow);
	}

	protected override void OpeningAnimationFinished() {

	}

	protected override void Closing() {

	}

	protected override void Destroying() {

	}

}
