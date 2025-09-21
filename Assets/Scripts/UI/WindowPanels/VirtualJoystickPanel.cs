using UnityEngine;
using ProjectEnums;
using System.Collections.Generic;

public class VirtualJoystickPanel : WindowBase {

	public VirtualJoystickVisual moveJoystick;
	public VirtualJoystickVisual rotateJoystick;

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		base.Init(parameters, windowStack);
		moveJoystick.Hide();
		rotateJoystick.Hide();
#if UNITY_EDITOR || UNITY_WEBGL
		rotateJoystick.ChangeBackground(JoystickBackground.AllAxis);
#endif
	}

	public override void UpdateUI() {

	}

	protected override void OpeningAnimationFinished() {

	}

	protected override void Closing() {

	}

	protected override void Destroying() {
		
	}

	public void UpdateMoveJoystick(Vector2 startPos, Vector2 direction) {
		moveJoystick.UpdateVisual(startPos, direction);
	}

	public void UpdateRotateJoystick(Vector2 startPos, Vector2 direction) {
		rotateJoystick.UpdateVisual(startPos, direction);
	}

	public void ShowMoveJoystick() {
		moveJoystick.Show();
	}

	public void HideMoveJoystick() {
		moveJoystick.Hide();
	}

	public void ShowRotateJoystick() {
		rotateJoystick.Show();
	}

	public void HideRotateJoystick() {
		rotateJoystick.Hide();
	}

}
