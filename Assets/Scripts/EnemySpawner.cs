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
    [SerializeField] private float _Time;
    [SerializeField] private Gradient _Color;
    private GlobalDataHandler _GlobalData;

    // Use this for initialization
    void Start () {
        _GlobalData = GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalDataHandler>();
        _Time = 0;
        _Map.CompressBounds();
        _SpawnTimer = _Time + 1;
        _MultiplierTimer = _Time + 30f;
        _Multiplier = 1f;
	}
	
	// Update is called once per frame
	void Update () {
        _Time = Mathf.Floor(Time.timeSinceLevelLoad / 10f) * 10f;
        if (_SpawnTimer < Time.timeSinceLevelLoad) {
            SpawnEnemy();
            _SpawnDelay = 1 / Mathf.Log(Mathf.Pow(_Time + 10, 2), 100f);
            _SpawnTimer = Time.timeSinceLevelLoad + _SpawnDelay;
        }
        if(_MultiplierTimer <= _Time)
        {
            _MultiplierTimer = _Time + 30f;
            _Multiplier += 0.25f;
        }
	}

    void SpawnEnemy()
    {
        var temp = Instantiate(_EnemyPrefab, transform);
        var bounds = _Map.localBounds;
        var position = new Vector3(Random.Range(bounds.min.x, bounds.max.x),  Random.Range(bounds.min.y, bounds.max.y),0);
        while(Vector3.Distance(_Player.position, position) <= 5)
        {
            position = new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), 0);
        }
        temp.transform.position = position;
        temp.GetComponent<EnemyBehaviour>().SetUp(Mathf.Log(_Time + 100, 100F) * _Multiplier,
            _Player, Mathf.FloorToInt(Mathf.Pow(Mathf.Log10(_Time + 10) * _Multiplier + 1f, 2)), 
            _Color.Evaluate(_Time % 100 / 100f), _GlobalData);
    }
}
