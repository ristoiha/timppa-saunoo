using ProjectEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

internal class LevelManager : MonoBehaviour {

	public static LevelManager instance;

	public AssetReference[] levelPrefabs;
	public Transform playerPrefab;
	public Transform enemyPrefab;

	//[System.NonSerialized]
	//public LevelID currentLevel;
	//[System.NonSerialized]
	//public LevelID overrideStartLevel;

	private AsyncOperationHandle<GameObject> asyncOperationHandle;

	private void Awake() {
		if (instance == null) {
			instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}

	private void OnDestroy() {
		if (instance == this) {
			instance = null; ;
		}
	}

	//public void LoadLevel(LevelID ID) {
	//	currentLevel = ID;
	//	AudioTrigger.instance.PlayAmbient();
	//	Timing.RunCoroutine(levelLoadRoutine(ID));
	//}

	//private IEnumerator<float> levelLoadRoutine(LevelID ID) {
	//	WindowManager.instance.ShowWindow(WindowPanel.LoadingScreen);
	//	yield return new WaitForSeconds(Gval.panelAnimationDuration);

	//	if (asyncOperationHandle.IsValid() == true) {
	//		Addressables.Release(asyncOperationHandle);
	//	}

	//	int currentLevel = (int)ID;
	//	asyncOperationHandle = levelPrefabs[currentLevel].InstantiateAsync();
	//	yield return asyncOperationHandle;
	//	if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded) {
	//		LevelScript levelScript = asyncOperationHandle.Result.GetComponent<LevelScript>();

	//		LightingManager.instance.ChangeProfile(LightingManager.GetLevelLightingProfile(ID));
	//		if (levelScript.levelStartCutscenePlayer != null) {
	//			levelScript.levelStartCutscenePlayer.PlayCutscene();
	//		}
	//		WindowManager.instance.CloseWindow(WindowPanel.LoadingScreen);
	//	}
	//}

}
