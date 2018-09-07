using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UpgradeShop : MonoBehaviour
    {

        [SerializeField] private Text _pointsText;
        private GlobalDataHandler _globalData;

        void Start() => _globalData = GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalDataHandler>();

        void Update()
        {
            _pointsText.text = _globalData.Points.ToString("0 Ps");
        }

        public void ExitToMainMenu()
        {
            StartCoroutine(LoadMainMenu());
        }

        private IEnumerator LoadMainMenu()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main Menu");
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
