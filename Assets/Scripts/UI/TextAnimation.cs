using MEC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAnimation : MonoBehaviour {

	private TMP_Text m_TextComponent;
	public AnimationCurve CharacterCurve = new AnimationCurve(new Keyframe(-0.5f, 0), new Keyframe(-0.2f, 2.0f), new Keyframe(0.5f, 0));
	public float CurveScale = 1.0f;
	public float AnimationLenght = 2;
	private float old_CurveScale;
	private AnimationCurve old_curve;
	private Mesh mesh;
	private Vector3[] vertices;
	private Matrix4x4 matrix;
	private bool Initialized = false;
	private bool _animating = false;
	private float Timer;

	private IEnumerator<float> routine;

	void Start() {
		m_TextComponent = gameObject.GetComponent<TMP_Text>();
		mesh = m_TextComponent.textInfo.meshInfo[0].mesh;
		m_TextComponent.havePropertiesChanged = true;
		CurveScale *= 10;

		old_CurveScale = CurveScale;
		old_curve = CopyAnimationCurve(CharacterCurve);

		CharacterCurve.preWrapMode = WrapMode.Clamp;
		CharacterCurve.postWrapMode = WrapMode.Clamp;
		Timer = 0 + CharacterCurve.keys[0].time;
		Initialized = true;
	}

	private void OnEnable() {
		routine = Animate();
		Timing.RunCoroutine(routine);
	}

	private void OnDisable() {
		StopAnimation();
	}
	private AnimationCurve CopyAnimationCurve(AnimationCurve curve) {
		AnimationCurve newCurve = new AnimationCurve();
		newCurve.keys = curve.keys;
		return newCurve;
	}

	public IEnumerator<float> Animate() {
		_animating = true;
		while (_animating) {
			PlayAnimation();
			yield return Timing.WaitForOneFrame;
		}
	}

	public void StopAnimation() {
		_animating = false;
		StopCoroutine(routine);

	}

	private void PlayAnimation() {
		if (Initialized) {
			old_CurveScale = CurveScale;
			old_curve = CopyAnimationCurve(CharacterCurve);

			m_TextComponent.ForceMeshUpdate(); // Generate the mesh and populate the textInfo with data we can use and manipulate.

			TMP_TextInfo textInfo = m_TextComponent.textInfo;
			int characterCount = textInfo.characterCount;


			for (int i = 0; i < characterCount; i++) {
				float CharacterTime = (AnimationLenght / characterCount) * i;

				if (!textInfo.characterInfo[i].isVisible)
					continue;

				int vertexIndex = textInfo.characterInfo[i].vertexIndex;

				// Get the index of the mesh used by this character.
				int meshIndex = textInfo.characterInfo[i].materialReferenceIndex;

				vertices = textInfo.meshInfo[meshIndex].vertices;

				// Compute the baseline mid point for each character
				Vector3 offsetToMidBaseline = new Vector2((vertices[vertexIndex + 0].x + vertices[vertexIndex + 2].x) / 2, textInfo.characterInfo[i].baseLine);

				// Apply offset to adjust our pivot point.
				vertices[vertexIndex + 0] += -offsetToMidBaseline;
				vertices[vertexIndex + 1] += -offsetToMidBaseline;
				vertices[vertexIndex + 2] += -offsetToMidBaseline;
				vertices[vertexIndex + 3] += -offsetToMidBaseline;

				float y0 = CharacterCurve.Evaluate(Timer - CharacterTime) * CurveScale;
				float y1 = CharacterCurve.Evaluate(Timer - CharacterTime) * CurveScale;

				Vector3 horizontal = new Vector3(1, 0, 0);

				matrix = Matrix4x4.TRS(new Vector3(0, y0, 0), Quaternion.Euler(0, 0, 0), Vector3.one);

				vertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 0]);
				vertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 1]);
				vertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 2]);
				vertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 3]);

				vertices[vertexIndex + 0] += offsetToMidBaseline;
				vertices[vertexIndex + 1] += offsetToMidBaseline;
				vertices[vertexIndex + 2] += offsetToMidBaseline;
				vertices[vertexIndex + 3] += offsetToMidBaseline;
			}

			Timer += Time.deltaTime;
			if (Timer > AnimationLenght + CharacterCurve.keys[CharacterCurve.keys.Length - 1].time) {
				Timer = 0 + CharacterCurve.keys[0].time;
			}
			// Upload the mesh with the revised information
			m_TextComponent.UpdateVertexData();
		}
	}
}
