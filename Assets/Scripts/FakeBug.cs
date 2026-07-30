using UnityEngine;
using UnityEngine.UIElements;

public class FakeBug : MonoBehaviour {
	[SerializeField] float flightSpeed;
	[SerializeField] float rotationSpeed;
	[SerializeField] float rotationRadius;
	[SerializeField] Vector3 minTarget;
	[SerializeField] Vector3 maxTarget;

	Vector3 _target;
	Vector3 _position;
	Vector3 _offset;

	private void Start() {
		_position = transform.position;
		_target = _position;
		_offset = new Vector3(rotationRadius, 0, 0);
	}

	void Update() {
		Vector3 dir = _target - _position;

		_position += flightSpeed * Time.deltaTime * dir.normalized;

		_offset = Rotate2D(_offset, rotationSpeed * Time.deltaTime);

		transform.position = _position + _offset;

		if (Vector3.SqrMagnitude(dir) <= .1f) {
			_target = new Vector3(Random.Range(minTarget.x, maxTarget.x), Random.Range(minTarget.y, maxTarget.y), 1);
		}
	}

	static Vector3 Rotate2D(Vector3 vec, float angle) {
		return new Vector3(vec.x * Mathf.Cos(angle) - vec.y * Mathf.Sin(angle), vec.x * Mathf.Sin(angle) + vec.y * Mathf.Cos(angle), 0);
	}
}
