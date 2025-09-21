using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit;

public class HoveredInteractable : MonoBehaviour {

	public static ActivateEventArgs activateArgsLeft;
	public static ActivateEventArgs activateArgsRight;

	public void SetHoveredInteractable(HoverEnterEventArgs hoverEnterEvent) {
		ActivateEventArgs activateArgs = new ActivateEventArgs();
		activateArgs.interactableObject = (UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable)hoverEnterEvent.interactableObject;
		activateArgs.interactorObject = (UnityEngine.XR.Interaction.Toolkit.Interactors.IXRActivateInteractor)hoverEnterEvent.interactorObject;
		HandScript handScript = activateArgs.interactorObject.transform.GetComponentInParent<HandScript>();
		if (handScript != null && handScript.handedness == Handedness.Left) {
			activateArgsLeft = activateArgs;
		}
		else {
			activateArgsRight = activateArgs;
		}
	}

	public void ClearHoveredInteractable(HoverExitEventArgs hoverExitEvent) {
		HandScript handScript = hoverExitEvent.interactorObject.transform.GetComponentInParent<HandScript>();
		if (handScript != null && handScript.handedness == Handedness.Left) {
			activateArgsLeft = null;
		}
		else {
			activateArgsRight = null;
		}
	}

}
