using System;
using UnityEngine;

public class Grass : Singleton<Grass> {
    [SerializeField] float startingHealth;
    [Tooltip("Rate of growth [seconds per stage]")]
    [SerializeField] float growthPace;
    [Tooltip("Rate of regeneration [HP per second]")]
    [SerializeField] float regenerationRate;
    [SerializeField] Sprite[] growthStages;

    SpriteRenderer _renderer;

    int _nextGrowth;
    bool _wasDamaged;
    bool _isFullyGrown;

	public float Growth { get; private set; }
    [field: SerializeField] public float Health { get; private set; }

    public event Action<int> OnGrowthStageChanged;
    public event Action OnTouched;
    public event Action OnDied;

    protected override void Awake() {
        base.Awake();

        _renderer = GetComponent<SpriteRenderer>();

        GameManager.OnAfterStateChange += Initiate;
    }

    void Initiate(GameState state) {
        if (state != GameState.Initiating) return;

		Health = startingHealth;
		Growth = 0;
        _nextGrowth = 0;
        _isFullyGrown = false;
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

        if (Growth >= growthStages.Length - 1 && !_isFullyGrown) {
            FullyGrown();
        }

        _wasDamaged = false;
    }

    void ShowGrowth() {
        _renderer.sprite = growthStages[Mathf.Clamp((int)Growth, 0, growthStages.Length - 1)];
    }

    void FullyGrown() {
        _isFullyGrown = true;
        PulsingManager.instance.StartPulse(_renderer);
    }

	private void OnMouseDown() {
		if (_isFullyGrown) {
            OnTouched?.Invoke();
        }
	}

	public void Damage(float damage) {
        _wasDamaged = true;

        Health -= damage;

        if (Health <= 0) {
            OnDied?.Invoke();
        }
    }
}
