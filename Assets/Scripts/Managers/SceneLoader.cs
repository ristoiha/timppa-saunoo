using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using ProjectEnums;
using System.Collections.Generic;
using MEC;

public class SceneLoader : MonoBehaviour {

	public static SceneLoader instance;
	public bool sceneLoadInProgress;

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
            instance = null;
        }
    }

    public void LoadScene(int sceneBuildIndex, object parameters = null) {
        Timing.RunCoroutine(LoadSceneInternal(SceneManager.GetSceneAt(sceneBuildIndex).name, parameters));
    }

    public void LoadScene(string sceneName, object parameters = null) {
        Timing.RunCoroutine(LoadSceneInternal(sceneName, parameters));
	}

    public void LoadScene(SceneID sceneID, object parameters = null) {
        Timing.RunCoroutine(LoadSceneInternal(sceneID.ToString(), parameters));
	}

	private IEnumerator<float> LoadSceneInternal(string sceneName, object parameters) {
		WindowManager.instance.OpenWindow(WindowPanel.LoadingScreen);
		sceneLoadInProgress = true;
		for (float i = 0F; i < Gval.panelAnimationDuration; i += Time.unscaledDeltaTime) {
			yield return Timing.WaitForOneFrame;
		}
		WindowManager.instance.CloseWindowsOnSceneLoad();
		ScriptableObjectManager.instance.OnSceneChange();
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);
        asyncOp.allowSceneActivation = false;
		for (float i = 0F; i < Gval.mininumLoadingScreenDisplayTime - Gval.panelAnimationDuration; i += Time.unscaledDeltaTime) {
			yield return Timing.WaitForOneFrame;
		}
        while(!asyncOp.isDone) {
			if (asyncOp.progress >= 0.9F) {
				sceneLoadInProgress = false;
				asyncOp.allowSceneActivation = true;
			}
			yield return Timing.WaitForOneFrame;
        }
		WindowManager.instance.CloseWindow(WindowPanel.LoadingScreen);
    }

}

namespace ProjectEnums {

	public enum SceneID {
		LoadingScene = -1,
		MainMenu = 0,
		Gameplay = 1,
	}

}