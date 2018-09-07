using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0.1,0.1,0]")]
	public partial class PlayerNetworkingNetworkObject : NetworkObject
	{
		public const int IDENTITY = 5;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		private Vector2 _Position;
		public event FieldEvent<Vector2> PositionChanged;
		public InterpolateVector2 PositionInterpolation = new InterpolateVector2() { LerpT = 0.1f, Enabled = true };
		public Vector2 Position
		{
			get { return _Position; }
			set
			{
				// Don't do anything if the value is the same
				if (_Position == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_Position = value;
				hasDirtyFields = true;
			}
		}

		public void SetPositionDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_Position(ulong timestep)
		{
			if (PositionChanged != null) PositionChanged(_Position, timestep);
			if (fieldAltered != null) fieldAltered("Position", _Position, timestep);
		}
		private Quaternion _Rotation;
		public event FieldEvent<Quaternion> RotationChanged;
		public InterpolateQuaternion RotationInterpolation = new InterpolateQuaternion() { LerpT = 0.1f, Enabled = true };
		public Quaternion Rotation
		{
			get { return _Rotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_Rotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_Rotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetRotationDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_Rotation(ulong timestep)
		{
			if (RotationChanged != null) RotationChanged(_Rotation, timestep);
			if (fieldAltered != null) fieldAltered("Rotation", _Rotation, timestep);
		}
		private Color _Color;
		public event FieldEvent<Color> ColorChanged;
		public Interpolated<Color> ColorInterpolation = new Interpolated<Color>() { LerpT = 0f, Enabled = false };
		public Color Color
		{
			get { return _Color; }
			set
			{
				// Don't do anything if the value is the same
				if (_Color == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x4;
				_Color = value;
				hasDirtyFields = true;
			}
		}

		public void SetColorDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_Color(ulong timestep)
		{
			if (ColorChanged != null) ColorChanged(_Color, timestep);
			if (fieldAltered != null) fieldAltered("Color", _Color, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			PositionInterpolation.current = PositionInterpolation.target;
			RotationInterpolation.current = RotationInterpolation.target;
			ColorInterpolation.current = ColorInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _Position);
			UnityObjectMapper.Instance.MapBytes(data, _Rotation);
			UnityObjectMapper.Instance.MapBytes(data, _Color);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_Position = UnityObjectMapper.Instance.Map<Vector2>(payload);
			PositionInterpolation.current = _Position;
			PositionInterpolation.target = _Position;
			RunChange_Position(timestep);
			_Rotation = UnityObjectMapper.Instance.Map<Quaternion>(payload);
			RotationInterpolation.current = _Rotation;
			RotationInterpolation.target = _Rotation;
			RunChange_Rotation(timestep);
			_Color = UnityObjectMapper.Instance.Map<Color>(payload);
			ColorInterpolation.current = _Color;
			ColorInterpolation.target = _Color;
			RunChange_Color(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _Position);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _Rotation);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _Color);

			// Reset all the dirty fields
			for (int i = 0; i < _dirtyFields.Length; i++)
				_dirtyFields[i] = 0;

			return dirtyFieldsData;
		}

		protected override void ReadDirtyFields(BMSByte data, ulong timestep)
		{
			if (readDirtyFlags == null)
				Initialize();

			Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
			data.MoveStartIndex(readDirtyFlags.Length);

			if ((0x1 & readDirtyFlags[0]) != 0)
			{
				if (PositionInterpolation.Enabled)
				{
					PositionInterpolation.target = UnityObjectMapper.Instance.Map<Vector2>(data);
					PositionInterpolation.Timestep = timestep;
				}
				else
				{
					_Position = UnityObjectMapper.Instance.Map<Vector2>(data);
					RunChange_Position(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (RotationInterpolation.Enabled)
				{
					RotationInterpolation.target = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RotationInterpolation.Timestep = timestep;
				}
				else
				{
					_Rotation = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RunChange_Rotation(timestep);
				}
			}
			if ((0x4 & readDirtyFlags[0]) != 0)
			{
				if (ColorInterpolation.Enabled)
				{
					ColorInterpolation.target = UnityObjectMapper.Instance.Map<Color>(data);
					ColorInterpolation.Timestep = timestep;
				}
				else
				{
					_Color = UnityObjectMapper.Instance.Map<Color>(data);
					RunChange_Color(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (PositionInterpolation.Enabled && !PositionInterpolation.current.UnityNear(PositionInterpolation.target, 0.0015f))
			{
				_Position = (Vector2)PositionInterpolation.Interpolate();
				//RunChange_Position(PositionInterpolation.Timestep);
			}
			if (RotationInterpolation.Enabled && !RotationInterpolation.current.UnityNear(RotationInterpolation.target, 0.0015f))
			{
				_Rotation = (Quaternion)RotationInterpolation.Interpolate();
				//RunChange_Rotation(RotationInterpolation.Timestep);
			}
			if (ColorInterpolation.Enabled && !ColorInterpolation.current.UnityNear(ColorInterpolation.target, 0.0015f))
			{
				_Color = (Color)ColorInterpolation.Interpolate();
				//RunChange_Color(ColorInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public PlayerNetworkingNetworkObject() : base() { Initialize(); }
		public PlayerNetworkingNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public PlayerNetworkingNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
