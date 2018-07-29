using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpgradeShop : MonoBehaviour {

    [SerializeField] private Text _Points;
    [SerializeField] private WorldInformation _WorldInfo;
	void Update () {
        _Points.text = _WorldInfo.Points.ToString("0 Ps");
	}

    public void ExitToMainMenu()
    {
        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator LoadMainMenu()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main Menu");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
