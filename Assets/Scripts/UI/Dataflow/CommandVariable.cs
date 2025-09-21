using ProjectEnums;
using UnityEngine;
using RoboRyanTron.SearchableEnum;

[System.Serializable]
public class CommandVariable {
	[SearchableEnum] public ButtonActionType buttonActionType = ButtonActionType.SetVariable;
	[SearchableEnum] public WindowPanelAction windowPanelAction;
	[SearchableEnum] public ContentPanelAction contentPanelAction;
	[SearchableEnum] public VariableType variableType;
	[SearchableEnum] public Command command;
	[SearchableEnum] public CustomCommand customCommand;
	[SearchableEnum] public LocID variable;
	[SearchableEnum] public LocID locIDValue;
	[SearchableEnum] public AudioID audioIDValue;
	[SearchableEnum] public SceneID sceneIDValue;
	[SearchableEnum] public WindowPanel windowPanelValue;
	public ContentPanelAsset contentPanelAsset;
	public ScriptableObject scriptableObjectValue;
	public float floatValue;
	public int intValue;
	public string stringValue;
	public bool boolValue;
}
