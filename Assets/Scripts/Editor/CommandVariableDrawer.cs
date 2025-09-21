using ProjectEnums;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(CommandVariable))]
public class CommandVariableDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);
		SerializedProperty buttonActionType = property.FindPropertyRelative("buttonActionType");
		SerializedProperty command = property.FindPropertyRelative("command");
		SerializedProperty customCommand = property.FindPropertyRelative("customCommand");
		SerializedProperty contentPanelAsset = property.FindPropertyRelative("contentPanelAsset");
		SerializedProperty scriptableObjectValue = property.FindPropertyRelative("scriptableObjectValue");
		SerializedProperty variableType = property.FindPropertyRelative("variableType");
		SerializedProperty windowPanelAction = property.FindPropertyRelative("windowPanelAction");
		SerializedProperty contentPanelAction = property.FindPropertyRelative("contentPanelAction");
		SerializedProperty variableLocID = property.FindPropertyRelative("variable");
		SerializedProperty floatValue = property.FindPropertyRelative("floatValue");
		SerializedProperty intValue = property.FindPropertyRelative("intValue");
		SerializedProperty boolValue = property.FindPropertyRelative("boolValue");
		SerializedProperty stringValue = property.FindPropertyRelative("stringValue");
		SerializedProperty locIDValue = property.FindPropertyRelative("locIDValue");
		SerializedProperty audioIDValue = property.FindPropertyRelative("audioIDValue");
		SerializedProperty sceneIDValue = property.FindPropertyRelative("sceneIDValue");
		SerializedProperty windowPanelValue = property.FindPropertyRelative("windowPanelValue");

		int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		float widthSize = position.width / 4F;

		Rect pos1 = new Rect(position.x, position.y, widthSize - 5F, position.height);
		Rect pos2 = new Rect(position.x + widthSize, position.y, widthSize - 5F, position.height);
		Rect pos3 = new Rect(position.x + widthSize * 2F, position.y, widthSize - 5F, position.height);
		Rect pos4 = new Rect(position.x + widthSize * 3F, position.y, widthSize, position.height);

		//GUIStyle style = new GUIStyle(EditorStyles.label);
		//style.alignment = TextAnchor.MiddleRight;

		EditorGUI.PropertyField(pos1, buttonActionType, GUIContent.none);

		if (buttonActionType.enumValueIndex == 0) { // Command
			EditorGUI.PropertyField(pos2, command, GUIContent.none);
			if (command.enumValueIndex == 5) { // Command / ShowNotification
				EditorGUI.PropertyField(pos3, locIDValue, GUIContent.none);
			}
			else if (command.enumValueIndex == 6) { // Command / LoadScene
				EditorGUI.PropertyField(pos3, sceneIDValue, GUIContent.none);
			}
		}
		else if (buttonActionType.enumValueIndex == 1) { // WindowPanel
			EditorGUI.PropertyField(pos2, windowPanelAction, GUIContent.none);
			EditorGUI.PropertyField(pos3, windowPanelValue, GUIContent.none);
		}
		else if (buttonActionType.enumValueIndex == 2) { // ContentPanel
			EditorGUI.PropertyField(pos2, contentPanelAction, GUIContent.none);
			if (contentPanelAction.enumValueIndex != 3) { // Not ContentPanelAction / SwitchToPreviousWindow
				EditorGUI.PropertyField(pos3, contentPanelAsset, GUIContent.none);
			}
		}
		else if (buttonActionType.enumValueIndex > 2 && buttonActionType.enumValueIndex < 6) { // SetSceneVariable | SetRuntimeVariable | SetVariable
			EditorGUI.PropertyField(pos2, variableLocID, GUIContent.none);
			EditorGUI.PropertyField(pos3, variableType, GUIContent.none);
			if (variableType.enumValueIndex == 0) { // Float
				EditorGUI.PropertyField(pos4, floatValue, GUIContent.none);
			}
			else if (variableType.enumValueIndex == 1) { // Int
				EditorGUI.PropertyField(pos4, intValue, GUIContent.none);
			}
			else if (variableType.enumValueIndex == 2) { // String
				EditorGUI.PropertyField(pos4, stringValue, GUIContent.none);
			}
			else if (variableType.enumValueIndex == 3) { // Bool
				EditorGUI.PropertyField(pos4, boolValue, GUIContent.none);
			}
			else if (variableType.enumValueIndex == 4) { // LocID
				EditorGUI.PropertyField(pos4, locIDValue, GUIContent.none);
			}
		}
		if (buttonActionType.enumValueIndex == 6) { // Custom command
			EditorGUI.PropertyField(pos2, customCommand, GUIContent.none);
			EditorGUI.PropertyField(pos3, variableType, GUIContent.none);
			if (variableType.enumValueIndex == 0) { // Float
				EditorGUI.PropertyField(pos4, floatValue, GUIContent.none);
			}
			else if (variableType.enumValueIndex == 1) { // Int
				EditorGUI.PropertyField(pos4, intValue, GUIContent.none);
			}
			else if (variableType.enumValueIndex == 2) { // String
				EditorGUI.PropertyField(pos4, stringValue, GUIContent.none);
			}
			else if (variableType.enumValueIndex == 3) { // Bool
				EditorGUI.PropertyField(pos4, boolValue, GUIContent.none);
			}
			else if (variableType.enumValueIndex == 4) { // LocID
				EditorGUI.PropertyField(pos4, locIDValue, GUIContent.none);
			}
			else if (variableType.enumValueIndex == 9) { // ScriptableObject
				EditorGUI.PropertyField(pos4, scriptableObjectValue, GUIContent.none);
			}
		}

		EditorGUI.EndProperty();
	}
}
