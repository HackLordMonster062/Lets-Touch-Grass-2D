using UnityEngine;

public class UIManager : Singleton<UIManager> {
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] WinMenu winMenu;
    [SerializeField] LoseMenu loseMenu;
    [field: SerializeField] public HUD HUD { get; private set; }

    public void TogglePause(bool isPaused) {
        pauseMenu.gameObject.SetActive(isPaused);
    }

    public void Win(float time) {
        winMenu.Initialize(time);
	}

    public void Lose(float time) {
        loseMenu.Initialize(time);
	}

	public static string FormatTime(float time) {
		return (time > 3600 ? $"{(int)time / 3600}:" : "") + $"{(int)time % 3600 / 60:00}:{time % 60:00.00}";
	}
}
