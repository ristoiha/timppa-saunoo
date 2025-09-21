using ProjectEnums;
using RoboRyanTron.SearchableEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableContent : ScriptableObject {

	[SearchableEnum] public LocID locID;
	public Transform uiPrefab;

	public virtual void InstantiateUIPrefab(Transform parent, LocID overrideLocID = LocID.None) {

	}

	public virtual void Initialize(LocID locID) {

	}

	public static void ResetValues() {
		FloatVariable.RevertValues();
		BoolVariable.RevertValues();
		LocIDVariable.RevertValues();
		StringVariable.RevertValues();
	}

}

public static class VariableOptionExtensions {

	public static float GetFloat(this VariableContent variableOption) {
		return FloatVariable.GetValue(variableOption.locID);
	}

	public static bool GetBool(this VariableContent variableOption) {
		return BoolVariable.GetValue(variableOption.locID);
	}

	public static LocID GetLocID(this VariableContent variableOption) {
		return LocIDVariable.GetValue(variableOption.locID);
	}

	public static string GetString(this VariableContent variableOption) {
		return StringVariable.GetValue(variableOption.locID);
	}

}

//public interface IVariableOptionInterface<T> {

//	public abstract T Value { get; set; }
//	public abstract T DefaultValue { get; }
//	public abstract T MinValue { get; }
//	public abstract T MaxValue { get; }
//	public abstract Transform Prefab { get; }


//}
