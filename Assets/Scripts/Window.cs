using System;
using System.Collections;
using UnityEngine;

public class Window : Obstacle {
    [SerializeField] Animator sunAnimator;
    [SerializeField] Blanket blanket;
    [Tooltip("Damage per second while the sun is visible")]
    [SerializeField] float sunDamage;

    public bool IsSunVisible { get; private set; }

	private void Awake() {
		GameManager.OnAfterStateChange += Initiate;

		sunAnimator.StopPlayback();
	}

	private void OnDestroy() {
		GameManager.OnAfterStateChange -= Initiate;
	}

	void Initiate(GameState state) {
		if (state != GameState.Initiating) return;

		sunAnimator.Rebind();
		sunAnimator.Update(0);
		IsSunVisible = false;
	}

	void Update() {
		if (GameManager.instance.State != GameState.Playing) return;

		Collider2D collider = Physics2D.OverlapPoint(transform.position);

        Blanket _blanket = collider?.GetComponentInParent<Blanket>();

		if (IsSunVisible && (collider == null || _blanket != blanket)) {
            Grass.instance.Damage(sunDamage * Time.deltaTime);
        }
    }

    public override void Enter(bool isIntroduced) {
		sunAnimator.SetTrigger("Rise");

        AudioManager.instance.PlaySound("Hum");

		if (isIntroduced) {
			blanket.Highlight();

			HighlightManager.instance.HighlightCaution(transform.position + new Vector3(0, .5f, 0), 4);
			HighlightManager.instance.HighlightInteraction(blanket.transform.position + new Vector3(0, .5f, 0), 3);
		}
	}

    public override void Exit() {
		base.Exit();

		IsSunVisible = false;
	}

    public void SunVisible() {
        IsSunVisible = true;
    }

    public void SunHidden() {
        IsSunVisible = false;
		blanket.StopHighlight();
	}
}
