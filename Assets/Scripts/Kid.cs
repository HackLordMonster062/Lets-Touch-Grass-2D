using UnityEngine;

public class Kid : MonoBehaviour {
    [SerializeField] Ball ball;
    [SerializeField] int hits;

    int _currHits = 0;

    void Start() {
        Show();
    }

    void Update() {
        
    }

	private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == ball.gameObject) {
            _currHits++;

            if (_currHits >= hits) {
                _currHits = 0;

                Hide();

                return;
            }

            ball.Shoot(Grass.instance.transform.position);
        }
	}

    public void Show() {
        gameObject.SetActive(true);
        ball.gameObject.SetActive(true);

        ball.transform.position = transform.position;
    }

    public void Hide() {
		gameObject.SetActive(false);
		ball.gameObject.SetActive(false);
	}
}
