using UnityEngine;

public class Swatter : MonoBehaviour, IPickup {
	[SerializeField] Sprite defaultSprite;
	[SerializeField] Sprite swattingSprite;
	[SerializeField] float swattingRadius;
	[SerializeField] LayerMask bugLayer;

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
		_isPickedUp = true;
	}

	public void Release() {
		Collider2D collider = Physics2D.OverlapCircle(transform.position, swattingRadius, bugLayer);

		if (collider != null && collider.TryGetComponent(out Bug bug)) {
			bug.Swat();
		}

		_isPickedUp = false;
	}
}
