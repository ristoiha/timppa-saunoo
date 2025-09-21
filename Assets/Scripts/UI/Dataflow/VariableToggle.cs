using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ProjectEnums;
using RoboRyanTron.SearchableEnum;
using TMPro;

public class VariableToggle : MonoBehaviour {

	public static Dictionary<LocID, VariableToggle> variableToggles = new Dictionary<LocID, VariableToggle>();

	[SearchableEnum] public LocID variable;
	public TextMeshProUGUI nameText;
	public Toggle toggle;
	public bool defaultValue;

	private void Start() {
		//if (variableToggles.ContainsKey(variable) == false) {
		//	variableToggles.Add(variable, this);
		//}
		if (BoolVariable.values == null) {
			BoolVariable.values = new Dictionary<LocID, bool>();
		}
		if (BoolVariable.values.ContainsKey(variable) == false) {
			BoolVariable.values.Add(variable, defaultValue);
		}
		nameText.text = LocalizationManager.instance.GetString(variable);
		UpdateValueWithoutNotify(BoolVariable.values[variable]);
	}

	private void OnDestroy() {
		variableToggles.Remove(variable);
	}

	public void UpdateValue(bool boolValue) {
		UpdateValueWithoutNotify(boolValue);
		if (BoolVariable.listeners.ContainsKey(variable) == true) {
			BoolVariable.listeners[variable]?.Invoke(boolValue);
		}
	}

	public void UpdateValueWithoutNotify(bool boolValue) {
		toggle.SetIsOnWithoutNotify(boolValue);
		BoolVariable.values[variable] = boolValue;

	}

}
