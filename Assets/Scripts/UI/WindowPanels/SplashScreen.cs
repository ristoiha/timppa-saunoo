using System.Collections;
using UnityEngine;
using ProjectEnums;
using System.Collections.Generic;
using MEC;

public class SplashScreen : WindowBase {

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		base.Init(parameters, windowStack);
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape) == true) {
			Timing.KillCoroutines(openingRoutine);
			AudioManager.StopMusic();
			WindowManager.instance.CloseWindow(WindowPanel.SplashScreen);
		}
	}

	protected override IEnumerator<float> OpeningRoutine() {
        //AudioManager.PlayEffectInMusicSource(AudioID.Effect_SplashIntro);
        AudioManager.PlayAudio(AudioID.Effect_SplashIntro);
        float introEffectDuration = AudioManager.GetAudioLength(AudioID.Effect_SplashIntro);
		panelAnimator.speed = 1F / introEffectDuration;
		panelAnimator.Play("PanelOpen", -1, 0F);
		panelAnimator.Update(0F);
		float normalizedTime = 0F;
		AnimatorStateInfo stateInfo = panelAnimator.GetCurrentAnimatorStateInfo(0);
		while (normalizedTime < 1F && stateInfo.IsName("PanelOpen") == true) {
			yield return Timing.WaitForOneFrame;
			stateInfo = panelAnimator.GetCurrentAnimatorStateInfo(0);
			normalizedTime = stateInfo.normalizedTime;
		}
		WindowManager.instance.CloseWindow(WindowPanel.SplashScreen);
	}

	public override void UpdateUI() {

	}

	protected override void OpeningAnimationFinished() {

	}

	protected override void Closing() {

	}

	protected override void Destroying() {

	}
}
