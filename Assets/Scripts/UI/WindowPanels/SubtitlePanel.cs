using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ProjectEnums;
using TMPro;
using System.Security.Cryptography;
using System.Collections.Generic;
using MEC;

public class SubtitlePanel : WindowBase {
	public static SubtitlePanel instance;
	public TextMeshProUGUI subtitleText;
	public Image subtitleBackground;

	private TextAudioPairAsset currentTextAudioAsset;
	private CoroutineHandle subtitleRoutine;
	private AudioSource speechSource;
	private bool subtitleVisible = false;
	private int currentTextIndex = 0;

	private void Awake() {
		if (instance == null) {
			instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}
	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		base.Init(parameters, windowStack);
		currentTextAudioAsset = (TextAudioPairAsset)parameters;
		speechSource = AudioManager.instance.GetSpeechAudioSource(currentTextAudioAsset);
		subtitleRoutine = Timing.RunCoroutine(PlaySubtitles());
	}

	public override void ReInit(object parameters) {
		base.ReInit(parameters);
		currentTextAudioAsset = (TextAudioPairAsset)parameters;
		if (subtitleRoutine.IsValid == true) {
			Timing.KillCoroutines(subtitleRoutine);
			subtitleVisible = false;
			currentTextIndex = 0;
		}
		speechSource = AudioManager.instance.GetSpeechAudioSource(currentTextAudioAsset);
		subtitleRoutine = Timing.RunCoroutine(PlaySubtitles());
	}
	public override void UpdateUI() {
		if (currentTextIndex < currentTextAudioAsset.subtitleEntries.Length && 
		speechSource.time > currentTextAudioAsset.subtitleEntries[currentTextIndex].startTime && 
		speechSource.time < currentTextAudioAsset.subtitleEntries[currentTextIndex].endTime) {
			subtitleVisible = true;
			subtitleText.text = LocalizationManager.instance.GetString(currentTextAudioAsset.subtitleEntries[currentTextIndex].locID);
			subtitleBackground.enabled = true;
		}
		else {
			subtitleVisible = false;
			subtitleText.text = "";
			subtitleBackground.enabled = false;
		}
	}

	private IEnumerator<float> PlaySubtitles() {
		AudioManager.instance.FadeOutMusicDuringVoiceOver(true);

		while (true) {
			if (currentTextIndex < currentTextAudioAsset.subtitleEntries.Length) {
				if (speechSource.time > currentTextAudioAsset.subtitleEntries[currentTextIndex].endTime) {
					// Clears text, if the new subtitle startTime is further away than the previous end time, otherwise changes new subtitle text
					currentTextIndex++;
					UpdateUI();
				}
				else if (speechSource.time >= currentTextAudioAsset.subtitleEntries[currentTextIndex].startTime && subtitleVisible == false) {
					// If subtitle is not visible, show current subtitle
					UpdateUI();
				}
			}
			if (speechSource.isPlaying == false ) {
				WindowManager.instance.CloseWindow(WindowPanel.SubtitlePanel);
				AudioManager.instance.FadeOutMusicDuringVoiceOver(false);
			}
			yield return Timing.WaitForOneFrame;
		}
	}

	protected override void OpeningAnimationFinished() {

	}

	public void StopPreviousSubtitles() {
		if (subtitleRoutine.IsValid == true) {
			Timing.KillCoroutines(subtitleRoutine);
			WindowManager.instance.CloseWindow(WindowPanel.SubtitlePanel);
			AudioManager.instance.FadeOutMusicDuringVoiceOver(false);
		}
	}

	protected override void Closing() {
		Timing.KillCoroutines(subtitleRoutine);
	}

	protected override void Destroying() {

	}

}
