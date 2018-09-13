using UnityEngine;
using UnityEngine.UI;

namespace Elementalist.UI
{
    public class Upgrade : MonoBehaviour
    {

        [SerializeField] private Text _upgradeValueText;
        [SerializeField] private Text _upgradeCostText;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private GlobalDataHandler.Stat _stat;
        private GlobalDataHandler _globalData;

        private void Start()
        {
            _globalData = GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalDataHandler>();
            _globalData.StatDictionary[_stat].SetWorldInfo(_globalData);
            UpdateText();
        }

        public void UpgradeStats()
        {
            if (_globalData.StatDictionary[_stat].CanBuy() && !_globalData.StatDictionary[_stat].IsMaxLevel())
            {
                _globalData.UsePoints(_globalData.StatDictionary[_stat].Upgrade());
                UpdateText();
                if (_globalData.StatDictionary[_stat].IsMaxLevel())
                    DeactivateButton();
            }
        }

        private void UpdateText()
        {
            _upgradeValueText.text = _globalData.StatDictionary[_stat].Format;
            _upgradeCostText.text = _globalData.StatDictionary[_stat].Cost.ToString("0 Ps");
        }

        private void DeactivateButton()
        {
            _upgradeButton.enabled = false;
        }
    }
}
