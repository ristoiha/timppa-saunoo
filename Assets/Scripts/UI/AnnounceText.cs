using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MEC;

public class AnnounceText : MonoBehaviour {

	public TextMeshProUGUI announceText;

	private CoroutineHandle announceRoutine;

	public void Announce(string text, float duration) {
		announceText.text = text;
        announceRoutine = Timing.RunCoroutine(AnnounceRoutine(duration));
	}

	private IEnumerator<float> AnnounceRoutine(float duration) {
		Color originalColor = announceText.color;
		announceText.color = Color.clear;
		yield return Timing.WaitUntilDone(Timing.RunCoroutine(Tools.ChangeTextColor(announceText, originalColor, 0.15F)));
		yield return Timing.WaitForSeconds(duration);
		yield return Timing.WaitUntilDone(Timing.RunCoroutine(Tools.ChangeTextColor(announceText, Color.clear, 0.15F)));
		AnnounceScreen.instance.RemoveAnnouncementFromList(this);
		announceRoutine = new CoroutineHandle();
		Destroy(gameObject);
	}

	private void OnDestroy() {
		Timing.KillCoroutines(announceRoutine);
	}

}
