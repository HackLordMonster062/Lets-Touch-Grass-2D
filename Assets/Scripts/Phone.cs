using System;
using System.Collections;
using UnityEngine;

public class Phone : Obstacle {
    [Tooltip("[seconds until full screen cover")]
    [SerializeField] float growthPace;
    [SerializeField] Vector3 maxPos;
    [SerializeField] Vector3 maxScale;

    Vector3 _startPos;
    Vector3 _startScale;

    Vector3 _targetPos;
    Vector3 _targetScale;

	private void Awake() {
        _startPos = transform.position;
        _startScale = transform.localScale;
	}

	public override void Enter() {
        _targetPos = maxPos;
        _targetScale = maxScale;

        StartCoroutine(LerpToScale());
    }

    public override void Exit() {
        base.Exit();

        _targetPos = _startPos;
        _targetScale = _startScale;
	}

    IEnumerator LerpToScale() {
        float progress = 0;

        while (progress < 1) {
            transform.localScale = Vector3.Lerp(_startScale, _targetScale, progress);
            transform.position = Vector3.Lerp(_startPos, _targetPos, progress);

            progress += Time.deltaTime / growthPace;

            yield return null;
        }
	}
}
