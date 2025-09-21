using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.XR.Management;
using UnityEditor.XR.Management.Metadata;
using UnityEngine;
using UnityEngine.XR.Management;

public class BuildScript : MonoBehaviour {

	[MenuItem("Build/OpenXR")]
	static void BuildGameOpenXR() {
		BuildTarget target = BuildTarget.Android;
		BuildTargetGroup group = BuildPipeline.GetBuildTargetGroup(target);
		NamedBuildTarget namedTarget = NamedBuildTarget.Android;
		string newSymbols = "USE_INPUT_SYSTEM_POSE_CONTROL;USE_STICK_CONTROL_THUMBSTICKS;VR;OpenXR";
		string buildName = "OpenXR/X-Ray_VR_AR";
		BuildPlayer(group, target, namedTarget, buildName, newSymbols);
	}

	[MenuItem("Build/OculusVR")]
	static void BuildGameOculusVR() {
		BuildTarget target = BuildTarget.Android;
		BuildTargetGroup group = BuildPipeline.GetBuildTargetGroup(target);
		NamedBuildTarget namedTarget = NamedBuildTarget.Android;
		string newSymbols = "USE_INPUT_SYSTEM_POSE_CONTROL;USE_STICK_CONTROL_THUMBSTICKS;VR";
		string buildName = "Oculus/X-Ray_VR_AR";
		BuildPlayer(group, target, namedTarget, buildName, newSymbols);
	}

	[MenuItem("Build/MobileAR")]
	static void BuildGameMobileAR() {
		BuildTarget target = BuildTarget.Android;
		BuildTargetGroup group = BuildPipeline.GetBuildTargetGroup(target);
		NamedBuildTarget namedTarget = NamedBuildTarget.Android;
		string newSymbols = "USE_INPUT_SYSTEM_POSE_CONTROL;USE_STICK_CONTROL_THUMBSTICKS";
		string buildName = "Mobile/X-Ray_VR_AR";
		BuildPlayer(group, target, namedTarget, buildName, newSymbols);
	}

	//[MenuItem("Build/PC")]
	//static void BuildGamePC() {
	//	BuildTarget target = BuildTarget.StandaloneWindows64;
	//	BuildTargetGroup group = BuildPipeline.GetBuildTargetGroup(target);
	//	string newSymbols = "";
	//	string buildName = "GameName";
	//	BuildPlayer(group, target, buildName, newSymbols);
	//}

	static void BuildPlayer(BuildTargetGroup group, BuildTarget target, NamedBuildTarget namedTarget, string buildName, string symbols) {
		AndroidSdkVersions previousTargetSDK = PlayerSettings.Android.targetSdkVersion;
		string fileExtension = "";
		switch (target) {
			case BuildTarget.Android:
				fileExtension = ".apk";
				break;

		}
		XRGeneralSettings buildTargetSettings = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(group);
		XRManagerSettings pluginsSettings = buildTargetSettings.AssignedSettings;

		if (symbols.Contains("VR") == true) {
			//if (symbols.Contains("OpenXR")) {
				XRPackageMetadataStore.AssignLoader(pluginsSettings, "UnityEngine.XR.OpenXR.OpenXRLoader", group);
				XRPackageMetadataStore.RemoveLoader(pluginsSettings, "UnityEngine.XR.Oculus.OculusLoader", group);
				XRPackageMetadataStore.RemoveLoader(pluginsSettings, "UnityEngine.XR.ARCore.ARCoreLoader", group);
				PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel32;
			//}
			//else {
				//XRPackageMetadataStore.AssignLoader(pluginsSettings, "UnityEngine.XR.Oculus.OculusLoader", group);
				//XRPackageMetadataStore.RemoveLoader(pluginsSettings, "UnityEngine.XR.OpenXR.OpenXRLoader", group);
				//XRPackageMetadataStore.RemoveLoader(pluginsSettings, "UnityEngine.XR.ARCore.ARCoreLoader", group);
			//}
		}
		else {
			XRPackageMetadataStore.AssignLoader(pluginsSettings, "UnityEngine.XR.ARCore.ARCoreLoader", group);
			XRPackageMetadataStore.RemoveLoader(pluginsSettings, "UnityEngine.XR.OpenXR.OpenXRLoader", group);
			XRPackageMetadataStore.RemoveLoader(pluginsSettings, "UnityEngine.XR.Oculus.OculusLoader", group);
			PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto; // TODO: Test if works when uploading to play store, remove if it doesn't work and use hard selected editor setting.
		}

		Debug.Log("Begin Build " + target.ToString());
		string playerPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/BuildFolder/ScriptBuilds/" + buildName + "_" + PlayerSettings.bundleVersion.ToString() + fileExtension;
		string previousDefineSymbols = PlayerSettings.GetScriptingDefineSymbols(namedTarget);
		EditorUserBuildSettings.SwitchActiveBuildTarget(group, target);
		PlayerSettings.SetScriptingDefineSymbols(namedTarget, symbols);
		BuildPipeline.BuildPlayer(GetScenePaths(symbols), playerPath, target, BuildOptions.None);
		Debug.Log("Resetting Script Define Symbols");
		PlayerSettings.SetScriptingDefineSymbols(namedTarget, previousDefineSymbols);
		PlayerSettings.Android.targetSdkVersion = previousTargetSDK;
	}

	static string[] GetScenePaths(string defineSymbols) {
		string[] scenes = new string[EditorBuildSettings.scenes.Length];
		for (int i = 0; i < scenes.Length; i++) {
			scenes[i] = EditorBuildSettings.scenes[i].path;
		}
		return scenes;
	}

}
