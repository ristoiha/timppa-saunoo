using MEC;
using ProjectEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class WindowManager : MonoBehaviour {

	public static WindowManager instance;

	public List<Transform> panelPrefabs;
	public Camera uiCamera;
	public Transform XRWindowHandlePrefab;

	private List<WindowBase> currentPanels = new List<WindowBase>();
	private static Dictionary<WindowPanel, Transform> panelDictionary = new Dictionary<WindowPanel, Transform>();

	private void Awake() {
		if (instance == null) {
			instance = this;
			for (int i = 0; i < panelPrefabs.Count; i++) {
				WindowBase windowBase = panelPrefabs[i].GetComponent<WindowBase>();
				if (panelDictionary.ContainsKey(windowBase.window) == false) {
					panelDictionary.Add(windowBase.window, panelPrefabs[i]);
				}
			}
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

	public CoroutineHandle ShowWindow(WindowPanel panel, object parameters = null, float duration = Gval.panelAnimationDuration) {
		WindowBase window = GetWindow(panel, parameters);
		if (window != null) {
			return window.Show(duration);
		}
		return new CoroutineHandle();
	}

	public CoroutineHandle HideWindow(WindowPanel panel, object parameters = null, float duration = Gval.panelAnimationDuration) {
		WindowBase window = GetWindow(panel, parameters);
		if (window != null) {
			return window.Hide(duration);
		}
		return new CoroutineHandle(); // null handle
	}

	public void OpenWindowDelayed(WindowPanel panel, object parameters = null, List<WindowStackEntry> windowStack = null, float delay = 0.01F) {
		Timing.RunCoroutine(OpenWindowDelayedRoutine(panel, parameters, windowStack, delay));
	}

	public WindowBase OpenWindow(WindowPanel panel, object parameters = null, List<WindowStackEntry> windowStack = null) {
		WindowBase windowBase = GetWindow(panel, parameters);
		if (windowBase == null && SceneLoader.instance.sceneLoadInProgress == false) {
			if (panelDictionary.TryGetValue(panel, out Transform panelPrefab) == true) {
				Transform newPanel = Instantiate(panelPrefab, gameObject.transform);
				newPanel.GetComponent<Canvas>().worldCamera = uiCamera;
				windowBase = newPanel.GetComponent<WindowBase>();
				currentPanels.Add(windowBase);
				windowBase.Init(parameters, windowStack);
				windowBase.Open();
			}
			else {
				Debug.LogError(panel.ToString() + " is not added to WindowManager panelPrefabs list");
			}
		}
		else if (windowBase != null && SceneLoader.instance.sceneLoadInProgress == false) {
			windowBase.ReInit(parameters);
		}
		if (windowBase != null) {
			windowBase.UpdateUI();
		}
		return windowBase;
	}

	public bool CloseWindow(WindowPanel panel, List<WindowStackEntry> windowStack) {
		return CloseWindow(panel, windowStack[windowStack.Count - 1].parameters);
	}

	public bool CloseWindow(WindowPanel panel, object parameters = null) {
		WindowBase window = GetWindow(panel, parameters);
		if (window != null) {
			currentPanels.Remove(window);
			window.Close();
			return true;
		}
		return false;
	}

	public void CloseWindowsInLocation(WindowLocation location) {
		for (int i = currentPanels.Count - 1; i > -1; i--) {
			if (currentPanels[i].windowLocation == location) {
				currentPanels[i].Close();
				currentPanels.RemoveAt(i);
			}
		}
	}

	public void CloseWindowImmediate(WindowPanel panel, object parameters = null) {
		WindowBase window = GetWindow(panel, parameters);
		if (window != null) {
			Destroy(window.gameObject);
			currentPanels.Remove(window);
		}
	}

	public void CloseAllWindows(bool closeLoadingScreen = true) {
		for (int i = currentPanels.Count - 1; i > -1; i--) {
			WindowBase window = currentPanels[i];
			if (window.window != WindowPanel.LoadingScreen || closeLoadingScreen == true) {
				currentPanels.Remove(window);
				window.Close();
			}
		}
	}

	public void CloseAllWindowsImmediate(bool closeLoadingScreen = true) {
		for (int i = currentPanels.Count - 1; i > -1; i--) {
			if (currentPanels[i].window != WindowPanel.LoadingScreen || closeLoadingScreen == true) {
				Destroy(currentPanels[i].gameObject);
				currentPanels.RemoveAt(i);
			}
		}
	}

	public void CloseWindowsOnSceneLoad() {
		for (int i = currentPanels.Count - 1; i > -1; i--) {
			if (currentPanels[i].persistOverSceneLoad == false) {
				Destroy(currentPanels[i].gameObject);
				currentPanels.RemoveAt(i);
			}
		}
	}

	public WindowBase GetWindow(WindowPanel panel, List<WindowStackEntry> windowStack) {
		return GetWindow(panel, windowStack[windowStack.Count - 1].parameters);
	}

	public WindowBase GetWindow(WindowPanel panel, object parameters = null) {
		if (panel == WindowPanel.GenericContentPanel) {
			if (parameters != null) {
				ContentPanelAsset contentPanelAsset = null;
				if (parameters is GenericContentWindow) {
					contentPanelAsset = ((GenericContentWindow)parameters).contentPanel;
				}
				else if (parameters is ContentPanelAsset) {
					contentPanelAsset = (ContentPanelAsset)parameters;
				}
				for (int i = 0; i < currentPanels.Count; i++) {
					if (currentPanels[i].window == panel && contentPanelAsset == ((GenericContentPanel)currentPanels[i]).contentPanelAsset) {
						return currentPanels[i];
					}
				}
			}
			else {
				return null;
			}
		}
		else {
			for (int i = 0; i < currentPanels.Count; i++) {
				if (currentPanels[i].window == panel) {
					return currentPanels[i];
				}
			}
		}
		return null;
	}

	public int GetCurrentPanelCount() {
		return currentPanels.Count;
	}

	public void UpdateWindow(WindowPanel panel) {
		for (int i = 0; i < currentPanels.Count; i++) {
			if (currentPanels[i].window == panel) {
				currentPanels[i].UpdateUI();
			}
		}
	}

	public bool WindowIsOpen(WindowPanel panel) {
		for (int i = 0; i < currentPanels.Count; i++) {
			if (currentPanels[i].window == panel) {
				return true;
			}
		}
		return false;
	}

	private IEnumerator<float> OpenWindowDelayedRoutine(WindowPanel panel, object parameters, List<WindowStackEntry> windowStack, float delay) {
		yield return Timing.WaitForSeconds(delay);
		OpenWindow(panel, parameters, windowStack);
	}
}
