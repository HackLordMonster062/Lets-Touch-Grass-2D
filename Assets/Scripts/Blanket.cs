using UnityEngine;

public class Blanket : MonoBehaviour, IPickup {
	[SerializeField] Vector3 releasedSize;
	[SerializeField] Vector3 pickedSize;
	[SerializeField] Sprite releasedSprite;
	[SerializeField] Sprite pickedSprite;

	Vector3 _startPosition;

	bool _isPickedUp;

	private void Awake() {
		_startPosition = transform.position;
	}

	private void Update() {
		if (_isPickedUp) return;

		transform.position = Vector3.Lerp(transform.position, _startPosition, Time.deltaTime * 10);
	}

	public void Pickup() {
		transform.localScale = pickedSize;
		_isPickedUp = true;

		AudioManager.instance.PlaySound("BlanketPickUp");
	}

	public void Release() {
		transform.localScale = releasedSize;
		_isPickedUp = false;

		AudioManager.instance.PlaySound("BlanketRelease");
	}
}
