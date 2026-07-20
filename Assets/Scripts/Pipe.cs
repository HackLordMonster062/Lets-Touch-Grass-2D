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

    SpriteRenderer _renderer;

	private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sprite = fixedSprite;
	}

	void Start() {
        StartCoroutine(StartDripping(drippingPoint.position));
	}

    public override void Enter() {
        StopAllCoroutines();
		StartCoroutine(StartDripping(holePoint.position));
		_renderer.sprite = rippedSprite;
	}

    public override void Exit() {
        base.Exit();

        StopAllCoroutines();
		StartCoroutine(StartDripping(drippingPoint.position));
		_renderer.sprite = fixedSprite;
	}

    IEnumerator StartDripping(Vector3 point) {
        while (true) {
            Instantiate(droplet, point, Quaternion.identity);

            yield return new WaitForSeconds(drippingPace);
        }
    }
}
