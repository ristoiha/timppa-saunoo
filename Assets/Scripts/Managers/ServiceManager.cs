using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Vivox;
using ProjectEnums;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using System;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.CloudSave.Models.Data.Player;
using Unity.VisualScripting;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Net.Http;
using Photon.Chat;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using System.Linq;
using UnityEngine.UIElements;



public class ServiceManager : MonoBehaviour {

	public static ServiceManager instance;
	public static ChatService selectedChatService = ChatService.Photon;
	public static List<Lobby> lobbyList;
	public Lobby selectedLobby;
	public List<Player> lobbyPlayers = new List<Player>();

	[System.NonSerialized]
	public bool lobbiesFetched;
	[System.NonSerialized]
	public bool onlineMode = false;
	[System.NonSerialized]
	public bool chatAllowed = true;

	private HashSet<VivoxParticipant> vivoxParticipants = new HashSet<VivoxParticipant>();
	private static bool isInitialized = false;
	private bool eventsBinded = false;
	private ChatClient photonClient = null;
	private Lobby currentLobby;
	private bool isLobbyHost = false;
	private static readonly float lobbyHeartbeatIntervalTime = 15F;
	private float lobbyHeartbeatIntervalTimer = lobbyHeartbeatIntervalTime;

	private void Awake() {
		if (instance == null) {
			instance = this;
			//	InitializeAsync();
		}
		else {
			Destroy(gameObject);
		}
	}

	private void OnDestroy() {
		if (instance == this) {
			instance = null;
			if (ServiceManager.selectedChatService == ChatService.Vivox) {
				BindVivoxSessionEvents(false);
			}
		}
	}

	private void Update() {
		if (ServiceManager.selectedChatService == ChatService.Photon) {
			if (photonClient != null) {
				photonClient.Service();
			}
		}
		if (isLobbyHost == true) {
			lobbyHeartbeatIntervalTimer -= Time.unscaledDeltaTime;
			if (lobbyHeartbeatIntervalTimer < 0F) {
				lobbyHeartbeatIntervalTimer = lobbyHeartbeatIntervalTime;
				LobbyService.Instance.SendHeartbeatPingAsync(currentLobby.Id);
			}
		}
	}

	public void OpenChat() {
		if (ServiceManager.instance.onlineMode == true) {
			if (WindowManager.instance.WindowIsOpen(WindowPanel.ChatPanel) == true) {
				if (ChatPanel.instance.inputField.text != "") {
					ChatPanel.instance.SendMessageLocal(ChatPanel.instance.inputField.text);
				}
				else {
					if (ChatPanel.InputFieldActive() == true) {
						ChatPanel.instance.ActivateInputField(false);
						ChatPanel.instance.FadeOutChatWindow();
					}
					else {
						ChatPanel.instance.SelectInputField();
					}
				}
			}
			else {
				WindowManager.instance.OpenWindow(WindowPanel.ChatPanel);
				ChatPanel.instance.SelectInputField();
			}
		}
	}

	public void ClearChat() {
		if (WindowManager.instance.WindowIsOpen(WindowPanel.ChatPanel) == true) {
			ChatPanel.instance.ClearInputFieldText();
		}
	}

	public async void DisconnectFromServices() {
		if (AuthenticationService.Instance != null) {
			AuthenticationService.Instance.SignOut();
			if (ServiceManager.selectedChatService == ChatService.Vivox) {
				await VivoxService.Instance.LeaveAllChannelsAsync();
				await VivoxService.Instance.LogoutAsync();
			}
			else if (ServiceManager.selectedChatService == ChatService.Photon &&
			ServiceManager.instance.photonClient != null) {
			//else if (ServiceManager.selectedChatService == ChatService.Photon &&
			//ServiceManager.instance.photonClient != null &&
			//ServiceManager.instance.photonClient.State == ChatState.Authenticated) {
				ServiceManager.instance.UnsubscribeFromPhotonChannel();
				ServiceManager.instance.photonClient.Disconnect();
			}
			ServiceManager.instance.LeaveLobby(); // Do last, as other disconnect functions need currentlobby.Id
		}
	}

