using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Linq;
using System.Collections.Generic;
using ProjectEnums;
using System.IO;

public class EditLocIDUtility : EditorWindow {

	private char[] splitLine = new char[] { '\t', ';' };
	private string[] loadedLocalizations;
	private static string locIDstringToEdit;
	private TextField textField;
	private DropdownField languageDropdown;
	private Label currentStringLabel;
	private string originalLocalizationStringLine;
	private int localizationStringLineIndex = -1;
	private string currentStringPrefix = "Current localization string: ";

	public static void Init(LocID locID) {
		locIDstringToEdit = locID.ToString();
		EditLocIDUtility window = EditorWindow.GetWindow<EditLocIDUtility>(true, "Edit LocID: " + locIDstringToEdit.ToString());
		window.ShowUtility();
	}

	void CreateGUI() {
		Label label = new Label("Language to edit:");
		
		List<string> languageStrings = Enum.GetNames(typeof(Language)).ToList();
		Language defaultLanguage = Language.Finnish;
		languageDropdown = new DropdownField(languageStrings, (int)defaultLanguage);
		languageDropdown.RegisterValueChangedCallback((evt) => {
			LoadLocalizationStrings();
			currentStringLabel.text = currentStringPrefix + loadedLocalizations[languageDropdown.index + 1];
			textField.value = loadedLocalizations[languageDropdown.index + 1];
			currentStringLabel.MarkDirtyRepaint();
			textField.MarkDirtyRepaint();
		});
		//languageDropdown.RegisterValueChangedCallback(evt => Debug.Log(evt.newValue));


		LoadLocalizationStrings();

		currentStringLabel = new Label(currentStringPrefix + loadedLocalizations[languageDropdown.index + 1]);
		textField = new TextField();
		textField.value = loadedLocalizations[languageDropdown.index + 1];

		rootVisualElement.Add(label);
		rootVisualElement.Add(languageDropdown);
		rootVisualElement.Add(currentStringLabel);
		rootVisualElement.Add(textField);

		Button buttonSave = new Button();
		buttonSave.style.backgroundColor = new Color(0.2F, 0.4F, 0.2F, 1F);
		buttonSave.text = "Replace current localization string";
		buttonSave.clicked += () => ReplaceLocalization();
		rootVisualElement.Add(buttonSave);
		
		Button buttonCancel = new Button();
		buttonCancel.text = "Cancel";
		buttonCancel.clicked += () => UndoEdits();
		rootVisualElement.Add(buttonCancel);
	}

	private void LoadLocalizationStrings() {
		string[] lines = File.ReadAllLines(Application.dataPath + "/Resources/LocalizationData.csv");
		for (int i = 0; i < lines.Length; i++) {
			//Skip empty lines
			if (string.IsNullOrWhiteSpace(lines[i])) {
				continue;
			}

			string[] localizations = lines[i].Split(splitLine, StringSplitOptions.None);

			string key = localizations[0].Trim(' '); // First column is key, remove whitespaces
			if (string.IsNullOrEmpty(key)) {
				continue;
			}

			if (key == locIDstringToEdit) {
				loadedLocalizations = localizations;
				if (localizationStringLineIndex == -1) {
					originalLocalizationStringLine = lines[i];
				}
				localizationStringLineIndex = i;
			}
		}

	}

	void ReplaceLocalization() {
		string[] lines = File.ReadAllLines(Application.dataPath + "/Resources/LocalizationData.csv");
		string previousLine = lines[localizationStringLineIndex];
		string[] localizations = lines[localizationStringLineIndex].Split(splitLine, StringSplitOptions.None);
		localizations[languageDropdown.index + 1] = textField.text;
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		for (int i = 0; i < localizations.Length; i++) {
			sb.Append(localizations[i]);
			if (i < localizations.Length - 1) {
				sb.Append('\t');
			}
		}
		lines[localizationStringLineIndex] = sb.ToString();
		loadedLocalizations = localizations;
		currentStringLabel.text = currentStringPrefix + loadedLocalizations[languageDropdown.index + 1];
		currentStringLabel.MarkDirtyRepaint();
		File.WriteAllLines(Application.dataPath + "/Resources/LocalizationData.csv", lines);
		Debug.Log("Changed localization line from: \"" + previousLine + "\" to \"" + lines[localizationStringLineIndex] + "\"");
	}

	void UndoEdits() {
		string[] lines = File.ReadAllLines(Application.dataPath + "/Resources/LocalizationData.csv");
		string previousLine = lines[localizationStringLineIndex];
		lines[localizationStringLineIndex] = originalLocalizationStringLine;
		File.WriteAllLines(Application.dataPath + "/Resources/LocalizationData.csv", lines);
		string[] localizations = lines[localizationStringLineIndex].Split(splitLine, StringSplitOptions.None);
		loadedLocalizations = localizations;
		currentStringLabel.text = currentStringPrefix + loadedLocalizations[languageDropdown.index + 1];
		currentStringLabel.MarkDirtyRepaint();
		textField.value = loadedLocalizations[languageDropdown.index + 1];
		textField.MarkDirtyRepaint();
		Debug.Log("Changed localization line from: \"" + previousLine + "\" to \"" + lines[localizationStringLineIndex] + "\"");
	}
}