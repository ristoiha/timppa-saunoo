using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProjectEnums;
using TMPro;

public class BoneSelectionPanel : WindowBase {

	public static BoneSelectionPanel instance;
	private void Awake() {
		if (instance == null) {
			instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}
	public override void Init(object parameters, List<WindowStackEntry> windowStack) {
		base.Init(parameters, windowStack);
	}

	public override void ReInit(object parameters = null) {

	}

	public override void UpdateUI() {

	}

	protected override void OpeningAnimationFinished() {

	}

	protected override void Closing() {
		UserProfile.SaveCurrent();
	}

	protected override void Destroying() {

	}

	//public void UpdateTrailsTrajectoryDuration(float showTime) {
	//	//UserProfile.CurrentProfile.trailsTrajectoryDuration = showTime;
	//	GameInitializer.instance.UpdateTrailsTrajectoryDuration();
	//	UpdateUI();
	//}

}