using ProjectEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.SearchableEnum;

[System.Serializable]
[CreateAssetMenu(fileName = "TutorialPanelSprite", menuName = "ScriptableObjects/TutorialPanelSprite", order = 1)]
public class TutorialPanelSpriteAsset : ScriptableObject {
	[SearchableEnum] public LocID taskID;
	public Sprite[] sprites;
}
