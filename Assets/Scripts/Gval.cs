using UnityEngine;

public static class Gval {

	public static readonly string defaultProfileName = "DefaultProfile";
	public static readonly string profileListName = "ProfileList";
	public static readonly string profileListFolder = "ProfileList";
	public static readonly string profileFolder = "Profiles";
	public static readonly string itemPicturesFolder = "ItemPictures";
	public static readonly string feedbackEmail = "feedbackmail@yourmail.com";
	public static readonly string configurationListFolder = "ConfigurationList";
	public static readonly string configurationListName = "ConfigurationList";
	public static readonly string configurationFolder = "Configurations";

	// Window panels related
	public const float panelAnimationDuration = 0.3F;
	public static readonly float mininumLoadingScreenDisplayTime = 0.8F;

	public static readonly float minFov = 60f;
	public static readonly float maxFov = 120f;
	public static readonly float minMouseSens = 1f;
	public static readonly float maxMouseSens = 10f;

	// Colors
	public static readonly Color selectedColor = new Color(0.4745098F, 0.4980392F, 0.2313726F, 1F);
	public static readonly Color unselectedColor = new Color(0.6705883F, 0.509804F, 0.427451F, 0.5F);
	public static readonly Color okColor = new Color(0.4745098F, 0.4980392F, 0.2313726F, 1F);
	public static readonly Color cancelColor = new Color(0.7333333F, 0.3254902F, 0.1764706F, 1F);

	// Audio
	public static readonly float crossFadeDuration = 5F;

	//public static readonly LayerMask roentgenImageLayer = 1 << LayerMask.NameToLayer("RoentgenImageRender");
	//public static readonly LayerMask roentgenImageAlphaLayer = 1 << LayerMask.NameToLayer("RoentgenImageAlphaRender");
	public static readonly LayerMask interactableLayer = 1 << LayerMask.NameToLayer("Interactable");
	public static readonly LayerMask wallLayer = 1 << LayerMask.NameToLayer("Wall");
	public static readonly LayerMask groundLayer = 1 << LayerMask.NameToLayer("Ground");
	public static readonly LayerMask floorCeilingLayer = 1 << LayerMask.NameToLayer("FloorCeiling");

	//public static readonly LayerMask wallLayer = LayerMask.NameToLayer("Wall");
	//public static readonly LayerMask playerLayer = LayerMask.NameToLayer("Player");
	//public static readonly LayerMask collectibleLayer = LayerMask.NameToLayer("Collectible");
	//public static readonly LayerMask interactableLayer = LayerMask.NameToLayer("Interactable");
	//public static readonly LayerMask enemyLayer = LayerMask.NameToLayer("Enemy");
	//public static readonly LayerMask spellLayer = LayerMask.NameToLayer("Spell");
	//public static readonly LayerMask floorLayer = LayerMask.NameToLayer("Floor");

	public static readonly float dragDistanceThreshold = 0.02F;

	// Material parameters
	public static readonly string materialColor = "_Color";
	public static readonly string materialOpacity = "_Opacity";

	//public static readonly int backgroundEnvironmentItemsBackgroundLayer = SortingLayer.NameToID("Background_Environment_Items_Background");
	//public static readonly int backgroundEnvironmentContainerLayer = SortingLayer.NameToID("Background_Environment_Container");
	//public static readonly int backgroundEnvironmentItemsForegroundLayer = SortingLayer.NameToID("Background_Environment_Items_Foreground");

	//public static readonly int foregroundEnvironmentItemsBackgroundLayer = SortingLayer.NameToID("Foreground_Environment_Items_Background");
	//public static readonly int foregroundEnvironmentContainerLayer = SortingLayer.NameToID("Foreground_Environment_Container");
	//public static readonly int foregroundEnvironmentItemsForegroundLayer = SortingLayer.NameToID("Foreground_Environment_Items_Foreground");

}