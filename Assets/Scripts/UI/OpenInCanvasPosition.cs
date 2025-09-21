using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectEnums;

public class OpenInCanvasPosition : MonoBehaviour {

	public WindowLocation location;

	void Start() {
		Canvas canvas = GetComponent<Canvas>();
		VRWorldCanvasPosition.canvasPositions[location].CopyCanvasSettings(canvas);
	}

}
