using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager> {

	public GameState State { get; private set; }

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

		ChangeState(GameState.Playing);
    }

    void Update() {
		if (_input.UI.Cancel.WasPressedThisFrame()) {
			TogglePause();
		}
    }

	public void TogglePause(bool pause) {
		if (!pause) {
			ChangeState(GameState.Playing);
		} else {
			ChangeState(GameState.Paused);
		}

		UIManager.instance.TogglePause(pause);
	}

	public void TogglePause() {
		TogglePause(GameState.Paused != State);
	}

	public void ChangeState(GameState newState) {
		OnBeforeStateChange?.Invoke(newState);

		State = newState;
		switch (newState) {
			case GameState.Initiating:
				break;
			case GameState.Paused:
				Time.timeScale = 0;

				break;
			case GameState.Playing:
				Time.timeScale = 1;

				break;
		}

		OnAfterStateChange?.Invoke(newState);
	}

	public void OnCancel(InputValue value) {
		TogglePause();
	}
}

public enum GameState {
	Initiating,
	Paused,
	Playing
}