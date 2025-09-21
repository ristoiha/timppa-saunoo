using System.Collections;
using UnityEngine;
using ProjectEnums;
using System.Collections.Generic;

public class DefaultWindow : WindowBase {

	//public static DefaultWindow instance;

	private void Awake() {
		//if (instance == null) {
		//	instance = this;
		//}
		//else {
		//	Destroy(gameObject);
		//}
	}

	protected override void OnDestroy() {
		//if (instance == this) {
		//	instance = null;
		//}
	}

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		base.Init(parameters, windowStack);
	}

	public override void UpdateUI() {

	}

	protected override void OpeningAnimationFinished() {

	}

	protected override void Closing() {
		//if (instance == this) {
		//	instance = null;
		//}
	}

	protected override void Destroying() {

	}

}
