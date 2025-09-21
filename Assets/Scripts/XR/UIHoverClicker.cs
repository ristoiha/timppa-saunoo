using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class UIHoverClicker : MonoBehaviour {

	private Dictionary<Selectable, HoverData> hoveredSelectables = new Dictionary<Selectable, HoverData>();
	private float hoverClickDelay = 0.3F;

	private bool debug = false;

	private void Update() {
		for (int i = 0; i < hoveredSelectables.Count; i++) {
			KeyValuePair<Selectable, HoverData> hoverData = hoveredSelectables.ElementAt(i);
			float previousHoverTime = hoverData.Value.hoverTime;
			hoverData.Value.hoverTime += Time.deltaTime;
			if (hoverData.Value.image != null) {
				hoverData.Value.image.fillAmount = Mathf.Clamp01(hoverData.Value.hoverTime / hoverClickDelay);
			}
			if (((hoverData.Value.hoverSelectable.singleClickPerHover == true && previousHoverTime < hoverClickDelay) || hoverData.Value.hoverSelectable.singleClickPerHover == false) && hoverData.Value.hoverTime >= hoverClickDelay) {
				TriggerClick(hoverData.Key);
			}
		}
	}

	private void TriggerClick(Selectable selectable) {
		Button button = selectable.GetComponent<Button>();
		Toggle toggle = selectable.GetComponent<Toggle>();
		if (button != null) {
			if (debug == true) Debug.Log(selectable.gameObject.name + " clicked");
			button.onClick?.Invoke();
		}
		if (toggle != null) {
			if (debug == true) Debug.Log(selectable.gameObject.name + " clicked");
			toggle.SetIsOnWithoutNotify(!toggle.isOn);
			toggle.onValueChanged?.Invoke(toggle.isOn);
		}
	}


	public void HoverEntered(UIHoverEventArgs args) {
		if (args.uiObject != null) {
			Selectable selectable = args.uiObject.GetComponent<Selectable>();
			Image image = args.uiObject.GetComponent<Image>();
			XRHoverSelectable hoverSelectable = args.uiObject.GetComponent<XRHoverSelectable>();
			if (selectable != null && hoverSelectable != null && hoverSelectable.isHoverSelectable == true) {
				if (debug == true) Debug.Log("Hover entered: " + args.uiObject.name);
				if (hoveredSelectables.ContainsKey(selectable) == false) {
					HoverData hoverData = new HoverData(0F, image, hoverSelectable);
					hoveredSelectables.Add(selectable, hoverData);
				}
			}
		}
#if UNITY_EDITOR
		else {
			Debug.LogWarning("Warning: args.uiObject == null");
		}
#endif
	}

	public void HoverExited(UIHoverEventArgs args) {
		if (args.uiObject != null) {
			Selectable selectable = args.uiObject.GetComponent<Selectable>();
			XRHoverSelectable hoverSelectable = args.uiObject.GetComponent<XRHoverSelectable>();
			if (selectable != null && hoverSelectable != null && hoverSelectable.isHoverSelectable == true && hoveredSelectables.ContainsKey(selectable) == true) {
				if (debug == true) Debug.Log("Hover exited: " + args.uiObject.name);
				if (hoveredSelectables[selectable].image != null) {
					hoveredSelectables[selectable].image.fillAmount = 0F;
				}
				hoveredSelectables.Remove(selectable);
			}
		}
#if UNITY_EDITOR
		else {
			Debug.LogWarning("Warning: args.uiObject == null");
		}
#endif
	}

}

[System.Serializable]
public class HoverData {
	public float hoverTime;
	public Image image;
	public XRHoverSelectable hoverSelectable;

	public HoverData(float hoverTime, Image image, XRHoverSelectable hoverSelectable) {
		this.hoverTime = hoverTime;
		this.image = image;
		this.hoverSelectable = hoverSelectable;
	}
}