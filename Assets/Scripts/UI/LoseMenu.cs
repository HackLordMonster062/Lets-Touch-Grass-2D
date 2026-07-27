using TMPro;
using UnityEngine;

public class LoseMenu : MonoBehaviour {
	[SerializeField] TMP_Text timeText;

	public void Initialize(float time) {
		gameObject.SetActive(true);

		timeText.text = UIManager.FormatTime(time);
	}

	public void Retry() {
		GameManager.instance.Retry();
	}

	public void Menu() {

	}
}
