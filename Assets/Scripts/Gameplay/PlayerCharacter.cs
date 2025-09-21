using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
//using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using ProjectEnums;
using UnityEngine.InputSystem;
using MEC;

public class PlayerCharacter : MonoBehaviour {

	public static PlayerCharacter instance;

	public Transform leftWrist;
	public Transform rightWrist;
	public UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor rightHandDirectInteractor;
	public AnimationCurve movementGainCurve;
	public GameObject[] teleportInteractors;
	public float moveSpeed = 1F;

	[System.NonSerialized]
	public XROrigin xrOrigin;
	[System.NonSerialized]
	public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable leftGrabbedInteractable;
	[System.NonSerialized]
	public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable rightGrabbedInteractable;
	[System.NonSerialized]
	public bool movementEnabled = true;

	private float rotationSpeed = 100F;
	private bool rotateModeActive = false;
	private bool palmMoveModeActive = false;
	private bool palmRotateModeActive = false;
	private bool joystickMoveModeActive = false;
	private bool grabMoveModeActive = false;
	private bool debug = false;
	private Transform grabMoveHand;
	private Vector3 grabMoveStartPos;
	private Vector3 previousGrabMoveDelta;
	private Vector3 palmMoveStartPos;
	private Vector3 palmRotateStartPos;
	private float joystickMoveZMultiplier = 1.6F; // magic number, because harder to rotate wrist forward and backward
	private float joystickMoveZOffset = 0.45F; // Magic number to move forward more easily
	private float grabMoveMultiplier = 2F;
	private float palmControlMultiplier = 0.1F; // max move/rotate when hand moves this much in meters
	private Vector3 leftHandPrevPos = Vector3.zero;
	private Vector3 rightHandPrevPos = Vector3.zero;

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

	private void Update() {
		if (movementEnabled == true) {
			MovementUpdate();
		}
	}

	public void MovementUpdate() {
		if (joystickMoveModeActive == true) {
			float xMove = Vector3.Dot(leftWrist.right, xrOrigin.Camera.transform.right);
			float zMove = (Vector3.Dot(leftWrist.right, xrOrigin.Camera.transform.forward) + joystickMoveZOffset) * joystickMoveZMultiplier;
			Vector2 alteredMoveVector = new Vector2(xMove, zMove);
			float moveStrength = movementGainCurve.Evaluate(Mathf.Clamp01(alteredMoveVector.magnitude));
			Vector2 moveVector = alteredMoveVector.normalized * moveStrength;
			ActivateMovementDirectionIndicator(moveVector);
			Move(moveVector);
		}
		if (palmMoveModeActive == true) {
			Vector3 posRelativeToCamera = leftWrist.position - xrOrigin.Camera.transform.position;
			float xMove = Vector3.Dot(posRelativeToCamera, xrOrigin.Camera.transform.right);
			float zMove = Vector3.Dot(posRelativeToCamera, xrOrigin.Camera.transform.forward);
			Vector2 newMovement = new Vector3(xMove - palmMoveStartPos.x, zMove - palmMoveStartPos.z);
			float moveStrength = movementGainCurve.Evaluate(Mathf.Clamp01(newMovement.magnitude / palmControlMultiplier));
			Move(newMovement.normalized * moveStrength);
		}
		if (palmRotateModeActive == true) {
			Vector3 posRelativeToCamera = rightWrist.position - xrOrigin.Camera.transform.position;
			float yRotate = Vector3.Dot(posRelativeToCamera, xrOrigin.Camera.transform.right);
			float zRotate = Vector3.Dot(posRelativeToCamera, xrOrigin.Camera.transform.forward);
			Vector2 newRotate = new Vector3(yRotate - palmRotateStartPos.x, zRotate - palmRotateStartPos.z);
			float rotateStrength = movementGainCurve.Evaluate(Mathf.Clamp01(newRotate.magnitude / palmControlMultiplier));
			Rotate(newRotate.normalized * rotateStrength);
		}
		else if (grabMoveModeActive == true) {
			Vector3 newGrabMoveDelta = (grabMoveHand.transform.position - xrOrigin.Camera.transform.position) - grabMoveStartPos;
			Vector3 newMovement = newGrabMoveDelta - previousGrabMoveDelta;
			previousGrabMoveDelta = newGrabMoveDelta;
			newMovement = new Vector3(newMovement.x, 0F, newMovement.z) * grabMoveMultiplier;
			xrOrigin.MoveCameraToWorldLocation(xrOrigin.Camera.transform.position - newMovement);
		}
		if (rotateModeActive == true) {
			float zMultiplier = 1.6F; // magic number, because harder to rotate wrist forward and backward
			float zOffset = 0.45F; // Magic number to move forward more easily
			float yRotate = Vector3.Dot(-rightWrist.right, xrOrigin.Camera.transform.right);
			float zRotate = (Vector3.Dot(-rightWrist.right, xrOrigin.Camera.transform.forward) + zOffset) * zMultiplier;
			Vector2 alteredRotateVector = new Vector2(yRotate, zRotate);
			float rotateStrength = movementGainCurve.Evaluate(Mathf.Clamp01(alteredRotateVector.magnitude));
			Vector2 rotateVector = alteredRotateVector.normalized * rotateStrength;
			ActivateRotationDirectionIndicator(rotateVector);
			Rotate(rotateVector);
		}
	}

