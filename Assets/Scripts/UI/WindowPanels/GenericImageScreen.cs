using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ProjectEnums;
using System;
using TMPro;
using UnityEngine.UI;

public class GenericImageScreen : WindowBase {

	public Transform buttonPrefab;
	public Transform buttonParent;
	public Image image;

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		base.Init(parameters, windowStack);
		if (parameters is Sprite) {
			image.sprite = (Sprite)parameters;
		}
		else if (parameters is Tuple<Sprite, List<GenericButton>>) {
			Tuple<Sprite, List<GenericButton>> tuple = (Tuple<Sprite, List<GenericButton>>)parameters;
			image.sprite = (Sprite)tuple.Item1;
			for (int i = 0; i < tuple.Item2.Count; i++) {
				Button button = Instantiate(buttonPrefab, buttonParent).GetComponent<Button>();
				button.onClick.AddListener(delegate { tuple.Item2[button.transform.GetSiblingIndex()].action.Invoke(); WindowManager.instance.CloseWindow(window); });
				button.GetComponentInChildren<TextMeshProUGUI>().text = LocalizationManager.instance.GetString(tuple.Item2[i].locID);
			}
		}
		else if (parameters is Tuple<Sprite, List<GenericButton<int>>>) {
			Tuple<Sprite, List<GenericButton<int>>> tuple = (Tuple<Sprite, List<GenericButton<int>>>)parameters;
			image.sprite = (Sprite)tuple.Item1;
			for (int i = 0; i < tuple.Item2.Count; i++) {
				Button button = Instantiate(buttonPrefab, buttonParent).GetComponent<Button>();
				button.onClick.AddListener(delegate { tuple.Item2[button.transform.GetSiblingIndex()].action.Invoke(5); WindowManager.instance.CloseWindow(window); });
				button.GetComponentInChildren<TextMeshProUGUI>().text = LocalizationManager.instance.GetString(tuple.Item2[i].locID);
			}
		}
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
