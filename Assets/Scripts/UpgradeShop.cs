using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpgradeShop : MonoBehaviour {

    [SerializeField] private Text _Points;
    private GlobalDataHandler _GlobalData;

    void Start() => _GlobalData = GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalDataHandler>();

	void Update () {
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
