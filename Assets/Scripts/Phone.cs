using System;
using System.Collections;
using UnityEngine;

public class Phone : Obstacle {
    [Tooltip("[seconds until full screen cover")]
    [SerializeField] float growthPace;
    [SerializeField] float shrinkPace;
    [SerializeField] Vector3 maxPos;
    [SerializeField] Vector3 maxScale;

    Vector3 _startPos;
    Vector3 _startScale;

	private void Awake() {
        _startPos = transform.position;
        _startScale = transform.localScale;

        GetComponentInChildren<PhonePowerButton>().OnPressed += Exit;
	}

	public override void Enter() {
        StartCoroutine(LerpToScale());
    }

    public override void Exit() {
        base.Exit();

        StopAllCoroutines();

        StartCoroutine(LerpToStart());
	}

    IEnumerator LerpToScale() {
        float progress = 0;

        while (progress < 1) {
            transform.localScale = Vector3.Lerp(_startScale, maxScale, progress);
            transform.position = Vector3.Lerp(_startPos, maxPos, progress);

            progress += Time.deltaTime / growthPace;

            yield return null;
        }
	}

	IEnumerator LerpToStart() {
		float progress = 0;

        Vector3 startPos = transform.position;
        Vector3 startScale = transform.localScale;

		while (progress < 1) {
			transform.localScale = Vector3.Lerp(startScale, _startScale, progress);
			transform.position = Vector3.Lerp(startPos, _startPos, progress);

			progress += Time.deltaTime / shrinkPace;

			yield return null;
		}
	}
}
