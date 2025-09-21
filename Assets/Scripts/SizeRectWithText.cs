using UnityEngine;
using TMPro;

//Sized rect transform to fit text with padding
//Usage:
//In an rect transform with i.e. textMeshProUGUI component as a child
[RequireComponent(typeof(RectTransform))]
public class SizeRectWithText : MonoBehaviour {
	[SerializeField] private TextMeshProUGUI text; 
	[SerializeField] private float minWidth = 300f, maxWidth = 500f;
	[SerializeField] private float minHeight = 100f, maxHeight = 200f;
	[SerializeField] private float heightPadding = 100f;
	[SerializeField] private float widthPadding = 10f;
	private RectTransform rectTransform;

	private void Awake() {
		rectTransform = GetComponent<RectTransform>();
	}
	void Start() {
		UpdateRectTransform();
	}

	public void UpdateRectTransform() {
		if (rectTransform == null) {
			rectTransform = GetComponent<RectTransform>();
		}
		if (text != null) {
			float preferredWidth = text.preferredWidth + widthPadding;
			float preferredHeight = text.preferredHeight + heightPadding;

			if (preferredWidth < minWidth) {
				preferredWidth = minWidth;
			}
			else if (preferredWidth > maxWidth) {
				preferredWidth = maxWidth;
			}
			if (preferredHeight < minHeight) {
				preferredHeight = minHeight;
			}
			else if (preferredHeight > maxHeight) {
				preferredHeight = maxHeight;
			}
			rectTransform.sizeDelta = new Vector2(preferredWidth, preferredHeight);
		}
	}
}
