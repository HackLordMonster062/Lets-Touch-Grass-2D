using System;
using System.Collections;
using UnityEngine;

public class Pipe : Obstacle {
    [SerializeField] Sprite fixedSprite;
    [SerializeField] Sprite rippedSprite;
    [SerializeField] Tape tape;
    [SerializeField] GameObject droplet;
    [SerializeField] Transform drippingPoint;
    [SerializeField] Transform holePoint;
    [SerializeField] float drippingPace;
    [SerializeField] float dryingDelay;
    [SerializeField] float dryingDamage;

    SpriteRenderer _renderer;

    float _breakingTime;
    bool _isBroken;

	private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sprite = fixedSprite;

		GameManager.OnAfterStateChange += Initiate;
	}

	private void OnDestroy() {
		GameManager.OnAfterStateChange -= Initiate;
	}

	void Initiate(GameState state) {
		if (state != GameState.Initiating) return;

		StopAllCoroutines();
		StartCoroutine(StartDripping(drippingPoint.position));
		_renderer.sprite = fixedSprite;
		_isBroken = false;
	}

	private void Update() {
		if (GameManager.instance.State != GameState.Playing) return;

		if (_isBroken && _breakingTime + dryingDelay <= Time.time) {
            Grass.instance.Damage(dryingDamage * Time.deltaTime);
        }
	}

    public override void Enter(bool isIntroduced) {
        StopAllCoroutines();
		StartCoroutine(StartDripping(holePoint.position));
		_renderer.sprite = rippedSprite;
        AudioManager.instance.PlaySound("HoseBreak");

        _breakingTime = Time.time;
        _isBroken = true;

        if (isIntroduced) {
            tape.Highlight();
        }
	}

    public override void Exit() {
        base.Exit();

        StopAllCoroutines();
		StartCoroutine(StartDripping(drippingPoint.position));
		_renderer.sprite = fixedSprite;

        _isBroken = false;
	}

    IEnumerator StartDripping(Vector3 point) {
        while (true) {
            Instantiate(droplet, point, Quaternion.identity);

            yield return new WaitForSeconds(drippingPace);
        }
    }
}
