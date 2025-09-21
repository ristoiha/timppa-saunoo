using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWallInteraction : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	public LayerMask collisionMask;

	private float fadeDuration = 0.15F;
	private float collisionRadius;
	private bool overlayOpaque = false;
	private CoroutineHandle transitionRoutine;

	private void Awake() {
		collisionRadius = GetComponent<SphereCollider>().radius;
	}

	private void Update() {
		if (Physics.CheckSphere(transform.position, collisionRadius, collisionMask, QueryTriggerInteraction.Ignore) == true) {
			if (overlayOpaque == false) {
				Timing.KillCoroutines(transitionRoutine);
				transitionRoutine = Timing.RunCoroutine(MakeOpaque(true));
			}
		}
		else {
			if (overlayOpaque == true) {
				Timing.KillCoroutines(transitionRoutine);
				transitionRoutine = Timing.RunCoroutine(MakeOpaque(false));
			}
		}
	}

	IEnumerator<float> MakeOpaque(bool opaque) {
		overlayOpaque = opaque;
		Color currentColor = spriteRenderer.color;
		Color targetColor = Color.black;
		if (opaque == false) {
			targetColor = Color.clear;
		}
		for (float i = 0F; i < 1F; i += Time.deltaTime / fadeDuration) {
			spriteRenderer.color = Color.Lerp(currentColor, targetColor, i);
			yield return Timing.WaitForOneFrame;
		}
		spriteRenderer.color = targetColor;
	}

}
