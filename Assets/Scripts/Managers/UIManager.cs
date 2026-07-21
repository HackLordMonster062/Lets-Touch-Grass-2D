using UnityEngine;

public class UIManager : Singleton<UIManager> {
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] WinMenu winMenu;
    [SerializeField] LoseMenu loseMenu;
    [field: SerializeField] public HUD HUD { get; private set; }

    public void TogglePause(bool isPaused) {
        pauseMenu.gameObject.SetActive(isPaused);
    }
}
