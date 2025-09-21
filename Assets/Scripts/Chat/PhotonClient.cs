using ExitGames.Client.Photon;
using Photon.Chat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class PhotonClient : IChatClientListener {
	public void DebugReturn(DebugLevel level, string message) {
		Debug.Log(message);
	}

	public void OnChatStateChange(ChatState state) {
		Debug.Log("Photon chat OnChatStateChange");
	}

	public void OnConnected() {
		ServiceManager.instance.ConnectedToPhoton();
	}

	public void OnDisconnected() {
		Debug.Log("Photon chat OnDisconnected");
	}

	public void OnGetMessages(string channelName, string[] senders, object[] messages) {
		for (int i = 0; i < senders.Length; i++) {
			string[] idAndName = ParseUserIDAndName(senders[i]);
			ServiceManager.instance.PhotonChannelMessageReceived(idAndName[1], messages[i].ToString());
		}
	}

	public void OnPrivateMessage(string sender, object message, string channelName) {
		Debug.Log("Photon chat OnPrivateMessage");
	}

	public void OnStatusUpdate(string user, int status, bool gotMessage, object message) {
		Debug.Log("Photon chat OnStatusUpdate");
	}

	public void OnSubscribed(string[] channels, bool[] results) {
		ServiceManager.instance.UserJoinedToChat(UserProfile.CurrentProfile.name);
	}

	public void OnUnsubscribed(string[] channels) {
		ServiceManager.instance.UserLeftChat(UserProfile.CurrentProfile.name);
	}

	public void OnUserSubscribed(string channel, string user) {
		string[] idAndName = ParseUserIDAndName(user);
		ServiceManager.instance.UserJoinedToChat(idAndName[1]);
	}

	public void OnUserUnsubscribed(string channel, string user) {
		string[] idAndName = ParseUserIDAndName(user);
		ServiceManager.instance.UserLeftChat(idAndName[1]);
	}

	public static string[] ParseUserIDAndName(string user) {
		string[] substrings = user.Split(new char[] { ':' }, 2);
		return substrings;
	}
}
