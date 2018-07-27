using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] private float _SpawnTimer;
    [SerializeField] private float _SpawnDelay;
    [SerializeField] private Transform _Player;
    [SerializeField] private GameObject _EnemyPrefab;
    [SerializeField] private Tilemap _Map;
    [SerializeField] private float _Multiplier;
    [SerializeField] private float _MultiplierTimer;
    // Use this for initialization
    void Start () {
        _Map.CompressBounds();
        _SpawnTimer = Time.timeSinceLevelLoad + 1;
        _MultiplierTimer = Time.timeSinceLevelLoad + 30f;
        _Multiplier = 1f;
	}
	
	// Update is called once per frame
	void Update () {
        if (_SpawnTimer < Time.timeSinceLevelLoad) {
            SpawnEnemy();
            _SpawnDelay = 1 / Mathf.Log10(Time.timeSinceLevelLoad * _Multiplier + 1);
            _SpawnTimer = Time.timeSinceLevelLoad + _SpawnDelay;
        }
        if(_MultiplierTimer <= Time.timeSinceLevelLoad)
        {
            _MultiplierTimer = Time.timeSinceLevelLoad + 30f;
            _Multiplier += 0.25f;
        }
	}

    void SpawnEnemy()
    {
        var temp = Instantiate(_EnemyPrefab, transform);
        var bounds = _Map.localBounds;
        var position = new Vector3(Random.Range(bounds.min.x, bounds.max.x),  Random.Range(bounds.min.y, bounds.max.y),0);
        temp.transform.position = position;
        temp.GetComponent<EnemyBehaviour>().SetUp(Mathf.Log10(Time.timeSinceLevelLoad + 1) * _Multiplier, _Player, Mathf.FloorToInt(Mathf.Log10(Mathf.Pow(Time.timeSinceLevelLoad + 1, 2)) * 10 * _Multiplier));
    }
}
