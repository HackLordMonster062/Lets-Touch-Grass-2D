using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
	[SerializeField] Slider health;
	[SerializeField] TMP_Text timer;

	private void Update() {
		health.value = Grass.instance.Health;

		timer.text = UIManager.FormatTime(GameManager.instance.Timer);
	}

	public void Pause() {
		GameManager.instance.TogglePause(true);
	}
}
