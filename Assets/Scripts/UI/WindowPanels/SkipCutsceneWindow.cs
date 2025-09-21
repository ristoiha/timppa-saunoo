using ProjectEnums;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipCutsceneWindow : WindowBase {

	private float skipCutsceneTimer = 0f;
	private const float skipCutsceneDuration = 2f;
	public Image loadingBar;

	CutscenePlayer currentCutscenePlayer;
	
	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		base.Init(parameters, windowStack);
		if (parameters is CutscenePlayer) {
			currentCutscenePlayer = (CutscenePlayer)parameters;
		} 
	}

	public void Update() {
		if (Input.anyKey == true) {
			skipCutsceneTimer += Time.deltaTime;
			if (skipCutsceneTimer >= skipCutsceneDuration) {
				if (currentCutscenePlayer != null) {
					currentCutscenePlayer.SkipCutscene();
				} 
				skipCutsceneTimer = 0f;
			}
		}
		else {
			skipCutsceneTimer = 0f;
		}
		UpdateUI();
	}

	public override void UpdateUI() {
		loadingBar.fillAmount = skipCutsceneTimer / skipCutsceneDuration;
	}

	protected override void OpeningAnimationFinished() {

	}

	protected override void Closing() {

	}

	protected override void Destroying() {

	}

}
