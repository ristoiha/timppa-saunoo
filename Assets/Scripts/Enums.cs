namespace ProjectEnums {

	//public enum MessageID {
	//	OpenWindow = 0,
	//	CloseWindow = 1,
	//	CloseAllWindows = 2,
	//	CloseAllWindowsImmediate = 3,
	//	CheckProfileValidity = 4,
	//	CreateProfile = 5,
	//	SelectProfile = 6,
	//	ToggleEffects = 7,
	//	ToggleMusic = 8,
	//	ShowInventory = 9,
	//	DeleteProfile = 10,
	//	ContinueWithSelectedProfile = 11,
	//	BackToMainMenu = 12,
	//	ExitGame = 13,
	//	LoadScene = 14,
	//	StartGame = 15,
	//	PauseAnimationToggle = 16,
	//	ResetData = 17,
	//	SendFeedback = 18,
	//	SendChatMessage = 19,
	//	PlayAudioID = 20,
	//	SwitchToPanel = 21,
	//	CreateLobby = 22,
	//	UpdateWindow = 23,
	//	SelectLobby = 24,
	//	JoinPasswordLobby = 25,
	//	TryJoinToLobby = 26,
	//	ShowNotificationText = 27,
	//	CompleteCurrentTask = 28,
	//	PlayTimelineAudioClip = 29,
	//	RevertContentVariables = 30,
	//	StartTutorial = 31,
	//	TogglePanel = 32,

	//	EnableMovement = 200,
	//}

	public enum LightingProfile {
		None = -1,
	}

	public enum ChatService {
		Vivox,
		Photon,
	}

	public enum SaveTarget {
		PersistentDataPath,
		PlayerPrefs,
	}

	public enum ContentType {
		Text = 0,
		Slider = 1,
		Toggle = 2,
		Dropdown = 3,
	}

	public enum NetworkSaveDataName {
		PlayerName,
		LobbyEmail,
		StoryText,
		ChatAllowed,
	}

	public enum MixerVolumeParameter {
		Master,
		MusicMaster,
		SFXMaster,
		VOMaster,
		MTrack1,
		MTrack2,
		SETrack1,
		SETrack2,
		SETrack3,
		SETrack4,
		SETrack5,
		SETrack6,
		SETrack7,
		SETrack8,
		VOTrack1,
		VOTrack2,
	}

	public enum MixerParameterSearchKey {
		MTrack,
		SETrack,
		VOTrack,
	}

	public enum AudioID {
		None = 0,
		Music_MainMenu = 1,
		Music_Gameplay = 2,

		Effect_ButtonClick1 = 100,
		Effect_ButtonClick2 = 101,

		Effect_SplashIntro = 1000,
		Effect_AutoMoveXRayTube = 1001,
		Effect_DoorOpen = 1002,
		Effect_DoorClose = 1003,
		Effect_Beep = 1004,
		Effect_ButtonClick = 1005,
		Effect_MouseButtonClick = 1006,
		Effect_Announcement = 1007,
		Effect_Success = 1008,
		Effect_Failure = 1009,
		Effect_Notification = 1010,
		Effect_TakeImage = 1011,
		Effect_TableMotor = 1012,
	}

	public enum SpeechID {
		None = -1,
	}

	public enum WindowPanel {
		// Project base panels
		LoadingScreen = 0,
		MainMenu = 1,
		ProfileSelection = 2,
		ProfileCreation = 3,
		SplashScreen = 4,
		
		GameUI = 6,
		PauseMenu = 7,
		DefaultWindow = 8,
		GenericMessagePanel = 9,
		SubtitlePanel = 10,
		SendEmailWindow = 11,
		ChatPanel = 12,
		TaskPanel = 13,

		LobbySelection = 15,
		LobbyCreation = 16,
		LobbyUI = 17,
		SkipCutsceneWindow = 18,
		CheckPasswordWindow = 19,
		AnnounceScreen = 20,
		VideoControlsPanel = 21,
		GenericImagePanel = 22,
		VirtualJoystickPanel = 23,
		GenericContentPanel = 24,
		PrivacyPolicyPanel = 25,

		// XR related panels
		GesturePanelLeftWrist = 50,
		GesturePanelRightWrist = 51,
		LeftHandMenuButton = 52,

		// Project related panels
		LiftPackageSelectionPanel = 100,
		LiftSelectionPanel = 101,
		LiftAnchoringPanel = 102,
		BoneSelectionPanel = 103,
		SaunaUI = 104,
	}

	public enum WindowStyle {
		NotificationSmall = 0,
		NotificationLarge,
		ContentSmall,
		ContentLarge,
	}

	public enum WindowLocation {
		ScreenSpace = 0,
		Camera = 1,
		Wrist = 2,
		LeftHand = 3,
		RightHand = 4,
	}

	public enum HandGestureID {
		CameraPinch = 0,
		ThumbUp = 1,
		CameraPoint = 2,
		PalmUpPoint = 3,
		PalmUpPinch = 4,
		PalmDownPinch = 5,
		OpenPalmUp = 6,
		Grab = 7,
		OpenHand = 8,
		Fist = 9,
	}

	public enum ParticleEffect {
		Sparks,
		BeamUp,
		Glow,
		Smoke,
		SmokeRing,
		Sparkles,
	}

	public enum Language {
		English = 0,
		Finnish = 1
	}

	public enum VariableType {
		Float = 0,
		Int = 1,
		String = 2,
		Bool = 3,
		LocID = 4,
		AudioID = 5,
		WindowPanel = 6,
		Command = 7,
		Dynamic = 8,
		ScriptableObject = 9,
	}

	public enum ButtonActionType {
		Command = 0,
		WindowPanel = 1,
		ContentPanel = 2,
		SetSceneVariable = 3,
		SetRuntimeVariable = 4,
		SetVariable = 5,
		CustomCommand = 6,
	}

	public enum VariableTarget {
		SceneValue,
		RuntimeValue,
		PermanentValue,
	}

	public enum WindowPanelAction {
		OpenWindow = 0,
		CloseWindow = 1,
		SwitchToWindow = 2,
		UpdateWindow = 3,
		ToggleWindow = 4,
	}

	public enum ContentPanelAction {
		OpenWindow = 0,
		CloseWindow = 1,
		SwitchToWindow = 2,
	}

	public enum AnimationName {
		AnimationPinch,
		AnimationGrab,
		AnimationFingerPoint,
		AnimationPinchRotate,
		PoseDefault,
		PoseHandOpen,
		PosePalmUp,
		PoseGrab,
		PoseThumbUp,
		PosePinch,
		PoseFingerPoint,
		PoseHandGun,
	}

	public enum Command {
		SaveChanges = 0,
		RevertChanges = 1,
		SwitchToPreviousWindow = 2,
		ExitGame = 3,
		StartGame = 4,
		ShowNotification = 5,
		LoadScene = 6,
	}

	public enum CustomCommand {
		OpenLiftPanel = 0,
	}

	public enum LocID {
		LanguageName = -1,
		None = 0,
		NameHasInvalidCharacters = 1,
		NameOK = 2,
		SelectProfile = 3,
		CreateProfile = 4,
		Options = 5,
		Create = 6,
		Select = 7,
		Accept = 8,
		Loading = 9,
		EnterName = 10,
		Value_InstructionsShown = 11,

		Paused = 13,
		Play = 14,
		Continue = 15,
		BackToMain = 16,
		ExitGame = 17,
		Password = 18,
		CreateLobby = 19,
		LobbyName = 20,
		EmailAddress = 21,
		LobbyPassword = 22,
		JoinLobby = 23,
		CreateNew = 24,
		RefreshLobbyList = 25,
		LobbyInfo = 26,
		SendFeedback = 27,
		SwitchProfile = 28,
		Skip = 29,
		WrongPassword = 30,
		MouseSensitivity = 31,
		FOV = 32,

		Load = 35,
		Back = 36,
		ResetData = 37,
		Online = 38,
		Offline = 39,
		LobbyRequiresPassword = 40,
		Tutorial = 41,
		Complete = 42,
		SelectDifficulty = 43,
		Yes = 44,
		No = 45,
		Action_EndAudio = 46,
		Credits = 47,
		PrivacyPolicy = 48,
		Welcome = 49,
		Seconds = 50,
		Minutes = 51,
		Hours = 52,

		NameAlreadyExists = 67,
		SendToEmail = 69,
		Send = 70,
		Save = 71,
		WriteYourEmailInfo = 72,
		Publish = 73,
		InfoText_ProfileSelection = 74,
		InfoText_ProfileCreation = 75,
		InfoText_LobbySelection = 76,
		InfoText_LobbyCreation = 77,
		InfoText_SendEmail = 78,

		Action_ChangeNextTask = 80,
		TaskComplete = 81,
		Reset = 82,
		Cancel = 83,
		InfoText_ResetInfo = 84,
		Ok = 85,
		LobbySelectionRequired = 86,

		GameOver = 89,

		NameTooShort = 93,
		SkipTask = 94,
		Players = 95,

		Profile = 106,

		Locale_fin = 110,
		Locale_eng = 111,

		Option_SFXVolume = 1000,
		Option_MusicVolume = 1001,
		Option_MovementStyle = 1002,
		Option_TurningStyle = 1003,
		Option_MasterVolume = 1004,
		Option_VOVolume = 1005,

		TutorialWelcomeTaskVR = 1045,
		CreditsContent = 1054,
		TutorialWelcomeTask = 1057,
		IntroInstructionsVR = 1058,

		TaskGroup_Tutorial = 1303,
		Task_LookAtWrist = 1426,
		Task_LookAtExtraInformation = 1427,
		Task_CloseExtraInfo = 1428,
		Task_ThumbUpGestureLeftHand = 1429,
		Task_ThumbUpGestureRightHand = 1430,
		Task_WalkForward = 1431,
		Task_WalkBackward = 1432,
		Task_WalkLeft = 1433,
		Task_WalkRight = 1434,
		Task_RotateLeft = 1435,
		Task_RotateRight = 1436,
		Task_PinchMove = 1437,
		Task_LookAtExtraInformationMobile = 1483,
		Task_WalkForward_Mobile = 1484,
		Task_WalkBackward_Mobile = 1485,
		Task_WalkLeft_Mobile = 1486,
		Task_WalkRight_Mobile = 1487,
		Task_RotateLeft_Mobile = 1488,
		Task_RotateRight_Mobile = 1489,

		MovementStyle_TeleportAndDrag = 2000,
		MovementStyle_NoDrag = 2001,
		MovementStyle_NoTeleport = 2002,
		MovementStyle_GestureOnly = 2003,

		TurningStyle_Snap = 2010,
		TurningStyle_Continuous = 2011,

		Value_LastLiftPackage = 3000,
		Value_LastLift = 3001,
		Value_GridEnabled = 3002,
		Value_ShowCenterOfMass = 3003,
		Value_TrailHips = 3004,
		Value_TrailLeftUpperLeg = 3005,
		Value_TrailRightUpperLeg = 3006,
		Value_TrailLeftFoot = 3007,
		Value_TrailRightFoot = 3008,
		Value_TrailLeftUpperArm = 3009,
		Value_TrailRightUpperArm = 3010,
		Value_TrailDuration = 3011,
		Value_SelectedSkin = 3012,

		Action_UpdateTrail = 4000,

		Skin_Female = 5000,
		Skin_StickFigure = 5001,

		SelectLiftStartTarget = 6000,
		LiftTarget_Floor = 6001,
		LiftTarget_Bed = 6002,
		LiftTarget_Wheelchair = 6003,

		SelectLift = 6030,
		Lift_FloorToStanding = 6031,
		Lift_BedToStanding = 6032,
		Lift_WheelchairToStanding = 6033,

		ArtInfo_Kissakuva = 8000,

	}
}