using ProjectEnums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KeyInputManager : MonoBehaviour {

	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape) == true) {
			GameInputLogic.EscapePressed();
		}

		if (Input.GetMouseButtonDown(1) == true) {
			GameInputLogic.SecondMouseButtonPressed();
		}

		if (Input.GetKey(KeyCode.LeftShift) == false && Input.GetKeyDown(KeyCode.Tab)) {
			GameInputLogic.TabPressed();
		}
		else if (Input.GetKey(KeyCode.LeftShift) == true && Input.GetKeyDown(KeyCode.Tab)) {
			GameInputLogic.ShiftTabPressed();
		}

		if (Input.GetKeyDown(KeyCode.Return) == true || Input.GetKeyDown(KeyCode.KeypadEnter) == true) {
			GameInputLogic.EnterOrReturnPressed();
		}

		if (Input.GetKeyDown(KeyCode.DownArrow) == true) {
			GameInputLogic.ClearChat();
		}
#if UNITY_EDITOR || UNITY_WEBGL
		//DetectMouseLook();
		DetectMovement();
#endif
	}

	private void DetectMovement() {
		Vector3 movementDirection = Vector3.zero;
		bool jump = Input.GetKeyDown(KeyCode.Space);
		if (Input.GetKey(KeyCode.W) == true) {
			movementDirection += Vector3.forward;
		}
		else if (Input.GetKey(KeyCode.S) == true) {
			movementDirection -= Vector3.forward;
		}

		if (Input.GetKey(KeyCode.A) == true) {
			movementDirection -= Vector3.right;
		}
		else if (Input.GetKey(KeyCode.D) == true) {
			movementDirection += Vector3.right;
		}

		if (Input.GetKey(KeyCode.Space) == true) {
			movementDirection += Vector3.up;
		}
		else if (Input.GetKey(KeyCode.C) == true || Input.GetKey(KeyCode.LeftControl) == true) {
			movementDirection -= Vector3.up;
		}

		//if (Input.GetKey(KeyCode.LeftShift) == true) {

		//}
		//else {

		//}
		GameInputLogic.Move(movementDirection, jump);
	}

	private void DetectMouseLook() {
		GameInputLogic.MouseLook(Input.mousePositionDelta);
	}
}
