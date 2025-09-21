using ProjectEnums;
using RoboRyanTron.SearchableEnum;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "WindowPanelVariable", menuName = "ScriptableObjects/WindowPanelVariable", order = 1)]
public class WindowPanelVariable : VariableContent {

	public static Dictionary<LocID, WindowPanel> values = new Dictionary<LocID, WindowPanel>();
	public static Dictionary<LocID, WindowPanel> sceneValues = new Dictionary<LocID, WindowPanel>();
	public static Dictionary<LocID, WindowPanel?> valuesAtInstantiation = new Dictionary<LocID, WindowPanel?>();

	public delegate void ValueUpdated(WindowPanel value);
	public static Dictionary<LocID, ValueUpdated> listeners = new Dictionary<LocID, ValueUpdated>();

	public WindowPanel defaultValue;

	public override void Initialize(LocID locID) {
		if (values.ContainsKey(locID) == false) {
			values.Add(locID, defaultValue);
		}
	}

	public override void InstantiateUIPrefab(Transform parent, LocID overrideLocID = LocID.None) {
		//Initialize(locID);
		//if (valuesAtInstantiation.ContainsKey(locID) == false) {
		//	valuesAtInstantiation.Add(locID, Tools.GetDictionaryValueOrNull(values, locID));
		//}
		//else {
		//	valuesAtInstantiation[locID] = Tools.GetDictionaryValueOrNull(values, locID);
		//}
		//Transform uiTransform = Instantiate(uiPrefab, parent);
		//VariableSlider variableSlider = uiTransform.GetComponent<VariableSlider>();
		//if (variableSlider != null) {
		//	variableSlider.variable = locID;
		//	variableSlider.slider.minValue = minValue;
		//	variableSlider.slider.maxValue = maxValue;
		//}
	}

	public static void RevertValues() {
		for (int i = valuesAtInstantiation.Count - 1; i > -1; i--) {
			KeyValuePair<LocID, WindowPanel?> item = valuesAtInstantiation.ElementAt(i);
			if (item.Value == null) {
				values.Remove(item.Key);
			}
			else {
				if (listeners.ContainsKey(item.Key) == true) {
					listeners[item.Key].Invoke(item.Value.GetValueOrDefault());
				}
			}
			values[item.Key] = item.Value.GetValueOrDefault();
			valuesAtInstantiation.Remove(item.Key);
		}
	}

	public static void RegisterListener(LocID locID, ValueUpdated listener) {
		if (listeners.ContainsKey(locID) == false) {
			listeners.Add(locID, listener);
		}
		else {
			listeners[locID] += listener;
		}
	}

	public static void UnregisterListener(LocID locID, ValueUpdated listener) {
		listeners[locID] -= listener;
	}

	public static WindowPanel GetValue(LocID variable) {
		ScriptableObjectManager.instance.GetVariableOption(variable); // Initializes default value if necessary
		if (values.TryGetValue(variable, out WindowPanel val)) {
			return val;
		}
		else {
			Debug.LogError(variable + " not found from dictionary");
			return WindowPanel.DefaultWindow;
		}
	}

	public static void SetValue(LocID variable, WindowPanel value) {
		if (values.ContainsKey(variable) == false) {
			values.Add(variable, value);
		}
		else {
			if (values[variable] != value) {
				values[variable] = value;
				if (listeners.ContainsKey(variable) == true) {
					listeners[variable]?.Invoke(value);
				}
			}
		}
	}

	public static WindowPanel GetSceneValue(LocID variable) {
		if (sceneValues.TryGetValue(variable, out WindowPanel val)) {
			return val;
		}
		else {
			Debug.LogError(variable + " not found from dictionary");
			return WindowPanel.DefaultWindow;
		}
	}

	public static void SetSceneValue(LocID variable, WindowPanel value) {
		if (sceneValues.ContainsKey(variable) == false) {
			sceneValues.Add(variable, value);
		}
		else {
			if (sceneValues[variable] != value) {
				sceneValues[variable] = value;
			}
		}
		if (listeners.ContainsKey(variable) == true) {
			listeners[variable]?.Invoke(value);
		}
	}

}
