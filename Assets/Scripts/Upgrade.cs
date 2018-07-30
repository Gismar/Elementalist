using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour {

    [SerializeField] private Text _UpgradeValue;
    [SerializeField] private Text _UpgradeCost;
    [SerializeField] private Button _UpgradeButton;
    [SerializeField] private GlobalDataHandler.Stats _Stat;
    private GlobalDataHandler _GlobalData;

    private void Start()
    {
        _GlobalData = GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalDataHandler>();
        _GlobalData.StatDictionary[_Stat].SetWorldInfo(_GlobalData);
        UpdateText();
    }

    public void UpgradeStats()
    {
        if (_GlobalData.StatDictionary[_Stat].CanBuy() && !_GlobalData.StatDictionary[_Stat].IsMaxLevel())
        {
            _GlobalData.UsePoints(_GlobalData.StatDictionary[_Stat].Upgrade());
            UpdateText();
            if (_GlobalData.StatDictionary[_Stat].IsMaxLevel())
                DeactivateButton();
        }
    }

    private void UpdateText()
    {
        _UpgradeValue.text = _GlobalData.StatDictionary[_Stat].Format;
        _UpgradeCost.text = _GlobalData.StatDictionary[_Stat].Cost.ToString("0 Ps");
    }

    private void DeactivateButton()
    {
        _UpgradeButton.enabled = false;
    }
}
