using UnityEngine;

public class CustomLightFalloff : MonoBehaviour {
	public Light spotlight;
	public float maxIntensity = 10f; // Maximum intensity of the spotlight
	public AnimationCurve falloffCurve; // Custom falloff curve

	void Update() {
		// Ensure spotlight reference is set
		if (spotlight == null) {
			Debug.LogError("Spotlight reference not set!");
			return;
		}

		// Calculate falloff based on distance
		float distance = Vector3.Distance(transform.position, spotlight.transform.position);
		float normalizedDistance = Mathf.Clamp01(distance / spotlight.range);
		float intensity = falloffCurve.Evaluate(normalizedDistance) * maxIntensity;

		// Set the intensity of the spotlight
		spotlight.intensity = intensity;
	}
}
