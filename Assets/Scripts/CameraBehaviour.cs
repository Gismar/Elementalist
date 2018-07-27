using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    [SerializeField] private Transform _Player;

	void Update () {
        transform.position = new Vector3(_Player.position.x, _Player.position.y, -1);
	}
}
