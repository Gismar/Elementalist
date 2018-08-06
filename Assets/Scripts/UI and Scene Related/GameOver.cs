using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
    [SerializeField] private Text _survivedTimeText;

	private GlobalDataHandler _globalData;

	void Start () {
        _globalData = GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalDataHandler>();
        _globalData.CheckScore();
        _survivedTimeText.text = FormatTime(_globalData.SurvivalTime);
    }
	
    public void ChangeScene (string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private string FormatTime(float time)
    {
        var s = Mathf.FloorToInt(time % 60);
        var m = Mathf.FloorToInt(time % 3600 / 60);
        return $"{m}m {s}s";
    }
}
