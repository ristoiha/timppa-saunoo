using ProjectEnums;
using RoboRyanTron.SearchableEnum;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "IntVariable", menuName = "ScriptableObjects/IntVariable", order = 1)]
public class IntVariable : VariableContent {

	public static Dictionary<LocID, int> values = new Dictionary<LocID, int>();
	public static Dictionary<LocID, int> runtimeValues = new Dictionary<LocID, int>();
	public static Dictionary<LocID, int> sceneValues = new Dictionary<LocID, int>();
	public static Dictionary<LocID, int?> valuesAtInstantiation = new Dictionary<LocID, int?>();

	public delegate void ValueUpdated(int value);
	public static Dictionary<LocID, ValueUpdated> listeners = new Dictionary<LocID, ValueUpdated>();

	public int defaultValue;
	public int minValue;
	public int maxValue;

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
		VariableSlider variableSlider = uiTransform.GetComponent<VariableSlider>();
		if (variableSlider != null) {
			variableSlider.variable = locID;
			variableSlider.slider.minValue = minValue;
			variableSlider.slider.maxValue = maxValue;
		}
	}

	public static void RevertValues() {
		for (int i = valuesAtInstantiation.Count - 1; i > -1; i--) {
			KeyValuePair<LocID, int?> item = valuesAtInstantiation.ElementAt(i);
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

	public static int GetValue(LocID variable) {
		ScriptableObjectManager.instance.GetVariableOption(variable); // Initializes default value if necessary
		if (values.TryGetValue(variable, out int val)) {
			return val;
		}
		else {
			Debug.LogError(variable + " not found from dictionary");
			return -1;
		}
	}

	public static void SetValue(LocID variable, int value) {
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

	public static void SetValue(LocID variable, int value, VariableTarget target) {
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

	public static int GetSceneValue(LocID variable) {
		if (sceneValues.TryGetValue(variable, out int val)) {
			return val;
		}
		else {
			Debug.LogError(variable + " not found from dictionary");
			return -1;
		}
	}

	public static void SetSceneValue(LocID variable, int value) {
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

	public static int GetRuntimeValue(LocID variable) {
		if (runtimeValues.TryGetValue(variable, out int val)) {
			return val;
		}
		else {
			Debug.LogError(variable + " not found from dictionary");
			return -1;
		}
	}

	public static void SetRuntimeValue(LocID variable, int value) {
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
