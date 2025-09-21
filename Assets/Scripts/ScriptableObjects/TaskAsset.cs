using ProjectEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.SearchableEnum;

[System.Serializable]
[CreateAssetMenu(fileName = "TaskAsset", menuName = "ScriptableObjects/TaskAsset", order = 1)]
public class TaskAsset : ScriptableObject {

	public bool showAllTasks;
	[SearchableEnum] public LocID taskGroupLocID;
	[SearchableEnum] public List<LocID> taskList;
}

public static class TaskAssetExtensions {

	public static bool CheckTaskSkip(this TaskAsset asset, LocID taskID) {
		//if (taskID == LocID.Task_CloseDoor && BoolVariable.GetSceneValue(LocID.Value_DoorOpen) == false) {
		//	return true;
		//}
		//else if (taskID == LocID.Task_TurnAECOn && BoolVariable.GetSceneValue(LocID.Value_AEC) == true) {
		//	return true;
		//}
		return false;
	}

}
