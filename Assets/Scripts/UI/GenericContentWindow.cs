using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectEnums;
using System;

public class GenericContentWindow : WindowInterface {

	public WindowStyleAsset style { get; }
	public ContentPanelAsset contentPanel;

	public List<GenericButton> buttons = new List<GenericButton>();

	public GenericContentWindow(WindowStyle windowStyle, ContentPanelAsset contentPanelAsset, List<GenericButton> buttonsList = null) {
		style = MonoBehaviour.Instantiate(ScriptableObjectManager.instance.GetWindowStyle(windowStyle));
		contentPanel = contentPanelAsset;
		if (buttonsList != null) {
			buttons = buttonsList;
		}
	}

}

public static class GenericContentWindowExtensions {

	public static void AddButton(this GenericContentWindow window, LocID locID, Action action) {
		window.buttons.Add(new GenericButton(locID, action));
	}

}


