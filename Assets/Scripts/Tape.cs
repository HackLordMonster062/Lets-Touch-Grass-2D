using System.Collections;
using UnityEngine;

public class Tape : MonoBehaviour, IPickup {
	[SerializeField] float fixingRadius;
	[SerializeField] LayerMask pipeLayer;

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
		StartCoroutine(Fix());
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
