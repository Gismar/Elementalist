using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

namespace Elementalist
{
    public class LevelManager : MonoBehaviourPunCallbacks
    {
        public static LevelManager Instance;

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Lobby");
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            Debug.Log($"Player {newPlayer.NickName} entered");

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log($"Player is Master : {PhotonNetwork.IsMasterClient}");
                LoadLevel();
            }
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            Debug.Log($"Player {otherPlayer.NickName} has left");

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log($"Player is Master : {PhotonNetwork.IsMasterClient}");
                LoadLevel();
            }
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        private void LoadLevel()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("Must be master client to load level");
            }
            Debug.Log($"Loading Level : {PhotonNetwork.CurrentRoom.PlayerCount}");
            PhotonNetwork.LoadLevel($"Room For {PhotonNetwork.CurrentRoom.PlayerCount}");
        }
    }
}