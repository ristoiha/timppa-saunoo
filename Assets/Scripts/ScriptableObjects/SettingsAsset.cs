using ProjectEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SettingsAsset", menuName = "ScriptableObjects/SettingsAsset", order = 1)]
public class SettingsAsset : ScriptableObject {
	
	[Header("Colors")]
	public Color chatSystemColor;
	public Color chatOwnColor;
	public Color chatOtherColor;
	public Color taskTextSuccessColor;
	public Color taskTextNormalColor;
	public Color keyHighlightColor;
	public Color combineGameHighlightColor;
	public Color okColor;
	public Color errorTextColor;
	public Color selectedColor;
	public Color unselectedColor;
	public Color endScreenCorrectText;
	public Color endScreenNotCorrectText;

	[Header("Sprites")]
	public Sprite selectedToggleSprite;
	public Sprite unselectedToggleSprite;
	public Sprite playButtonSprite;
	public Sprite pauseButtonSprite;

}
