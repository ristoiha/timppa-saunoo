using ProjectEnums;
using RoboRyanTron.SearchableEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ContentPanelAsset", menuName = "ScriptableObjects/ContentPanelAsset", order = 1)]
public class ContentPanelAsset : ScriptableObject {

	[SearchableEnum] public WindowStyle style;
	[SearchableEnum] public LocID header = LocID.None;
	public VariableContentEntry[] contents;
	public CommandButton[] commandButtons;

}

[System.Serializable]
public class VariableContentEntry {
	public VariableContent content;
	[SearchableEnum] public LocID overrideLocID;
}


[System.Serializable]
public class CommandButton {
	[SearchableEnum] public LocID buttonLocID;
	public CommandVariable[] commands;
}
