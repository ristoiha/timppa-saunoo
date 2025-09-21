using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AspectRatioMatcher : MonoBehaviour {

	public bool reverseMatch = false;

	private void Update() {
		
		CanvasScaler canvasScaler = transform.GetComponent<CanvasScaler>();
		float aspect = Screen.width / Screen.height;
		float inverseAspect = Screen.height / Screen.width;
		float screenAspect;
		bool landscape = canvasScaler.referenceResolution.x > canvasScaler.referenceResolution.y;

		if (landscape == true) {
			screenAspect = aspect;
		}
		else {
			screenAspect = inverseAspect;
		}

		if (screenAspect > 16F / 9F || (screenAspect < 16F / 9F && reverseMatch == true)) { // Screen is wider than 16:9 (longer when portrait)
			canvasScaler.matchWidthOrHeight = 0F;
		}
		else { // Screen is narrower than 16:9 (shorter when portrait)
			canvasScaler.matchWidthOrHeight = 1F;
		}
	}
}