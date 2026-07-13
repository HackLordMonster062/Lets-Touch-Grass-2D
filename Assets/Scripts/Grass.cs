using UnityEngine;

public class Grass : Singleton<Grass> {
    [SerializeField] float startingHealth;
    [Tooltip("Rate of growth [seconds per stage]")]
    [SerializeField] float growthRate;
    [SerializeField] Sprite[] growthStages;

    Color _finalColor;
    SpriteRenderer _renderer;

    public float Growth { get; private set; }
    [field: SerializeField] public float Health { get; private set; }

    protected override void Awake() {
        base.Awake();

        Health = startingHealth;
        Growth = 0;

        _renderer = GetComponent<SpriteRenderer>();
        _finalColor = _renderer.color;

        _renderer.color = Color.black;
    }

    void Update() {
        Growth += Time.deltaTime / growthRate;

        ShowGrowth();
    }

    void ShowGrowth() {
        _renderer.color = Color.Lerp(Color.black, _finalColor, Mathf.Floor(Growth) / growthStages.Length);
    }

    void Die() {

    }

    public void Damage(float damage) {
        Health -= damage;

        if (Health <= 0) {
            Die();
        }
    }
}
