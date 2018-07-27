using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour {

    [SerializeField] private Text _UpgradeValue;
    [SerializeField] private Text _UpgradeCost;
    [SerializeField] private Button _UpgradeButton;
    [SerializeField] private WorldSettings.Stats _Stat;

    private void Start()
    {
        UpdateText();
    }

    public void UpgradeStats()
    {
        if (WorldSettings.StatDictionary[_Stat].CanBuy() && !WorldSettings.StatDictionary[_Stat].IsMaxLevel())
        {
            WorldSettings.UsePoints(WorldSettings.StatDictionary[_Stat].Upgrade());
            UpdateText();
            if (WorldSettings.StatDictionary[_Stat].IsMaxLevel())
                DeactivateButton();
        }
    }

    private void UpdateText()
    {
        _UpgradeValue.text = WorldSettings.StatDictionary[_Stat].Value;
        _UpgradeCost.text = WorldSettings.StatDictionary[_Stat].Cost.ToString("0 Ps");
    }

    private void DeactivateButton()
    {
        _UpgradeButton.enabled = false;
    }
}
