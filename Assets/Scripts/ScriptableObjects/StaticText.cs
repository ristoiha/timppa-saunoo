using ProjectEnums;
using RoboRyanTron.SearchableEnum;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
[CreateAssetMenu(fileName = "StaticText", menuName = "ScriptableObjects/StaticText", order = 1)]
public class StaticText : VariableContent {

	public override void InstantiateUIPrefab(Transform parent, LocID overrideLocID = LocID.None) {
		Transform uiTransform = Instantiate(uiPrefab, parent);
		TextMeshProUGUI textComponent = uiTransform.GetComponent<TextMeshProUGUI>();
		if (textComponent != null) {
			if (overrideLocID != LocID.None) {
				textComponent.text = LocalizationManager.instance.GetString(overrideLocID);
			}
			else {
				textComponent.text = LocalizationManager.instance.GetString(locID);
			}
			return;
		}
	}

}
