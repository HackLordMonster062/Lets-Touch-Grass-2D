using UnityEngine;

public class PauseMenu : MonoBehaviour {
	bool _isMuted;

	private void Start() {
		_isMuted = PlayerPrefs.GetInt("SoundOn") == 0;
	}

	public void ChangeSfxVolume(float newValue) {
		AudioManager.instance.SetVolumeSFX(newValue);
	}
	
	public void ChangeMusicVolume(float newValue) {
		AudioManager.instance.SetVolumeMusic(newValue);
	}

	public void ToggleMute() {
		_isMuted = !_isMuted;
		AudioManager.instance.ToggleMusic(_isMuted);
	}
}
