using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialHandler : MonoBehaviour {

    [SerializeField] private InputField[] _inputs;
    [SerializeField] private GameObject[] _panels;
    private GlobalDataHandler _globalData;
    private int _panelCount = -1;

    private void Start()
    {
        _globalData = GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalDataHandler>();
        SwitchPanels();
        for (int i = 0; i < _inputs.Length; i++)
        {
            if (i == 4) continue;
            _inputs[i].text = _globalData.Keys[(GlobalDataHandler.Key)i].ToString();
        }
        
    }

    public void ChangeKey(int index)
    {
        _inputs[index].text = _inputs[index].text.ToUpper();
        Debug.Log($"Index: {index}, text{_inputs[index].text}");
        _globalData.Keys[(GlobalDataHandler.Key) index] = (KeyCode)System.Enum.Parse(typeof(KeyCode), _inputs[index].text);
    }

    public void SwitchPanels()
    {
        _panelCount = (_panelCount + 1) == _panels.Length ? 0 : _panelCount + 1;
        for (int i = 0; i < _panels.Length; i++)
        {
            _panels[i].SetActive(i == _panelCount ? true : false);
        }
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
