using ProjectEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "LightingProfileAsset", menuName = "ScriptableObjects/LightingProfileAsset", order = 1)]
public class LightingProfileAsset : ScriptableObject {
	public Material skyboxMaterial;
	public Color skyboxTintColor;
	[ColorUsage(false, true)] public Color ambientGroundColor;
	[ColorUsage(false, true)] public Color ambientEquatorColor;
	[ColorUsage(false, true)] public Color ambientSkyColor;
	public bool fog;
	public float fogStartDistance;
	public float fogEndDistance;
	public Color fogColor;
}
