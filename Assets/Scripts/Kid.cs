using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Kid : Obstacle {
    [SerializeField] Ball ball;
    [SerializeField] int hits;

    int _currHits = 0;
    bool _isActive;

    Animator _anim;

	private void Awake() {
        _anim = GetComponent<Animator>();

		gameObject.SetActive(false);
		ball.gameObject.SetActive(false);
	}

	void Start() {
        
    }

    void Update() {
        
    }

	private void OnTriggerEnter2D(Collider2D other) {
        if (_isActive && other.gameObject == ball.gameObject) {
            _currHits++;

            if (_currHits >= hits) {
                _currHits = 0;

                Exit();

                return;
            }

            ball.Shoot(Grass.instance.transform.position);
        }
	}

    public override void Enter() {
        gameObject.SetActive(true);
        ball.gameObject.SetActive(true);

        _anim.SetTrigger("Enter");
    }

    public void Ready() {
        _isActive = true;
        _anim.enabled = false;
		ball.Shoot(Grass.instance.transform.position);
	}

    public override void Exit() {
		base.Exit();

		_isActive = false;
        _anim.enabled = true;
		_anim.SetTrigger("Exit");
	}

    public void Hide() {
		gameObject.SetActive(false);
		ball.gameObject.SetActive(false);
	}
}
