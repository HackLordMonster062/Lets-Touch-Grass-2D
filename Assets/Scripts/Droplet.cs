using UnityEngine;

public class Droplet : MonoBehaviour {
	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.CompareTag("Grass")) {
			AudioManager.instance.PlaySound("DripGrass");
		}
		if (collision.collider.CompareTag("Bucket")) {
			AudioManager.instance.PlaySound("DripBucket");
		}

		Destroy(gameObject);
	}
}
