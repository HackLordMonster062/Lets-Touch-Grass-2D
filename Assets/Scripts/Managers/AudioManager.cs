using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager> {
	[SerializeField] AudioSource musicSource;
	[SerializeField] AudioSource sfxSource;
	[SerializeField] AudioMixer mixer;
	[SerializeField] List<AudioClip> soundEffects; 

	Dictionary<string, AudioClip> _sfxDict;
	Dictionary<string, AudioSource> _persistentSfxSources;

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
		_persistentSfxSources = new();

		if (PlayerPrefs.GetInt("SoundOn") == 0) {
			musicSource.mute = true;
			sfxSource.mute = true;
		}

		SetVolumeMusic(PlayerPrefs.GetFloat("MusicVolume"));
		SetVolumeSFX(PlayerPrefs.GetFloat("SFXVolume"));

		musicSource.Play();
	}

	private void InitializeSFXDictionary() {
		_sfxDict = new Dictionary<string, AudioClip>();
		foreach (var clip in soundEffects) {
			_sfxDict[clip.name] = clip;
		}
	}

	public void PlaySound(string clipName) {
		if (_sfxDict.TryGetValue(clipName, out AudioClip clip)) {
			sfxSource.PlayOneShot(clip);
		} else {
			print($"Sound {clipName} not found");
		}
	}

	public void PlaySoundPersistent(string clipName) {
		if (!_persistentSfxSources.TryGetValue(clipName, out AudioSource source)) {
			if (_sfxDict.TryGetValue(clipName, out AudioClip clip)) {
				source = new GameObject().AddComponent<AudioSource>();
				source.transform.parent = transform;

				source.loop = true;
				source.outputAudioMixerGroup = _masterGroup;
				source.clip = clip;
			} else {
				print($"Sound {clipName} not found");

				return;
			}

			_persistentSfxSources[clipName] = source;
		}

		source.Play();
	}

	public void StopSound(string clipName) {
		if (_persistentSfxSources.ContainsKey(clipName))
			_persistentSfxSources[clipName].Stop();
		else
			print($"Sound {clipName} not found");
	}

	public void ToggleMusic(bool play = true) {
		musicSource.mute = !play;
	}

	public void ToggleSound(bool on = true) {
		sfxSource.mute = !on;

		foreach (var (_, source) in _persistentSfxSources) {
			source.mute = !on;
		}
	}

	public void SetVolumeSFX(float sfxVolume) {
		sfxSource.volume = sfxVolume;
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
