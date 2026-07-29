using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
	[SerializeField] Sprite soundOn;
	[SerializeField] Sprite soundOff;
	[SerializeField] Image muteButton;
	[SerializeField] Slider sfxSlider;
	[SerializeField] Slider musicSlider;

	bool _isMuted;

	private void Awake() {
		GameManager.OnBeforeStateChange += UpdateValues;

		sfxSlider.onValueChanged.AddListener(ChangeSfxVolume);
		musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
	}

	private void UpdateValues(GameState state) {
		if (state != GameState.Paused) return;

		SetMute(PlayerPrefs.GetInt("SoundOn") == 0);
		sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
		musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
	}

	public void ChangeSfxVolume(float newValue) {
		AudioManager.instance.SetVolumeSFX(newValue);
	}
	
	public void ChangeMusicVolume(float newValue) {
		AudioManager.instance.SetVolumeMusic(newValue);
	}

	public void Continue() {
		GameManager.instance.TogglePause();
	}

	public void ToggleMute() {
		AudioManager.instance.ToggleSound(!_isMuted);
		AudioManager.instance.ToggleMusic(!_isMuted);

		SetMute(!_isMuted);
	}

	public void SetMute(bool mute) {
		_isMuted = mute;

		muteButton.sprite = mute ? soundOff : soundOn;
	}
}
