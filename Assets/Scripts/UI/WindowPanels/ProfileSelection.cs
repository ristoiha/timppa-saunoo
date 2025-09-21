using ProjectEnums;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileSelection : WindowBase {

	[SerializeField] private Transform m_profileEntryPrefab = default;
	[SerializeField] private Transform m_scrollViewContentParent = default;

	private List<ProfileEntryUI> profileEntries = new List<ProfileEntryUI>();
	public static int selectedProfileIndex = 0;

	//public Toggle onlineToggle;

	//public static bool onlineMode = true;
	public const string onlineModeKey = "OnlineMode";
	public TextMeshProUGUI onlineToggleText;

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		base.Init(parameters, windowStack);
		//onlineMode = PlayerPrefs.GetInt(onlineModeKey, 1) == 1;
		//onlineToggle.isOn = onlineMode;
	}

	public override void UpdateUI() {
		string[] profileList = UserProfile.GetProfileListing();
		profileEntries.Clear();
		for (int i = 0; i < profileList.Length; i++) {
			Transform profileEntryTransform;
			if (m_scrollViewContentParent.childCount - 1 < i) {
				profileEntryTransform = Instantiate(m_profileEntryPrefab, m_scrollViewContentParent);
			}
			else {
				profileEntryTransform = m_scrollViewContentParent.GetChild(i);
			}
			ProfileEntryUI profileEntry = profileEntryTransform.GetComponent<ProfileEntryUI>();
			profileEntry.nameString = UserProfile.Load(profileList[i]).name;
			profileEntries.Add(profileEntry);
		}
		for (int i = m_scrollViewContentParent.childCount - 1; i >= profileList.Length; i--) {
			Destroy(m_scrollViewContentParent.GetChild(i).gameObject);
		}
		
		onlineToggleText.text = LocalizationManager.instance.GetString(LocID.Online);
		UpdateProfileEntriesUI(profileList);
	}

	private void UpdateProfileEntriesUI(string[] profileList) {
		for (int i = 0; i < profileEntries.Count; i++) {
			bool selected = (i == selectedProfileIndex);
			profileEntries[i].UpdateUI(selected);
		}
	}

	public void SelectProfile(int profileIndex) {
		selectedProfileIndex = profileIndex;
		UpdateUI();

	}

	public void DeleteProfile(int profileIndex) {
		if (UserProfile.GetProfileListing().Length > 1) {
			if (profileIndex == selectedProfileIndex) {
				selectedProfileIndex = 0;
			}
			if (profileIndex < selectedProfileIndex) {
				selectedProfileIndex--;
			}
			string profileName = profileEntries[profileIndex].nameString;
			UserProfile.Delete(profileName);
			UpdateUI();
		}
	}

	protected override void OpeningAnimationFinished() {
	}

	public void OfflineToggle(bool value) {
		// Toggle the onlineMode variable
		//onlineMode = value;
		// Save the updated onlineMode state to PlayerPrefs
		//PlayerPrefs.SetInt(onlineModeKey, onlineMode ? 1 : 0);
		//PlayerPrefs.Save();
		//UpdateUI();
	}

	protected override void Closing() {
		string[] profileList = UserProfile.GetProfileListing();
		UserProfile.CurrentProfile = UserProfile.Load(profileList[selectedProfileIndex]);
		PlayerPrefs.SetString("LAST_PROFILE", profileList[selectedProfileIndex]);
		UpdateProfileEntriesUI(profileList);
		//MainMenu mainMenu = (MainMenu)WindowManager.instance.GetWindow(ProjectEnums.WindowPanel.MainMenu);
		//if (mainMenu != null) {
		//	mainMenu.ProfileSelected();
		//}
		//ServiceManager.instance.onlineMode = onlineMode;
		if (ServiceManager.instance.onlineMode == true) {
			ServiceManager.SignInWhenInitialized(UserProfile.CurrentProfile.userID, UserProfile.CurrentProfile.password);
			WindowManager.instance.OpenWindow(WindowPanel.LobbySelection);
		}
		else {
			if (ChatPanel.instance != null) {
				WindowManager.instance.CloseWindow(WindowPanel.ChatPanel);
			}
		}
		AudioManager.LoadAudioSettings();

	}

	protected override void Destroying() {

	}
}
