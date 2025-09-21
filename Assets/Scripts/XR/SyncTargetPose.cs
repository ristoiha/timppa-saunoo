using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncTargetPose : MonoBehaviour {

	public Transform target;
	public float followSpeed;
	public float rotateSpeed;

	private void Update() {
		if (target != null) {
			target.position = Vector3.MoveTowards(target.position, transform.position, followSpeed * Time.deltaTime);
			target.rotation = Quaternion.RotateTowards(target.rotation, transform.parent.rotation, rotateSpeed * Time.deltaTime);
		}
		else {
			Destroy(transform.root.gameObject);
		}
	}

}
