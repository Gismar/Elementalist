using UnityEngine;
using UnityEngine.SceneManagement;

namespace Elementalist.UI
{
    public class MainMenu : MonoBehaviour
    {

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}