	public void Move(Vector2 direction) {
		if (movementEnabled == true) {
			Vector3 moveDirection = Camera.main.transform.right * direction.x + Camera.main.transform.forward * direction.y;
			moveDirection = new Vector3(moveDirection.x, 0F, moveDirection.z);
#if VR
			xrOrigin.MoveCameraToWorldLocation(xrOrigin.Camera.transform.position + moveDirection * moveSpeed * Time.deltaTime);
#else
			transform.position += moveDirection * moveSpeed * Time.deltaTime;
#endif
		}
	}

	public void MoveToPosition(Vector3 position) {
		if (movementEnabled == true) {
			movementEnabled = false;
			Timing.RunCoroutine(MoveToPositionRoutine(transform.position, position, 0.3F));
		}
	}

	public void Rotate(Vector2 direction) {
		if (movementEnabled == true) {
			float yRotation = direction.x * rotationSpeed * Time.deltaTime;
			float zRotation = -direction.y * rotationSpeed * Time.deltaTime;
//#if UNITY_WEBGL || (!VR && UNITY_EDITOR)
//			xrOrigin.Camera.transform.Rotate(Vector3.up, yRotation, Space.World);
//			xrOrigin.Camera.transform.Rotate(Vector3.right, zRotation, Space.Self);
//#else
//			xrOrigin.RotateAroundCameraPosition(Vector3.up, yRotation);
//			// xrOrigin.RotateAroundCameraPosition(Vector3.right, zRotation); // Not needed, player controls vertical rotation with AR/VR device
//#endif
		}
	}

	private void ActivateMovementDirectionIndicator(Vector2 moveVector) {
		MovementDirection directionIndicator = MovementDirection.None;
		float directionIndicatorThreshold = 0.15F;
		if (moveVector.x < -directionIndicatorThreshold) {
			directionIndicator |= MovementDirection.Left;
		}
		if (moveVector.x > directionIndicatorThreshold) {
			directionIndicator |= MovementDirection.Right;
		}
		if (moveVector.y < -directionIndicatorThreshold) {
			directionIndicator |= MovementDirection.Back;
		}
		if (moveVector.y > directionIndicatorThreshold) {
			directionIndicator |= MovementDirection.Forward;
		}
		if (GesturePanelLeftWrist.instance != null) {
			GesturePanelLeftWrist.instance.ChangeMovementDirection(directionIndicator);
		}
	}

	private void ActivateRotationDirectionIndicator(Vector2 rotateVector) {
		RotateDirection directionIndicator = RotateDirection.None;
		float directionIndicatorThreshold = 0.15F;
		if (rotateVector.x < -directionIndicatorThreshold) {
			directionIndicator |= RotateDirection.Left;
		}
		if (rotateVector.x > directionIndicatorThreshold) {
			directionIndicator |= RotateDirection.Right;
		}
		if (GesturePanelRightWrist.instance != null) {
			GesturePanelRightWrist.instance.ChangeRotateDirection(directionIndicator);
		}
	}

	public void ActivateTeleportMode(bool activate, Handedness handedness) {
		LocID movementStyle = LocIDVariable.GetValue(LocID.Option_MovementStyle);
		if (movementStyle == LocID.MovementStyle_NoDrag || movementStyle == LocID.MovementStyle_TeleportAndDrag) {
			if (handedness == Handedness.Left) {
				teleportInteractors[0].gameObject.SetActive(activate);
			}
			else if (handedness == Handedness.Right) {
				teleportInteractors[1].gameObject.SetActive(activate);
			}
			if (debug == true) Debug.Log("Teleportation mode activate: " + activate);
		}
	}

	public void ActivateJoystickMoveMode(bool activate) {
		joystickMoveModeActive = activate;
		if (GesturePanelLeftWrist.instance != null) {
			GesturePanelLeftWrist.instance.Activate(activate);
		}
		if (debug == true) Debug.Log("Joystic move mode activate: " + activate);
	}

