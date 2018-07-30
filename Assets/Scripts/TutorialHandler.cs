using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialHandler : MonoBehaviour {

    private GlobalDataHandler _GlobalData;
    [SerializeField] private InputField[] _Inputs;

    private void Start()
    {
        _GlobalData = GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalDataHandler>();
        _Inputs[0].text = _GlobalData.Up.ToString();
        _Inputs[1].text = _GlobalData.Down.ToString();
        _Inputs[2].text = _GlobalData.Left.ToString();
        _Inputs[3].text = _GlobalData.Right.ToString();
        _Inputs[4].text = _GlobalData.Swap.ToString();
    }

    public void ChangeUpKey()
    {
        _GlobalData.Up = (KeyCode)System.Enum.Parse(typeof(KeyCode), _Inputs[0].text.ToUpper());
    }
    public void ChangeDownKey()
    {
        _GlobalData.Down = (KeyCode)System.Enum.Parse(typeof(KeyCode), _Inputs[1].text.ToUpper());
    }
    public void ChangeLeftKey()
    {
        _GlobalData.Left = (KeyCode)System.Enum.Parse(typeof(KeyCode), _Inputs[2].text.ToUpper());
    }
    public void ChangeRightKey()
    {
        _GlobalData.Right = (KeyCode)System.Enum.Parse(typeof(KeyCode), _Inputs[3].text.ToUpper());
    }
    public void ChangeSwapKey()
    {
        _GlobalData.Swap = (KeyCode)System.Enum.Parse(typeof(KeyCode), _Inputs[5].text.ToUpper());
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
