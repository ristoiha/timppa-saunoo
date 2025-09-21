using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGOInMobile : MonoBehaviour {
#if UNITY_ANDROID || UNITY_IOS
	private void Start() {
		gameObject.SetActive(false);
	}
#endif
}
