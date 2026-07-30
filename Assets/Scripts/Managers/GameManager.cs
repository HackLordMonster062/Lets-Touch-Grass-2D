using Playgama;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager> {

	public GameState State { get; private set; }
	public float Timer { get; private set; }

	public static event Action<GameState> OnBeforeStateChange;
	public static event Action<GameState> OnAfterStateChange;

	InputMap _input;

	protected override void Awake() {
		base.Awake();
		if (instance != this) return;

		_input = new();
		_input.UI.Enable();

		SceneManager.sceneLoaded += (scene, loadMode) => { if (scene.name == "MainView") StartGame(); if (scene.name == "MainMenu") ChangeState(GameState.GameLoaded); };
	}

	private void Start() {
		Bridge.platform.SendMessage("game_ready");
	}

	public void StartGame() {
		ChangeState(GameState.Initiating);

		Timer = 0;

		Grass.instance.OnTouched += Win;
		Grass.instance.OnDied += Lose;

		ChangeState(GameState.Playing);

		Bridge.platform.SendMessage("level_started");
	}

    void Update() {
		if (_input.UI.Cancel.WasPressedThisFrame()) {
			TogglePause();
		}

		if (State == GameState.Playing) {
			Timer += Time.deltaTime;
		}
    }

	public void TogglePause(bool pause) {
		if (!pause && State == GameState.Paused) {
			ChangeState(GameState.Playing);

			Bridge.platform.SendMessage("level_resumed");
		} else if (pause && State == GameState.Playing) {
			ChangeState(GameState.Paused);

			Bridge.platform.SendMessage("level_paused");
		}
	}

	public void TogglePause() {
		TogglePause(GameState.Paused != State);
	}

	public void Win() {
		ChangeState(GameState.Won);
		ChangeState(GameState.Cleanup);

		Bridge.platform.SendMessage("level_completed");
	}

	public void Lose() {
		ChangeState(GameState.Lost);
		ChangeState(GameState.Cleanup);

		Bridge.platform.SendMessage("level_failed");
	}

	public void ChangeState(GameState newState) {
		OnBeforeStateChange?.Invoke(newState);

		State = newState;
		switch (newState) {
			case GameState.GameLoaded:
				Time.timeScale = 1;
				AudioManager.instance.TogglePause(false);
				break;
			case GameState.Initiating:
				break;
			case GameState.Paused:
				AudioManager.instance.TogglePause(true); 
				Time.timeScale = 0;
				UIManager.instance.TogglePause(true);

				break;
			case GameState.Playing:
				Time.timeScale = 1;
				UIManager.instance.TogglePause(false);
				AudioManager.instance.TogglePause(false);

				break;
			case GameState.Lost:
				AudioManager.instance.TogglePause(true);
				Time.timeScale = 0;
				UIManager.instance.Lose(Timer);

				break;
			case GameState.Won:
				AudioManager.instance.TogglePause(true);
				Time.timeScale = 0;
				UIManager.instance.Win(Timer);

				break;
			case GameState.Cleanup:
				Grass.instance.OnTouched -= Win;
				Grass.instance.OnDied -= Lose;

				break;
			default:
				break;
		}

		OnAfterStateChange?.Invoke(newState);
	}

	public void OnCancel(InputValue value) {
		TogglePause();
	}

	public void Retry() {
		TogglePause(false);

		StartGame();
	}
}

public enum GameState {
	GameLoaded,
	Initiating,
	Paused,
	Playing,
	Won,
	Lost,
	Cleanup
}