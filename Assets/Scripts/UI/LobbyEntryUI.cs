using UnityEngine;
using TMPro;

public class LobbyEntryUI : MonoBehaviour {

	public TextMeshProUGUI lobbyNameText;
	[System.NonSerialized] public string nameString;

	public void UpdateUI(bool selected) {
		if (selected == true) {
			lobbyNameText.color = Gval.selectedColor;
		}
		else {
			lobbyNameText.color = Gval.unselectedColor;
		}
		lobbyNameText.text = nameString;
	}

}

