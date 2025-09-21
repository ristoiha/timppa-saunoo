using ProjectEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RoboRyanTron.SearchableEnum;

[RequireComponent(typeof(ActivateGOWhenTaskActive))]
public class GazeTarget : MonoBehaviour {

	public Image activeImage;
	public Image progressImage;
	public float maxDistance = 1.1F;

	private float minAngle = 12F;
	private float gazeTime = 1.3F;
	private float gazeTimer = 0F;
	private float previousGazeTimer = 0F;

	private void LateUpdate() {
		Vector3 cameraForward = PlayerCharacter.instance.xrOrigin.Camera.transform.forward;
		Vector3 direction = transform.position - PlayerCharacter.instance.xrOrigin.Camera.transform.position;
		transform.rotation = Quaternion.LookRotation(-direction);
		if (Vector3.Angle(cameraForward, direction) < minAngle && direction.magnitude < maxDistance) {
			activeImage.gameObject.SetActive(false);
			previousGazeTimer = gazeTimer;
			gazeTimer += Time.deltaTime;
			progressImage.fillAmount = Mathf.Clamp01(gazeTimer / gazeTime);
			if (gazeTimer > gazeTime && previousGazeTimer <= gazeTime) {
				GazeComplete();
			}
		}
		else {
			activeImage.gameObject.SetActive(true);
			gazeTimer = 0F;
			progressImage.fillAmount = 0F;
		}
	}

	private void GazeComplete() {
		LocIDVariable.SetSceneValue(LocID.TaskComplete, GetComponent<ActivateGOWhenTaskActive>().relatedTask);
	}

}
