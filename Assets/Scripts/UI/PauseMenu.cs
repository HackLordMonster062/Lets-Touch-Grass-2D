using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
	[SerializeField] Sprite soundOn;
	[SerializeField] Sprite soundOff;
	[SerializeField] Image muteButton;

	bool _isMuted;

	private void Start() {
		SetMute(PlayerPrefs.GetInt("SoundOn") == 0);
	}

	public void ChangeSfxVolume(float newValue) {
		AudioManager.instance.SetVolumeSFX(newValue);
	}
	
	public void ChangeMusicVolume(float newValue) {
		AudioManager.instance.SetVolumeMusic(newValue);
	}

	public void ToggleMute() {
		SetMute(!_isMuted);
	}

	public void SetMute(bool mute) {
		_isMuted = mute;

		muteButton.sprite = mute ? soundOff : soundOn;

		AudioManager.instance.ToggleMusic(_isMuted);
	}
}
