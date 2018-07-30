using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void Start()
    {
        Debug.Log(GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalDataHandler>());
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
