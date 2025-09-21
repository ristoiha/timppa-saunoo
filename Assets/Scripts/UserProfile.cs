using ProjectEnums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.XR.ARSubsystems;

public class UserProfile {
	public static UserProfile CurrentProfile = null;
	public static SaveTarget saveTarget = SaveTarget.PersistentDataPath;
	private static string[] splitFile = new string[] { "\r\n", "\r", "\n" };

	public string name;
	public string userID;
	public string password;

	public Dictionary<LocID, string> stringValues;
	public Dictionary<LocID, float> floatValues;
	public Dictionary<LocID, int> intValues;
	public Dictionary<LocID, bool> boolValues;
	public Dictionary<LocID, LocID> locIDValues;
	public Dictionary<LocID, List<bool>> boolListValues;
	//public SaveData saveValues = new SaveData(); // TODO: Move to SaveData / SaveVariable later for list value support and easier editability

	UserProfile() { } // Default constructor for Newtonsoft.Json

	private UserProfile(string userName) {
		name = userName;
	}

	public static void DeleteSaveData() {
		//CurrentProfile.completedTasks.Clear();
		//CurrentProfile.currentTaskGroupIndex = 0;
		//CurrentProfile.currentTaskIndex = 0;
		SaveCurrent();
	}

	public static UserProfile Create(string name) {
		UserProfile profile = new UserProfile(name);

		Guid g = Guid.NewGuid(); // Create profile ID for each user.
		string guidString = g.ToString().Substring(0, 20); // Limit to 20 characters
		profile.userID = guidString;
		profile.password = "Password_1"; // Store the provided password

		Save(profile);
		AddProfileToListing(profile);
		PlayerPrefs.SetString("LAST_PROFILE", profile.name);
		CurrentProfile = profile;
		//Debug.Log("userId: " + profile.userID + ", password: " + profile.password);
		return profile;
	}

	public static LocID GetValidityLocID(string name) {
		if (name.Contains("<") || name.Contains(">") || name.Contains("|") || name.Contains("!") || name.Contains("?") || name.Contains("*") || name.Contains("\"") || name.Contains("/") || name.Contains("\\")) {
			return LocID.NameHasInvalidCharacters;
		}
		if (name.Length == 0) {
			return LocID.NameTooShort;
		}
		string[] profileNames = UserProfile.GetProfileListing();
		for (int i = 0; i < profileNames.Length; i++) {
			if (name == profileNames[i]) {
				return LocID.NameAlreadyExists;
			}
		}
		return LocID.NameOK;
	}

	public static void GetLatestProfileAtStartup() {
		if (PlayerPrefs.HasKey("LAST_PROFILE") == true) {
			string lastProfile = PlayerPrefs.GetString("LAST_PROFILE");
			string[] profileList = UserProfile.GetProfileListing();
			int selectedProfileIndex = Array.IndexOf(profileList, lastProfile);
			if (selectedProfileIndex > -1) {
				UserProfile.CurrentProfile = UserProfile.Load(profileList[selectedProfileIndex]);
				ProfileSelection.selectedProfileIndex = selectedProfileIndex;
			}
			else {
				// Last Profile moved/deleted/corrupted
				PlayerPrefs.DeleteKey("LAST_PROFILE");
				selectedProfileIndex = Array.IndexOf(profileList, Gval.defaultProfileName);
				if (selectedProfileIndex > -1) {
					// Load previous default profile
					UserProfile.CurrentProfile = UserProfile.Load(profileList[selectedProfileIndex]);
					ProfileSelection.selectedProfileIndex = selectedProfileIndex;
				}
				else {
					// Default profile not found, create default 
					UserProfile defaultProfile = Create(Gval.defaultProfileName);
					Save(defaultProfile);
				}
			}
		}
		else {
			// First startup, create default profile
			UserProfile defaultProfile = Create(Gval.defaultProfileName);
			Save(defaultProfile);
		}
	}

