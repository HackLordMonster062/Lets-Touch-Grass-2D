using System;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour {
	public event Action OnExit;
	public abstract void Enter(bool isIntroduced);
	public virtual void Exit() {
		OnExit?.Invoke();
	}
}
