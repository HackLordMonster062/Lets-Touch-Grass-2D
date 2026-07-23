using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Bug : Obstacle {
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite splatSprite;
    [SerializeField] float flightSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float randomRotationAmount;
    [SerializeField] float rotationRadius;
    [SerializeField] float arrivalDistance;
    [SerializeField] float damage;

    Vector3 _startPosition;

    Vector3 _position;
    Vector3 _offset;

    SpriteRenderer _renderer;

    bool _isAlive = true;

	void Awake() {
        _startPosition = transform.position;

        gameObject.SetActive(false);

        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sprite = defaultSprite;
    }

    void Update() {
        if (!_isAlive) return;

        Vector3 dir = Grass.instance.transform.position - _position;

        if (dir.sqrMagnitude <= arrivalDistance * arrivalDistance) {
            Grass.instance.Damage(damage * Time.deltaTime);

            return;
        }

		_position += flightSpeed * Time.deltaTime * dir.normalized;

        if (dir.magnitude <= rotationRadius + arrivalDistance) {
            _offset = _offset.normalized * (dir.magnitude - arrivalDistance);
        }

        _offset = Rotate2D(_offset, rotationSpeed * Time.deltaTime);

        transform.position = _position + _offset;
    }

    static Vector3 Rotate2D(Vector3 vec, float angle) {
        return new Vector3(vec.x * Mathf.Cos(angle) - vec.y * Mathf.Sin(angle), vec.x * Mathf.Sin(angle) + vec.y * Mathf.Cos(angle), 0);
    }

    public override void Enter() {
        _isAlive = true;

		_offset = new Vector3(rotationRadius, 0, 0);
		_position = _startPosition;
		transform.position = _position + _offset;

        _renderer.sprite = defaultSprite;

		gameObject.SetActive(true);

        AudioManager.instance.PlaySoundPersistent("Buzz");
	}

	public override void Exit() {
        _isAlive = false;

        StartCoroutine(Splat());
    }

    IEnumerator Splat() {
        _renderer.sprite = splatSprite;
		AudioManager.instance.PlaySound("Splat");
		AudioManager.instance.StopSound("Buzz");

        yield return new WaitForSeconds(.5f);

		gameObject.SetActive(false);

		base.Exit();
	}
}