	public static UserProfile Load(string identifier) {
		string json;
		if (saveTarget == SaveTarget.PersistentDataPath) {
			json = File.ReadAllText(PrepareProfilePath(identifier));
		}
		else {
			json = PlayerPrefs.GetString(identifier);
		}
		UserProfile profile = JsonConvert.DeserializeObject<UserProfile>(json);
		UserProfile.SetNewFeatureDefaultValues(ref profile);
		IntVariable.values = profile.intValues;
		LocIDVariable.values = profile.locIDValues;
		StringVariable.values = profile.stringValues;
		FloatVariable.values = profile.floatValues;
		BoolVariable.values = profile.boolValues;
		return profile;
	}

	private static void SetNewFeatureDefaultValues(ref UserProfile profile) {
		
	}

	public static void Save(UserProfile profile) {
		profile.floatValues = FloatVariable.values;
		profile.boolValues = BoolVariable.values;
		profile.locIDValues = LocIDVariable.values;
		profile.stringValues = StringVariable.values;
		profile.intValues = IntVariable.values;
		string json = JsonConvert.SerializeObject(profile);
		if (saveTarget == SaveTarget.PersistentDataPath) {
			File.WriteAllText(PrepareProfilePath(profile.name), json);
		}
		else {
			PlayerPrefs.SetString(profile.name, json);
		}
	}

	public static void Delete(string profileName) {
		if (saveTarget == SaveTarget.PersistentDataPath) {
			File.Delete(PrepareProfilePath(profileName));
		}
		else {
			PlayerPrefs.DeleteKey(profileName);
		}
		RemoveProfileFromListing(profileName);
	}

	public static void SaveCurrent() {
		Save(UserProfile.CurrentProfile);
	}

	public static string[] GetProfileListing() {
		
		if (saveTarget == SaveTarget.PersistentDataPath) {
			string directory = Path.Combine(Application.persistentDataPath, Gval.profileListFolder);
			if (!Directory.Exists(directory)) { return new string[] { }; }
			string path = Path.Combine(directory, Gval.profileListName);
			if (!File.Exists(path)) { return new string[] { }; }
			return File.ReadAllLines(path);
		}
		else {
			string listString = PlayerPrefs.GetString(Gval.profileListName);
			return listString.Split(splitFile, StringSplitOptions.RemoveEmptyEntries);
		}
	}

	private static string PrepareProfilePath(string userIdentifier) {
		string directory = Path.Combine(Application.persistentDataPath, Gval.profileFolder);
		if (!Directory.Exists(directory)) { Directory.CreateDirectory(directory); }
		string path = Path.Combine(directory, userIdentifier);
		return path;
	}

	private static void AddProfileToListing(UserProfile profile) {
		if (saveTarget == SaveTarget.PersistentDataPath) {
			string directory = Path.Combine(Application.persistentDataPath, Gval.profileListFolder);
			if (!Directory.Exists(directory)) { Directory.CreateDirectory(directory); }
			string path = Path.Combine(directory, Gval.profileListName);
			File.AppendAllLines(path, new string[] { profile.name });
		}
		else {
			string listString = PlayerPrefs.GetString(Gval.configurationListName);
			listString += "\n" + profile.name;
			PlayerPrefs.SetString(Gval.configurationListName, listString);
		}
	}

	private static void RemoveProfileFromListing(string profile) {
		if (saveTarget == SaveTarget.PersistentDataPath) {
			string directory = Path.Combine(Application.persistentDataPath, Gval.profileListFolder);
			if (!Directory.Exists(directory)) { Directory.CreateDirectory(directory); }
			string path = Path.Combine(directory, Gval.profileListName);
			List<string> profileNames = File.ReadAllLines(path).ToList();
			profileNames.Remove(profile);
			File.WriteAllLines(path, profileNames.ToArray());
		}
		else {
			string listString = PlayerPrefs.GetString(Gval.configurationListName);
			List<string> configurationList = listString.Split(splitFile, StringSplitOptions.RemoveEmptyEntries).ToList();
			configurationList.Remove(profile);
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for (int i = 0; i < configurationList.Count; i++) {
				sb.AppendLine(configurationList[i]);
			}
			PlayerPrefs.SetString(Gval.configurationListName, sb.ToString());
		}
	}
}
