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

		SceneManager.sceneLoaded += (scene, loadMode) => { if (scene.name == "MainView") StartGame(); };
	}

	public void StartGame() {
		ChangeState(GameState.Initiating);

		Timer = 0;

		PlayerPrefs.SetFloat("MusicVolume", 1);
		PlayerPrefs.SetFloat("SFXVolume", 1);
		PlayerPrefs.SetInt("SoundOn", 1);

		Grass.instance.OnTouched += Win;
		Grass.instance.OnDied += Lose;

		ChangeState(GameState.Playing);
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
		} else if (pause && State == GameState.Playing) {
			ChangeState(GameState.Paused);
		}
	}

	public void TogglePause() {
		TogglePause(GameState.Paused != State);
	}

	public void Win() {
		ChangeState(GameState.Won);
		ChangeState(GameState.Cleanup);
	}

	public void Lose() {
		ChangeState(GameState.Lost);
		ChangeState(GameState.Cleanup);
	}

	public void ChangeState(GameState newState) {
		OnBeforeStateChange?.Invoke(newState);

		State = newState;
		switch (newState) {
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
	Initiating,
	Paused,
	Playing,
	Won,
	Lost,
	Cleanup
}