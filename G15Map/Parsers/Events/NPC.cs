using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace G15Map.Parsers.Events
{
	public enum NPCMovementType
	{
		WalkingAllDirections = 0x01,
		LookingAllDirections = 0x02,
		WalkingNorthSouth = 0x03,
		WalkingEastWest = 0x04,
		LookingSouth = 0x05,
		LookingNorth = 0x06,
		LookingWest = 0x07,
		LookingEast = 0x08,
	}

	public class NPC : IEventObject
	{
		public long RomOffset { get; private set; }

		public byte Sprite { get; private set; }
		public byte Y { get; private set; }
		public byte X { get; private set; }
		public NPCMovementType MovementType { get; private set; }
		public byte MovementDistances { get; private set; }
		public ushort Unknown3 { get; private set; }    // ex. FFFF
		public ushort Unknown4 { get; private set; }    // ex. 0000
		public ushort Unknown5 { get; private set; }    // ex. 0000
		public ushort Unknown6 { get; private set; }    // ex. 0000

		public byte MovementDistanceEastWest { get { return (byte)(MovementDistances >> 4); } }
		public byte MovementDistanceNorthSouth { get { return (byte)(MovementDistances & 0x0F); } }

		public NPC(BinaryReader reader)
		{
			RomOffset = reader.BaseStream.Position;

			Sprite = reader.ReadByte();
			Y = reader.ReadByte();
			X = reader.ReadByte();
			MovementType = (NPCMovementType)reader.ReadByte();
			MovementDistances = reader.ReadByte();
			Unknown3 = reader.ReadUInt16();
			Unknown4 = reader.ReadUInt16();
			Unknown5 = reader.ReadUInt16();
			Unknown6 = reader.ReadUInt16();
		}
	}
}
