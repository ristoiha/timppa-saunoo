using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class ListParent : MonoBehaviour {

	public float startDelay;
	public float showItemInterval;

	[System.NonSerialized]
	public List<ListItem> listItems = new List<ListItem>();

	private void Awake() {
		GetComponentInParent<WindowBase>()?.listsToAnimate.Add(this);
	}

	public void HighlightItem(ListItem listItem) {
		for (int i = 0; i < listItems.Count; i++) {
			bool highlight = listItems[i] == listItem;
			listItem.Highlight(highlight);
		}
	}

	public void HighlightAt(int index) {
		HighlightItem(listItems[index]);
	}

	public void AddListItem(ListItem listItem) {
		listItems.Add(listItem);
	}

	public void ShowListItemsSequentially() {
		Timing.RunCoroutine(ShowListItemsSequentiallyRoutine().CancelWith(gameObject));
	}

	private IEnumerator<float> ShowListItemsSequentiallyRoutine() {
		if (startDelay > 0F) {
			yield return Timing.WaitForSeconds(startDelay);
		}
		float delay = 0F;
		for (int i = 0; i < listItems.Count; i++) {
			listItems[i].Show(true);
			for (; delay < showItemInterval; delay += Time.deltaTime) {
				yield return Timing.WaitForOneFrame;
			}
			delay -= showItemInterval;
		}
	}

}
