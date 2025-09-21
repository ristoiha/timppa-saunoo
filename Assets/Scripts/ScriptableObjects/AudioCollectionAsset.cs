using ProjectEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "AudioCollectionAsset", menuName = "ScriptableObjects/AudioCollectionAsset", order = 1)]
public class AudioCollectionAsset : ScriptableObject {

	[Header("Musics")]
	public AudioEntryAsset[] musics;

	[Header("Effects")]
	public AudioEntryAsset[] effects;
}

