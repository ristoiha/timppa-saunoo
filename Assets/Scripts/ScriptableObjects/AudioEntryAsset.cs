using ProjectEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "AudioEntryAsset", menuName = "ScriptableObjects/AudioEntryAsset", order = 1)]
public class AudioEntryAsset : ScriptableObject {

	public AudioID id;
	public AudioClip clip;
	public AudioClip clipLoop;
	public AudioClip clipEnd;
	public float volume = 1F;

}

