using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialHandler : MonoBehaviour {

    private GlobalDataHandler _GlobalData;
    [SerializeField] private InputField[] _Inputs;
    [SerializeField] private GameObject[] _Panels;
    private int _PanelCount = -1;

    private void Start()
    {
        _GlobalData = GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalDataHandler>();
        SwitchPanels();
        _Inputs[0].text = _GlobalData.Up.ToString();
        _Inputs[1].text = _GlobalData.Down.ToString();
        _Inputs[2].text = _GlobalData.Left.ToString();
        _Inputs[3].text = _GlobalData.Right.ToString();
        _Inputs[4].text = _GlobalData.Swap.ToString();
    }

    public void ChangeUpKey()
    {
        _Inputs[0].text = _Inputs[0].text.ToUpper();
        _GlobalData.Up = (KeyCode)System.Enum.Parse(typeof(KeyCode), _Inputs[0].text);
    }
    public void ChangeDownKey()
    {
        _Inputs[1].text = _Inputs[1].text.ToUpper();
        _GlobalData.Down = (KeyCode)System.Enum.Parse(typeof(KeyCode), _Inputs[1].text);
    }
    public void ChangeLeftKey()
    {
        _Inputs[2].text = _Inputs[2].text.ToUpper();
        _GlobalData.Left = (KeyCode)System.Enum.Parse(typeof(KeyCode), _Inputs[2].text);
    }
    public void ChangeRightKey()
    {
        _Inputs[3].text = _Inputs[3].text.ToUpper();
        _GlobalData.Right = (KeyCode)System.Enum.Parse(typeof(KeyCode), _Inputs[3].text);
    }
    public void ChangeSwapKey()
    {
        _Inputs[4].text = _Inputs[4].text.ToUpper();
        _GlobalData.Swap = (KeyCode)System.Enum.Parse(typeof(KeyCode), _Inputs[4].text);
    }

    public void SwitchPanels()
    {
        _PanelCount = (_PanelCount + 1) == _Panels.Length ? 0 : _PanelCount + 1;
        for (int i = 0; i < _Panels.Length; i++)
        {
            _Panels[i].SetActive(i == _PanelCount ? true : false);
        }
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
