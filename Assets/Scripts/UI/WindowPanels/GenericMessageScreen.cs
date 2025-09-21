using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ProjectEnums;
using System;
using TMPro;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Reflection;

public class GenericMessageScreen : WindowBase {

	public Image background;
	public Transform buttonPrefab;
	public Transform buttonParent;
	public TextMeshProUGUI bodyText;
	public TextMeshProUGUI headerText;

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		base.Init(parameters, windowStack);
		if (parameters is GenericWindow) {
			GenericWindow genericWindow = (GenericWindow)parameters;
			bodyText.text = LocalizationManager.instance.GetString(genericWindow.body);
			background.rectTransform.sizeDelta = new Vector2(genericWindow.style.width, genericWindow.style.height);
			bodyText.font = genericWindow.style.fontAsset;
			bodyText.color = genericWindow.style.fontColor;
			headerText.font = genericWindow.style.fontAsset;
			headerText.color = genericWindow.style.fontColor;
			if (genericWindow.header != LocID.None) {
				headerText.text = LocalizationManager.instance.GetString(genericWindow.header);
				background.sprite = genericWindow.style.backgroundWithHeaderSprite;
			}
			else {
				background.sprite = genericWindow.style.backgroundSprite;
				headerText.text = "";
			}
			for (int i = 0; i < genericWindow.buttons.Count; i++) {
				Button button = Instantiate(buttonPrefab, buttonParent).GetComponent<Button>();
				button.onClick.AddListener(delegate {
					genericWindow.buttons[button.transform.GetSiblingIndex()].action.Invoke();
				});
				button.GetComponentInChildren<TextMeshProUGUI>().text = LocalizationManager.instance.GetString(genericWindow.buttons[i].locID);
			}
		}
		else if (parameters is GenericWindow<int>) {
			GenericWindow<int> genericWindow = (GenericWindow<int>)parameters;
			bodyText.text = LocalizationManager.instance.GetString(genericWindow.body);
			background.rectTransform.sizeDelta = new Vector2(genericWindow.style.width, genericWindow.style.height);
			bodyText.font = genericWindow.style.fontAsset;
			bodyText.color = genericWindow.style.fontColor;
			headerText.font = genericWindow.style.fontAsset;
			headerText.color = genericWindow.style.fontColor;
			if (genericWindow.header != LocID.None) {
				headerText.text = LocalizationManager.instance.GetString(genericWindow.header);
				background.sprite = genericWindow.style.backgroundWithHeaderSprite;
			}
			else {
				background.sprite = genericWindow.style.backgroundSprite;
				headerText.text = "";
			}
			for (int i = 0; i < genericWindow.buttons.Count; i++) {
				Button button = Instantiate(buttonPrefab, buttonParent).GetComponent<Button>();
				int value = 0;
				if (genericWindow.parameters.Count > 0 && i < genericWindow.parameters.Count) {
					value = genericWindow.parameters[i];
				}
				button.onClick.AddListener(delegate {
					genericWindow.buttons[button.transform.GetSiblingIndex()].action.Invoke(value);
				});
				button.GetComponentInChildren<TextMeshProUGUI>().text = LocalizationManager.instance.GetString(genericWindow.buttons[i].locID);
			}
		}
		else {
			Debug.LogError("Generic message screen changed, edit code to use GenericWindow as parameter");
		}
	}

	private void SetStyle(WindowInterface genericWindow)  {
		
	}

	public override void UpdateUI() {

	}

	protected override void OpeningAnimationFinished() {

	}

	protected override void Closing() {

	}

	protected override void Destroying() {

	}
}
