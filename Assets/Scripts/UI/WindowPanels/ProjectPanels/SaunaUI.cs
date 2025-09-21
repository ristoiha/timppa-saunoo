using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ProjectEnums;
using System.Collections.Generic;

public class SaunaUI : WindowBase {

	public static SaunaUI instance;

	public Image kauha;
	public Image dragIndicator;
	public bool accelerationDetection = true;

	private Vector2 prevPos;
	private Vector2 prevDelta;
	private readonly static int maxAccStackCount = 6;
	private float[] accelerations = new float[maxAccStackCount];
	private int accIndex = 0;
	private bool dragActive = false;
	private Vector2 dragStartPos;

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		if (instance == null) {
			instance = this;
			base.Init(parameters, windowStack);
		}
		else {
			Destroy(gameObject);
		}
	}

	protected override void OnDestroy() {
		base.OnDestroy();
		ClosingCleanup();
	}

	public void PoikaSaunoo(Vector2 mousePoint) {
		if (accelerationDetection == true) {
			Vector2 movementDelta = prevPos - mousePoint;
			kauha.transform.position = mousePoint;
			prevPos = mousePoint;
			float acceleration = prevDelta.y - movementDelta.y;
			prevDelta = movementDelta;
			float prevAvg = AvgAcceleration();
			if (accIndex == maxAccStackCount) {
				accIndex = 0;
			}
			accelerations[accIndex] = movementDelta.y;
			accIndex++;
			float currentAvg = AvgAcceleration();
			if (Mathf.Sign(prevAvg) != Mathf.Sign(currentAvg)) {
				Debug.Log("jarrua saaatana; " + prevAvg + ", " + currentAvg);
			}
		}
		else {
			Vector2 newScale = new Vector2(1F, (mousePoint - dragStartPos).magnitude);
			dragIndicator.transform.localScale = new Vector3(newScale.x, newScale.y, 1F);
			Vector2 newPos = (mousePoint + dragStartPos) / 2F;
			dragIndicator.transform.position = new Vector3(newPos.x, newPos.y, dragIndicator.transform.position.z);
			Debug.Log(dragIndicator.transform.position);
		}
	}

	public void StartDrag(Vector3 mousePosition) {
		if (accelerationDetection == false) {
			dragActive = true;
			dragStartPos = mousePosition;
			dragIndicator.gameObject.SetActive(true);
		}
	}

	public void EndDrag(Vector3 mousePosition) {
		if (accelerationDetection == false) {
			dragActive = false;
			dragIndicator.gameObject.SetActive(false);
		}
	}

	private float AvgAcceleration() {
		float total = 0F;
		for (int i = 0; i < accelerations.Length; i++) {
			total += accelerations[i];
		}
		return total / accelerations.Length;
	}

	public override void UpdateUI() {

	}

	protected override void OpeningAnimationFinished() {

	}

	protected override void Closing() {
		ClosingCleanup();
	}

	protected override void Destroying() {

	}

	private void ClosingCleanup() {
		if (instance == this) {
			instance = null;
		}
	}

}
