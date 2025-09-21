using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectEnums;
using System;
using UnityEngine.XR.Hands;

public class GestureDetection : MonoBehaviour {

	private static Dictionary<HandGestureID, Action<Handedness>> gestureActivated = new Dictionary<HandGestureID, Action<Handedness>>();
	private static Dictionary<HandGestureID, Action<Handedness>> gestureDeactivated = new Dictionary<HandGestureID, Action<Handedness>>();
	private static HashSet<HandGestureID> activeGesturesLeft = new HashSet<HandGestureID>();
	private static HashSet<HandGestureID> activeGesturesRight = new HashSet<HandGestureID>();

	private void Awake() {
		RegisterActivationActions();
		RegisterDeactivationActions();
	}

	private void OnDestroy() {
		gestureActivated.Clear();
		gestureDeactivated.Clear();
		activeGesturesLeft.Clear();
		activeGesturesRight.Clear();
	}

	public void RegisterActivationActions() {
		gestureActivated.Add(HandGestureID.CameraPinch, (handedness) => {
			
		});

		gestureActivated.Add(HandGestureID.CameraPoint, (handedness) => {
			
		});

		gestureActivated.Add(HandGestureID.PalmUpPinch, (handedness) => {
			
		});
		
#if !UNITY_WSA
		gestureActivated.Add(HandGestureID.PalmUpPoint, (handedness) => {
			PlayerCharacter.instance.ActivateTeleportMode(true, handedness);
		});

		gestureActivated.Add(HandGestureID.ThumbUp, (handedness) => {
			PlayerCharacter.instance.GrabHovered(handedness);
			if (handedness == Handedness.Left) {
				PlayerCharacter.instance.ActivateJoystickMoveMode(true);
			}
			else {
				PlayerCharacter.instance.ActivateDynamicRotateMode(true);
			}
		});
#endif

		gestureActivated.Add(HandGestureID.PalmDownPinch, (handedness) => {
			PlayerCharacter.instance.ActivateGrabMoveMode(true, handedness);
		});

#if !UNITY_ANDROID
		gestureActivated.Add(HandGestureID.OpenPalmUp, (handedness) => {
			if (handedness == Handedness.Left) {
				PlayerCharacter.instance.ActivatePalmMoveMode(true);
			}
			else {
				PlayerCharacter.instance.ActivatePalmRotateMode(true);
			}
		});
#endif

		gestureActivated.Add(HandGestureID.Grab, (handedness) => {
			
		});

		gestureActivated.Add(HandGestureID.OpenHand, (handedness) => {
			PlayerCharacter.instance.DropGrabbed(handedness);
		});

		gestureActivated.Add(HandGestureID.Fist, (handedness) => {

		});
	}

	public void RegisterDeactivationActions() {
		gestureDeactivated.Add(HandGestureID.CameraPinch, (handedness) => {
			
		});

		gestureDeactivated.Add(HandGestureID.CameraPoint, (handedness) => {
			
		});

		gestureDeactivated.Add(HandGestureID.PalmUpPinch, (handedness) => {
			
		});

#if !UNITY_WSA
		gestureDeactivated.Add(HandGestureID.PalmUpPoint, (handedness) => {
			PlayerCharacter.instance.ActivateTeleportMode(false, handedness);
		});

		gestureDeactivated.Add(HandGestureID.ThumbUp, (handedness) => {
			if (handedness == Handedness.Left) {
				PlayerCharacter.instance.ActivateJoystickMoveMode(false);
			}
			else if (handedness == Handedness.Right) {
				PlayerCharacter.instance.ActivateDynamicRotateMode(false);
			}
		});
#endif

		gestureDeactivated.Add(HandGestureID.PalmDownPinch, (handedness) => {
			PlayerCharacter.instance.ActivateGrabMoveMode(false, handedness);
		});

#if !UNITY_ANDROID
		gestureDeactivated.Add(HandGestureID.OpenPalmUp, (handedness) => {
			if (handedness == Handedness.Left) {
				PlayerCharacter.instance.ActivatePalmMoveMode(false);
			}
			else if (handedness == Handedness.Right) {
				PlayerCharacter.instance.ActivatePalmRotateMode(false);
			}
		});
#endif

		gestureDeactivated.Add(HandGestureID.Grab, (handedness) => {
			
		});

		gestureDeactivated.Add(HandGestureID.OpenHand, (handedness) => {
			
		});

		gestureDeactivated.Add(HandGestureID.Fist, (handedness) => {

		});
	}

	public void ActivateGesture(HandGestureID id, Handedness handedness) {
		if (gestureActivated.ContainsKey(id) == true) {
			if (handedness == Handedness.Left) {
				if (activeGesturesLeft.Contains(id) == false) {
					activeGesturesLeft.Add(id);
					gestureActivated[id].Invoke(handedness);
				}
			}
			else if (handedness == Handedness.Right) {
				if (activeGesturesRight.Contains(id) == false) {
					activeGesturesRight.Add(id);
					gestureActivated[id].Invoke(handedness);
				}
			}
		}
	}

	public void DeactivateGesture(HandGestureID id, Handedness handedness) {
		if (gestureDeactivated.ContainsKey(id) == true) {
			if (handedness == Handedness.Left) {
				if (activeGesturesLeft.Contains(id) == true) {
					activeGesturesLeft.Remove(id);
					gestureDeactivated[id].Invoke(handedness);
				}
			}
			else if (handedness == Handedness.Right) {
				if (activeGesturesRight.Contains(id) == true) {
					activeGesturesRight.Remove(id);
					gestureDeactivated[id].Invoke(handedness);
				}
			}
		}
	}

	public bool GestureActive(HandGestureID id, Handedness handedness) {
		if (handedness == Handedness.Left) {
			return activeGesturesLeft.Contains(id);
		}
		else if (handedness == Handedness.Right) {
			return activeGesturesRight.Contains(id);
		}
		return activeGesturesLeft.Contains(id) || activeGesturesRight.Contains(id);
	}

}
