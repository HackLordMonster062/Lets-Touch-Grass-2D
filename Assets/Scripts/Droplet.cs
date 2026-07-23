using UnityEngine;

public class Droplet : MonoBehaviour {
    [SerializeField] LayerMask grassMask;
    [SerializeField] LayerMask bucketMask;

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.layer == grassMask) {
			AudioManager.instance.PlaySound("DripGrass");
		}
		if (collision.gameObject.layer == bucketMask) {
			AudioManager.instance.PlaySound("DripBucket");
		}

		Destroy(gameObject);
	}
}
