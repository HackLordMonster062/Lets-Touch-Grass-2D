using System.Collections;
using UnityEngine;

public class Swatter : MonoBehaviour, IPickup {
	[SerializeField] GameObject defaultSprite;
	[SerializeField] GameObject swattingSprite;
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

	public bool Pickup() {
		if (_isPickedUp) return false;

		_isPickedUp = true;

		return true;
	}

	public void Release() {
		defaultSprite.SetActive(false);
		swattingSprite.SetActive(true);

		StartCoroutine(Swat());
	}

	IEnumerator Swat() {
		Collider2D collider = Physics2D.OverlapCircle(transform.position, swattingRadius, bugLayer);

		if (collider != null && collider.TryGetComponent(out Bug bug)) {
			bug.Exit();
		}

		AudioManager.instance.PlaySound("Swat");

		yield return new WaitForSeconds(.1f);

		_isPickedUp = false;

		defaultSprite.SetActive(true);
		swattingSprite.SetActive(false);
	}
}
