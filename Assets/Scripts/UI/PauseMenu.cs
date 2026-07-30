using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
	[SerializeField] SoundControls soundControls;

	private void Awake() {
		GameManager.OnBeforeStateChange += UpdateValues;
	}

	private void UpdateValues(GameState state) {
		if (state != GameState.Paused) return;

		soundControls.UpdateValues();
	}

	public void Continue() {
		GameManager.instance.TogglePause();
	}
}
