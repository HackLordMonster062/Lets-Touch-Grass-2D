using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Tape : MonoBehaviour, IPickup {
	[SerializeField] float fixingRadius;
	[SerializeField] LayerMask pipeLayer;

	Vector3 _startPosition;

	bool _isPickedUp;
	bool _isHighlighted;

	SpriteRenderer _renderer;

	private void Awake() {
		_startPosition = transform.position;

		_renderer = GetComponent<SpriteRenderer>();
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
		StartCoroutine(Fix());
	}

	public void Highlight() {
		_isHighlighted = true;
		HighlightManager.instance.StartPulse(_renderer);
	}

	public void StopHighlight() {
		if (!_isHighlighted) return;

		HighlightManager.instance.StopPulse(_renderer);
		_isHighlighted = false;
	}

	IEnumerator Fix() {
		Collider2D collider = Physics2D.OverlapPoint(transform.position, pipeLayer);

		if (collider != null && collider.TryGetComponent(out Pipe pipe)) {
			pipe.Exit();
		}

		AudioManager.instance.PlaySound("Tape");

		yield return new WaitForSeconds(.1f);

		_isPickedUp = false;
	}
}
