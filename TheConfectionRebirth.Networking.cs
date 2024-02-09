using System.IO;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.ModSupport.Thorium;

namespace TheConfectionRebirth;

partial class TheConfectionRebirth {
	public enum MessageType : byte {
		SyncThoriumDLCPlayer
	}

	public override void HandlePacket(BinaryReader reader, int whoAmI) {
		switch ((MessageType)reader.ReadByte()) {
			case MessageType.SyncThoriumDLCPlayer:
				int playerId = reader.ReadByte();
				Main.player[playerId].GetModPlayer<ThoriumDLCPlayer>().ReceivePlayerSync(reader);
				break;
			default:
				Logger.Error("Invalid packet ID!");
				break;
		}
	}
}
