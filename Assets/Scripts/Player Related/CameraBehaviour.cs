using UnityEngine;

namespace Elementalist
{
    public class CameraBehaviour : MonoBehaviour
    {
        public Transform Player { get; set; }
        void Update() => transform.position = new Vector3(Player.position.x, Player.position.y, -1f);
    }
}