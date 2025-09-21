using MEC;
using ProjectEnums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class Tools : MonoBehaviour {

	public static IEnumerator<float> ChangeTextColor(TextMeshProUGUI text, Color targetColor, float duration) {
		Color currentColor = text.color;
		if (duration > 0F) {
			for (float i = 0F; i < 1F; i += Time.deltaTime / duration) {
				text.color = Color.Lerp(currentColor, targetColor, i);
				yield return Timing.WaitForOneFrame;
			}
		}
		text.color = targetColor;
	}

	public static IEnumerator<float> ChangeTextColor(TMP_Text text, Color targetColor, float duration) {
		Color currentColor = text.color;
		for (float i = 0F; i < 1F; i += Time.deltaTime / duration) {
			text.color = Color.Lerp(currentColor, targetColor, i);
			yield return Timing.WaitForOneFrame;
		}
		text.color = targetColor;
	}

	public static IEnumerator<float> ChangeImageColor(Image image, Color targetColor, float duration) {
		Color currentColor = image.color;
		if (duration > 0F) {
			for (float i = 0F; i < 1F; i += Time.deltaTime / duration) {
				image.color = Color.Lerp(currentColor, targetColor, i);
				yield return Timing.WaitForOneFrame;
			}
		}
		image.color = targetColor;
	}

	public static void ShuffleList<T>(ref List<T> list) {
		list = list.OrderBy(item => Random.value).ToList<T>();
	}

	public static IEnumerator<float> ChangeCanvasGroupAlpha(CanvasGroup canvasGroup, float targetAlpha, float duration) {
		float currentAlpha = canvasGroup.alpha;
		if (duration > 0F) {
			for (float i = 0F; i < 1F; i += Time.deltaTime / duration) {
				canvasGroup.alpha = Mathf.Lerp(currentAlpha, targetAlpha, i);
				yield return Timing.WaitForOneFrame;
			}
		}
		canvasGroup.alpha = targetAlpha;
	}

	public static IEnumerator<float> WaitResponseMessage(GenericWindow window) {
		WindowBase messageScreen = WindowManager.instance.OpenWindow(ProjectEnums.WindowPanel.GenericMessagePanel, window);
		while (messageScreen != null) {
			yield return Timing.WaitForOneFrame;
		}
	}

	public static IEnumerator<float> WaitResponseMessage(GenericWindow<int> window) {
		WindowBase messageScreen = WindowManager.instance.OpenWindow(ProjectEnums.WindowPanel.GenericMessagePanel, window);
		while (messageScreen != null) {
			yield return Timing.WaitForOneFrame;
		}
	}

	public static void ForceUpdateLayoutChildrenFirst(RectTransform parent) {
		List<RectTransform> rectTransformList = new List<RectTransform>();
		rectTransformList.Add(parent);
		for (int i = 0; i < rectTransformList.Count; i++) { // Add children breadth first
			for (int j = 0; j < rectTransformList[i].childCount; j++) {
				rectTransformList.Add(rectTransformList[i].GetChild(j).GetComponent<RectTransform>());
			}
		}
		for (int i = rectTransformList.Count - 1; i > -1; i--) {
			ContentSizeFitter contentSizeFitter = rectTransformList[i].GetComponent<ContentSizeFitter>();
			if (contentSizeFitter != null) {
				LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransformList[i]);
			}
		}
	}

	public static float LinearToDecibel(bool linearToDecibel, float volume) {
		if (linearToDecibel) {
			float dbVolume = Mathf.Log10(volume) * 20;
			return dbVolume;
		}
		else {
			float linearVolume = Mathf.Pow(10, volume / 20);
			return linearVolume;
		}
	}

	public static float ConvertLinearVolumeToLogarithmic(float linearVolume) {
		float logVolume;
		if (linearVolume == 0) {
			linearVolume = 0.0001f;
		}
		logVolume = Mathf.Log10(linearVolume) * 20;
		return logVolume;
	}

	public static float ConvertLogarithmicVolumeToLinear(float logVolume) {
		float linearVolume = Mathf.Pow(10, logVolume / 20);
		if (Mathf.Approximately(linearVolume, 0.0001F) == true) {
			linearVolume = 0F;
		}
		return linearVolume;
	}

	public static bool CheckEmailAddressValidity(string text) {
		if (text.Contains("@") == false) { // Email contains @
			return false;
		}
		if (text.Contains(".") == false) { // Email contains .
			return false;
		}
		if (text.LastIndexOf('@') > text.LastIndexOf('.')) { // There is . after @
			return false;
		}
		if (text.Length - text.LastIndexOf('.') < 3) { // There is at least two characters long top level domain
			return false;
		}
		return true;
	}

	public static float ConvertJoulesToMWh(float joules) {
		return joules / (3.6F * Mathf.Pow(10, 9));
	}

	public static float ConvertJoulesToGigaJoules(float joules) {
		return joules / (1F * Mathf.Pow(10, 9));
	}

	public static float ConvertMWhToJoules(float MWh) {
		return MWh * 3.6F * Mathf.Pow(10, 9);
	}

	public static float CalculateCircumference(float radius) {
		return 2F * Mathf.PI * radius;
	}

	public static float CalculateCircleSurfaceArea(float radius) {
		return Mathf.PI * Mathf.Pow(radius, 2F);
	}

	public static string GetTimeString(float timeInSeconds) {
		int hours = Mathf.FloorToInt(timeInSeconds / 3600F);
		int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
		int seconds = Mathf.FloorToInt(timeInSeconds % 60F);
		if (hours > 0) {
			return hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
		}
		else {
			return minutes.ToString("00") + ":" + seconds.ToString("00");
		}
	}

	public static bool Vector2Approximately(Vector2 v1, Vector2 v2) {
		if (Mathf.Approximately(v1.x, v2.x) == true && Mathf.Approximately(v1.y, v2.y) == true) {
			return true;
		}
		return false;
	}

	public static bool Vector3Approximately(Vector3 v1, Vector3 v2) {
		if (Mathf.Approximately(v1.x, v2.x) == true && Mathf.Approximately(v1.y, v2.y) == true && Mathf.Approximately(v1.z, v2.z) == true) {
			return true;
		}
		return false;
	}

	public static T2? GetDictionaryValueOrNull<T1, T2>(Dictionary<T1, T2> dictionary, T1 key) where T2 : struct {
		if (dictionary.ContainsKey(key) == true) {
			return dictionary[key];
		}
		else {
			return null;
		}
	}

	public static string GetDictionaryValueOrNull(Dictionary<LocID, string> dictionary, LocID key) {
		if (dictionary.ContainsKey(key) == true) {
			return dictionary[key];
		}
		else {
			return "";
		}
	}

	public static Vector3 GetAverageVector(List<Vector3> characterPositions) {

		Vector3 averageVector = Vector3.zero;
		foreach (Vector3 val in characterPositions) {
			averageVector += val;
		}
		return averageVector / characterPositions.Count;
	}

}
