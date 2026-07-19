using System;
using UnityEngine;

public class Pipe : Obstacle {
    [SerializeField] ParticleSystem tipDripping;
    [SerializeField] ParticleSystem holeDripping;

	void Start() {
		holeDripping.Stop();
		tipDripping.Play();
	}

    void Update() {
        
    }

    public override void Enter() {
        tipDripping.Stop();
        holeDripping.Play();
    }

    public override void Exit() {
        base.Exit();

        holeDripping.Stop();
		tipDripping.Play();
	}
}
