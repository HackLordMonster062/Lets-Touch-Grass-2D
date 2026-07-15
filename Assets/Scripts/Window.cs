using System.Collections;
using UnityEngine;

public class Window : MonoBehaviour {
    [SerializeField] Color dayColor;
    [SerializeField] Color nightColor;
    [SerializeField] Color sunColor;
    [SerializeField] Animator sunAnimator;
    [SerializeField] float cycleLength;
    [Tooltip("Damage per second while the sun is visible")]
    [SerializeField] float sunDamage;

    public bool IsSunVisible { get; private set; }

    SpriteRenderer _renderer;

    void Start() {
        _renderer = GetComponent<SpriteRenderer>();

        StartCoroutine(DayNightCycle());
    }

    void Update() {
        Collider2D collider = Physics2D.OverlapPoint(transform.position);

		if (IsSunVisible && (collider == null || !collider.TryGetComponent(out Blanket blanket))) {
            Grass.instance.Damage(sunDamage * Time.deltaTime);
        }
    }

    IEnumerator DayNightCycle() {
        while (true) {
            _renderer.color = dayColor;

            yield return new WaitForSeconds(cycleLength / 6);

            //_renderer.color = sunColor;
            sunAnimator.SetTrigger("Rise");
            IsSunVisible = true;

            yield return new WaitForSeconds(cycleLength / 6);

            //_renderer.color = dayColor;
            IsSunVisible = false;

            yield return new WaitForSeconds(cycleLength / 6);

            _renderer.color = nightColor;

            yield return new WaitForSeconds(cycleLength / 2);
        }
    }
}
