using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ProjectEnums;
using RoboRyanTron.SearchableEnum;
using TMPro;
using System;

public class VariableSlider : MonoBehaviour {

	[SearchableEnum] public LocID variable;
	[SearchableEnum] public LocID unit = LocID.None;
	public TextMeshProUGUI nameText;
	public Slider slider;
	public TMP_InputField inputField;
	public float defaultValue;

	private void Start() {
		if (FloatVariable.values == null) {
			FloatVariable.values = new Dictionary<LocID, float>();
		}
		if (FloatVariable.values.ContainsKey(variable) == false) {
			FloatVariable.values.Add(variable, defaultValue);
		}
		if (unit != LocID.None) {
			nameText.text = LocalizationManager.instance.GetString(variable) + " (" + LocalizationManager.instance.GetString(unit) + ")";
		}
		else {
			nameText.text = LocalizationManager.instance.GetString(variable);
		}
		UpdateValueWithoutNotify(FloatVariable.values[variable]);
	}

	public void UpdateValue(float floatValue) {
		FloatVariable.SetValue(variable, floatValue);
		UpdateValueWithoutNotify(floatValue);
	}

	public void UpdateValueWithoutNotify(float floatValue) {
		int intValue = Mathf.RoundToInt(floatValue);
		if (slider.wholeNumbers == true) {
			inputField.SetTextWithoutNotify(intValue.ToString("0"));
		}
		else {
			inputField.SetTextWithoutNotify(floatValue.ToString("0.0000"));
		}
		slider.SetValueWithoutNotify(floatValue);
		FloatVariable.values[variable] = floatValue;
	}

	public void UpdateTextValue(string textValue) {
		if (int.TryParse(textValue, out int intResult)) {
			UpdateValue(intResult);
		}
		else if (float.TryParse(textValue, out float floatResult)) {
			UpdateValue(floatResult);
		}
	}

}