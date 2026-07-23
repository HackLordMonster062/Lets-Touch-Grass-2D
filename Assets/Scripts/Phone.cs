using System;
using System.Collections;
using UnityEngine;

public class Phone : Obstacle {
    [Tooltip("[seconds until full screen cover")]
    [SerializeField] float growthDuration;
    [SerializeField] float shrinkDuration;
    [SerializeField] Vector3 maxPos;
    [SerializeField] Vector3 maxScale;
    [SerializeField] SpriteRenderer fadeOut;
    [SerializeField] SpriteRenderer graphics;
    [SerializeField] float maxOpacity;
    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;

    Vector3 _startPos;
    Vector3 _startScale;

	private void Awake() {
        _startPos = transform.position;
        _startScale = transform.localScale;

        GetComponentInChildren<PhonePowerButton>().OnPressed += Exit;
	}

	public override void Enter() {
        graphics.sprite = onSprite;

        StartCoroutine(LerpToScale());
    }

    public override void Exit() {
        base.Exit();

        graphics.sprite = offSprite;

        StopAllCoroutines();

        StartCoroutine(LerpToStart());
	}

    IEnumerator LerpToScale() {
        float progress = 0;

        AudioManager.instance.FadeOut(growthDuration * 3 / 4);

        while (progress < 1) {
            transform.localScale = Vector3.Lerp(_startScale, maxScale, progress);
            transform.position = Vector3.Lerp(_startPos, maxPos, progress);
            fadeOut.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, maxOpacity), progress);

            progress += Time.deltaTime / growthDuration;

            yield return null;
        }
	}

	IEnumerator LerpToStart() {
		float progress = 0;

        AudioManager.instance.FadeIn(shrinkDuration);

        Vector3 startPos = transform.position;
        Vector3 startScale = transform.localScale;
        Color startColor = fadeOut.color;

		while (progress < 1) {
			transform.localScale = Vector3.Lerp(startScale, _startScale, progress);
			transform.position = Vector3.Lerp(startPos, _startPos, progress);
			fadeOut.color = Color.Lerp(startColor, new Color(0, 0, 0, 0), progress);

			progress += Time.deltaTime / shrinkDuration;

			yield return null;
		}
	}
}
