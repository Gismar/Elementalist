using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelUI : MonoBehaviour {

    [SerializeField] private PlayerMovement _player;
    [SerializeField] private Text _health;
    [SerializeField] private Text _timer;
    [SerializeField] private Text _points;
    private float _pointTimer;
    private GlobalDataHandler _globalData;

    private void Start() => _globalData = GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalDataHandler>();

    void Update () {
        if(_player._currentHealth == 0)
        {
            _globalData.SurvivalTime = Mathf.FloorToInt(Time.timeSinceLevelLoad);
            ChangeScenes("Game Over");
        }
        _points.text = _globalData.Points.ToString("0 Ps");
        _timer.text = FormatTime();
        _health.text = _player._currentHealth.ToString("HP: 0");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _globalData.CheckScore();
            ChangeScenes("Main Menu");
        }
        if (_pointTimer < Time.time)
        {
            _globalData.Points += 1;
            _pointTimer = Time.time + 1;
        }
	}

    public void ChangeScenes(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private string FormatTime()
    {
        var s = Mathf.FloorToInt(Time.timeSinceLevelLoad % 60);
        var m = Mathf.FloorToInt(Time.timeSinceLevelLoad % 3600 / 60);
        return $"{m}m {s}s";
    }
}
