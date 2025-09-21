using System.Collections;
using UnityEngine;
using ProjectEnums;
using TMPro;
using UnityEngine.UIElements;
using System.Drawing;
using System.Collections.Generic;

public class InteractInfoPanel : WindowBase {

	public TextMeshProUGUI interactText;

	private LocID currentLocID;
	private string keyString;
	private string interactString;

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		base.Init(parameters, windowStack);
		string color = ColorUtility.ToHtmlStringRGB(GameManager.instance.settings.keyHighlightColor);
		keyString = " <color=#" + color + ">" + "( E )</color>";
	}

	public void ChangeText(LocID baseLocID, string overrideString = "") {
		if (currentLocID != baseLocID) {
			currentLocID = baseLocID;
			if (overrideString == "") {
				interactString = LocalizationManager.instance.GetString(currentLocID);
			}
			else {
				interactString = overrideString;
			}
			UpdateUI();
		}
	}

	public void ClearText() {
		currentLocID = LocID.None;
		interactText.text = "";
	}

	public override void UpdateUI() {
		if (currentLocID != LocID.None) {
			interactText.text = interactString + keyString;
		}
	}

	protected override void OpeningAnimationFinished() {

	}

	protected override void Closing() {

	}

	protected override void Destroying() {

	}

}
