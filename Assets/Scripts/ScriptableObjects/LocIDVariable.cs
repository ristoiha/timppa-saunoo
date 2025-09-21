using ProjectEnums;
using RoboRyanTron.SearchableEnum;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
[CreateAssetMenu(fileName = "LocIDVariable", menuName = "ScriptableObjects/LocIDVariable", order = 1)]
public class LocIDVariable : VariableContent {

	public static Dictionary<LocID, LocID> values = new Dictionary<LocID, LocID>();
	public static Dictionary<LocID, LocID> runtimeValues = new Dictionary<LocID, LocID>();
	public static Dictionary<LocID, LocID> sceneValues = new Dictionary<LocID, LocID>();
	public static Dictionary<LocID, LocID?> valuesAtInstantiation = new Dictionary<LocID, LocID?>();

	public delegate void ValueUpdated(LocID value);
	public static Dictionary<LocID, ValueUpdated> listeners = new Dictionary<LocID, ValueUpdated>();

	private static bool debug = false;
	
	[SearchableEnum] public LocID defaultValue;
	[SearchableEnum] public LocID[] parameters;
	

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
		LocIDDropdown locIDDropdown = uiTransform.GetComponent<LocIDDropdown>();
		if (locIDDropdown != null) {
			locIDDropdown.variable = locID;
			locIDDropdown.options = parameters;
			return;
		}
		TextMeshProUGUI textComponent = uiTransform.GetComponent<TextMeshProUGUI>();
		if (textComponent != null) {
			textComponent.text = LocalizationManager.instance.GetString(values[locID]);
			return;
		}
	}

	public static void RevertValues() {
		for (int i = valuesAtInstantiation.Count - 1; i > -1; i--) {
			KeyValuePair<LocID, LocID?> item = valuesAtInstantiation.ElementAt(i);
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

	public static LocID GetValue(LocID variable) {
		ScriptableObjectManager.instance.GetVariableOption(variable); // Initializes default value if necessary
		if (values.TryGetValue(variable, out LocID val)) {
			return val;
		}
		else {
			if (debug) Debug.LogError(variable + " not found from dictionary");
			return LocID.None;
		}
	}

	public static void SetValue(LocID variable, LocID value) {
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

	public static void SetValue(LocID variable, LocID value, VariableTarget target) {
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

	public static LocID GetSceneValue(LocID variable) {
		if (sceneValues.TryGetValue(variable, out LocID val)) {
			return val;
		}
		else {
			if (debug) Debug.LogError(variable + " not found from dictionary");
			return LocID.None;
		}
	}

	public static void SetSceneValue(LocID variable, LocID value) {
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

	public static LocID GetRuntimeValue(LocID variable) {
		if (runtimeValues.TryGetValue(variable, out LocID val)) {
			return val;
		}
		else {
			if (debug) Debug.LogError(variable + " not found from dictionary");
			return LocID.None;
		}
	}

	public static void SetRuntimeValue(LocID variable, LocID value) {
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

	public static bool SceneValueExists(LocID variable) {
		return sceneValues.ContainsKey(variable);
	}

}
