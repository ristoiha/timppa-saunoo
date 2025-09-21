using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectEnums;
using System;

public class GenericWindow : WindowInterface {

	public WindowStyleAsset style { get; }
	public LocID header { get; }
	public LocID body { get; }

	public List<GenericButton> buttons = new List<GenericButton>();

	public GenericWindow(WindowStyle windowStyle, LocID bodyText, LocID headerText = LocID.None, List<GenericButton> buttonsList = null) {
		style = MonoBehaviour.Instantiate(ScriptableObjectManager.instance.GetWindowStyle(windowStyle));
		header = headerText;
		body = bodyText;
		if (buttonsList != null) {
			buttons = buttonsList;
		}
	}

}

public static class GenericWindowExtensions {

	public static void AddButton(this GenericWindow window, LocID locID, Action action) {
		window.buttons.Add(new GenericButton(locID, action));
	}

	public static void AddButton<T>(this GenericWindow<T> window, LocID locID, Action<T> action) {
		window.buttons.Add(new GenericButton<T>(locID, action));
	}

	public static void AddParameter<T>(this GenericWindow<T> window, T param) {
		window.parameters.Add(param);
	}

}

public class GenericWindow<T> : WindowInterface {

	public WindowStyleAsset style { get; }
	public LocID header { get; }
	public LocID body { get; }

	public List<T> parameters = new List<T>();
	public List<GenericButton<T>> buttons = new List<GenericButton<T>>();

	public GenericWindow(WindowStyle windowStyle, LocID bodyText, LocID headerText = LocID.None, List<T> parametersList = null, List<GenericButton<T>> buttonsList = null) {
		style = MonoBehaviour.Instantiate(ScriptableObjectManager.instance.GetWindowStyle(windowStyle));
		header = headerText;
		body = bodyText;
		if (parametersList != null) {
			parameters = parametersList;
		}
		if (buttonsList != null) {
			buttons = buttonsList;
		}
	}

}

public interface WindowInterface {

	public WindowStyleAsset style { get; }

}

