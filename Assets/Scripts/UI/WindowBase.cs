using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProjectEnums;
using RoboRyanTron.SearchableEnum;
using UnityEngine.XR.Interaction.Toolkit.UI;
using LitMotion.Animation;
using UnityEditor;
using MEC;

public class WindowBase : MonoBehaviour {

	[SearchableEnum] public WindowPanel window;
	public bool addToEscapeStack;
	public bool persistOverSceneLoad = false;
	public bool parentToLocationTarget = false;
	public WindowLocation windowLocation;
	public RectTransform overrideHandleTarget;
	public Animator panelAnimator;
	public LitMotionAnimation tweenAnimation;

	[System.NonSerialized]
	public Canvas canvas;
	[System.NonSerialized]
	public List<WindowStackEntry> windowStack = new List<WindowStackEntry>();
	[System.NonSerialized]
	public List<ListParent> listsToAnimate = new List<ListParent>();

	protected CoroutineHandle openingRoutine;
	protected CoroutineHandle alphaRoutine;
	protected GraphicRaycaster graphicRaycaster;
	protected TrackedDeviceGraphicRaycaster trackedDeviceGraphicRaycaster;
	protected CanvasGroup canvasGroup;

	public virtual void Init(object parameters = null, List<WindowStackEntry> windowStack = null) {
		UpdateWindowStack(windowStack, parameters);

		canvasGroup = GetComponent<CanvasGroup>();
		if (canvasGroup == null) {
			canvasGroup = gameObject.AddComponent(typeof(CanvasGroup)) as CanvasGroup;
		}
		canvas = GetComponent<Canvas>();
#if VR
		if (windowLocation != WindowLocation.ScreenSpace) {
			if (VRWorldCanvasPosition.canvasPositions.ContainsKey(windowLocation) == true) {
				VRWorldCanvasPosition.canvasPositions[windowLocation].CopyCanvasSettings(canvas);
			}
			else {
				Debug.LogError("No canvas location found for WindowLocation: " + windowLocation);
			}
		}
		trackedDeviceGraphicRaycaster = GetComponent<TrackedDeviceGraphicRaycaster>();
		if (trackedDeviceGraphicRaycaster == null) {
			trackedDeviceGraphicRaycaster = gameObject.AddComponent(typeof(TrackedDeviceGraphicRaycaster)) as TrackedDeviceGraphicRaycaster;
		}
#else
		graphicRaycaster = GetComponent<GraphicRaycaster>();
		if (graphicRaycaster == null) {
			graphicRaycaster = gameObject.AddComponent(typeof(GraphicRaycaster)) as GraphicRaycaster;
		}
		if (windowLocation == WindowLocation.LeftHand || windowLocation == WindowLocation.RightHand) {
			WindowManager.instance.CloseWindow(window);
		}
		else if (windowLocation != WindowLocation.ScreenSpace && windowLocation != WindowLocation.Wrist) {
			if (VRWorldCanvasPosition.canvasPositions.ContainsKey(windowLocation) == true) {
				VRWorldCanvasPosition.canvasPositions[windowLocation].CopyCanvasSettings(canvas);
			}
			else {
				Debug.LogError("No canvas location found for WindowLocation: " + windowLocation);
			}
		}
#endif
	}

	private void UpdateWindowStack(List<WindowStackEntry> previousStack, object parameters) {
		windowStack = new List<WindowStackEntry>();
		if (previousStack != null) {
			for (int i = 0; i < previousStack.Count; i++) {
				windowStack.Add(previousStack[i].Copy());
			}
		}
		if (windowStack.Count == 0 || (windowStack.Count > 0 && windowStack[windowStack.Count - 1].windowPanel != window)) {
			WindowStackEntry newEntry = new WindowStackEntry();
			newEntry.windowPanel = window;
			newEntry.location = windowLocation;
			newEntry.parameters = parameters;
			windowStack.Add(newEntry);
		}
	}

	public virtual void ReInit(object parameters = null) {

	}

	public virtual void UpdateUI() {

	}

	protected virtual void OpeningAnimationFinished() {

	}

	protected virtual void Closing() {

	}

	protected virtual void Destroying() {

	}

	public void Open() {
		if (panelAnimator != null || tweenAnimation != null) {
			openingRoutine = Timing.RunCoroutine(OpeningRoutine());
		}
		else {
			AnimateLists();
			OpeningAnimationFinished();
		}
	}

	public void Close() {
		Closing();
		if (panelAnimator != null) {
			Timing.RunCoroutine(ClosingRoutine());
		}
		else {
			Destroy(gameObject);
		}
	}

	public CoroutineHandle Show(float duration) {
		if (alphaRoutine != null) {
			Timing.KillCoroutines(alphaRoutine);
		}
		alphaRoutine = Timing.RunCoroutine(Tools.ChangeCanvasGroupAlpha(canvasGroup, 1F, duration));
		return alphaRoutine;
	}

	public CoroutineHandle Hide(float duration) {
		if (alphaRoutine != null) {
			Timing.KillCoroutines(alphaRoutine);
		}
		alphaRoutine = Timing.RunCoroutine(Tools.ChangeCanvasGroupAlpha(canvasGroup, 0F, duration));
		return alphaRoutine;
	}

	protected virtual void OnDestroy() {
		StopAllCoroutines();
	}

	virtual protected IEnumerator<float> OpeningRoutine() {
		if (panelAnimator != null) {
			panelAnimator.speed = 1F / Gval.panelAnimationDuration;
			panelAnimator.Play("PanelOpen", -1, 0F);
			panelAnimator.Update(0F);
			float normalizedTime = 0F;
			AnimatorStateInfo stateInfo = panelAnimator.GetCurrentAnimatorStateInfo(0);
			while (normalizedTime < 1F && stateInfo.IsName("PanelOpen") == true) {
				yield return Timing.WaitForOneFrame;
				stateInfo = panelAnimator.GetCurrentAnimatorStateInfo(0);
				normalizedTime = stateInfo.normalizedTime;
			}
		}
		else if (tweenAnimation != null) {
			tweenAnimation.Play();
			while (tweenAnimation.IsPlaying == true) {
				yield return Timing.WaitForOneFrame;
			}
			tweenAnimation.Stop();
		}
		AnimateLists();
		OpeningAnimationFinished();
	}

	virtual protected IEnumerator<float> ClosingRoutine(bool destroy = true) {
		panelAnimator.speed = 1F / Gval.panelAnimationDuration;
		panelAnimator.Play("PanelClose", -1, 0F);
		panelAnimator.Update(0F);
		float normalizedTime = 0F;
		AnimatorStateInfo stateInfo = panelAnimator.GetCurrentAnimatorStateInfo(0);
		while (normalizedTime < 1F && stateInfo.IsName("PanelClose") == true) {
			yield return Timing.WaitForOneFrame;
			stateInfo = panelAnimator.GetCurrentAnimatorStateInfo(0);
			normalizedTime = stateInfo.normalizedTime;
		}
		if (destroy == true) {
			Destroy(gameObject);
		}
	}

	private void AnimateLists() {
		for (int i = 0; i < listsToAnimate.Count; i++) {
            if (listsToAnimate[i] != null) {
			    listsToAnimate[i].ShowListItemsSequentially();
            }
		}
	}
}