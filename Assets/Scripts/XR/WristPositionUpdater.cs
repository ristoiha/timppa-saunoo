using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WristPositionUpdater : MonoBehaviour {

	public Transform wristTransform;
	public Transform controllerTransform;

    void Update() {
		if (wristTransform.gameObject.activeInHierarchy == true) {
			transform.position = wristTransform.transform.position;
		}
		else if (controllerTransform.gameObject.activeInHierarchy == true) {
			transform.position = controllerTransform.transform.position;
		}
    }
}
