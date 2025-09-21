using ProjectEnums;
using UnityEngine;

public class CustomLightArea : MonoBehaviour {

	public LightingProfile profile;
	public float transitionDuration = 0.5F;
	public bool disableTriggerAfterActivation = false;
	private LightingProfile previousProfile;

	private void OnTriggerEnter() {
		previousProfile = LightingManager.instance.currentProfile;
		LightingManager.instance.ChangeProfile(profile, transitionDuration);
		if (disableTriggerAfterActivation == true) {
			gameObject.SetActive(false);
		}
	}

	private void OnTriggerExit() {
		LightingManager.instance.ChangeProfile(previousProfile, transitionDuration);
	}

}
