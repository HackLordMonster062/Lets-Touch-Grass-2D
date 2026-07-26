using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
	[SerializeField] Slider health;
	[SerializeField] TMP_Text timer;

	private void Update() {
		health.value = Grass.instance.Health;

		timer.text = FormatTime(GameManager.instance.Timer);
	}

	string FormatTime(float time) {
		return (time > 3600 ? $"{(int)time / 3600}:" : "") + $"{(int)time % 3600 / 60:00}:{time % 60:00.00}";
	}
}
