using UnityEngine;
using UnityEngine.Audio;

public class UIManager : PersistentSingleton<UIManager> {
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] WinMenu winMenu;
    [SerializeField] LoseMenu loseMenu;
	[SerializeField] HUD hud;
	[SerializeField] Transform canvas;

	PauseMenu _pauseMenu;
	WinMenu _winMenu;
	LoseMenu _loseMenu;
	public HUD HUD { get; private set; }

	Transform _canvas;

	protected override void Awake() {
		base.Awake();
		if (instance != this) return;

		GameManager.OnBeforeStateChange += Initiate;
	}

	public void Initiate(GameState state) {
		if (state != GameState.Initiating) return;

		if (_canvas != null) {
			Destroy(_canvas.gameObject);
		}

		_canvas = Instantiate(canvas).transform;
		HUD = Instantiate(hud, _canvas);
		_pauseMenu = Instantiate(pauseMenu, _canvas);
		_winMenu = Instantiate(winMenu, _canvas);
		_loseMenu = Instantiate(loseMenu, _canvas);
	}

	public void TogglePause(bool isPaused) {
        _pauseMenu.gameObject.SetActive(isPaused);
    }

    public void Win(float time) {
        _winMenu.Initialize(time);
	}

    public void Lose(float time) {
        _loseMenu.Initialize(time);
	}

	public static string FormatTime(float time) {
		return (time > 3600 ? $"{(int)time / 3600}:" : "") + $"{(int)time % 3600 / 60:00}:{time % 60:00.00}";
	}
}
