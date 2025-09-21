using ProjectEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "TextAudioPair", menuName = "ScriptableObjects/TextAudioPair", order = 1)]
public class TextAudioPairAsset : ScriptableObject {
	public SpeechID speechID;
	public AudioClip[] audio;
	public SubtitleEntry[] subtitleEntries;
}
