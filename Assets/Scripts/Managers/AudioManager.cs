using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;

public class AudioManager : Singleton<AudioManager> {
	[SerializeField] AudioSource musicSource;
	[SerializeField] AudioMixer mixer;
	[SerializeField] List<AudioClip> soundEffects; 

	Dictionary<string, AudioSource> _sfxDict;

	public bool IsOn { get; private set; }

	AudioMixerGroup _masterGroup;

	protected override void Awake() {
		base.Awake();

		AudioMixerGroup[] groups = mixer.FindMatchingGroups("Master");
		_masterGroup = groups[0];

		GameManager.OnBeforeStateChange += Initiate;
	}

	private void Initiate(GameState state) {
		if (state != GameState.Playing) return;

		InitializeSFXDictionary();

		if (PlayerPrefs.GetInt("SoundOn") == 0) {
			musicSource.mute = true;


		}

		SetVolumeMusic(PlayerPrefs.GetFloat("MusicVolume"));
		SetVolumeSFX(PlayerPrefs.GetFloat("SFXVolume"));

		musicSource.Play();
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
		foreach (var (_, source) in _sfxDict) {
			AudioListener.pause = pause;
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

	public void SetVolumeSFX(float sfxVolume) {
		foreach (var (_, source) in _sfxDict) {
			source.volume = sfxVolume;
		}
	}

	public void SetVolumeMusic(float musicVolume) {
		musicSource.volume = musicVolume;
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
			t += Time.deltaTime;

			float db = Mathf.Lerp(startDb, targetDb, t / duration);
			mixer.SetFloat(exposedParam, db);

			yield return null;
		}

		mixer.SetFloat(exposedParam, targetDb);
	}
}
