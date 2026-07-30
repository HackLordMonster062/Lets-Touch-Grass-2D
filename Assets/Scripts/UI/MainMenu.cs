using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	[SerializeField] SoundControls soundControls;

	void Start() {
		GameManager.OnAfterStateChange += (state) => { if (state == GameState.GameLoaded) soundControls.UpdateValues(); };
	}

	public void StartGame() {
		SceneManager.LoadSceneAsync("MainView");
	}
}
