using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LitMotion.Animation;
using TMPro;
using ProjectEnums;

public class ListItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public LitMotionAnimation highlightAnim;
	public LitMotionAnimation normalAnim;
	public LitMotionAnimation appearAnim;

	private CanvasGroup canvasGroup;

	public virtual void Awake() {
		canvasGroup = gameObject.AddComponent<CanvasGroup>();
		ListParent listParent = transform.parent.GetComponent<ListParent>();
		if (listParent != null) {
			Show(false);
			listParent.AddListItem(this);
		}
	}

	public void Highlight(bool highlight) {
		if (highlight == true && highlightAnim != null) {
			if (normalAnim != null) {
				normalAnim.Stop();
			}
			highlightAnim.Restart();
			highlightAnim.Play();
		}
		else if (highlight == false && normalAnim != null) {
			if (highlightAnim != null) {
				highlightAnim.Stop();
			}
			normalAnim.Restart();
			normalAnim.Play();
		}
	}

	public void Show(bool show) {
		if (show == true) {
			canvasGroup.alpha = 1F;
		}
		else {
			canvasGroup.alpha = 0F;
		}
		if (show == true) {
			if (appearAnim != null) {
				appearAnim.Restart();
				appearAnim.Play();
			}
		}
	}

	public void UpdateAsset(ScriptableObject scriptableObject) {
		//if (scriptableObject is ExhibitAsset) {
		//	ExhibitAsset liftPackageAsset = (ExhibitAsset)scriptableObject;
		//	GetComponentInChildren<TextMeshProUGUI>().text = LocalizationManager.instance.GetString(liftPackageAsset.locID);
		//	GetComponent<Image>().sprite = liftPackageAsset.sprite;
		//	GetComponent<ButtonMessenger>().buttonActions[0].scriptableObjectValue = liftPackageAsset;
		//}
		//else if (scriptableObject is RoomAsset) {
		//	RoomAsset liftAsset = (RoomAsset)scriptableObject;
		//	int sequenceNumber = transform.GetSiblingIndex() + 1;
		//	transform.GetComponent<ButtonMessenger>().buttonActions[0].locIDValue = liftAsset.locID;
		//	transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sequenceNumber.ToString();
		//	transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = LocalizationManager.instance.GetString(liftAsset.locID);
		//	transform.GetChild(2).GetComponent<ButtonMessenger>().buttonActions[0].locIDValue = liftAsset.descriptionLocID;
		//}
	}

	public void OnPointerEnter(PointerEventData eventData) {
		Highlight(true);
	}

	public void OnPointerExit(PointerEventData eventData) {
		Highlight(false);
	}

}
