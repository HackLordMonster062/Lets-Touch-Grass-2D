using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class HighlightManager : Singleton<HighlightManager> {
	[SerializeField] float indicatorPulsingDuration;
	[SerializeField] float pulsingSpeed;
	[SerializeField] float minIntensity;
	[SerializeField] float maxIntensity;
	[SerializeField] GameObject interaction;
	[SerializeField] GameObject caution;

	Dictionary<SpriteRenderer, (Color, IEnumerator)> pulses;
	List<Coroutine> highlights;

	protected override void Awake() {
		base.Awake();

		pulses = new();
		highlights = new();

		GameManager.OnAfterStateChange += CleanUp;
	}

	void CleanUp(GameState state) {
		if (state != GameState.Cleanup) return;

		StopAllCoroutines();

		pulses = new();
		highlights = new();
	}

	void Indicate(GameObject indicator, bool pulse, Vector3 position, float duration, Transform parent = null) {
		GameObject instance = Instantiate(indicator, position, Quaternion.identity, parent);

		Coroutine routine = StartCoroutine(pulse ? PulseIndicator(instance, duration) : BounceIndicator(instance, duration));

		highlights.Add(routine);
	}

	public void HighlightInteraction(Vector3 position, float duration, Transform parent = null) {
		Indicate(interaction, false, position, duration, parent);
	}

	public void HighlightCaution(Vector3 position, float duration, Transform parent = null) {
		Indicate(caution, true, position, duration, parent);
	}

	public void StartPulse(SpriteRenderer renderer) {
		if (pulses.ContainsKey(renderer)) StopCoroutine(pulses[renderer].Item2);

		Color originalColor = renderer.material.color;
		pulses[renderer] = (originalColor, Pulse(renderer, originalColor));

		StartCoroutine(pulses[renderer].Item2);
	}

	public void StopPulse(SpriteRenderer renderer) {
		if (!pulses.ContainsKey(renderer)) return;

		renderer.material.color = pulses[renderer].Item1;

		StopCoroutine(pulses[renderer].Item2);
		pulses.Remove(renderer);
	}

	IEnumerator Pulse(SpriteRenderer renderer, Color originalColor) {
		float t = 0;

		while (true) {
			if (GameManager.instance.State != GameState.Playing) {
				yield return null;
				continue;
			}

			t = (Mathf.Sin(Time.time * pulsingSpeed) + 1) / 2;

			renderer.material.color = originalColor * Mathf.Lerp(minIntensity, maxIntensity, t);

			yield return null;
		}
	}

	IEnumerator PulseIndicator(GameObject indicator, float duration) {
		for (int i = 0; i < duration / indicatorPulsingDuration; i++) {
			indicator.SetActive(true);

			yield return new WaitForSeconds(indicatorPulsingDuration / 2);

			indicator.SetActive(false);

			yield return new WaitForSeconds(indicatorPulsingDuration / 2);
		}

		Destroy(indicator);
	}

	IEnumerator BounceIndicator(GameObject indicator, float duration) {
		for (int i = 0; i < duration / indicatorPulsingDuration; i++) {
			indicator.transform.Translate(new Vector3(0, .5f, 0));

			yield return new WaitForSeconds(indicatorPulsingDuration / 2);

			indicator.transform.Translate(new Vector3(0, -.5f, 0));

			yield return new WaitForSeconds(indicatorPulsingDuration / 2);
		}

		Destroy(indicator);
	}
}
