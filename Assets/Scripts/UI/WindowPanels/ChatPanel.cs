using System.Collections;
using UnityEngine;
using ProjectEnums;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using MEC;

public class ChatPanel : WindowBase {

	public static ChatPanel instance;
	public TMP_InputField inputField;
	public TextMeshProUGUI chatText;
	public Image sendButtonImage;

	private CoroutineHandle fadeOutRoutine;
	private static readonly bool systemMessagesEnabled = true;

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		if (instance == null) {
			instance = this;
			base.Init(parameters, windowStack);
			chatText.text = "";
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

	public void SelectInputField() {
		Timing.KillCoroutines(fadeOutRoutine);
		canvasGroup.alpha = 1F;
		inputField.gameObject.SetActive(true);
		ActivateInputField(true);
	}

	public void ActivateInputField(bool activate) {
		if (activate == true) {
			inputField.ActivateInputField();
			inputField.gameObject.SetActive(true);
			sendButtonImage.raycastTarget = true;
		}
		else {
			inputField.DeactivateInputField();
			inputField.gameObject.SetActive(false);
			sendButtonImage.raycastTarget = false;
		}
	}

	public void FadeOutChatWindow() {
		if (InputFieldActive() == false) {
			Timing.KillCoroutines(fadeOutRoutine);
			fadeOutRoutine = Timing.RunCoroutine(FadeOutRoutine());
		}
	}

	public override void UpdateUI() {

	}

	public void SendMessageLocal(string msg) {
		//AppendToChat(UserProfile.CurrentProfile.name, msg);
		ServiceManager.instance.SendNetworkChatMessage(msg);
		ClearInputFieldText();
		ActivateInputField(true);
	}

	public void AppendToChat(string user, string msg) {
		string color = ColorUtility.ToHtmlStringRGB(GameManager.instance.settings.chatOwnColor);
		if (user != UserProfile.CurrentProfile.name) {
			color = ColorUtility.ToHtmlStringRGB(GameManager.instance.settings.chatOtherColor);
		}
		chatText.text = chatText.text + "\n" + "<color=#" + color + ">" + user + "</color>" + ": <noparse>" + msg + "</noparse>";
		canvasGroup.alpha = 1F;
		FadeOutChatWindow();
	}

	public static void SystemMessageToChat(string msg) {
		if (ChatPanel.systemMessagesEnabled == true) {
			ChatPanel chatPanel = (ChatPanel)WindowManager.instance.OpenWindow(WindowPanel.ChatPanel);
			string color = ColorUtility.ToHtmlStringRGB(GameManager.instance.settings.chatSystemColor);
			chatPanel.chatText.text += "\n" + "<color=#" + color + ">[" + msg + "]</color>";
			chatPanel.canvasGroup.alpha = 1F;
			chatPanel.FadeOutChatWindow();
		}
	}

	public static bool InputFieldActive() {
		ChatPanel chatPanel = (ChatPanel)WindowManager.instance.GetWindow(WindowPanel.ChatPanel);
		if (chatPanel != null) {
			return chatPanel.inputField.IsActive();
		}
		else {
			return false;
		}
	}

	public void ClearInputFieldText() {
		inputField.text = "";
	}

	private IEnumerator<float> FadeOutRoutine() {
		inputField.gameObject.SetActive(false);
		float chatStayTimeAfterWrite = 5F;
		float fadeDuration = 1F;
		yield return Timing.WaitForSeconds(chatStayTimeAfterWrite / Time.deltaTime);
		for (float i = 0F; i < 1F; i += Time.unscaledDeltaTime / fadeDuration) {
			canvasGroup.alpha = 1F - i;
			yield return Timing.WaitForOneFrame;
		}
		canvasGroup.alpha = 0F;
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
