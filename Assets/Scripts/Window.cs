using System;
using System.Collections;
using UnityEngine;

public class Window : Obstacle {
    [SerializeField] Animator sunAnimator;
    [Tooltip("Damage per second while the sun is visible")]
    [SerializeField] float sunDamage;

    public bool IsSunVisible { get; private set; }

    SpriteRenderer _renderer;

	void Start() {
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        Collider2D collider = Physics2D.OverlapPoint(transform.position);

		if (IsSunVisible && (collider == null || !collider.TryGetComponent(out Blanket blanket))) {
            Grass.instance.Damage(sunDamage * Time.deltaTime);
        }
    }

    public override void Enter() {
		sunAnimator.SetTrigger("Rise");
		IsSunVisible = true;

        AudioManager.instance.PlaySound("Hum");
	}

    public override void Exit() {
		base.Exit();

		IsSunVisible = false;
	}
}
