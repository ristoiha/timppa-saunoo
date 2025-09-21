using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchIdentifier : MonoBehaviour {

	private static float doubleClickProtectionTime = 0.03F;
	private static float doubleClickProtectionTimer = 0F;

	// Touch
	private Vector2[] touchPosition = new Vector2[20];
	private float[] touchTime = new float[20];

	// Click
	private Vector2 clickPosition;
	private float clickTime;

	// General
	private float minimumSwipeLength = 0.03F; //Percentage of screen longer edge
	private float screenLength = 0F;
	private bool clickActive = false;
	private bool swipeActive = false;
	private bool debug = false;
	private List<CustomTouch> cTouches = new List<CustomTouch>();
	private List<CustomTouch> pinchTouches = new List<CustomTouch>();

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
	private bool pinchUpdated = false;
#endif

	void Update() {
		screenLength = Mathf.Max(Screen.width, Screen.height);
		if (Input.touchCount == 0 && clickActive == false) {
			GameInputLogic.NoActiveInput();
		}
		doubleClickProtectionTimer -= Time.unscaledDeltaTime;

		if (doubleClickProtectionTimer < 0F) {
#if UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX
			if (Input.GetMouseButtonDown(0)) {
				if (EventSystem.current.IsPointerOverGameObject() == false) {
					clickPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
					clickTime = Time.time;
					if (debug == true) Debug.Log("Click start, position: " + clickPosition);
					GameInputLogic.TouchStart(clickPosition);
					clickActive = true;
				}
			}
			if (clickActive == true) {
				Vector2 currentMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				Vector2 swipeDirection = currentMousePos - clickPosition;
				float swipeLength = swipeDirection.magnitude / screenLength; // Percentage
				if (Input.GetMouseButton(0) && (swipeLength > minimumSwipeLength || swipeActive == true)) {
					swipeActive = true;
					GameInputLogic.SwipeActive(clickPosition, swipeDirection, swipeLength);
				}
				else if (Input.GetMouseButtonUp(0)) {
					if (swipeActive == true) {
						if (debug == true) Debug.Log("Swipe direction: " + swipeDirection);
						GameInputLogic.SwipeEnd(clickPosition, swipeDirection, swipeLength);
					}
					else {
						if (debug == true) Debug.Log("Click end, position:" + currentMousePos);
						GameInputLogic.TouchEnd(clickPosition);
					}
					clickActive = false;
					swipeActive = false;
				}
			}
#elif (UNITY_ANDROID || UNITY_IOS) && !VR
			for (int i = 0; i < Input.touchCount; i++) {
				Touch touch = Input.GetTouch(i);
				if (touch.phase == TouchPhase.Began) {
					if (EventSystem.current.IsPointerOverGameObject(i) == false) {
						CustomTouch cTouch = new CustomTouch();
						cTouch.startPos = touch.position;
						cTouch.time = Time.time;
						cTouch.fingerId = touch.fingerId;
						cTouches.Add(cTouch);
						if (debug == true) Debug.Log("Touch " + i + " position: " + cTouch.startPos);
						GameInputLogic.TouchStart(cTouch.startPos);
						if (cTouches.Count == 2 && pinchTouches.Count == 0) {
							PinchStart(cTouches[0], cTouches[1]);
						}
						continue;
					}				
				}
				for (int j = 0; j < cTouches.Count; j++) {
					if (touch.fingerId == cTouches[j].fingerId) {
						Vector2 swipeDirection = touch.position - cTouches[j].startPos;
						float swipeLength = swipeDirection.magnitude / screenLength; // Percentage
						float touchDuration = Time.time - cTouches[j].time;
						if (touch.phase == TouchPhase.Moved) {
							cTouches[j].currentPos = touch.position;
							PinchUpdate(cTouches[j]);
						}
						if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
							if (swipeLength > minimumSwipeLength || cTouches[j].swipeActive == true) {
								cTouches[j].swipeActive = true;
								GameInputLogic.SwipeActive(cTouches[j].startPos, swipeDirection, swipeLength);
							}
						}
						else if (touch.phase == TouchPhase.Ended) {
							if (cTouches.Count < 2 && pinchTouches.Count == 2) {
								PinchEnd();
								cTouches.Clear();
							}
							else if (swipeLength > minimumSwipeLength || cTouches[j].swipeActive == true) {
								if (debug == true) Debug.Log("Swipe " + i + " direction: " + swipeDirection);
								GameInputLogic.SwipeEnd(cTouches[j].startPos, swipeDirection, swipeLength);
								cTouches.RemoveAt(j);
								break;
							}
							else {
								if (debug == true) Debug.Log("Touch " + i + " duration: " + touchDuration);
								GameInputLogic.TouchEnd(cTouches[j].currentPos);
								cTouches.RemoveAt(j);
								break;
							}
						}
					}
				}
			}
			if (pinchUpdated == true) {
				float startDist = Vector2.Distance(pinchTouches[0].startPos,pinchTouches[1].startPos);
				float currentDist = Vector2.Distance(pinchTouches[0].currentPos,pinchTouches[1].currentPos);
				GameInputLogic.UpdatePinch(startDist, currentDist);
				pinchUpdated = false;
			}
#endif
		}
	}

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
	private void PinchStart(CustomTouch touch1, CustomTouch touch2) {
		pinchTouches.Add(touch1);
		pinchTouches.Add(touch2);
	}

	private void PinchUpdate(CustomTouch touch) {
		int index = pinchTouches.IndexOf(touch);
		if (index > -1) {
			pinchTouches[index].currentPos = touch.currentPos;
			pinchUpdated = true;
		}
	}

	private void PinchEnd() {
		float startDist = Vector2.Distance(pinchTouches[0].startPos, pinchTouches[1].startPos);
		float currentDist = Vector2.Distance(pinchTouches[0].currentPos, pinchTouches[1].currentPos);
		GameInputLogic.ApplyPinch(startDist, currentDist);
		pinchTouches.Clear();
	}
#endif

	public static void EnableDoubleClickProtection() {
		doubleClickProtectionTimer = doubleClickProtectionTime;
	}

	//private int GetYoungestTouchIndex(List<CustomTouch> touches) {
	//	float maxTime = -1F;
	//	int index = -1;
	//	for (int i = 0; i < touches.Count; i++) {
	//		if (touches[i].time > maxTime) {
	//			index = i;
	//			maxTime = touches[i].time;
	//		}
	//	}
	//	return index;
	//}
}
