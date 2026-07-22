using System;
using UnityEngine;

public class Grass : Singleton<Grass> {
    [SerializeField] float startingHealth;
    [Tooltip("Rate of growth [seconds per stage]")]
    [SerializeField] float growthPace;
    [Tooltip("Rate of regeneration [HP per second]")]
    [SerializeField] float regenerationRate;
    [SerializeField] Sprite[] growthStages;

    Color _finalColor;
    SpriteRenderer _renderer;

    int _nextGrowth;
    bool _wasDamaged;
    bool _isFullyGrown;

	public float Growth { get; private set; }
    [field: SerializeField] public float Health { get; private set; }

    public event Action<int> OnGrowthStageChanged;

    protected override void Awake() {
        base.Awake();

        Health = startingHealth;
        Growth = 0;

        _renderer = GetComponent<SpriteRenderer>();
        _finalColor = _renderer.color;

        _renderer.color = Color.black;
    }

    void Update() {
        if (!_wasDamaged && Health < startingHealth) {
            Health += regenerationRate * Time.deltaTime;
            Health = Mathf.Clamp(Health, 0, startingHealth);
        }

        if (!_isFullyGrown) {
            Growth += Time.deltaTime / growthPace * (Health / startingHealth);

            if (Growth >= _nextGrowth)
                OnGrowthStageChanged?.Invoke((int)Growth);

            _nextGrowth = (int)Growth + 1;
        }

        ShowGrowth();

        if (Growth >= growthStages.Length - 1) {
            FullyGrown();
        }

        _wasDamaged = false;
    }

    void ShowGrowth() {
        _renderer.color = Color.Lerp(Color.black, _finalColor, Mathf.Floor(Growth) / growthStages.Length);
        //_renderer.sprite = growthStages[Mathf.Clamp((int)Growth, 0, growthStages.Length - 1)];
    }

    void Die() {

    }

    void FullyGrown() {
        _isFullyGrown = true;
    }

    public void Damage(float damage) {
        _wasDamaged = true;

        Health -= damage;

        if (Health <= 0) {
            Die();
        }
    }
}
