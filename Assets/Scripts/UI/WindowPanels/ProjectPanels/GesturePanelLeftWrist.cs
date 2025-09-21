using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ProjectEnums;
using System.Collections.Generic;
using System;

public class GesturePanelLeftWrist : WindowBase {

	public static GesturePanelLeftWrist instance;

	public Transform movementArrowsParent;
	public Image[] movementArrows;

	private MovementDirection movementDirection;

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
		for (int i = 0; i < movementArrows.Length; i++) {
			movementArrows[i].color = new Color(1F, 1F, 1F, 0.5F);
		}
		Color activeColor = new Color(0.6F, 1F, 0.6F, 1F);
		//if (movementDirection.HasFlag(MovementDirection.Forward)) { movementArrows[0].color = activeColor; TaskManager.instance.CompleteTask(LocID.Task_WalkForward); }
		//if (movementDirection.HasFlag(MovementDirection.Back)) { movementArrows[1].color = activeColor; TaskManager.instance.CompleteTask(LocID.Task_WalkBackward); }
		//if (movementDirection.HasFlag(MovementDirection.Left)) { movementArrows[2].color = activeColor; TaskManager.instance.CompleteTask(LocID.Task_WalkLeft); }
		//if (movementDirection.HasFlag(MovementDirection.Right)) { movementArrows[3].color = activeColor; TaskManager.instance.CompleteTask(LocID.Task_WalkRight); }
	}

	public void Activate(bool activate) {
		movementArrowsParent.gameObject.SetActive(activate);
	}

	private void LateUpdate() {
		float yOffset = 0.05F;
		transform.position = transform.parent.position + Vector3.up * yOffset;
		transform.rotation = Quaternion.LookRotation(transform.position - PlayerCharacter.instance.xrOrigin.Camera.transform.position);
	}

	public void ChangeMovementDirection(MovementDirection direction) {
		bool changed = movementDirection != direction;
		movementDirection = direction;
		if (changed == true) {
			UpdateUI();
		}
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

	[Flags]	public enum MovementDirection {
		None = 0,
		Forward = 1 << 0,
		Back = 1 << 1,
		Left = 1 << 2,
		Right = 1 << 3,
	}

}
