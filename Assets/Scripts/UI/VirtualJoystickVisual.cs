using ProjectEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualJoystickVisual : MonoBehaviour {

	public Image background;
	public RectTransform handle;
	public float handleMaxDistance;
	public Sprite[] backgroundSprites;

	private RectTransform rectTransform;
	private CanvasScaler canvasScaler;

	private void Awake() {
		rectTransform = GetComponent<RectTransform>();
		canvasScaler = GetComponentInParent<CanvasScaler>();
	}

	public void UpdateVisual(Vector2 screenPos, Vector2 direction) {
		rectTransform.anchoredPosition = new Vector2(screenPos.x / Screen.width * canvasScaler.referenceResolution.x, screenPos.y / Screen.height * canvasScaler.referenceResolution.y);
		handle.anchoredPosition = new Vector2(direction.x * handleMaxDistance, direction.y * handleMaxDistance);
	}

	public void Show() {
		gameObject.SetActive(true);
	}

	public void Hide() {
		gameObject.SetActive(false);
	}

	public void ChangeBackground(JoystickBackground joystickBackground) {
		background.sprite = backgroundSprites[(int)joystickBackground];
	}

}

namespace ProjectEnums {
	public enum JoystickBackground {
		AllAxis = 0,
		HorizontalAxis,
		VerticalAxis,
	}
}
