using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ProjectEnums;
using System.Collections.Generic;

public class GameUI : WindowBase {

	public static GameUI instance;

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		if (instance == null) {
			instance = this;
			base.Init(parameters, windowStack);
		}
		else {
			Destroy(gameObject);
		}
	}

	protected override void OnDestroy() {
		base.OnDestroy();
		ClosingCleanup();
	}

	public override void UpdateUI() {

	}

	protected override void OpeningAnimationFinished() {

	}

	protected override void Closing() {
		ClosingCleanup();
	}

	protected override void Destroying() {

	}

	private void ClosingCleanup() {
		if (instance == this) {
			instance = null;
		}
	}

}
