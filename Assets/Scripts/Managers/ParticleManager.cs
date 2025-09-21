using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectEnums;

public class ParticleManager : MonoBehaviour {

	public static ParticleManager instance;

	public ParticleSystem[] oneShotParticles;

	private void Awake() {
		if (instance == null) {
			instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}

	private void OnDestroy() {
		if (instance == this) {
			instance = null;
		}
	}

	public static void PlayOneShot(ParticleEffect effect, RectTransform rect) {
		Instantiate(ParticleManager.instance.oneShotParticles[(int)effect], rect.transform.position, Quaternion.identity, rect);
	}

	public static void PlayOneShot(ParticleEffect effect, Vector3 pos) {
		Instantiate(ParticleManager.instance.oneShotParticles[(int)effect], pos, Quaternion.identity);
	}
}
