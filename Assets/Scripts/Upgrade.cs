using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour {

    [SerializeField] private Text _UpgradeValue;
    [SerializeField] private Text _UpgradeCost;
    [SerializeField] private Button _UpgradeButton;
    [SerializeField] private WorldInformation _WorldInfo;
    [SerializeField] private WorldInformation.Stats _Stat;

    private void Start()
    {
        _WorldInfo.StatDictionary[_Stat].SetWorldInfo(_WorldInfo);
        UpdateText();
    }

    public void UpgradeStats()
    {
        if (_WorldInfo.StatDictionary[_Stat].CanBuy() && !_WorldInfo.StatDictionary[_Stat].IsMaxLevel())
        {
            _WorldInfo.UsePoints(_WorldInfo.StatDictionary[_Stat].Upgrade());
            UpdateText();
            if (_WorldInfo.StatDictionary[_Stat].IsMaxLevel())
                DeactivateButton();
        }
    }

    private void UpdateText()
    {
        _UpgradeValue.text = _WorldInfo.StatDictionary[_Stat].Format;
        _UpgradeCost.text = _WorldInfo.StatDictionary[_Stat].Cost.ToString("0 Ps");
    }

    private void DeactivateButton()
    {
        _UpgradeButton.enabled = false;
    }
}
