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

public class GenericContentPanel : WindowBase {

	public Image background;
	public TextMeshProUGUI headerText;
	public ScrollRect scrollRect;
	public Transform textPrefab;
	public Transform locIDSliderPrefab;
	public Transform locIDDropdownPrefab;
	public Transform locIDTogglePrefab;
	public Transform buttonPrefab;
	public Transform buttonParent;
	public Transform contentParent;

	[System.NonSerialized]
	public ContentPanelAsset contentPanelAsset;

	//private List<KeyValuePair<LocID, float?>> sliderValuesAtStart = new List<KeyValuePair<LocID, float?>>();
	//private List<KeyValuePair<LocID, bool?>> toggleValuesAtStart = new List<KeyValuePair<LocID, bool?>>();
	//private List<KeyValuePair<LocID, LocID?>> dropdownValuesAtStart = new List<KeyValuePair<LocID, LocID?>>();

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		if (parameters is GenericContentWindow) {
			GenericContentWindow genericContentWindow = (GenericContentWindow)parameters;
			contentPanelAsset = genericContentWindow.contentPanel;
			SetStyle(genericContentWindow.style);
			if (contentPanelAsset.header != LocID.None) {
				headerText.text = LocalizationManager.instance.GetString(contentPanelAsset.header);
				RectTransform scrollRectRectTransform = scrollRect.GetComponent<RectTransform>();
				scrollRectRectTransform.sizeDelta = new Vector2(scrollRectRectTransform.sizeDelta.x, scrollRectRectTransform.sizeDelta.y - headerText.rectTransform.sizeDelta.y);
				scrollRectRectTransform.anchoredPosition = new Vector2(
					scrollRectRectTransform.anchoredPosition.x,
					scrollRectRectTransform.anchoredPosition.y - headerText.rectTransform.sizeDelta.y / 2F
				);
				headerText.gameObject.SetActive(true);
			}
			else {
				headerText.gameObject.SetActive(false);
			}
			for (int i = 0; i < contentPanelAsset.contents.Length; i++) {
				VariableContentEntry optionEntry = contentPanelAsset.contents[i];
				optionEntry.content.InstantiateUIPrefab(contentParent, optionEntry.overrideLocID);
			}
			for (int i = 0; i < contentPanelAsset.commandButtons.Length; i++) {
				Button button = Instantiate(buttonPrefab, buttonParent).GetComponent<Button>();
				Action action = null;
				for (int j = 0; j < contentPanelAsset.commandButtons[i].commands.Length; j++) {
					CommandVariable commandVariable = contentPanelAsset.commandButtons[i].commands[j];
					action += () => UIEventManager.instance.TriggerEvent(commandVariable, transform);
				}
				button.onClick.AddListener(delegate {
					action.Invoke();
				});
				//button.GetComponentInChildren<TextMeshProUGUI>().text = LocalizationManager.instance.GetString(genericContentWindow.buttons[i].locID);
				button.GetComponentInChildren<TextMeshProUGUI>().text = LocalizationManager.instance.GetString(contentPanelAsset.commandButtons[i].buttonLocID);
			}
		}
		else {
			Debug.LogError("Generic message screen changed, edit code to use GenericWindow as parameter");
		}
		base.Init(parameters, windowStack);
	}

	private void SetStyle(WindowStyleAsset style)  {
		background.sprite = style.backgroundSprite;
		background.rectTransform.sizeDelta = new Vector2(style.width, style.height);
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
