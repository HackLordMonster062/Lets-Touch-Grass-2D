using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Pickup : MonoBehaviour {
	Transform _currHolding;

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
