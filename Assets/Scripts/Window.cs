using System.Collections;
using UnityEngine;

public class Window : MonoBehaviour {
    [SerializeField] Color dayColor;
    [SerializeField] Color nightColor;
    [SerializeField] Color sunColor;
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Grass.instance.transform.position - transform.position);

		if (IsSunVisible && hit.collider != null && hit.collider.gameObject == Grass.instance.gameObject) {
            Grass.instance.Damage(sunDamage * Time.deltaTime);
        }
    }

    IEnumerator DayNightCycle() {
        while (true) {
            _renderer.color = dayColor;

            yield return new WaitForSeconds(cycleLength / 6);

            _renderer.color = sunColor;
            IsSunVisible = true;

            yield return new WaitForSeconds(cycleLength / 6);

            _renderer.color = dayColor;
            IsSunVisible = false;

            yield return new WaitForSeconds(cycleLength / 6);

            _renderer.color = nightColor;

            yield return new WaitForSeconds(cycleLength / 2);
        }
    }
}
