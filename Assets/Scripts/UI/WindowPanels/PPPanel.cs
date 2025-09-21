using UnityEngine;
using UnityEngine.UI;
using ProjectEnums;
using TMPro;
using System.Collections.Generic;

public class PPPanel : WindowBase {

	public TextMeshProUGUI text;
	public TextAsset privacyPolicyText;

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		text.text = privacyPolicyText.ToString();
		text.ForceMeshUpdate();
		Tools.ForceUpdateLayoutChildrenFirst(overrideHandleTarget);
		base.Init(parameters, windowStack);
	}

	public override void UpdateUI() {

	}

	protected override void OpeningAnimationFinished() {

	}

	protected override void Closing() {

	}

	protected override void Destroying() {

	}

}