	public static async void SignInWhenInitialized(string userID, string password) {
		if (isInitialized == false) {
			await UnityServices.InitializeAsync();
			isInitialized = true;
		}

		ServiceManager.instance.DisconnectFromServices();

		//if (AuthenticationService.Instance != null) {
		//	AuthenticationService.Instance.SignOut();
		//	if (ServiceManager.selectedChatService == ChatService.Vivox) {
		//		await VivoxService.Instance.LogoutAsync();
		//	}
		//	else if (ServiceManager.selectedChatService == ChatService.Photon &&
		//	ServiceManager.instance.photonClient != null &&
		//	ServiceManager.instance.photonClient.State == ChatState.Authenticated) {
		//		ServiceManager.instance.photonClient.Disconnect();
		//	}
		//}

		await instance.SignInWhenInitializedRoutine(userID, password);
	}



	private async Task SignInWhenInitializedRoutine(string userID, string password) {
		//while (!isInitialized) {
		//	await Task.Delay(10); // Delay to avoid busy waiting
		//}
		await SignInWithUsernamePasswordAsync(userID, password);
		//await WaitSignInToInitializeChat();
	}


	private async Task SignInWithUsernamePasswordAsync(string userID, string password) {
		try {
			await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(userID, password);
			Debug.Log("SignIn is successful.");
		}
		catch (AuthenticationException ex) {
			if (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked) {

			}
			// Compare error code to AuthenticationErrorCodes
			// Notify the player with the proper error message
		}
		catch {
			// Compare error code to CommonErrorCodes
			// Notify the player with the proper error message
			// Exception account doesn't exist
			await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(userID, password);
			Debug.Log("Created new account.");
		}
	}

	//private async Task WaitSignInToInitializeChat() {
	//	//while (!AuthenticationService.Instance.IsSignedIn) {
	//	//	await Task.Delay(10); // Delay to avoid busy waiting
	//	//}
	//	InitializeChat();
	//}

	private async void InitializeChat() {
		if (ServiceManager.selectedChatService == ChatService.Vivox) {
			await VivoxService.Instance.InitializeAsync();
			//Debug.Log("Chat initialized");
			//Debug.Log("Login to vivox");
			await LoginToVivoxAsync();
		}
		else if (ServiceManager.selectedChatService == ChatService.Photon) {
			photonClient = new ChatClient(new PhotonClient());
			// Set your favourite region. "EU", "US", and "ASIA" are currently supported.
			photonClient.ChatRegion = "EU";
			AuthenticationValues authenticationValues = new AuthenticationValues();
			authenticationValues.UserId = UserProfile.CurrentProfile.userID + ":" + UserProfile.CurrentProfile.name;
			photonClient.Connect("622a4689-bdf7-4fce-96f4-3f66308f7e92", "1.0", authenticationValues);
		}
	}

	public async Task LoginToVivoxAsync() {
		LoginOptions options = new LoginOptions();
		options.DisplayName = UserProfile.CurrentProfile.name;
		options.EnableTTS = false;
		options.PlayerId = UserProfile.CurrentProfile.userID;
		await VivoxService.Instance.LoginAsync(options);
		//Debug.Log("Logged in to vivox");
		if (!eventsBinded) {
			BindVivoxSessionEvents(true);
		}
		VivoxJoinToChannel(currentLobby.Id);
	}

	public void ConnectedToPhoton() {
		photonClient.Subscribe(currentLobby.Id, 0, 0);
	}

	public void UnsubscribeFromPhotonChannel() {
		photonClient.Unsubscribe(new string[] { currentLobby.Id });
	}

	private void BindVivoxSessionEvents(bool doBind) {
		//Debug.Log("BindSessionEvents " + doBind);
		if (doBind) {
			eventsBinded = true;
			VivoxService.Instance.ParticipantAddedToChannel += onVivoxParticipantAddedToChannel;
			VivoxService.Instance.ParticipantRemovedFromChannel += onVivoxParticipantRemovedFromChannel;
			VivoxService.Instance.ChannelMessageReceived += VivoxChannelMessageReceived;
		}
		else {
			eventsBinded = false;
			VivoxService.Instance.ParticipantAddedToChannel -= onVivoxParticipantAddedToChannel;
			VivoxService.Instance.ParticipantRemovedFromChannel -= onVivoxParticipantRemovedFromChannel;
			VivoxService.Instance.ChannelMessageReceived -= VivoxChannelMessageReceived;
		}
	}

	private void VivoxChannelMessageReceived(VivoxMessage message) {
		NetworkMessageReceived(message.SenderDisplayName, message.MessageText);
	}

	public void PhotonChannelMessageReceived(string user, string msg) {
		NetworkMessageReceived(user, msg);
	}

