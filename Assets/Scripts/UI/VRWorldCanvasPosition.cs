using ProjectEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRWorldCanvasPosition : MonoBehaviour {

	public static Dictionary<WindowLocation, VRWorldCanvasPosition> canvasPositions = new Dictionary<WindowLocation, VRWorldCanvasPosition>();

	public Canvas canvas;
	public WindowLocation location;
	public bool ignoreXRotation = false;
	public bool addHandle = false;
	public bool parentCanvasToThis = false;

	private void Awake() {
		if (canvasPositions.ContainsKey(location) == false) {
			canvasPositions.Add(location, this);
		}
		else {
			canvasPositions[location] = this;
		}
	}

	private void OnDestroy() {
		if (canvasPositions.ContainsKey(location) == true) {
			canvasPositions.Remove(location);
		}
	}

	public void CopyCanvasSettings(Canvas windowCanvas) {
		WindowBase windowBase = windowCanvas.GetComponent<WindowBase>();
		windowCanvas.renderMode = canvas.renderMode;
		windowCanvas.worldCamera = Camera.main;
		windowCanvas.transform.localScale = transform.localScale;
		windowCanvas.GetComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta;
		windowCanvas.sortingLayerID = canvas.sortingLayerID;
		windowCanvas.transform.position = transform.position;
		windowCanvas.transform.rotation = transform.rotation;
		if (windowBase != null && ignoreXRotation == true && (parentCanvasToThis == false && windowBase.parentToLocationTarget == false)) {
			Vector3 forward = Vector3.Cross(transform.right, Vector3.up);
			windowCanvas.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
			windowCanvas.transform.position = transform.parent.position + forward * transform.localPosition.z;
		}
#if VR
		if (windowBase != null && addHandle == true && windowBase.parentToLocationTarget == false) {
			float canvasHeight = GetComponent<RectTransform>().sizeDelta.y;
			if (windowBase.overrideHandleTarget != null) {
				canvasHeight = windowBase.overrideHandleTarget.sizeDelta.y;
			}
			Vector3 lowerEdgePosition = windowCanvas.transform.position - Vector3.up * canvasHeight / 2F * transform.localScale.y;
			Vector3 handlePosition = lowerEdgePosition - Vector3.up * 0.05F;
			Transform handle = Instantiate(WindowManager.instance.XRWindowHandlePrefab, handlePosition, windowCanvas.transform.rotation);
			handle.GetChild(0).position = windowCanvas.transform.position;
			handle.GetComponentInChildren<SyncTargetPose>().target = windowCanvas.transform;
		}
#endif
		if (windowBase != null && (parentCanvasToThis == true || windowBase.parentToLocationTarget == true)) {
			windowCanvas.transform.SetParent(gameObject.transform);
		}
	}

}
