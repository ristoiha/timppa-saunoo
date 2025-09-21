using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectEnums;

[System.Serializable]
public class SaveData {

	public static SaveVariable<int> intValues = new SaveVariable<int>();
	public static SaveVariable<bool> boolValues = new SaveVariable<bool>();
	public static SaveVariable<LocID> locIDValues = new SaveVariable<LocID>();
	public static SaveVariable<float> floatValues = new SaveVariable<float>();
	public static SaveVariable<string> stringValues = new SaveVariable<string>();

	public static int GetInt(LocID variable, VariableTarget target) {
		return intValues.GetValue(variable, target);
	}

	public static List<int> GetInts(LocID variable, VariableTarget target) {
		return intValues.GetValues(variable, target);
	}

	public static bool GetBool(LocID variable, VariableTarget target) {
		return boolValues.GetValue(variable, target);
	}

	public static List<bool> GetBools(LocID variable, VariableTarget target) {
		return boolValues.GetValues(variable, target);
	}

	public static LocID GetLocID(LocID variable, VariableTarget target) {
		return locIDValues.GetValue(variable, target);
	}

	public static List<LocID> GetLocIDs(LocID variable, VariableTarget target) {
		return locIDValues.GetValues(variable, target);
	}

	public static float GetFloat(LocID variable, VariableTarget target) {
		return floatValues.GetValue(variable, target);
	}

	public static List<float> GetFloats(LocID variable, VariableTarget target) {
		return floatValues.GetValues(variable, target);
	}

	public static string GetString(LocID variable, VariableTarget target) {
		return stringValues.GetValue(variable, target);
	}

	public static List<string> GetStrings(LocID variable, VariableTarget target) {
		return stringValues.GetValues(variable, target);
	}

	public static void SetValue(LocID variable, int value, VariableTarget target) {
		intValues.SetValue(variable, value, target);
	}

	public static void SetValues(LocID variable, List<int> values, VariableTarget target) {
		intValues.SetValues(variable, values, target);
	}

	public static void SetValue(LocID variable, bool value, VariableTarget target) {
		boolValues.SetValue(variable, value, target);
	}

	public static void SetValues(LocID variable, List<bool> values, VariableTarget target) {
		boolValues.SetValues(variable, values, target);
	}

	public static void SetValue(LocID variable, LocID value, VariableTarget target) {
		locIDValues.SetValue(variable, value, target);
	}

	public static void SetValues(LocID variable, List<LocID> values, VariableTarget target) {
		locIDValues.SetValues(variable, values, target);
	}

	public static void SetValue(LocID variable, float value, VariableTarget target) {
		floatValues.SetValue(variable, value, target);
	}

	public static void SetValues(LocID variable, List<float> values, VariableTarget target) {
		floatValues.SetValues(variable, values, target);
	}

	public static void SetValue(LocID variable, string value, VariableTarget target) {
		stringValues.SetValue(variable, value, target);
	}

	public static void SetValues(LocID variable, List<string> values, VariableTarget target) {
		stringValues.SetValues(variable, values, target);
	}
}
