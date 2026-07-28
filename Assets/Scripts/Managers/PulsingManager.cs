using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingManager : Singleton<PulsingManager> {
	[SerializeField] float pulsingSpeed;
	[SerializeField] float minIntensity;
	[SerializeField] float maxIntensity;

	Dictionary<SpriteRenderer, (Color, IEnumerator)> pulses;

	protected override void Awake() {
		base.Awake();

		pulses = new();
	}

	public void StartPulse(SpriteRenderer renderer) {
		if (pulses.ContainsKey(renderer)) StopCoroutine(pulses[renderer].Item2);

		Color originalColor = renderer.material.color;
		pulses[renderer] = (originalColor, Pulse(renderer, originalColor));

		StartCoroutine(pulses[renderer].Item2);
	}

	public void StopPulse(SpriteRenderer renderer) {
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
}
