using TMPro;
using ProjectEnums;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class LobbyCreation : WindowBase {

	public TMP_InputField nameField;
	public TMP_InputField emailField;
	public TMP_InputField passwordField;

	private EventSystem system;

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		base.Init(parameters, windowStack);
		system = EventSystem.current;
		EventSystem.current.SetSelectedGameObject(nameField.gameObject);
	}

	public override void UpdateUI() {

	}

	public void CreateLobby() {
		if (Tools.CheckEmailAddressValidity(emailField.text) == false) {
			Debug.Log("Email not valid");
			EventSystem.current.SetSelectedGameObject(emailField.gameObject);
			return;
		}
		if (NameIsValid() == false) {
			EventSystem.current.SetSelectedGameObject(nameField.gameObject);
			Debug.Log("Name not valid");
			return;
		}
		ServiceManager.instance.CreateLobby(nameField.text, emailField.text, passwordField.text);
		WindowManager.instance.CloseWindow(WindowPanel.LobbyCreation);
	}

	private bool NameIsValid() {
		if (nameField.text.Length < 3) {
			return false;
		}
		return true;
	}

	protected override void OpeningAnimationFinished() {

	}

	protected override void Closing() {
		if (Tools.CheckEmailAddressValidity(emailField.text) == false || NameIsValid() == false) {
			WindowManager.instance.OpenWindow(WindowPanel.LobbySelection);
		}
	}

	protected override void Destroying() {
	}

}
