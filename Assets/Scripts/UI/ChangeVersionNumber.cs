using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeVersionNumber : MonoBehaviour {

	public TextMeshProUGUI versionText;

	private void Awake() {
		versionText.text = "v. " + Application.version.ToString();
	}

}
