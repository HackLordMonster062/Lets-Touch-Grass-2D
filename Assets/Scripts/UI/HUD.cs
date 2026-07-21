using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
	[SerializeField] Slider health;

	private void Update() {
		health.value = Grass.instance.Health;
	}
}
