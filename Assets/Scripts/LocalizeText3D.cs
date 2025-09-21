using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizeText3D : LocalizedObject {

	private TextMeshPro localizedText;
	public TMP_FontAsset font {
		get { return localizedText.font; }
		set {
			Setup();
			localizedText.font = value;
		}
	}
	public Color color {
		get { return localizedText.color; }
		set {
			Setup();
			localizedText.color = value;
		}
	}
	protected override void Setup() {
		if (localizedText != null)
			return;
		localizedText = GetComponent<TextMeshPro>();
		if (localizedText == null) {
			Debug.LogError($"No text component found in {gameObject.name}", gameObject);
			return;
		}
		base.Setup();

	}

	protected override void UpdateContent() {
		if (localizedText)
			localizedText.text = LocalizationManager.instance.GetString(locID, gameObject);
		base.UpdateContent();
	}
}
