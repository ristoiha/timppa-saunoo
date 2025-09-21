using UnityEngine;

public class StaticBatchingFixer : MonoBehaviour
{
	private void OnEnable() {
		StaticBatchingUtility.Combine(this.gameObject);
	}
}
