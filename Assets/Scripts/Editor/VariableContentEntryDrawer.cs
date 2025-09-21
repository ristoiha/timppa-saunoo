using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(VariableContentEntry))]
public class VariableContentEntryDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);
		SerializedProperty variableContent = property.FindPropertyRelative("content");
		SerializedProperty overrideLocID = property.FindPropertyRelative("overrideLocID");

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
		float labelGapSize = 5F;

		Rect pos1 = new Rect(position.x, position.y, widthSize, position.height);
		Rect pos2 = new Rect(position.x + widthSize, position.y, widthSize + labelSizeModifier, position.height);
		Rect pos3 = new Rect(position.x + widthSize * 2F + labelSizeModifier + labelGapSize, position.y, widthSize - labelSizeModifier - labelGapSize, position.height);

		GUIStyle style = new GUIStyle(EditorStyles.label);
		style.alignment = TextAnchor.MiddleRight;

		EditorGUI.PropertyField(pos1, variableContent, GUIContent.none);
		EditorGUI.LabelField(pos2, "Override LocID:", style);
		EditorGUI.PropertyField(pos3, overrideLocID, GUIContent.none);

		EditorGUI.EndProperty();
	}
}
