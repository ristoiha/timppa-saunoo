using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class XRRaycasterSelector : MonoBehaviour {

	private void Awake() {
		GraphicRaycaster graphicRaycaster = GetComponent<GraphicRaycaster>();
		TrackedDeviceGraphicRaycaster trackedDeviceGraphicRaycaster = GetComponent<TrackedDeviceGraphicRaycaster>();
		
#if VR
		if (graphicRaycaster != null) {
			graphicRaycaster.enabled = false;
		}
		if (trackedDeviceGraphicRaycaster != null) {
			trackedDeviceGraphicRaycaster.enabled = true;
		}
#else
		if (graphicRaycaster != null) {
			graphicRaycaster.enabled = true;
		}
		if (trackedDeviceGraphicRaycaster != null) {
			trackedDeviceGraphicRaycaster.enabled = false;
		}
#endif
	}

}
