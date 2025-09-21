using ProjectEnums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets.DynamicMoveProvider;

public static class GameInputLogic {

	public static void EscapePressed() {
		
	}

	public static void SecondMouseButtonPressed() {
		
	}

	public static void TabPressed() {
		if (EventSystem.current.currentSelectedGameObject != null) {
			Selectable next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnRight();
			if (next != null) {
				InputField inputfield = next.GetComponent<InputField>();
				if (inputfield != null) {
					inputfield.OnPointerClick(new PointerEventData(EventSystem.current));  //if it's an input field, also set the text caret
				}
				EventSystem.current.SetSelectedGameObject(next.gameObject, new BaseEventData(EventSystem.current));
			}
		}
		else {
			
		}
	}

	public static void ShiftTabPressed() {
		if (EventSystem.current.currentSelectedGameObject != null) {
			Selectable next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnLeft();
			if (next != null) {
				InputField inputfield = next.GetComponent<InputField>();
				if (inputfield != null) {
					inputfield.OnPointerClick(new PointerEventData(EventSystem.current));  //if it's an input field, also set the text caret
				}
				EventSystem.current.SetSelectedGameObject(next.gameObject, new BaseEventData(EventSystem.current));
			}
		}
	}

	public static void EnterOrReturnPressed() {
		//if (WindowManager.instance.escapeableWindowStack.Count == 0) {
		//	ServiceManager.instance.OpenChat();
		//}
		//else {
			// TODO: Check code, most likely contains errors and doesn't do what it is supposed to
			GameObject selectedGameObject = EventSystem.current.currentSelectedGameObject;
			if (selectedGameObject != null) {
				Button button = selectedGameObject.GetComponent<Button>();
				if (button != null) {
					button.onClick.Invoke();
				}
				else {
					// Try to find the next input field
					Selectable next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
					if (next != null) {
						InputField inputfield = next.GetComponent<InputField>();
						if (inputfield != null) {
							inputfield.OnPointerClick(new PointerEventData(EventSystem.current));
						}

						// If the next selectable is a button, invoke its onClick event automatically
						//Button nextButton = next.GetComponent<Button>();
						//if (nextButton != null) {
						//	nextButton.onClick.Invoke();
						//}

						EventSystem.current.SetSelectedGameObject(next.gameObject, new BaseEventData(EventSystem.current));
					}
					else {
						// If no next input field is found, set focus to the button
						// Assuming your button is the last element in the tab order
						EventSystem.current.SetSelectedGameObject(selectedGameObject);
					}
				}
			}
		//}
	}

	public static void ClearChat() {
		ServiceManager.instance.ClearChat();
	}

	public static void TouchStart(Vector2 startPos) {
		RaycastHit hitInfo;
		LayerMask raycastLayer = Gval.interactableLayer;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(startPos), out hitInfo, 10F, raycastLayer)) {
			UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable baseInteractable = hitInfo.collider.GetComponentInParent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
			if (baseInteractable != null) {
				SelectEnterEventArgs selectEnterEventArgs = new SelectEnterEventArgs();
				selectEnterEventArgs.interactorObject = PlayerCharacter.instance.rightHandDirectInteractor;
				selectEnterEventArgs.interactableObject = baseInteractable;
				baseInteractable.selectEntered?.Invoke(selectEnterEventArgs);
			}
		}
		else {
			((SaunaUI)WindowManager.instance.GetWindow(WindowPanel.SaunaUI)).StartDrag(startPos);
		}
	}

	public static void TouchEnd(Vector2 currentPos) {
		RaycastHit hitInfo;
		LayerMask raycastLayer = Gval.interactableLayer;
		if (Physics.Raycast(PlayerCharacter.instance.xrOrigin.Camera.ScreenPointToRay(currentPos), out hitInfo, 10F, raycastLayer)) {
			UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable baseInteractable = hitInfo.collider.GetComponentInParent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
			//XRSocketInteractor socketInteractor = hitInfo.collider.GetComponentInParent<XRSocketInteractor>();
			if (baseInteractable != null) {
				SelectExitEventArgs selectExitEventArgs = new SelectExitEventArgs();
				selectExitEventArgs.interactorObject = PlayerCharacter.instance.rightHandDirectInteractor;
				selectExitEventArgs.interactableObject = baseInteractable;
				baseInteractable.selectExited?.Invoke(selectExitEventArgs);
			}
		}
		else {
			((SaunaUI)WindowManager.instance.GetWindow(WindowPanel.SaunaUI)).EndDrag(currentPos);
		}
		PlayerCharacter.instance.DropGrabbed(UnityEngine.XR.Hands.Handedness.Right);
	}

	public static void SwipeActive(Vector2 startPos, Vector2 direction, float swipeLength) {
		if (PlayerCharacter.instance.movementEnabled == true) {
			VirtualJoystick.instance.HandleSwipe(startPos, direction, swipeLength);
			((SaunaUI)WindowManager.instance.GetWindow(WindowPanel.SaunaUI)).PoikaSaunoo(startPos + direction);
		}
		else {
			Camera cam = Camera.main;
			Vector3 horizontalDirection = cam.transform.right * direction.x;
			Vector3 depthDirection = cam.transform.forward * direction.y;
			horizontalDirection.y = 0F;
			depthDirection.y = 0F;
			Vector3 combinedDirection = horizontalDirection + depthDirection;
			combinedDirection.y = direction.y; // Use raw swipe y direction for up/down movement
			if (PlayerCharacter.instance.rightGrabbedInteractable != null) {
				// Do something with swipe
			}
		}
	}

	public static void SwipeEnd(Vector2 startPos, Vector2 direction, float swipeLength) {
		if (PlayerCharacter.instance.movementEnabled == true) {
			VirtualJoystick.instance.HandleSwipeEnd(startPos);
		}
		PlayerCharacter.instance.DropGrabbed(UnityEngine.XR.Hands.Handedness.Right);
	}

	public static void UpdatePinch(float startDistance, float currentDistance) {
		
	}

	public static void ApplyPinch(float startDistance, float currentDistance) {
		
	}

	public static void Move(Vector3 movementDirection, bool jump) {
		PlayerCharacter.instance.Move(new Vector2(movementDirection.x, movementDirection.z));
	}
	
	public static void MouseLook(Vector2 mouseDelta) {
		//PlayerCharacter.instance.Rotate(new Vector2(mouseDelta.x, mouseDelta.y));
	}

	public static void NoActiveInput() {
		VirtualJoystick.instance.ResetAll();
	}

	public static void UnlockCursor() {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public static void LockCursor() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

}
