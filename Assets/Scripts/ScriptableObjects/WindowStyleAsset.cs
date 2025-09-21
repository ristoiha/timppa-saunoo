using ProjectEnums;
using UnityEngine;
using TMPro;

[System.Serializable]
[CreateAssetMenu(fileName = "WindowStyleAsset", menuName = "ScriptableObjects/WindowStyleAsset", order = 1)]
public class WindowStyleAsset : ScriptableObject {

	public WindowStyle style;
	public Sprite backgroundSprite;
	public Sprite backgroundWithHeaderSprite;
	public Sprite buttonSprite;
	public float width;
	public float height;
	public TMP_FontAsset fontAsset;
	public Color fontColor;

}

