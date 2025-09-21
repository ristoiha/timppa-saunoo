using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ProjectEnums;
using System.Collections.Generic;
using System;
using static UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets.DynamicMoveProvider;

public class GesturePanelRightWrist : WindowBase {

	public static GesturePanelRightWrist instance;

	public Transform rotateArrowsParent;
	public Image[] rotateArrows;

	private RotateDirection rotateDirection;

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		if (instance == null) {
			instance = this;
			base.Init(parameters, windowStack);
			Activate(false);
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
		for (int i = 0; i < rotateArrows.Length; i++) {
			rotateArrows[i].color = new Color(1F, 1F, 1F, 0.5F);
		}
		Color activeColor = new Color(0.6F, 1F, 0.6F, 1F);
		//if (rotateDirection == RotateDirection.Left) { rotateArrows[0].color = activeColor; TaskManager.instance.CompleteTask(LocID.Task_RotateLeft); }
		//else if (rotateDirection == RotateDirection.Right) { rotateArrows[1].color = activeColor; TaskManager.instance.CompleteTask(LocID.Task_RotateRight); }
	}

	public void ChangeRotateDirection(RotateDirection direction) {
		bool changed = rotateDirection != direction;
		rotateDirection = direction;
		if (changed == true) {
			UpdateUI();
		}
	}

	private void LateUpdate() {
		float yOffset = 0.05F;
		transform.position = transform.parent.position + Vector3.up * yOffset;
		transform.rotation = Quaternion.LookRotation(transform.position - PlayerCharacter.instance.xrOrigin.Camera.transform.position);
	}

	public void Activate(bool activate) {
		rotateArrowsParent.gameObject.SetActive(activate);
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

namespace ProjectEnums {

	public enum RotateDirection {
		None,
		Left,
		Right,
	}

}
