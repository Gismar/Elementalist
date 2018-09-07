using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;
using Enemy;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;

public class LevelManager : LevelManagerBehavior

{
	void Start () {
        var player = NetworkManager.Instance.InstantiatePlayerNetworking().GetComponent<Player.PlayerMovement>();

        if (networkObject.IsServer)
        {
            NetworkManager.Instance.InstantiateEnemySpawner().GetComponent<EnemySpawner>().Players = new List<Transform>();
        }

        NetworkManager.Instance.EnemySpawnerNetworkObject[0]
            .GetComponent<EnemySpawner>().LookForPlayers();

    }
}
