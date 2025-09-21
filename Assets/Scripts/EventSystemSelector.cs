using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class EventSystemSelector : MonoBehaviour {

	public StandaloneInputModule standaloneInputModule;
	public XRUIInputModule xrUIInputModule;

	private void Awake() {
#if VR
		xrUIInputModule.enabled = true;
#else
		standaloneInputModule.enabled = true;
#endif
	}

}
