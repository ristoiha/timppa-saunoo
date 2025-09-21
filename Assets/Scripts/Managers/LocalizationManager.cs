using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using ProjectEnums;

public class LocalizationManager : MonoBehaviour {

	public static LocalizationManager instance;
	public Language selectedLanguage = Language.English;

	[SerializeField] private TextAsset m_translations = default;

	private string[] splitFile = new string[] { "\r\n", "\r", "\n" };
	private char[] splitLine = new char[] { '\t' };

	private Dictionary<string, string[]> m_localizationDictionary = new Dictionary<string, string[]>();

	private void Awake() {
		if (instance == null) {
			instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}
	private void OnDestroy() {
		if (instance == this) {
			instance = null;
		}
	}
	private void Start() {
		LoadLocalizationStrings();
		ChangeLanguage(Language.Finnish);
	}

	private void LoadLocalizationStrings() {
		byte[] bytes = Encoding.Default.GetBytes(m_translations.text);
		string utf8Text = Encoding.UTF8.GetString(bytes);
		string[] lines = utf8Text.Split(splitFile, StringSplitOptions.None);
		for (int i = 0; i < lines.Length; i++) {
			//Skip empty lines
			if (string.IsNullOrWhiteSpace(lines[i])) {
				continue;
			}

			string[] localizations = lines[i].Split(splitLine, StringSplitOptions.None);

			string key = localizations[0].Trim(' '); // First column is key, remove whitespaces
			if (string.IsNullOrEmpty(key))
				continue;
			string[] localizedStrings = new string[localizations.Length - 1];
			int languageAmount = System.Enum.GetValues(typeof(Language)).Length;
			for (int k = 0; k < languageAmount; k++) {
				string localizedString = localizations[k + 1];
				if (string.IsNullOrEmpty(localizedString)) {
					Debug.LogError("Missing localization for key " + key + " in language " + (Language)k);
				}
				else {
					localizedStrings[k] = localizedString;
				}
			}

			if (!m_localizationDictionary.ContainsKey(key)) {
				m_localizationDictionary.Add(key, localizedStrings);
			}
		}

	}
	public void ChangeLanguage(Language language) {
		selectedLanguage = language;
		LocalizedObject.UpdateLocalization();
	}

	private string GetString(string key, GameObject caller = null) {
		if (!string.IsNullOrEmpty(key)) {
			if (m_localizationDictionary.TryGetValue(key, out string[] value)) {
				return value[(int)selectedLanguage];
			}
			else {
				if (caller == null) {
					Debug.LogError($"Localization couldn't find a key {key}");
				}
				else {
					Debug.LogError($"Localization couldn't find a key {key} from caller {caller.name}");
				}
				return "LOC_UNDEFINED";
			}
		}
		else {
			Debug.LogError("Trying to get localization string with null key");
			return "LOC_NULLKEY";
		}

	}

	public string GetString(LocID id, GameObject caller = null) {
		if (id == LocID.None)
			return "";
		return GetString(id.ToString(), caller);
	}
	public List<Language> GetAvailableLanguages() {
		List<Language> languages = new List<Language>();
		foreach (Language language in Enum.GetValues(typeof(Language))) {
			languages.Add(language);
		}
		return languages;
	}

	public LocID GetLanguageLocID(Language language) {
		switch (language) {
			case Language.English:
				return LocID.Locale_eng;
			case Language.Finnish:
				return LocID.Locale_fin;
		}
		return LocID.None;
	}
}