using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    

    public void PlayGame()
    {
        StartCoroutine(LoadScene("Level 1"));
    }

    public void UpgradeShop()
    {
        StartCoroutine(LoadScene("Upgrade Shop"));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
