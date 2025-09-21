using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectEnums;
using RoboRyanTron.SearchableEnum;

[System.Serializable]
public class SubtitleEntry {

	[SearchableEnum] public LocID locID;
	public float startTime;
	public float endTime;

}
