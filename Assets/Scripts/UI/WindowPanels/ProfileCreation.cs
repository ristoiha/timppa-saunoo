using UnityEngine;
using UnityEngine.UI;
using ProjectEnums;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ProfileCreation : WindowBase {

	public TMP_InputField profileNameInputField;
	public TextMeshProUGUI profileValidityText;
	public Transform closeButtonTransform;
	public UnityEngine.UI.Button blackOverlayButton;

	private EventSystem system;

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		base.Init(parameters, windowStack);
		if (PlayerPrefs.HasKey("LAST_PROFILE") == false) {
			closeButtonTransform.gameObject.SetActive(false);
			blackOverlayButton.enabled = false;
		}
		system = EventSystem.current;
		EventSystem.current.SetSelectedGameObject(profileNameInputField.gameObject);
	}

	public override void UpdateUI() {

	}

	protected override void OpeningAnimationFinished() {

	}

	protected override void Closing() {
	}

	protected override void Destroying() {

	}

	public void SetProfileValidityString(LocID locID) {
		if (locID == LocID.NameOK) {
			profileValidityText.color = Gval.okColor;
		}
		else {
			profileValidityText.color = Gval.cancelColor;
		}
		profileValidityText.text = LocalizationManager.instance.GetString(locID);
	}

	public string GetProfileNameString() {
		return profileNameInputField.text;
	}

}
