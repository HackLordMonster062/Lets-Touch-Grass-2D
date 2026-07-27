using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

	public GameState State { get; private set; }
	public float Timer { get; private set; }

	public static event Action<GameState> OnBeforeStateChange;
	public static event Action<GameState> OnAfterStateChange;

	InputMap _input;

	protected override void Awake() {
		base.Awake();

		_input = new();
		_input.UI.Enable();
	}

	void Start() {
		ChangeState(GameState.Initiating);

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
	}

	public void Lose() {
		ChangeState(GameState.Lost);
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
				Time.timeScale = 0;
				UIManager.instance.Lose(Timer);

				break;
			case GameState.Won:
				Time.timeScale = 0;
				UIManager.instance.Win(Timer);

				break;
		}

		OnAfterStateChange?.Invoke(newState);
	}

	public void OnCancel(InputValue value) {
		TogglePause();
	}

	public void Retry() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}

public enum GameState {
	Initiating,
	Paused,
	Playing,
	Won,
	Lost
}