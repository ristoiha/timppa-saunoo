using ProjectEnums;
using System.Collections.Generic;
using UnityEngine;

public class SaveVariable<T> {

	public Dictionary<LocID, List<T>> values = new Dictionary<LocID, List<T>>();
	public Dictionary<LocID, List<T>> runtimeValues = new Dictionary<LocID, List<T>>();
	public Dictionary<LocID, List<T>> sceneValues = new Dictionary<LocID, List<T>>();

	public delegate void ValueUpdated(List<T> value);
	public Dictionary<LocID, ValueUpdated> listeners = new Dictionary<LocID, ValueUpdated>();

	private static bool debug = false;

	public void RegisterListener(LocID locID, ValueUpdated listener) {
		if (listeners.ContainsKey(locID) == false) {
			listeners.Add(locID, listener);
		}
		else {
			listeners[locID] += listener;
		}
	}

	public void UnregisterListener(LocID locID, ValueUpdated listener) {
		listeners[locID] -= listener;
	}

	public T GetValue(LocID variable, VariableTarget target) {
		Dictionary<LocID, List<T>> dictionary = values;
		if (target == VariableTarget.RuntimeValue) {
			dictionary = runtimeValues;
		}
		else if (target == VariableTarget.SceneValue) {
			dictionary = sceneValues;
		}
		if (dictionary != null) {
			if (dictionary.TryGetValue(variable, out List<T> val)) {
				if (val.Count > 0) {
					return val[0];
				}
			}
		}
		if (debug == true) Debug.LogError(variable + " not found from dictionary");
		return default(T);
	}

	public List<T> GetValues(LocID variable, VariableTarget target) {
		Dictionary<LocID, List<T>> dictionary = values;
		if (target == VariableTarget.RuntimeValue) {
			dictionary = runtimeValues;
		}
		else if (target == VariableTarget.SceneValue) {
			dictionary = sceneValues;
		}
		if (dictionary != null) {
			if (dictionary.TryGetValue(variable, out List<T> val)) {
				return val;
			}
		}
		if (debug == true) Debug.LogError(variable + " not found from dictionary");
		return new List<T> { default(T) };
	}

	public void SetValue(LocID variable, T value, VariableTarget target) {
		Dictionary<LocID, List<T>> dictionary = values;
		if (target == VariableTarget.RuntimeValue) {
			dictionary = runtimeValues;
		}
		else if (target == VariableTarget.SceneValue) {
			dictionary = sceneValues;
		}
		if (dictionary.ContainsKey(variable) == false) {
			dictionary.Add(variable, new List<T> { value });
		}
		else {
			dictionary[variable][0] = value;
			if (listeners.ContainsKey(variable) == true) {
				listeners[variable]?.Invoke(new List<T> { value });
			}
		}
	}

	public void SetValues(LocID variable, List<T> newValues, VariableTarget target) {
		Dictionary<LocID, List<T>> dictionary = values;
		if (target == VariableTarget.RuntimeValue) {
			dictionary = runtimeValues;
		}
		else if (target == VariableTarget.SceneValue) {
			dictionary = sceneValues;
		}
		if (dictionary.ContainsKey(variable) == false) {
			dictionary.Add(variable, newValues);
		}
		else {
			dictionary[variable] = newValues;
			if (listeners.ContainsKey(variable) == true) {
				listeners[variable]?.Invoke(newValues);
			}
		}
	}

}
