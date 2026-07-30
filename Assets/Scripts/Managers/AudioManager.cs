using Playgama;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : PersistentSingleton<AudioManager> {
	[SerializeField] AudioSource musicSource;
	[SerializeField] AudioMixer mixer;
	[SerializeField] List<AudioClip> soundEffects; 

	Dictionary<string, AudioSource> _sfxDict;

	public bool IsSoundOn { get; private set; }
	public float SFXVolume { get; private set; }
	public float MusicVolume { get; private set; }

	public static event Action OnVolumeChanged;

	AudioMixerGroup _masterGroup;

	protected override void Awake() {
		base.Awake();
		if (instance != this) return;

		AudioMixerGroup[] groups = mixer.FindMatchingGroups("Master");
		_masterGroup = groups[0];

		GameManager.OnBeforeStateChange += Initiate;
		Bridge.platform.audioStateChanged += ToggleAllSound;
	}

	private void Initiate(GameState state) {
		if (state != GameState.GameLoaded) return;

		InitializeSFXDictionary();

		Bridge.storage.Get(new List<string>() { "MusicVolume", "SFXVolume", "SoundOn" }, AcceptVolumeData);

		musicSource.Play();
	}

	void AcceptVolumeData(bool hasSucceeded, List<string> values) {
		if (hasSucceeded && !values.Contains(null)) {
			SetVolumeMusic(int.Parse(values[0]) / 100f);
			SetVolumeSFX(int.Parse(values[1]) / 100f);
			ToggleAllSound(bool.Parse(values[2]));
		} else {
			Bridge.storage.Set(new List<string>() { "MusicVolume", "SFXVolume", "SoundOn" }, new() { "100", "100", "true" });
			SetVolumeMusic(1);
			SetVolumeSFX(1);
		}
	}

	private void InitializeSFXDictionary() {
		_sfxDict = new Dictionary<string, AudioSource>();
		foreach (var clip in soundEffects) {
			AudioSource source = new GameObject().AddComponent<AudioSource>();
			source.transform.parent = transform;

			source.loop = false;
			source.outputAudioMixerGroup = _masterGroup;
			source.clip = clip;
			source.playOnAwake = false;

			_sfxDict[clip.name] = source;
		}
	}

	public void PlaySound(string clipName) {
		if (_sfxDict.TryGetValue(clipName, out AudioSource clip)) {
			clip.loop = false;
			clip.Play();
		} else {
			print($"Sound {clipName} not found");
		}
	}

	public void PlaySoundPersistent(string clipName) {
		if (_sfxDict.TryGetValue(clipName, out AudioSource source)) {
			source.loop = true;
			source.Play();
		} else {
			print($"Sound {clipName} not found");
		}
	}

	public void StopSound(string clipName) {
		if (_sfxDict.TryGetValue(clipName, out AudioSource source))
			source.Stop();
		else
			print($"Sound {clipName} not found");
	}

	public void TogglePause(bool pause) {
		AudioListener.pause = pause;
		foreach (var (_, source) in _sfxDict) {
			if (pause) source.Pause();
			else source.UnPause();
		}
	}

	public void ToggleMusic(bool play = true) {
		musicSource.mute = !play;
	}

	public void ToggleSound(bool on = true) {
		foreach (var (_, source) in _sfxDict) {
			source.mute = !on;
		}
	}

	public void ToggleAllSound(bool on) {
		if (!Bridge.platform.isAudioEnabled) on = false;

		ToggleSound(on);
		ToggleMusic(on);

		IsSoundOn = on;
		OnVolumeChanged?.Invoke();

		Bridge.storage.Set("SoundOn", on);
	}

	public void SetVolumeSFX(float sfxVolume) {
		foreach (var (_, source) in _sfxDict) {
			source.volume = sfxVolume;
		}

		SFXVolume = sfxVolume;
		OnVolumeChanged?.Invoke();

		Bridge.storage.Set("SFXVolume", (int)(sfxVolume * 100));
	}

	public void SetVolumeMusic(float musicVolume) {
		musicSource.volume = musicVolume;

		MusicVolume = musicVolume;
		OnVolumeChanged?.Invoke();

		Bridge.storage.Set("SFXVolume", (int)(musicVolume * 100));
	}

	public void FadeOut(float duration) {
		StopAllCoroutines();
		StartCoroutine(FadeMixerGroup("MasterVolume", .05f, duration));
	}

	public void FadeIn(float duration) {
		StopAllCoroutines();
		StartCoroutine(FadeMixerGroup("MasterVolume", 1, duration));
	}

	IEnumerator FadeMixerGroup(string exposedParam, float targetLinear, float duration) {
		if (mixer == null) yield break;

		mixer.GetFloat(exposedParam, out float startDb);
		float targetDb = Mathf.Log10(Mathf.Clamp(targetLinear, 0.0001f, 1f)) * 20f;
		float t = 0f;

		while (t < duration) {
			if (GameManager.instance.State != GameState.Playing) {
				yield return null;
				continue;
			}

			t += Time.deltaTime;

			float db = Mathf.Lerp(startDb, targetDb, t / duration);
			mixer.SetFloat(exposedParam, db);

			yield return null;
		}

		mixer.SetFloat(exposedParam, targetDb);
	}
}
