using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using RoboRyanTron.SearchableEnum;
using ProjectEnums;
using MEC;

public class GameInitializer : MonoBehaviour {

	public static GameInitializer instance;

	public ARRaycastManager raycastManager;
	[SerializeField] protected WindowPanel[] m_windowPanelsAtStart = default;

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

	void Start() {
		Timing.RunCoroutine(StartUpRoutine());
	}

	private IEnumerator<float> StartUpRoutine() {
		for (int i = 0; i < m_windowPanelsAtStart.Length; i++) {
			WindowManager.instance.OpenWindow(m_windowPanelsAtStart[i]);
		}
		yield return Timing.WaitForOneFrame;
	}

	public void Pause() {
	}
	public void Unpause() {
	}

	private void Update() {

	}


}