using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour {
	Transform _currHolding;

	InputMap _input;

	private void Awake() {
		_input = new();
		_input.Player.Attack.started += OnInteract;
		_input.Player.Attack.canceled += OnInteract;
	}

	private void OnEnable() {
		_input.Player.Enable();
	}

	private void OnDisable() {
		_input.Player.Disable();
	}

	private void Update() {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue()).Modify(z: 1);

		if (_currHolding != null) {
			_currHolding.position = mousePos;
		}
	}

	public void OnInteract(InputAction.CallbackContext context) {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue()).Modify(z: 1);

		if (context.canceled && _currHolding != null) {
			_currHolding.GetComponent<IPickup>().Release();
			_currHolding = null;

			return;
		}

		Collider2D collider = Physics2D.OverlapPoint(mousePos);

		if (collider != null && collider.TryGetComponent(out IPickup pickup)) {
			if (context.started) {
				pickup.Pickup();
				_currHolding = collider.transform;
			}
		}
    }
}