	private void NetworkMessageReceived(string user, string msg) {
		if (chatAllowed == true) {
			WindowManager.instance.OpenWindow(WindowPanel.ChatPanel);
			if (ChatPanel.instance != null) {
				ChatPanel.instance.AppendToChat(user, msg);
			}
		}
	}

	public void SendNetworkChatMessage(string msg) {
		if (ServiceManager.selectedChatService == ChatService.Vivox) {
			ServiceManager.instance.VivoxSendChatMessage(msg);
		}
		else if (ServiceManager.selectedChatService == ChatService.Photon) {
			ServiceManager.instance.PhotonSendChatMessage(msg);
		}
	}

	public void VivoxSendChatMessage(string msg) {
		VivoxService.Instance.SendChannelTextMessageAsync(currentLobby.Id, msg);
	}

	public void PhotonSendChatMessage(string msg) {
		photonClient.PublishMessage(currentLobby.Id, msg);
	}

	public void VivoxJoinToChannel(string channel) {
		ChatCapability chatCapability = new ChatCapability();
		ChannelOptions channelOptions = new ChannelOptions();
		VivoxService.Instance.JoinEchoChannelAsync(channel, chatCapability, channelOptions);
	}

	public void VivoxLeaveChannel(string channel) {
		VivoxService.Instance.LeaveChannelAsync(channel);
	}

	private void onVivoxParticipantAddedToChannel(VivoxParticipant participant) {
		Debug.Log("onParticipantAddedToChannel " + participant.DisplayName);
		///RosterItem is a class intended to store the participant object, and reflect events relating to it into the game's UI.
		///It is a sample of one way to use these events, and is detailed just below this snippet.
		//RosterItem newRosterItem = new RosterItem();
		//newRosterItem.SetupRosterItem(participant);
		//rosterList.Add(newRosterItem);
		UserLeftChat(participant.DisplayName);
		vivoxParticipants.Add(participant);
		//participant.PlayerId;
	}

	private void onVivoxParticipantRemovedFromChannel(VivoxParticipant participant) {
		Debug.Log("onParticipantRemovedFromChannel " + participant.DisplayName);
		//RosterItem rosterItemToRemove = rosterList.FirstOrDefault(p => p.Participant.PlayerId == participant.PlayerId);
		//rosterList.Remove(rosterItemToRemove);
		UserJoinedToChat(participant.DisplayName);
		vivoxParticipants.Remove(participant);
	}

	public void UserJoinedToChat(string username) {
		WindowManager.instance.OpenWindow(WindowPanel.ChatPanel);
		if (ChatPanel.instance != null) {
			ChatPanel.instance.AppendToChat(username, "Joined chat");
		}
	}

	public void UserLeftChat(string username) {
		WindowManager.instance.OpenWindow(WindowPanel.ChatPanel);
		if (ChatPanel.instance != null) {
			ChatPanel.instance.AppendToChat(username, "Left chat");
		}
	}

	public async void JoinToLobby(Lobby lobbyToJoin, string password) {
		try {

			Dictionary<string, PlayerDataObject> dataDictionary = new Dictionary<string, PlayerDataObject>() {
				{ NetworkSaveDataName.PlayerName.ToString(), new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, UserProfile.CurrentProfile.name) }
			};
			Player player = new Player(AuthenticationService.Instance.PlayerId, null, dataDictionary);


			JoinLobbyByIdOptions options = new JoinLobbyByIdOptions();
			options.Player = player;
			options.Password = PadPassword(password);

			Lobby lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyToJoin.Id, options);

			LobbyEventCallbacks callbacks = new LobbyEventCallbacks();
			callbacks.LobbyChanged += OnLobbyChanged;
			await LobbyService.Instance.SubscribeToLobbyEventsAsync(lobby.Id, callbacks);

