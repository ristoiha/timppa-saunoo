using ProjectEnums;
using RoboRyanTron.SearchableEnum;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
[CreateAssetMenu(fileName = "StringVariable", menuName = "ScriptableObjects/StringVariable", order = 1)]
public class StringVariable : VariableContent {

	public static Dictionary<LocID, string> values = new Dictionary<LocID, string>();
	public static Dictionary<LocID, string> runtimeValues = new Dictionary<LocID, string>();
	public static Dictionary<LocID, string> sceneValues = new Dictionary<LocID, string>();
	public static Dictionary<LocID, string> valuesAtInstantiation = new Dictionary<LocID, string>();

	public delegate void ValueUpdated(string value);
	public static Dictionary<LocID, ValueUpdated> listeners = new Dictionary<LocID, ValueUpdated>();

	public string defaultValue;

	public override void Initialize(LocID locID) {
		if (values.ContainsKey(locID) == false) {
			values.Add(locID, defaultValue);
		}
	}

	public override void InstantiateUIPrefab(Transform parent, LocID overrideLocID = LocID.None) {
		Initialize(locID);
		if (valuesAtInstantiation.ContainsKey(locID) == false) {
			valuesAtInstantiation.Add(locID, Tools.GetDictionaryValueOrNull(values, locID));
		}
		else {
			valuesAtInstantiation[locID] = Tools.GetDictionaryValueOrNull(values, locID);
		}
		Transform uiTransform = Instantiate(uiPrefab, parent);
		// VariableInputField variableInputField = uiTransform.GetComponent<VariableInputField>();
		//if (variableInputField != null) {
			
		//}
	}

	public static void RevertValues() {
		for (int i = valuesAtInstantiation.Count - 1; i > -1; i--) {
			KeyValuePair<LocID, string> item = valuesAtInstantiation.ElementAt(i);
			if (item.Value == null) {
				values.Remove(item.Key);
			}
			else {
				if (listeners.ContainsKey(item.Key) == true) {
					listeners[item.Key].Invoke(item.Value);
				}
			}
			values[item.Key] = item.Value;
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

	public static string GetValue(LocID variable) {
		ScriptableObjectManager.instance.GetVariableOption(variable); // Initializes default value if necessary
		if (values.TryGetValue(variable, out string val)) {
			return val;
		}
		else {
			Debug.LogError(variable + " not found from dictionary");
			return "";
		}
	}

	public static void SetValue(LocID variable, string value) {
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

	public static void SetValue(LocID variable, string value, VariableTarget target) {
		if (target == VariableTarget.SceneValue) {
			SetSceneValue(variable, value);
		}
		else if (target == VariableTarget.RuntimeValue) {
			SetRuntimeValue(variable, value);
		}
		else if (target == VariableTarget.PermanentValue) {
			SetValue(variable, value);
		}
	}

	public static string GetSceneValue(LocID variable) {
		if (sceneValues.TryGetValue(variable, out string val)) {
			return val;
		}
		else {
			Debug.LogError(variable + " not found from dictionary");
			return "";
		}
	}

	public static void SetSceneValue(LocID variable, string value) {
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

	public static string GetRuntimeValue(LocID variable) {
		if (runtimeValues.TryGetValue(variable, out string val)) {
			return val;
		}
		else {
			Debug.LogError(variable + " not found from dictionary");
			return "";
		}
	}

	public static void SetRuntimeValue(LocID variable, string value) {
		if (runtimeValues.ContainsKey(variable) == false) {
			runtimeValues.Add(variable, value);
		}
		else {
			if (runtimeValues[variable] != value) {
				runtimeValues[variable] = value;
			}
		}
		if (listeners.ContainsKey(variable) == true) {
			listeners[variable]?.Invoke(value);
		}
	}

}
