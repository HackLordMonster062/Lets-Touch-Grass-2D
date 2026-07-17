using UnityEngine;

public class Pipe : MonoBehaviour {
    [SerializeField] ParticleSystem tipDripping;
    [SerializeField] ParticleSystem holeDripping;

    void Start() {
        Fix();
    }

    void Update() {
        
    }

    public void Break() {
        tipDripping.Stop();
        holeDripping.Play();
    }

    public void Fix() {
        holeDripping.Stop();
		tipDripping.Play();
	}
}
