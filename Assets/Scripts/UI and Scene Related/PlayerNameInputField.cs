using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Elementalist.UI
{
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        private const string PlayerPrefNameKey = "PlayerName";

        // Use this for initialization
        void Start()
        {
            string defaultName = string.Empty;
            InputField inputField = GetComponent<InputField>();
            if(inputField != null)
            {
                if (PlayerPrefs.HasKey(PlayerPrefNameKey))
                {
                    defaultName = PlayerPrefs.GetString(PlayerPrefNameKey);
                    inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
        }

        public void SetPlayerName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogError("Player name Input Field is empty");
                return;
            }

            PhotonNetwork.NickName = name;

            PlayerPrefs.SetString(PlayerPrefNameKey, name);
        }
    }
}