using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Elementalist.Networking
{
    public class LobbyBehaviour : MonoBehaviourPunCallbacks
    {
        [SerializeField] private byte _maxPlayersPerRoom;
        [SerializeField] private GameObject _controlPanel;
        [SerializeField] private GameObject _connectingPanel;

        private string _gameVersion;
        private bool _isConnecting;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            _controlPanel.SetActive(true);
            _connectingPanel.SetActive(false);
        }

        public void Connect()
        {
            PhotonNetwork.GameVersion = _gameVersion;
            PhotonNetwork.ConnectUsingSettings();
            _controlPanel.SetActive(false);
            _connectingPanel.SetActive(true);
            _isConnecting = true;
        }

        public override void OnConnectedToMaster()
        {
            if (_isConnecting)
            {
                Debug.Log("Lobby Connected");
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarning("Lobby Disconnected");
            _controlPanel.SetActive(true);
            _connectingPanel.SetActive(false);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Joining Random Room Failed. Creating new room");
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = _maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Room Joined from Lobby");
            if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("Loading `Room For 1`");
                PhotonNetwork.LoadLevel("Room For 1");
            }
        }
    }
}