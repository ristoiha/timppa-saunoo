using UnityEngine;
using ProjectEnums;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine.XR;
using Unity.XR.CoreUtils;

public class MainMenuInitializer : MonoBehaviour {

	static readonly List<XRInputSubsystem> s_InputSubsystems = new List<XRInputSubsystem>();
	[SerializeField] protected WindowPanel[] m_windowPanelsAtStart = default;
	public ContentPanelAsset introContentPanel;
	private Coroutine ambientFadeCoroutine;

	void Start() {
		Timing.RunCoroutine(StartUpRoutine());
	}

	private IEnumerator<float> StartUpRoutine() {
		AudioManager.PlayAudio(AudioID.Music_MainMenu);
#if VR // Always takes two frames to update camera position. TODO: Why? Manually update position if possible and remove waits.
		yield return Timing.WaitForOneFrame;
		yield return Timing.WaitForOneFrame;
#endif
		for (int i = 0; i < m_windowPanelsAtStart.Length; i++) {
			WindowManager.instance.OpenWindow(m_windowPanelsAtStart[i]);
		}
		yield return Timing.WaitForOneFrame;
	}

}
