using System;
using System.Collections;
using UnityEngine;

public class Pipe : Obstacle {
    [SerializeField] Sprite fixedSprite;
    [SerializeField] Sprite rippedSprite;
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
	}

	private void Update() {
		if (_isBroken && _breakingTime + dryingDelay <= Time.time) {
            Grass.instance.Damage(dryingDamage * Time.deltaTime);
        }
	}

	void Start() {
        StartCoroutine(StartDripping(drippingPoint.position));
	}

    public override void Enter() {
        StopAllCoroutines();
		StartCoroutine(StartDripping(holePoint.position));
		_renderer.sprite = rippedSprite;

        _breakingTime = Time.time;
        _isBroken = true;
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
