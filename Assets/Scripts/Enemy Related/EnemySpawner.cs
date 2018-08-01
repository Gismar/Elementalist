using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] private Transform _Player;
    [SerializeField] private GameObject[] _EnemyPrefab;
    [SerializeField] private Tilemap _Map;
    private GlobalDataHandler _GlobalData;
    private float _SpawnTimer;
    private float _SpawnDelay;
    private float _Multiplier;
    private float _MultiplierTimer;
    private float _Time;

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
            var count = 0;
            foreach(GameObject enemy in _EnemyPrefab)
            {
                if (Time.timeSinceLevelLoad > enemy.GetComponent<IEnemy>().EnemyInfo.StartingSpawnTime) count++;
            }
            SpawnEnemy(Random.Range(0, count));
            _SpawnDelay = 1 / Mathf.Log(Mathf.Pow(_Time + 10, 2), 100f);
            _SpawnTimer = Time.timeSinceLevelLoad + _SpawnDelay;
        }
        if(_MultiplierTimer <= _Time)
        {
            _MultiplierTimer = _Time + 30f;
            _Multiplier += 0.3f;
        }
	}

    void SpawnEnemy(int index)
    {
        var temp = Instantiate(_EnemyPrefab[index], transform);
        var bounds = _Map.localBounds;
        var position = new Vector3(Random.Range(bounds.min.x, bounds.max.x),  Random.Range(bounds.min.y, bounds.max.y),0);
        while(Vector3.Distance(_Player.position, position) <= 5)
        {
            position = new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), 0);
        }

        temp.transform.position = position;
        temp.GetComponent<IEnemy>().Setup(Mathf.Log(_Time + 100, 100F) * _Multiplier, //speed
            _Player, (Mathf.Log10(_Time + 10) * _Multiplier) * 10, //player and health
            Mathf.FloorToInt(_Time/10f) % 35, _GlobalData); //Tier and GlobalData

    }
}
