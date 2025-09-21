using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class VideoControlsPanel : WindowBase {

	public static VideoControlsPanel instance;

	public Image overlayPauseButtonImage;
	public Image controlsPauseButtonImage;
	public Slider seekSlider;
	public Slider loopMinSlider;
	public Slider loopMaxSlider;
	public TextMeshProUGUI clipInfoText;

	//private const float hideControlsTime = 2F;
	//private float hideControlsTimer = hideControlsTime;
	//private float minimumLoopLength = 0.05F;
	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		if (instance == null) {
			instance = this;
			base.Init(parameters, windowStack);
			//loopMinSlider.SetValueWithoutNotify(GameInitializer.instance.loopMinTime);
			//loopMaxSlider.SetValueWithoutNotify(GameInitializer.instance.loopMaxTime);
		}
		else {
			Destroy(gameObject);
		}
	}

	protected override void OnDestroy() {
		base.OnDestroy();
		if (instance == this) {
			instance = null;
		}
	}

	private void Update() {
		//if (GameInitializer.instance.IsPlaying() == true) {
		//	hideControlsTimer -= Time.unscaledDeltaTime;
		//}
		//if (hideControlsTimer < 0F) {
		//	WindowManager.instance.CloseWindow(this.window);
		//}
		//UpdateUI();
	}

	public override void UpdateUI() {
		//AnimatorStateInfo stateInfo = GameInitializer.instance.characterAnimators[0].GetCurrentAnimatorStateInfo(0);
		//AnimatorClipInfo[] clipInfo = GameInitializer.instance.characterAnimators[0].GetCurrentAnimatorClipInfo(0);
		//float normalizedTime = stateInfo.normalizedTime % 1F;
		//seekSlider.SetValueWithoutNotify(normalizedTime);
		//clipInfoText.text = Tools.GetTimeString(Mathf.FloorToInt(clipInfo[0].clip.length * normalizedTime)) + " / " + Tools.GetTimeString(clipInfo[0].clip.length) + " " + "Waltz";
		//if (GameInitializer.instance.IsPlaying() == false) {
		//	overlayPauseButtonImage.sprite = GameManager.instance.settings.playButtonSprite;
		//	controlsPauseButtonImage.sprite = GameManager.instance.settings.playButtonSprite;
		//}
		//else {
		//	overlayPauseButtonImage.sprite = GameManager.instance.settings.pauseButtonSprite;
		//	controlsPauseButtonImage.sprite = GameManager.instance.settings.pauseButtonSprite;
		//}
	}

	public void TogglePause() {
		//ResetHideTimer();
		//if (GameInitializer.instance.IsPlaying() == true) {
		//	for (int i = 0; i < GameInitializer.instance.characterAnimators.Count; i++) {
		//		GameInitializer.instance.characterAnimators[i].speed = 0F;
		//	}
		//}
		//else {
		//	for (int i = 0; i < GameInitializer.instance.characterAnimators.Count; i++) {
		//		// TODO: Change to speed option, not to 1F
		//		GameInitializer.instance.characterAnimators[i].speed = 1F;
		//	}
		//}
	}

	public void ManualSeek(float normalizedTime) {
		ResetHideTimer();
		//GameInitializer.instance.Seek(normalizedTime);
	}

	public void ChangeLoopMin(float normalizedMinTime) {
		//ResetHideTimer();
		//if (GameInitializer.instance.loopMaxTime - minimumLoopLength < normalizedMinTime) {
		//	GameInitializer.instance.loopMaxTime = Mathf.Clamp01(normalizedMinTime + minimumLoopLength);
		//}
		//if (normalizedMinTime + minimumLoopLength > GameInitializer.instance.loopMaxTime) {
		//	normalizedMinTime = GameInitializer.instance.loopMaxTime - minimumLoopLength;
		//}
		//GameInitializer.instance.loopMinTime = normalizedMinTime;
		//loopMinSlider.SetValueWithoutNotify(GameInitializer.instance.loopMinTime);
		//loopMaxSlider.SetValueWithoutNotify(GameInitializer.instance.loopMaxTime);
	}

	public void ChangeLoopMax(float normalizedMaxTime) {
		//ResetHideTimer();
		//if (GameInitializer.instance.loopMinTime + minimumLoopLength > normalizedMaxTime) {
		//	GameInitializer.instance.loopMinTime = Mathf.Clamp01(normalizedMaxTime - minimumLoopLength);
		//}
		//if (normalizedMaxTime - minimumLoopLength < GameInitializer.instance.loopMinTime) {
		//	normalizedMaxTime = GameInitializer.instance.loopMinTime + minimumLoopLength;
		//}
		//GameInitializer.instance.loopMaxTime = normalizedMaxTime;
		//loopMinSlider.SetValueWithoutNotify(GameInitializer.instance.loopMinTime);
		//loopMaxSlider.SetValueWithoutNotify(GameInitializer.instance.loopMaxTime);
	}

	public void ResetHideTimer() {
		//hideControlsTimer = hideControlsTime;
	}

	protected override void OpeningAnimationFinished() {

	}

	protected override void Closing() {
		overlayPauseButtonImage.enabled = false;
		if (instance == this) {
			instance = null;
		}
	}

	protected override void Destroying() {

	}

}
