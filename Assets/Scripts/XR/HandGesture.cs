using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Hands.Gestures;
using ProjectEnums;

public class HandGesture : MonoBehaviour {

	public XRHandTrackingEvents handTrackingEvents;
	public ScriptableObject handShapeOrPose;
	public HandGestureID id;
	public float minimumHoldTime = 0.1F;
	public float gestureDetectionInterval = 0.1F;
	public float gestureStayBeforeDeactivation = 0F;
	public Transform targetTransform;

	private GestureDetection gestureDetection;
	private XRHandShape handShape;
	private XRHandPose handPose;
	private bool wasDetected;
	private bool performedTriggered;
	private float timeOfLastConditionCheck;
	private float holdStartTime;
	private float deactivateStartTime;

	private void Awake() {
		gestureDetection = transform.GetComponentInParent<GestureDetection>();
	}

	void OnEnable() {
		handTrackingEvents.jointsUpdated.AddListener(OnJointsUpdated);
		handShape = handShapeOrPose as XRHandShape;
		handPose = handShapeOrPose as XRHandPose;
		if (handPose != null && handPose.relativeOrientation != null) {
			handPose.relativeOrientation.targetTransform = targetTransform;
		}
	}

	void OnDisable() {
		handTrackingEvents.jointsUpdated.RemoveListener(OnJointsUpdated);
	}

	void OnJointsUpdated(XRHandJointsUpdatedEventArgs eventArgs) {
		if (!isActiveAndEnabled || Time.timeSinceLevelLoad < timeOfLastConditionCheck + gestureDetectionInterval) {
			return;
		}
		bool detected =	handTrackingEvents.handIsTracked &&
			handShape != null && handShape.CheckConditions(eventArgs) ||
			handPose != null && handPose.CheckConditions(eventArgs);

		if (wasDetected == false && detected == true) {
			holdStartTime = Time.timeSinceLevelLoad;
		}
		else if (wasDetected == true && detected == false) {
			deactivateStartTime = Time.timeSinceLevelLoad;
		}

		wasDetected = detected;
		if (performedTriggered == false && detected == true) {
			float holdTimer = Time.timeSinceLevelLoad - holdStartTime;
			if (holdTimer > minimumHoldTime) {
				gestureDetection.ActivateGesture(id, handTrackingEvents.handedness);
				performedTriggered = true;
			}
		}

		if (performedTriggered == true && detected == false) {
			float releaseTimer = Time.timeSinceLevelLoad - deactivateStartTime;
			if (releaseTimer >= gestureStayBeforeDeactivation) {
				gestureDetection.DeactivateGesture(id, handTrackingEvents.handedness);
				performedTriggered = false;
			}
		}
		timeOfLastConditionCheck = Time.timeSinceLevelLoad;
	}
}