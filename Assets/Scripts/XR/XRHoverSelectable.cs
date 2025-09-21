using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ProjectEnums;

public class XRHoverSelectable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {

	public bool isHoverSelectable = false;
	public bool singleClickPerHover = true;
	public bool isContinuousUIButton = false;
	public ButtonMessenger buttonMessenger;

	private bool isPressing = false;
	private Button button = null;

	private void Awake() {
		button = transform.GetComponent<Button>();
		if (isContinuousUIButton == true) {
			isHoverSelectable = false;
		}
		else {
#if VR
			this.enabled = false;
			if (button != null) {
				button.enabled = false;
			}
			else {
				Toggle toggle = GetComponent<Toggle>();
				if (toggle != null) {
					toggle.enabled = false;
				}
			}
#endif
		}
	}

	private void Update() {
		if (isContinuousUIButton == true && isPressing == true && button != null) {
			button.onClick?.Invoke();
		}
	}

	public void OnPointerDown(PointerEventData eventData) {
		isPressing = true;
		if (buttonMessenger != null) {
			buttonMessenger.PlayAudio();
		}
	}

	public void OnPointerUp(PointerEventData eventData) {
		if (isPressing == true) {
			isPressing = false;
			if (buttonMessenger != null) {
				buttonMessenger.PlayReleaseAudio();
			}
		}
	}
	
	public void OnPointerExit(PointerEventData eventData) {
		if (isPressing == true) {
			isPressing = false;
			if (buttonMessenger != null) {
				buttonMessenger.PlayReleaseAudio();
			}
		}
	}

}
