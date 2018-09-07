using BeardedManStudios.Forge.Networking.Generated;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Unity
{
	public partial class NetworkManager : MonoBehaviour
	{
		public delegate void InstantiateEvent(INetworkBehavior unityGameObject, NetworkObject obj);
		public event InstantiateEvent objectInitialized;
		private BMSByte metadata = new BMSByte();

		public GameObject[] EnemyNetworkingNetworkObject = null;
		public GameObject[] EnemySpawnerNetworkObject = null;
		public GameObject[] LevelManagerNetworkObject = null;
		public GameObject[] OrbNetworkingNetworkObject = null;
		public GameObject[] PlayerNetworkingNetworkObject = null;

		private void SetupObjectCreatedEvent()
		{
			Networker.objectCreated += CaptureObjects;
		}

		private void OnDestroy()
		{
		    if (Networker != null)
				Networker.objectCreated -= CaptureObjects;
		}
		
		private void CaptureObjects(NetworkObject obj)
		{
			if (obj.CreateCode < 0)
				return;
				
			if (obj is EnemyNetworkingNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (EnemyNetworkingNetworkObject.Length > 0 && EnemyNetworkingNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(EnemyNetworkingNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<NetworkBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
			else if (obj is EnemySpawnerNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (EnemySpawnerNetworkObject.Length > 0 && EnemySpawnerNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(EnemySpawnerNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<NetworkBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
			else if (obj is LevelManagerNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (LevelManagerNetworkObject.Length > 0 && LevelManagerNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(LevelManagerNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<NetworkBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
			else if (obj is OrbNetworkingNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (OrbNetworkingNetworkObject.Length > 0 && OrbNetworkingNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(OrbNetworkingNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<NetworkBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
			else if (obj is PlayerNetworkingNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (PlayerNetworkingNetworkObject.Length > 0 && PlayerNetworkingNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(PlayerNetworkingNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<NetworkBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
		}

		private void InitializedObject(INetworkBehavior behavior, NetworkObject obj)
		{
			if (objectInitialized != null)
				objectInitialized(behavior, obj);

			obj.pendingInitialized -= InitializedObject;
		}

		[Obsolete("Use InstantiateEnemyNetworking instead, its shorter and easier to type out ;)")]
		public EnemyNetworkingBehavior InstantiateEnemyNetworkingNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(EnemyNetworkingNetworkObject[index]);
			var netBehavior = go.GetComponent<EnemyNetworkingBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<EnemyNetworkingBehavior>().networkObject = (EnemyNetworkingNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		[Obsolete("Use InstantiateEnemySpawner instead, its shorter and easier to type out ;)")]
		public EnemySpawnerBehavior InstantiateEnemySpawnerNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(EnemySpawnerNetworkObject[index]);
			var netBehavior = go.GetComponent<EnemySpawnerBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<EnemySpawnerBehavior>().networkObject = (EnemySpawnerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		[Obsolete("Use InstantiateLevelManager instead, its shorter and easier to type out ;)")]
		public LevelManagerBehavior InstantiateLevelManagerNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(LevelManagerNetworkObject[index]);
			var netBehavior = go.GetComponent<LevelManagerBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<LevelManagerBehavior>().networkObject = (LevelManagerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		[Obsolete("Use InstantiateOrbNetworking instead, its shorter and easier to type out ;)")]
		public OrbNetworkingBehavior InstantiateOrbNetworkingNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(OrbNetworkingNetworkObject[index]);
			var netBehavior = go.GetComponent<OrbNetworkingBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<OrbNetworkingBehavior>().networkObject = (OrbNetworkingNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		[Obsolete("Use InstantiatePlayerNetworking instead, its shorter and easier to type out ;)")]
		public PlayerNetworkingBehavior InstantiatePlayerNetworkingNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(PlayerNetworkingNetworkObject[index]);
			var netBehavior = go.GetComponent<PlayerNetworkingBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<PlayerNetworkingBehavior>().networkObject = (PlayerNetworkingNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}

		public EnemyNetworkingBehavior InstantiateEnemyNetworking(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(EnemyNetworkingNetworkObject[index]);
			var netBehavior = go.GetComponent<EnemyNetworkingBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					metadata.Clear();
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<EnemyNetworkingBehavior>().networkObject = (EnemyNetworkingNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		public EnemySpawnerBehavior InstantiateEnemySpawner(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(EnemySpawnerNetworkObject[index]);
			var netBehavior = go.GetComponent<EnemySpawnerBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					metadata.Clear();
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<EnemySpawnerBehavior>().networkObject = (EnemySpawnerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		public LevelManagerBehavior InstantiateLevelManager(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(LevelManagerNetworkObject[index]);
			var netBehavior = go.GetComponent<LevelManagerBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					metadata.Clear();
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<LevelManagerBehavior>().networkObject = (LevelManagerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		public OrbNetworkingBehavior InstantiateOrbNetworking(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(OrbNetworkingNetworkObject[index]);
			var netBehavior = go.GetComponent<OrbNetworkingBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					metadata.Clear();
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<OrbNetworkingBehavior>().networkObject = (OrbNetworkingNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		public PlayerNetworkingBehavior InstantiatePlayerNetworking(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(PlayerNetworkingNetworkObject[index]);
			var netBehavior = go.GetComponent<PlayerNetworkingBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					metadata.Clear();
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<PlayerNetworkingBehavior>().networkObject = (PlayerNetworkingNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
	}
}