using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelUI : MonoBehaviour {

    [SerializeField] private PlayerMovement _Player;
    [SerializeField] private Text _Health;
    [SerializeField] private Text _Timer;
    [SerializeField] private Text _Points;
    [SerializeField] private float _PointTimer;
    [SerializeField] private GameObject _GameOver;
    [SerializeField] private Text _GameOverTimer;
    private GlobalDataHandler _GlobalData;
    private bool _Dead = false;

    private void Start() => _GlobalData = GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalDataHandler>();

    void Update () {
        if (_Dead) return;
        if(_Player._CurrentHealth == 0)
        {
            if(Time.timeSinceLevelLoad > _GlobalData.HighestTime)
            {
                _GlobalData.HighestTime = Mathf.FloorToInt(Time.timeSinceLevelLoad);
            }
            _GameOver.SetActive(true);
            _GameOverTimer.text = FormatTime();
            _Dead = true;
        }
        _Points.text = _GlobalData.Points.ToString("0 Ps");
        _Timer.text = FormatTime();
        _Health.text = _Player._CurrentHealth.ToString("HP: 0");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeSinceLevelLoad > _GlobalData.HighestTime)
            {
                _GlobalData.HighestTime = Mathf.FloorToInt(Time.timeSinceLevelLoad);
            }
            GoToMainMenu();
        }
        if (_PointTimer < Time.time)
        {
            _GlobalData.Points += 1;
            _PointTimer = Time.time + 1;
        }
	}

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private string FormatTime()
    {
        var s = Mathf.FloorToInt(Time.timeSinceLevelLoad % 60);
        var m = Mathf.FloorToInt(Time.timeSinceLevelLoad % 3600 / 60);
        return $"{m}m {s}s";
    }
}
