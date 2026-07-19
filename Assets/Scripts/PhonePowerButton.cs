using System;
using UnityEngine;

public class PhonePowerButton : MonoBehaviour {
    public event Action OnPressed;

	private void OnMouseDown() {
		OnPressed?.Invoke();
	}
}
