using UnityEngine;

public class Blanket : MonoBehaviour, IPickup {
	[SerializeField] GameObject releasedSprite;
	[SerializeField] GameObject pickedSprite;
	[SerializeField] SpriteRenderer restRenderer;

	Vector3 _startPosition;

	bool _isPickedUp;
	bool _isHighlighted;

	private void Awake() {
		_startPosition = transform.position;
		GameManager.OnAfterStateChange += Cleanup;
	}

	void Cleanup(GameState state) {
		if (state != GameState.Initiating) return;

		StopHighlight();
	}

	private void Update() {
		if (_isPickedUp) return;

		transform.position = Vector3.Lerp(transform.position, _startPosition, Time.deltaTime * 10);
	}

	public void Highlight() {
		_isHighlighted = true;
		PulsingManager.instance.StartPulse(restRenderer);
	}

	public void StopHighlight() {
		if (!_isHighlighted) return;

		PulsingManager.instance.StopPulse(restRenderer);
		_isHighlighted = false;
	}

	public bool Pickup() {
		if (_isHighlighted) StopHighlight();

		releasedSprite.SetActive(false);
		pickedSprite.SetActive(true);
		_isPickedUp = true;

		AudioManager.instance.PlaySound("BlanketPickUp");

		return true;
	}

	public void Release() {
		releasedSprite.SetActive(true);
		pickedSprite.SetActive(false);
		_isPickedUp = false;

		AudioManager.instance.PlaySound("BlanketRelease");
	}
}
