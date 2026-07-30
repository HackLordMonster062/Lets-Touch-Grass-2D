using UnityEngine;
using UnityEngine.UI;

public class SoundControls : MonoBehaviour {
	[SerializeField] Sprite soundOn;
	[SerializeField] Sprite soundOff;
	[SerializeField] Image muteButton;
	[SerializeField] Slider sfxSlider;
	[SerializeField] Slider musicSlider;

	bool _isMuted;

	private void Awake() {
		sfxSlider.onValueChanged.AddListener(ChangeSfxVolume);
		musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
		AudioManager.OnVolumeChanged += UpdateValues;
	}

	private void OnDestroy() {
		AudioManager.OnVolumeChanged -= UpdateValues;
	}

	public void UpdateValues() {
		SetMute(!AudioManager.instance.IsSoundOn);
		sfxSlider.value = AudioManager.instance.SFXVolume;
		musicSlider.value = AudioManager.instance.MusicVolume;
	}

	public void ChangeSfxVolume(float newValue) {
		AudioManager.instance.SetVolumeSFX(newValue);
	}

	public void ChangeMusicVolume(float newValue) {
		AudioManager.instance.SetVolumeMusic(newValue);
	}

	public void ToggleMute() {
		AudioManager.instance.ToggleAllSound(_isMuted);
	}

	public void SetMute(bool mute) {
		_isMuted = mute;

		muteButton.sprite = mute ? soundOff : soundOn;
	}
}
