using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Swatter : MonoBehaviour, IPickup {
	[SerializeField] SpriteRenderer restRenderer;
	[SerializeField] GameObject defaultSprite;
	[SerializeField] GameObject swattingSprite;
	[SerializeField] float swattingRadius;
	[SerializeField] LayerMask bugLayer;

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

	public bool Pickup() {
		if (_isPickedUp) return false;

		_isPickedUp = true;

		StopHighlight();

		return true;
	}

	public void Release() {
		defaultSprite.SetActive(false);
		swattingSprite.SetActive(true);

		StartCoroutine(Swat());
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
