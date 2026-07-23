using UnityEngine;

public class Blanket : MonoBehaviour, IPickup {
	[SerializeField] GameObject releasedSprite;
	[SerializeField] GameObject pickedSprite;

	Vector3 _startPosition;

	bool _isPickedUp;

	private void Awake() {
		_startPosition = transform.position;
	}

	private void Update() {
		if (_isPickedUp) return;

		transform.position = Vector3.Lerp(transform.position, _startPosition, Time.deltaTime * 10);
	}

	public bool Pickup() {
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
