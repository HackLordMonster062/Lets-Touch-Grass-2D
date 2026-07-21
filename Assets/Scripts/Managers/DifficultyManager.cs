using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DifficultyManager : Singleton<DifficultyManager> {
    [SerializeField] DifficultyData[] difficulties;
    [SerializeField] ObstacleData[] obstacles;

    public int Difficulty { get; private set; }

    Dictionary<ObstacleData, float> _timers;

    protected override void Awake() {
        base.Awake();

        _timers = new();

        foreach (ObstacleData obstacle in obstacles) {
            obstacle.obstacle.OnExit += () => ResetTimer(obstacle);
        }
    }

	private void Start() {
        Grass.instance.OnGrowthStageChanged += UpdateDifficulty;
	}

	void Update() {
        foreach (var (obstacle, time) in _timers.ToArray()) {
            if (Time.time >= time) {
                obstacle.obstacle.Enter();
				_timers.Remove(obstacle);
			}
        }
    }

    void ResetTimer(ObstacleData obstacle) {
        ObstacleDifficultyData data = difficulties[Difficulty].GetByID(obstacle.id);

        if (data.isPresent)
            _timers[obstacle] = Time.time + Random.Range(data.cooldownMin, data.cooldownMax);
        else if (_timers.ContainsKey(obstacle))
            _timers.Remove(obstacle);
    }

    void UpdateDifficulty(int difficulty) {
        Difficulty = difficulty;

        foreach (ObstacleDifficultyData data in difficulties[difficulty].obstacles) {
            if (data.isPresent) {
                ObstacleData obstacle = GetObstacleByID(data.id);

				_timers[obstacle] = Time.time + Random.Range(obstacle.entranceDelayMin, obstacle.entranceDelayMax);
			}
        }
    }

    ObstacleData GetObstacleByID(string id) {
		return obstacles.First(obs => obs.id == id);
	}
}

[Serializable]
struct DifficultyData {
    public ObstacleDifficultyData[] obstacles;

    public ObstacleDifficultyData GetByID(string id) {
        return obstacles.First(obs => obs.id == id);
    }
}

[Serializable]
struct ObstacleDifficultyData {
    public string id;
    public bool isPresent;
    public float cooldownMin;
    public float cooldownMax;
}

[Serializable]
struct ObstacleData {
    public string id;
    public Obstacle obstacle;
    public float entranceDelayMin;
    public float entranceDelayMax;
}