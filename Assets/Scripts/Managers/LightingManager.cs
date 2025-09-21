using MEC;
using ProjectEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightingManager : MonoBehaviour {

	public static LightingManager instance;
	public LightingProfileAsset[] lightingProfiles;
	public LightingProfile currentProfile;
	private CoroutineHandle transitionRoutine;

	private void Awake() {
		if (instance == null) {
			instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}

	private void OnDestroy() {
		if (instance == this) {
			instance = null;
		}
	}

	public void ChangeProfile(LightingProfile profile, float duration = 0F) {
		Timing.KillCoroutines(transitionRoutine);
		currentProfile = profile;
		transitionRoutine = Timing.RunCoroutine(TransitionToProfile(lightingProfiles[(int)profile], duration));
	}

	private IEnumerator<float> TransitionToProfile(LightingProfileAsset newProfile, float duration = 0F) {
		float changeDuration = duration;
		if (changeDuration > 0F) {
			LightingProfileAsset currentLightingProfile = ScriptableObject.CreateInstance<LightingProfileAsset>();
			currentLightingProfile.skyboxMaterial = RenderSettings.skybox;
			currentLightingProfile.skyboxTintColor = RenderSettings.skybox.GetColor("_Tint");
			currentLightingProfile.ambientGroundColor = RenderSettings.ambientGroundColor;
			currentLightingProfile.ambientEquatorColor = RenderSettings.ambientEquatorColor;
			currentLightingProfile.ambientSkyColor = RenderSettings.ambientSkyColor;
			currentLightingProfile.fog = RenderSettings.fog;
			currentLightingProfile.fogStartDistance = RenderSettings.fogStartDistance;
			currentLightingProfile.fogEndDistance = RenderSettings.fogEndDistance;
			currentLightingProfile.fogColor = RenderSettings.fogColor;
			RenderSettings.fog = newProfile.fog;
			RenderSettings.skybox = newProfile.skyboxMaterial;
			for (float i = 0F; i < 1F; i += Time.deltaTime / changeDuration) {
				RenderSettings.skybox.SetColor("_Tint", Color.Lerp(currentLightingProfile.skyboxTintColor, newProfile.skyboxTintColor, i));
				RenderSettings.ambientGroundColor = Color.Lerp(currentLightingProfile.ambientGroundColor, newProfile.ambientGroundColor, i);
				RenderSettings.ambientEquatorColor = Color.Lerp(currentLightingProfile.ambientEquatorColor, newProfile.ambientEquatorColor, i);
				RenderSettings.ambientSkyColor = Color.Lerp(currentLightingProfile.ambientSkyColor, newProfile.ambientSkyColor, i);
				RenderSettings.fogColor = Color.Lerp(currentLightingProfile.fogColor, newProfile.fogColor, i);
				RenderSettings.fogStartDistance = Mathf.Lerp(currentLightingProfile.fogStartDistance, newProfile.fogStartDistance, i);
				RenderSettings.fogEndDistance = Mathf.Lerp(currentLightingProfile.fogEndDistance, newProfile.fogEndDistance, i);
				yield return Timing.WaitForOneFrame;
			}
		}
		RenderSettings.fog = newProfile.fog;
		RenderSettings.skybox = newProfile.skyboxMaterial;
		RenderSettings.skybox.SetColor("_Tint", newProfile.skyboxTintColor);
		RenderSettings.ambientGroundColor = newProfile.ambientGroundColor;
		RenderSettings.ambientEquatorColor = newProfile.ambientEquatorColor;
		RenderSettings.ambientSkyColor = newProfile.ambientSkyColor;
		RenderSettings.fogStartDistance = newProfile.fogStartDistance;
		RenderSettings.fogEndDistance = newProfile.fogEndDistance;
		RenderSettings.fogColor = newProfile.fogColor;
	}

}
