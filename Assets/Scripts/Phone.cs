using System.Collections;
using UnityEngine;

public class Phone : MonoBehaviour {
    [SerializeField] float growthRate;
    [SerializeField] Vector3 maxPos;
    [SerializeField] Vector3 maxScale;

    Vector3 _startPos;
    Vector3 _startScale;

    Vector3 _targetPos;
    Vector3 _targetScale;

    void Start() {
        
    }

    void Update() {
        transform.localScale += new Vector3(growthRate, growthRate, growthRate);
        transform.position = Vector3.Lerp(transform.position, Vector3.zero, moveRate);
    }

    public void FillScreen() {
        _targetPos = maxPos;
        _targetScale = maxScale;

        StartCoroutine(LerpToScale());
    }

    public void TurnOff() {
        _targetPos = _startPos;
        _targetScale = _startScale;
    }

    IEnumerator LerpToScale() {
        float progress = 0;

        while (progress < 1) {
            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, progress);
            transform.position = Vector3.Lerp(transform.position, _targetPos, progress);

            progress += growthRate * Time.deltaTime;

            yield return null;
        }
	}
}
