using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class TouchTest : MonoBehaviour {
	private void OnEnable() {
		EnhancedTouchSupport.Enable();
	}

	private void OnDisable() {
		EnhancedTouchSupport.Disable();
	}

	private void Update() {
		if (Touch.activeTouches.Count > 0) {
			Touch myTouch = Touch.activeTouches[0];

			if (myTouch.phase == TouchPhase.Began) {
				Debug.Log($"[EnhancedTouch] BEGAN at {myTouch.screenPosition}");
			} else if (myTouch.phase == TouchPhase.Moved) {
				Debug.Log($"[EnhancedTouch] MOVED to {myTouch.screenPosition}");
			} else if (myTouch.phase == TouchPhase.Ended) {
				Debug.Log($"[EnhancedTouch] ENDED at {myTouch.screenPosition}");
			} else if (myTouch.phase == TouchPhase.Canceled) {
				Debug.Log($"[EnhancedTouch] CANCELED");
			}
		}
	}
}