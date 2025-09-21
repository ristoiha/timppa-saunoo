using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabInteractable : MonoBehaviour {

	public bool movementEnabledWhenGrabbed = true;
	public bool performGravityCheckOnDrop = false;

	private Rigidbody rBody;

	private void Awake() {
		rBody = transform.GetComponent<Rigidbody>();
		if (rBody != null) {
			rBody.Sleep();
		}
	}

	public void GrabOnSelect(SelectEnterEventArgs selectEnterEventArgs) {
		HandScript handScript = selectEnterEventArgs.interactorObject.transform.GetComponentInParent<HandScript>(true);
		if (handScript != null) {
			PlayerCharacter.instance.GrabInteractable((UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable)selectEnterEventArgs.interactableObject, handScript.handedness, movementEnabledWhenGrabbed);
		}
	}

	public void DropOnDeselect(SelectExitEventArgs selectExitEventArgs) {
		HandScript handScript = selectExitEventArgs.interactorObject.transform.GetComponentInParent<HandScript>(true);
		if (handScript != null) {
			PlayerCharacter.instance.DropGrabbed(handScript.handedness);
		}
#if VR
		if (performGravityCheckOnDrop == true && rBody != null) {
			LayerMask layer = Gval.interactableLayer + Gval.wallLayer;
			if (Physics.Raycast(selectExitEventArgs.interactableObject.transform.position, Vector3.down, 0.3F, layer) == false) {
                rBody.useGravity = false;
			}
			else {
                rBody.useGravity = true;
			}
		}
#endif
	}

}