using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ProjectEnums;
using RoboRyanTron.SearchableEnum;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class LocIDDropdown : MonoBehaviour {

	//public static Dictionary<LocID, LocIDDropdown> locIDDropdowns = new Dictionary<LocID, LocIDDropdown>();

	[SearchableEnum] public LocID variable;
	public TextMeshProUGUI nameText;
	[SearchableEnum] public LocID[] options;
	public TMP_Dropdown dropdown;

	[System.NonSerialized] public int currentIndex;

	private List<string> stringList = new List<string>();

	private void Start() {
		if (options.Length > 0) {
			ChangeOptions(options);
		}
		//if (locIDDropdowns.ContainsKey(variable) == false) {
		//	locIDDropdowns.Add(variable, this);
		//}
		if (LocIDVariable.values == null) {
			LocIDVariable.values = new Dictionary<LocID, LocID>();
		}
		if (LocIDVariable.values.ContainsKey(variable) == false) {
			LocIDVariable.values.Add(variable, options[0]);
		}
		nameText.text = LocalizationManager.instance.GetString(variable);
		SelectOption(LocIDVariable.values[variable]);
	}

	public void ChangeOptions(LocID[] newOptions) {
		dropdown.ClearOptions();
		CreateStringListFromOptions(newOptions);
		dropdown.AddOptions(stringList);
		options = newOptions;
	}

	public void OptionSelected(int index) {
		currentIndex = index;
		LocIDVariable.values[variable] = options[index];
		EventSystem.current.SetSelectedGameObject(null);
	}

	public void SelectOption(LocID locID) {
		int index = Array.IndexOf(options, locID);
		LocIDVariable.values[variable] = locID;
		dropdown.SetValueWithoutNotify(index);
		currentIndex = index;
	}

	private void CreateStringListFromOptions(LocID[] newOptions) {
		stringList.Clear();
		for (int i = 0; i < newOptions.Length; i++) {
			stringList.Add(LocalizationManager.instance.GetString(newOptions[i]));
		}
	}

}
