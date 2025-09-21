using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectEnums;
using static UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets.DynamicMoveProvider;

public class VirtualJoystick : MonoBehaviour {

	public static VirtualJoystick instance;

	public AnimationCurve gainCurve;

	private bool moveActive = false;
	private bool rotateActive = false;
	private Vector2 moveStartPos;
	private Vector2 rotateStartPos;
	private float maxSwipeLength = 0.12F;

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

	public void HandleSwipe(Vector2 startPos, Vector2 direction, float swipeLength) {
		if (startPos.x < Screen.width / 2F) {
			HandleMove(startPos, direction, swipeLength);
		}
		else {
#if UNITY_EDITOR || UNITY_WEBGL
			HandleRotation(startPos, new Vector2(direction.x, direction.y), swipeLength);
#else
		HandleRotation(startPos, new Vector2(direction.x, 0F), swipeLength);
#endif

		}
	}

	public void HandleSwipeEnd(Vector2 startPos) {
		if (startPos.x < Screen.width / 2F) {
			DeactivateMove(startPos);
		}
		else {
			DeactivateRotation(startPos);
		}
	}

	private void HandleMove(Vector2 startPos, Vector2 direction, float swipeLength) {
		ActivateMove(startPos);
		if (Tools.Vector2Approximately(startPos, moveStartPos) == true) {
			float swipeStr = GetSwipeStrength(swipeLength);
			PlayerCharacter.instance.Move(direction.normalized * swipeStr);
			//if (direction.y > 0F) { TaskManager.instance.CompleteTask(LocID.Task_WalkForward_Mobile); }
			//if (direction.y < 0F) { TaskManager.instance.CompleteTask(LocID.Task_WalkBackward_Mobile); }
			//if (direction.x < 0F) { TaskManager.instance.CompleteTask(LocID.Task_WalkLeft_Mobile); }
			//if (direction.x > 0F) { TaskManager.instance.CompleteTask(LocID.Task_WalkRight_Mobile); }
			VirtualJoystickPanel joystickPanel = (VirtualJoystickPanel)WindowManager.instance.OpenWindow(WindowPanel.VirtualJoystickPanel);
			joystickPanel.UpdateMoveJoystick(startPos, direction.normalized * Mathf.Clamp01(swipeLength / maxSwipeLength));
		}
	}

	private void HandleRotation(Vector2 startPos, Vector2 direction, float swipeLength) {
		ActivateRotation(startPos);
		if (Tools.Vector2Approximately(startPos, rotateStartPos) == true) {
			float swipeStr = GetSwipeStrength(swipeLength);
			PlayerCharacter.instance.Rotate(direction.normalized * swipeStr);
			//if (direction.x < 0F) { TaskManager.instance.CompleteTask(LocID.Task_RotateLeft_Mobile); }
			//else if (direction.x > 0F) { TaskManager.instance.CompleteTask(LocID.Task_RotateRight_Mobile); }
			VirtualJoystickPanel joystickPanel = (VirtualJoystickPanel)WindowManager.instance.OpenWindow(WindowPanel.VirtualJoystickPanel);
			joystickPanel.UpdateRotateJoystick(startPos, direction.normalized * Mathf.Clamp01(swipeLength / maxSwipeLength));
		}
	}

	private void ActivateMove(Vector2 startPos) {
		if (moveActive == false) {
			moveActive = true;
			moveStartPos = startPos;
			VirtualJoystickPanel joystickPanel = (VirtualJoystickPanel)WindowManager.instance.OpenWindow(WindowPanel.VirtualJoystickPanel);
			joystickPanel.ShowMoveJoystick();
		}
	}

	private void DeactivateMove(Vector2 startPos, bool forceDeactivate = false) {
		if (moveActive == true && (Tools.Vector2Approximately(moveStartPos, startPos) == true || forceDeactivate == true)) {
			moveActive = false;
			moveStartPos = -Vector2.one;
			VirtualJoystickPanel joystickPanel = (VirtualJoystickPanel)WindowManager.instance.OpenWindow(WindowPanel.VirtualJoystickPanel);
			joystickPanel.HideMoveJoystick();
		}
	}

	private void ActivateRotation(Vector2 startPos) {
		if (rotateActive == false) {
			rotateActive = true;
			rotateStartPos = startPos;
			VirtualJoystickPanel joystickPanel = (VirtualJoystickPanel)WindowManager.instance.OpenWindow(WindowPanel.VirtualJoystickPanel);
			joystickPanel.ShowRotateJoystick();
		}
	}

	private void DeactivateRotation(Vector2 startPos, bool forceDeactivate = false) {
		if (rotateActive == true && (Tools.Vector2Approximately(rotateStartPos, startPos) == true || forceDeactivate == true)) {
			rotateActive = false;
			rotateStartPos = -Vector2.one;
			VirtualJoystickPanel joystickPanel = (VirtualJoystickPanel)WindowManager.instance.OpenWindow(WindowPanel.VirtualJoystickPanel);
			joystickPanel.HideRotateJoystick();
		}
	}

	private float GetSwipeStrength(float swipeLength) {
		return gainCurve.Evaluate(Mathf.Clamp01(swipeLength / maxSwipeLength));
	}

	public void ResetAll() {
		DeactivateMove(Vector2.zero, true);
		DeactivateRotation(Vector2.zero, true);
	}
}
