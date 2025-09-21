using ProjectEnums;
using RoboRyanTron.SearchableEnum;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BoolVariable", menuName = "ScriptableObjects/BoolVariable", order = 1)]
public class BoolVariable : VariableContent {

	public static Dictionary<LocID, bool> values = new Dictionary<LocID, bool>();
	public static Dictionary<LocID, bool> runtimeValues = new Dictionary<LocID, bool>();
	public static Dictionary<LocID, bool> sceneValues = new Dictionary<LocID, bool>();
	public static Dictionary<LocID, bool?> valuesAtInstantiation = new Dictionary<LocID, bool?>();

	public delegate void ValueUpdated(bool value);
	public static Dictionary<LocID, ValueUpdated> listeners = new Dictionary<LocID, ValueUpdated>();
	
	private static bool debug = false;

	public bool defaultValue;


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
		VariableToggle variableToggle = uiTransform.GetComponent<VariableToggle>();
		if (variableToggle != null) {
			variableToggle.variable = locID;
		}
	}

	public static void RevertValues() {
		for (int i = valuesAtInstantiation.Count - 1; i > -1; i--) {
			KeyValuePair<LocID, bool?> item = valuesAtInstantiation.ElementAt(i);
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

	public static bool GetValue(LocID variable) {
		ScriptableObjectManager.instance.GetVariableOption(variable); // Initializes default value if necessary
		if (values.TryGetValue(variable, out bool val)) {
			return val;
		}
		else {
			if (debug == true) Debug.LogError(variable + " not found from dictionary");
			return false;
		}
	}

	public static void SetValue(LocID variable, bool value) {
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

	public static void SetValue(LocID variable, bool value, VariableTarget target) {
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

	public static bool GetSceneValue(LocID variable) {
		if (sceneValues.TryGetValue(variable, out bool val)) {
			return val;
		}
		else {
			if (debug == true) Debug.LogError(variable + " not found from dictionary");
			return false;
		}
	}

	public static void SetSceneValue(LocID variable, bool value) {
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

	public static bool GetRuntimeValue(LocID variable) {
		if (runtimeValues.TryGetValue(variable, out bool val)) {
			return val;
		}
		else {
			if (debug == true) Debug.LogError(variable + " not found from dictionary");
			return false;
		}
	}

	public static void SetRuntimeValue(LocID variable, bool value) {
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
