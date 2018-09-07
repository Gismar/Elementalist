using BeardedManStudios.Forge.Networking.Frame;
using System;
using MainThreadManager = BeardedManStudios.Forge.Networking.Unity.MainThreadManager;

namespace BeardedManStudios.Forge.Networking.Generated
{
	public partial class NetworkObjectFactory : NetworkObjectFactoryBase
	{
		public override void NetworkCreateObject(NetWorker networker, int identity, uint id, FrameStream frame, Action<NetworkObject> callback)
		{
			if (networker.IsServer)
			{
				if (frame.Sender != null && frame.Sender != networker.Me)
				{
					if (!ValidateCreateRequest(networker, identity, id, frame))
						return;
				}
			}
			
			bool availableCallback = false;
			NetworkObject obj = null;
			MainThreadManager.Run(() =>
			{
				switch (identity)
				{
					case EnemyNetworkingNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new EnemyNetworkingNetworkObject(networker, id, frame);
						break;
					case EnemySpawnerNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new EnemySpawnerNetworkObject(networker, id, frame);
						break;
					case LevelManagerNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new LevelManagerNetworkObject(networker, id, frame);
						break;
					case OrbNetworkingNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new OrbNetworkingNetworkObject(networker, id, frame);
						break;
					case PlayerNetworkingNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new PlayerNetworkingNetworkObject(networker, id, frame);
						break;
				}

				if (!availableCallback)
					base.NetworkCreateObject(networker, identity, id, frame, callback);
				else if (callback != null)
					callback(obj);
			});
		}

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}