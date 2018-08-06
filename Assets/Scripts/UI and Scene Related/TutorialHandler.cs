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
        _inputs[0].text = _globalData.Up.ToString();
        _inputs[1].text = _globalData.Down.ToString();
        _inputs[2].text = _globalData.Left.ToString();
        _inputs[3].text = _globalData.Right.ToString();
        _inputs[4].text = _globalData.Swap.ToString();
    }

    public void ChangeUpKey()
    {
        _inputs[0].text = _inputs[0].text.ToUpper();
        _globalData.Up = (KeyCode)System.Enum.Parse(typeof(KeyCode), _inputs[0].text);
    }
    public void ChangeDownKey()
    {
        _inputs[1].text = _inputs[1].text.ToUpper();
        _globalData.Down = (KeyCode)System.Enum.Parse(typeof(KeyCode), _inputs[1].text);
    }
    public void ChangeLeftKey()
    {
        _inputs[2].text = _inputs[2].text.ToUpper();
        _globalData.Left = (KeyCode)System.Enum.Parse(typeof(KeyCode), _inputs[2].text);
    }
    public void ChangeRightKey()
    {
        _inputs[3].text = _inputs[3].text.ToUpper();
        _globalData.Right = (KeyCode)System.Enum.Parse(typeof(KeyCode), _inputs[3].text);
    }
    public void ChangeSwapKey()
    {
        _inputs[4].text = _inputs[4].text.ToUpper();
        _globalData.Swap = (KeyCode)System.Enum.Parse(typeof(KeyCode), _inputs[4].text);
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
