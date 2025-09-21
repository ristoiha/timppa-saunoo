using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : WindowBase {

	public static LobbyUI instance;

	public TextMeshProUGUI playerCountValueText;

	//private static readonly float fetchInterval = 30F;
	//private float fetchIntervalTimer = fetchInterval;

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		if (instance == null) {
			instance = this;
			base.Init(parameters, windowStack);
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

	//void Update() {
	//	fetchIntervalTimer -= Time.unscaledDeltaTime;
	//	if (fetchIntervalTimer < 0F) {
	//		fetchIntervalTimer = fetchInterval;
	//		FetchPlayers();
	//	}
	//}

	//public void FetchPlayers() {
	//	ServiceManager.instance.FetchLobbyPlayersAsync();
	//}

	public override void UpdateUI() {
		playerCountValueText.text = ServiceManager.instance.GetCurrentLobbyPlayerCount().ToString("0");
	}

	public void ToggleLobbyInfoUI(bool enable) {
		if (enable == true) {
			panelAnimator.Play("PanelOpen", -1, 0F);
		}
		else {
			panelAnimator.Play("PanelClose", -1, 0F);
		}
	}

	public void ToggleChatAllowed(bool allowed) {
		ServiceManager.instance.UpdateChatAllowed(allowed);
	}

	public void PlayersFetched() {
		UpdateUI();
	}

	protected override void OpeningAnimationFinished() {

	}

	protected override void Closing() {
		if (instance == this) {
			instance = null;
		}
	}

	protected override void Destroying() {

	}

}
