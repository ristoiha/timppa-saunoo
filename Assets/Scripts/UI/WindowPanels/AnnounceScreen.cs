using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectEnums;

public class AnnounceScreen : WindowBase {

	public static AnnounceScreen instance;

	public Transform announceTextPrefab;
	public Transform announcementsParent;

	private List<AnnounceText> activeAnnouncements = new List<AnnounceText>();

	public override void Init(object parameters, List<WindowStackEntry> windowStack = null) {
		if (instance == null) {
			instance = this;
			base.Init(parameters, windowStack);
		}
		else {
			Destroy(gameObject);
		}
	}

	protected override void OnDestroy() {
		base.OnDestroy();
		if (instance == this) {
			instance = null;
		}
	}

	public void Announce(string message, float duration) {
		Transform announcement = Instantiate(announceTextPrefab, announcementsParent);
		AnnounceText announcementText = announcement.GetComponent<AnnounceText>();
		activeAnnouncements.Add(announcementText);
		announcementText.Announce(message, duration);
		AudioManager.PlayAudio(AudioID.Effect_Announcement);
	}

	public void RemoveAnnouncementFromList(AnnounceText announcement) {
		if (activeAnnouncements.Contains(announcement)) {
			activeAnnouncements.Remove(announcement);
		}
	}

	public override void UpdateUI() {

	}

	protected override void OpeningAnimationFinished() {

	}

	protected override void Closing() {
		if (instance == this) {
			instance = null;
		}
	}

	protected override void Destroying() {

	}
}
