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
    private bool _Dead = false;

	void Update () {
        if (_Dead) return;
        if(_Player._CurrentHealth == 0)
        {
            _GameOver.SetActive(true);
            _GameOverTimer.text = FormatTime();
            _Dead = true;
        }
        _Points.text = WorldSettings.Points.ToString("0 Ps");
        _Timer.text = FormatTime();
        _Health.text = _Player._CurrentHealth.ToString("HP: 0");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToMainMenu();
        }
        if (_PointTimer < Time.time)
        {
            WorldSettings.Points += 1;
            _PointTimer = Time.time + 1;
        }
	}

    public void GoToMainMenu()
    {
        StartCoroutine(LoadScene());
    }

    private string FormatTime()
    {
        var s = Mathf.FloorToInt(Time.timeSinceLevelLoad % 60);
        var m = Mathf.FloorToInt(Time.timeSinceLevelLoad % 3600 / 60);
        return $"{m}m {s}s";
    }

    private IEnumerator LoadScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main Menu");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
