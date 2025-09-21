using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ProjectEnums;
using System.Collections.Generic;
using UnityEngine.XR.Hands;

public class LeftHandMenuButton : WindowBase {

	public static LeftHandMenuButton instance;

	public AnimationCurve buttonAlphaCurve;
	public Button menuButton;
	private XRHandSubsystem handSubsystem;

	private bool tracked = true;

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		if (instance == null) {
			instance = this;
			base.Init(parameters, windowStack);
			List<XRHandSubsystem> systemList = new List<XRHandSubsystem>();
			SubsystemManager.GetSubsystems(systemList);
			for (int i = 0; i < systemList.Count; i++) {
				if (systemList[i].running == true) {
					handSubsystem = systemList[i];
					handSubsystem.trackingAcquired += OnTrackingAcquired;
					handSubsystem.trackingLost += OnTrackingLost;
				}
			}
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

	private void LateUpdate() {
		if (tracked == true && PlayerCharacter.instance.leftGrabbedInteractable == null) {
			Vector3 offsetX = PlayerCharacter.instance.leftWrist.right;
			Vector3 offsetY = PlayerCharacter.instance.leftWrist.up;
			Vector3 offsetZ = PlayerCharacter.instance.leftWrist.forward;
			Vector3 offsetMultiplier = new Vector3(0F, -0.02F, 0F);
			Vector3 offset = offsetX * offsetMultiplier.x + offsetY * offsetMultiplier.y + offsetZ * offsetMultiplier.z;
			transform.position = transform.parent.position + offset;
			float angle = Vector3.Angle(PlayerCharacter.instance.xrOrigin.Camera.transform.forward, PlayerCharacter.instance.leftWrist.up);
			transform.rotation = Quaternion.LookRotation(transform.position - PlayerCharacter.instance.xrOrigin.Camera.transform.position);
			float minimumAngle = 90F;
			float curveposition = Mathf.Clamp01((-angle + minimumAngle) / minimumAngle);
			canvasGroup.alpha = buttonAlphaCurve.Evaluate(curveposition);
		}
		else {
			canvasGroup.alpha = 0F;
		}
		if (Mathf.Approximately(canvasGroup.alpha, 0F) == true) {
			menuButton.enabled = false;
		}
		else {
			menuButton.enabled = true;
		}
	}

	protected override void OpeningAnimationFinished() {

	}

	void OnTrackingLost(XRHand hand) {
		if (hand.handedness == Handedness.Left) {
			tracked = false;
		}
	}

	void OnTrackingAcquired(XRHand hand) {
		if (hand.handedness == Handedness.Left) {
			tracked = true;
		}
	}

	protected override void Closing() {
		ClosingCleanup();
	}

	protected override void Destroying() {

	}

	private void ClosingCleanup() {
		if (instance == this) {
			instance = null;
			if (handSubsystem != null) {
				handSubsystem.trackingAcquired -= OnTrackingAcquired;
				handSubsystem.trackingLost -= OnTrackingLost;
			}
		}
	}

}