			currentLobby = lobby;
			InitializeChat();
			WindowManager.instance.CloseWindow(WindowPanel.LobbySelection);
		}
		catch (LobbyServiceException e) {
			if (e.Reason == LobbyExceptionReason.IncorrectPassword) {
				WindowManager.instance.OpenWindow(WindowPanel.CheckPasswordWindow, LocID.WrongPassword);
			}
			else if (e.Reason == LobbyExceptionReason.LobbyNotFound) {
				LobbySelection lobbySelectionWindow = (LobbySelection)WindowManager.instance.GetWindow(WindowPanel.LobbySelection);
				if (lobbySelectionWindow != null) {
					lobbySelectionWindow.ManualLobbyRefresh(true);
				}
			}
			Debug.Log(e.Reason.ToString());
		}
	}

	public async void LeaveLobby() {
		if (currentLobby != null) {
			try {
				//Ensure you sign-in before calling Authentication Instance
				//See IAuthenticationService interface
				string playerId = AuthenticationService.Instance.PlayerId;
				await LobbyService.Instance.RemovePlayerAsync(currentLobby.Id, playerId);
				currentLobby = null;
			}
			catch (LobbyServiceException e) {
				Debug.Log(e);
			}
		}
	}

	public async void CreateLobby(string lobbyName, string email, string password) {
		int maxPlayers = 32;

		Dictionary<string, PlayerDataObject> playerData = new Dictionary<string, PlayerDataObject>() {
			{ NetworkSaveDataName.PlayerName.ToString(), new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, UserProfile.CurrentProfile.name) },
		};

		Dictionary<string, DataObject> lobbyData = new Dictionary<string, DataObject>() {
			{ NetworkSaveDataName.LobbyEmail.ToString(), new DataObject(DataObject.VisibilityOptions.Public, email) },
			{ NetworkSaveDataName.ChatAllowed.ToString(), new DataObject(DataObject.VisibilityOptions.Public, true.ToString()) }
		};

		CreateLobbyOptions options = new CreateLobbyOptions();
		options.Player = new Player(AuthenticationService.Instance.PlayerId, null, playerData);
		options.Data = lobbyData;
		options.IsPrivate = false;
		options.Password = PadPassword(password);
		Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

		LobbyEventCallbacks callbacks = new LobbyEventCallbacks();
		callbacks.LobbyChanged += OnLobbyChanged;
		await LobbyService.Instance.SubscribeToLobbyEventsAsync(lobby.Id, callbacks);

		currentLobby = lobby;
		WindowManager.instance.OpenWindow(WindowPanel.LobbyUI);
		InitializeChat();
		isLobbyHost = true;
		Debug.Log("Lobby " + lobby.Name + " created");
	}

	public void UpdateChatAllowed(bool allowed) {
		UpdateLobbyOptions options = new UpdateLobbyOptions();
		options.Data = new Dictionary<string, DataObject>() {
			{ NetworkSaveDataName.ChatAllowed.ToString(), new DataObject(DataObject.VisibilityOptions.Public, allowed.ToString()) }
		};
		chatAllowed = allowed; // UpdateLobbyAsync only updates to remote connections, change local value manually
		LobbyService.Instance.UpdateLobbyAsync(currentLobby.Id, options);
	}

	private void OnLobbyChanged(ILobbyChanges changes) {
		changes.ApplyToLobby(currentLobby);
		GetLobbyChatAllowed();
		if (LobbyUI.instance != null) {
			LobbyUI.instance.UpdateUI();
		}
		//if (changes.Data.Changed == true) {
		//	Dictionary<string, ChangedOrRemovedLobbyValue<DataObject>> changedData = changes.Data.Value;
		//	if (changedData.TryGetValue(NetworkSaveDataName.ChatAllowed.ToString(), out ChangedOrRemovedLobbyValue<DataObject> data) == true) {
		//		if (bool.TryParse(data.Value.Value, out bool result) == true) {
		//			bool chatAllowed = result;

		//		}
		//	}
		//}
		//if (changes.PlayerJoined.Value.Count > 0) {
		//	for (int i = 0; i < changes.PlayerJoined.Value.Count; i++) {
		//		lobbyPlayers.Add(changes.PlayerJoined.Value[i].Player);
		//	}
		//}
	}

	public async void FetchLobbiesAsync() {
		try {
			lobbiesFetched = false;
			QueryLobbiesOptions options = new QueryLobbiesOptions();
			options.Count = 25;

			// Filter for open lobbies only
			options.Filters = new List<QueryFilter>() {
				new QueryFilter(
					field: QueryFilter.FieldOptions.AvailableSlots,
					op: QueryFilter.OpOptions.GT,
					value: "0")
			};

			// Order by newest lobbies first
			options.Order = new List<QueryOrder>() {
				new QueryOrder(
					asc: false,
					field: QueryOrder.FieldOptions.Created)
			};

			QueryResponse lobbies = await LobbyService.Instance.QueryLobbiesAsync(options);
			lobbyList = lobbies.Results;
			lobbiesFetched = true;
		}
		catch (LobbyServiceException e) {
			Debug.Log(e);
		}
	}

	public async void FetchLobbyPlayersAsync() {
		try {
			currentLobby = await LobbyService.Instance.GetLobbyAsync(currentLobby.Id);
			for (int i = 0; i < currentLobby.Players.Count; i++) {
				if (lobbyPlayers.Any(lobbyPlayer => lobbyPlayer.Id == currentLobby.Players[i].Id) == false) {
					lobbyPlayers.Add(currentLobby.Players[i]);
				}
			}
			RemoveDisconnectedPlayersFromPlayerList();
			//if (PlayerWritingFetcher.instance != null) {
			//	PlayerWritingFetcher.instance.PlayersFetched();
			//}
			if (LobbyUI.instance != null) {
				LobbyUI.instance.PlayersFetched();
			}
		}
		catch (LobbyServiceException e) {
			Debug.Log(e);
		}
	}

	public int GetCurrentLobbyPlayerCount() {
		return currentLobby.Players.Count;
	}

	private void RemoveDisconnectedPlayersFromPlayerList() {
		for (int i = lobbyPlayers.Count - 1; i > -1 ; i--) {
			if (currentLobby.Players.Any(lobbyPlayer => lobbyPlayer.Id == lobbyPlayers[i].Id) == false) {
				//if (PlayerWritingFetcher.instance != null) {
				//	PlayerWritingFetcher.instance.RemovePlayerFromSlot(lobbyPlayers[i]);
				//}
				lobbyPlayers.RemoveAt(i);
			}
		}
	}

	public async void SaveData() {
		Dictionary<string, object> playerData = new Dictionary<string, object>{
			{NetworkSaveDataName.StoryText.ToString(), "Data to be saved"},
		};
		await CloudSaveService.Instance.Data.Player.SaveAsync(playerData, new Unity.Services.CloudSave.Models.Data.Player.SaveOptions(new PublicWriteAccessClassOptions()));
		Debug.Log($"Saved data {string.Join(',', playerData)}");
	}

	public async Task<string> LoadPublicDataByPlayerId(string playerId, NetworkSaveDataName key) {
		Dictionary<string, Item> playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { key.ToString() }, new LoadOptions(new PublicReadAccessClassOptions(playerId)));
		if (playerData.TryGetValue(key.ToString(), out Item savedDataItem) == true) {
			//Debug.Log($"keyName: {savedDataItem.Value.GetAs<string>()}");
			return savedDataItem.Value.GetAs<string>();
		}
		return "";
	}

	public string GetLobbyPlayerName(Player player) {
		if (player.Data.TryGetValue(NetworkSaveDataName.PlayerName.ToString(), out PlayerDataObject playerDataObject) == true) {
			string name = playerDataObject.Value.ToString();
			return name;
		}
		return null;
	}

	public string GetLobbyEmail() {
		if (currentLobby != null) {
			if (currentLobby.Data.TryGetValue(NetworkSaveDataName.LobbyEmail.ToString(), out DataObject dataObject) == true) {
				string email = dataObject.Value.ToString();
				if (Tools.CheckEmailAddressValidity(email) == true) {
					return email;
				}
			}
		}
		return "";
	}

	public string GetLobbyChatAllowed() {
		if (currentLobby != null) {
			if (currentLobby.Data.TryGetValue(NetworkSaveDataName.ChatAllowed.ToString(), out DataObject dataObject) == true) {
				if (bool.TryParse(dataObject.Value, out bool result) == true) {
					chatAllowed = result;
				}
			}
		}
		return "";
	}

	public void SendMail(string to, string subject, string body) {
		if (Tools.CheckEmailAddressValidity(to) == true) {
			//string host = "smtp.hostinger.com";
			//string from = "tarinatalo@pelaakeksikirjoita.fi";
			//string subject = "Tarinatalo kirjoitus";
			//string body = UserProfile.CurrentProfile.storyText;
			//MailMessage message = new MailMessage(from, to, subject, body);
			////IPHostEntry hostInfo = Dns.GetHostEntry(host);
			////IPAddress ipAddress = hostInfo.AddressList.FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
			//var client = new SmtpClient(host, 587) {
			////var client = new SmtpClient(ipAddress.ToString(), 587) {
			//	Credentials = new NetworkCredential("tarinatalo@pelaakeksikirjoita.fi", "8pFkMNmrzH5dFfk4MgJf!"),
			//	EnableSsl = true,
			//	UseDefaultCredentials = false,
			//};
			////client.Send(message);
			Application.OpenURL("mailto:" + to + "?subject=" + subject + "&body=" + body);
		}
	}

	public static string PadPassword(string password) {
		return password.PadRight(8, '0');
	}

}