	public void ActivatePalmMoveMode(bool activate) {
		palmMoveModeActive = activate;
		Vector3 posRelativeToCamera = leftWrist.position - xrOrigin.Camera.transform.position;
		float xMove = Vector3.Dot(posRelativeToCamera, xrOrigin.Camera.transform.right);
		float zMove = Vector3.Dot(posRelativeToCamera, xrOrigin.Camera.transform.forward);
		palmMoveStartPos = new Vector3(xMove, 0F, zMove);
		if (debug == true) Debug.Log("Palm move mode activate: " + activate);
	}

	public void ActivatePalmRotateMode(bool activate) {
		palmRotateModeActive = activate;
		Vector3 posRelativeToCamera = rightWrist.position - xrOrigin.Camera.transform.position;
		float yRotate = Vector3.Dot(posRelativeToCamera, xrOrigin.Camera.transform.right);
		float zRotate = Vector3.Dot(posRelativeToCamera, xrOrigin.Camera.transform.forward);
		palmRotateStartPos = new Vector3(yRotate, 0F, zRotate);
		if (debug == true) Debug.Log("Palm move mode activate: " + activate);
	}

	public void ActivateGrabMoveMode(bool activate, Handedness handedness) {
		LocID movementStyle = LocIDVariable.GetValue(LocID.Option_MovementStyle);
		if (movementStyle == LocID.MovementStyle_NoTeleport || movementStyle == LocID.MovementStyle_TeleportAndDrag) {
			if (activate == true && grabMoveHand == null) {
				if (handedness == Handedness.Right && rightGrabbedInteractable == null) {
					grabMoveHand = rightWrist;
				}
				else if (handedness == Handedness.Left && leftGrabbedInteractable == null) {
					grabMoveHand = leftWrist;
				}

				if (grabMoveHand != null) {
					grabMoveStartPos = grabMoveHand.position - xrOrigin.Camera.transform.position;
					previousGrabMoveDelta = Vector3.zero;
				}
			}
			else if (activate == false && grabMoveHand != null) {
				if ((handedness == Handedness.Right && grabMoveHand == rightWrist) || (handedness == Handedness.Left && grabMoveHand == leftWrist)) {
					grabMoveHand = null;
				}
			}
			if (grabMoveHand != null && activate == true) {
				grabMoveModeActive = true;
			}
			else if (grabMoveHand == null && activate == false) {
				grabMoveModeActive = false;
			}
			if (debug == true) Debug.Log("Grab move mode activate: " + activate);
		}
	}

	public void ActivateDynamicRotateMode(bool activate) {
		rotateModeActive = activate;
		if (GesturePanelRightWrist.instance != null) {
			GesturePanelRightWrist.instance.Activate(activate);
		}
		//LocIDVariable.SetSceneValue(LocID.TaskComplete, LocID.Task_ThumbUpGestureRightHand);
		if (debug == true) Debug.Log("Rotate mode activate: " + activate);
	}

	public void EnableMovement(bool enable) {
		movementEnabled = enable;
		if (debug == true) Debug.Log("Movement enabled: " + enable);
	}

	public void GrabHovered(Handedness handedness) {
		if (handedness == Handedness.Left) {
			if (HoveredInteractable.activateArgsLeft != null) {
				GrabInteractable((UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable)HoveredInteractable.activateArgsLeft.interactableObject, handedness, false);
			}
		}
		else {
			if (HoveredInteractable.activateArgsRight != null) {
				GrabInteractable((UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable)HoveredInteractable.activateArgsRight.interactableObject, handedness, false);
			}
		}
		if (debug == true) Debug.Log("Grab hover activated");
	}

	public void DropGrabbed(Handedness handedness) {
		if (handedness == Handedness.Left) {
			if (leftGrabbedInteractable != null) {
				leftGrabbedInteractable = null;
				if (leftGrabbedInteractable == null && rightGrabbedInteractable == null) {
					EnableMovement(true);
				}
			}
		}
		else {
			if (rightGrabbedInteractable != null) {
				rightGrabbedInteractable = null;
				if (leftGrabbedInteractable == null && rightGrabbedInteractable == null) {
					EnableMovement(true);
				}
			}
		}
		if (debug == true) Debug.Log("Grab hover deactivated");
	}

	public void GrabInteractable(UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable, Handedness handedness, bool setMovementEnabled = false) {
		if (handedness == Handedness.Left) {
			leftGrabbedInteractable = interactable;
		}
		else {
			rightGrabbedInteractable = interactable;
		}
		EnableMovement(setMovementEnabled);
	}

	private IEnumerator<float> MoveToPositionRoutine(Vector3 startPos, Vector3 endPos, float duration) {
		for (float i = 0F; i < 1F; i += 1F / duration * Time.unscaledDeltaTime) {
			transform.position = Vector3.Lerp(startPos, endPos, i);
			yield return Timing.WaitForOneFrame;
		}
		transform.position = endPos;
		movementEnabled = true;
	}

}
