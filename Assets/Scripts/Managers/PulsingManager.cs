using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingManager : Singleton<PulsingManager> {
	[SerializeField] float pulsingSpeed;
	[SerializeField] float minIntensity;
	[SerializeField] float maxIntensity;

	Dictionary<SpriteRenderer, IEnumerator> pulses;

    public void StartPulse(SpriteRenderer renderer) {
		if (pulses.ContainsKey(renderer)) StopCoroutine(pulses[renderer]);

		pulses[renderer] = Pulse(renderer);

		StartCoroutine(pulses[renderer]);
	}

	public void StopPulse(SpriteRenderer renderer) {
		StopCoroutine(pulses[renderer]);
		pulses.Remove(renderer);
	}

	IEnumerator Pulse(SpriteRenderer renderer) {
		float t = 0;

		Color originalColor = renderer.color;

		while (true) {
			if (GameManager.instance.State != GameState.Playing) {
				yield return null;
				continue;
			}

			t = (Mathf.Sin(Time.time * pulsingSpeed) + 1) / 2;

			renderer.color = originalColor * Mathf.Lerp(minIntensity, maxIntensity, t);
		}
	}
}
