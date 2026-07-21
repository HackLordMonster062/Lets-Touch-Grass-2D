using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour {
    [SerializeField] Vector3 target;
    [SerializeField] float arcHeight;
    [SerializeField] float shootingSpeed;
    [SerializeField] float shootingTorque;
    [SerializeField] float damage;

    Rigidbody2D _rb;

    void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject == Grass.instance.gameObject) {
			Grass.instance.Damage(50);
		}
	}

	private void OnMouseDown() {
		Shoot(target);
	}

	public void Shoot(Vector3 target) {
		Vector3 delta = target - transform.position;

		float y = delta.y;

		delta.y = 0;
		float x = delta.magnitude;

		float g = Mathf.Abs(Physics2D.gravity.y);

		float vY = Mathf.Sqrt(2 * g * Mathf.Max(y + arcHeight, arcHeight));
		float tUp = vY / g;

		float tDown = Mathf.Sqrt(2 * Mathf.Max(arcHeight - y, arcHeight) / g);

		Vector3 vX = delta.normalized * (x / (tUp + tDown));

		_rb.linearVelocity = vX + Vector3.up * vY;

		_rb.AddTorque(Random.Range(-shootingTorque, shootingTorque));

		AudioManager.instance.PlaySound("Hit");
	}
}
