using UnityEngine;

public class DragAndDrop : MonoBehaviour {

	public GameObject correctPaper;
	private bool moving;

	private float startPosX;
	private float startPosY;

	private Vector3 resetPos;

	void Start() {
		resetPos = this.transform.localPosition;
	}

	void Update() {
		//OnMouseDown();
		//OnMouseUp();
		if (moving) {
			Vector3 mousePos;
			mousePos = Input.mousePosition;
			mousePos = Camera.main.ScreenToWorldPoint(mousePos);

			this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, this.gameObject.transform.localPosition.z);


		}
	}

	private void OnMouseDown() {
		if (Input.GetMouseButtonDown(0)) {
			Debug.Log("clicked");
			Vector3 mousePos;
			mousePos = Input.mousePosition;
			mousePos = Camera.main.ScreenToWorldPoint(mousePos);

			startPosX = mousePos.x - this.transform.localPosition.x;
			startPosY = mousePos.y - this.transform.localPosition.y;

			moving = true;

		}
	}

	private void OnMouseUp() {
		moving = false;

		if (Mathf.Abs(this.transform.localPosition.x - correctPaper.transform.localPosition.x) <= 0.5f &&
			Mathf.Abs(this.transform.localPosition.y - correctPaper.transform.localPosition.y) <= 0.5f) {
			this.transform.localPosition = new Vector3(correctPaper.transform.localPosition.x, correctPaper.transform.localPosition.y, correctPaper.transform.localPosition.z);
		}
		else {
			this.transform.localPosition = new Vector3(resetPos.x, resetPos.y, resetPos.z);
		}
	}
}