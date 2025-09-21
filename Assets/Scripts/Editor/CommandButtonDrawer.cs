using ProjectEnums;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[CustomPropertyDrawer(typeof(CommandButton))]
public class CommandButtonDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(new Rect(position.x, position.y, position.width, position.height * 2), label, property);
		SerializedProperty buttonLocID = property.FindPropertyRelative("buttonLocID");
		SerializedProperty commands = property.FindPropertyRelative("commands");

	//Rect labelPosition = new Rect(position.x, position.y, position.width, position.height);

	//position = EditorGUI.PrefixLabel(
	//	labelPosition,
	//	EditorGUIUtility.GetControlID(FocusType.Passive),
	//	new GUIContent(
	//		variableOption.objectReferenceValue != null ?
	//		(variableOption.objectReferenceValue as VariableOption).name :
	//		"Empty"
	//	)
	//);

	int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		float widthSize = position.width / 3F;
		float labelSizeModifier = -30F;
		// float labelGapSize = 5F;
		float lineHeight = EditorGUIUtility.singleLineHeight + 2;

		Rect pos1 = new Rect(position.x, position.y, widthSize, lineHeight);
		Rect pos2 = new Rect(position.x + widthSize, position.y, widthSize + labelSizeModifier, lineHeight);
		Rect pos3 = new Rect(position.x, position.y + position.y + lineHeight, position.width, lineHeight * 2);
		Rect pos4 = new Rect(position.x + 15, position.y + lineHeight - 1, widthSize, lineHeight);

		GUIStyle style = new GUIStyle(EditorStyles.label);
		style.alignment = TextAnchor.MiddleLeft;

		EditorGUI.LabelField(pos1, "ButtonLocID:", style);
		EditorGUI.PropertyField(pos2, buttonLocID, GUIContent.none);
		EditorGUI.PropertyField(pos3, commands, GUIContent.none);
		EditorGUI.LabelField(pos4, "Commands:", style);

		EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		float totalHeight = EditorGUI.GetPropertyHeight(property, label, true) * 2 + EditorGUIUtility.standardVerticalSpacing;
		var typeProperty = property.FindPropertyRelative("commands");
		if (!typeProperty.isExpanded) {
			totalHeight = EditorGUI.GetPropertyHeight(property, label, true) * 2 + EditorGUIUtility.standardVerticalSpacing + 2;
		}
		else {
			totalHeight = EditorGUI.GetPropertyHeight(property, label, true) * 2 + (2 + Mathf.Clamp(typeProperty.arraySize, 1, Mathf.Infinity)) * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing); // if expanded, one line for the label, one for the size, and one for each element
		}
		return totalHeight;
	}
}
