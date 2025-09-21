using ProjectEnums;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Lobbies.Models;
using MEC;

public class LobbySelection : WindowBase {

	public static LobbySelection instance;
	public static int selectedLobbyIndex = -1;

	public Transform lobbyEntryPrefab;
	public Transform scrollViewContentParent;

	private List<LobbyEntryUI> lobbyEntries = new List<LobbyEntryUI>();
	private static readonly float lobbyRefreshCooldownTime = 10F;
	private static readonly float manualLobbyRefreshCooldownTime = 5F;
	private float refreshCooldownTimer = 5F;
	private float manualLobbyRefreshCooldownTimer = 5F;
	private CoroutineHandle lobbyRefreshRoutine;

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		base.Init(parameters, windowStack);
		if (instance == null) {
			instance = this;
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
		refreshCooldownTimer -= Time.unscaledDeltaTime;
		manualLobbyRefreshCooldownTimer -= Time.unscaledDeltaTime;
		if (refreshCooldownTimer < 0F && lobbyRefreshRoutine.IsValid == false) {
			lobbyRefreshRoutine = Timing.RunCoroutine(UpdateLobbyList());
		}
	}

	public override void UpdateUI() {
		
	}

	public void ManualLobbyRefresh(bool forceUpdate = false) {
		if ((manualLobbyRefreshCooldownTimer < 0F && lobbyRefreshRoutine.IsValid == false) || forceUpdate == true) {
			lobbyRefreshRoutine = Timing.RunCoroutine(UpdateLobbyList());
		}
	}

	public void SelectLobby(int lobbyIndex) {
		selectedLobbyIndex = lobbyIndex;
		ServiceManager.instance.selectedLobby = ServiceManager.lobbyList[selectedLobbyIndex];
		UpdateLobbyEntriesUI();
	}

	public void SelectLobby(Lobby lobby) {
		selectedLobbyIndex = -1;
		for (int i = 0; i < ServiceManager.lobbyList.Count; i++) {
			if (ServiceManager.lobbyList[i].Id == lobby.Id) {
				selectedLobbyIndex = i;
				break;
			}
		}
		UpdateLobbyEntriesUI();
	}

	public void TryJoinToLobby() {
		if (LobbyIndexValid(selectedLobbyIndex) == true) {
			if (ServiceManager.instance.selectedLobby.HasPassword == true) {
				WindowManager.instance.OpenWindow(WindowPanel.CheckPasswordWindow);
			}
			else {
				ServiceManager.instance.JoinToLobby(ServiceManager.instance.selectedLobby, "");
			}
		}
		else {
			GenericWindow window = new GenericWindow(WindowStyle.NotificationSmall, LocID.LobbySelectionRequired);
			window.AddButton(LocID.Ok, () => WindowManager.instance.CloseWindow(WindowPanel.GenericMessagePanel));
			WindowManager.instance.OpenWindow(WindowPanel.GenericMessagePanel, window);
		}
	}

	public bool LobbyIndexValid(int lobbyIndex) {
		return (lobbyIndex > -1 && lobbyIndex < lobbyEntries.Count);
	}

	private IEnumerator<float> UpdateLobbyList() {
		refreshCooldownTimer = lobbyRefreshCooldownTime;
		manualLobbyRefreshCooldownTimer = manualLobbyRefreshCooldownTime;
		ServiceManager.instance.FetchLobbiesAsync();
		while (ServiceManager.instance.lobbiesFetched == false) {
			yield return Timing.WaitForOneFrame;
		}

		lobbyEntries.Clear();
		for (int i = 0; i < ServiceManager.lobbyList.Count; i++) {
			Transform lobbyEntryTransform;
			if (scrollViewContentParent.childCount - 1 < i) {
				lobbyEntryTransform = Instantiate(lobbyEntryPrefab, scrollViewContentParent);
			}
			else {
				lobbyEntryTransform = scrollViewContentParent.GetChild(i);
			}
			LobbyEntryUI lobbyEntry = lobbyEntryTransform.GetComponent<LobbyEntryUI>();
			lobbyEntry.nameString = ServiceManager.lobbyList[i].Name;
			lobbyEntries.Add(lobbyEntry);
		}
		for (int i = scrollViewContentParent.childCount - 1; i >= ServiceManager.lobbyList.Count; i--) {
			Destroy(scrollViewContentParent.GetChild(i).gameObject);
		}
		if (ServiceManager.instance.selectedLobby != null) {
			SelectLobby(ServiceManager.instance.selectedLobby);
		}
		UpdateLobbyEntriesUI();
		lobbyRefreshRoutine = new CoroutineHandle();
	}

	private void UpdateLobbyEntriesUI() {
		for (int i = 0; i < lobbyEntries.Count; i++) {
			bool selected = (i == selectedLobbyIndex);
			lobbyEntries[i].UpdateUI(selected);
		}
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
