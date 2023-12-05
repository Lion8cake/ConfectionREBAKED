using System;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace TheConfectionRebirth.ModSupport.Thorium;

public sealed class ThoriumDLCPlayer : ModPlayer {
	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	private int rockinStarStage;
	private int yumdropKissStage;

	public bool JawbreakerPickEffects { get; set; }
	public bool JawbreakerSetEffects { get; set; }

	public int InspirationConsumed { get; set; }

	public bool NeapoliniteBard { get; set; }
	public ref int RockinStarStage => ref rockinStarStage;

	public bool NeapoliniteHealer { get; set; }
	public ref int YumdropKissStage => ref yumdropKissStage;

	public override void ResetEffects() {
		NeapoliniteBard = false;
		NeapoliniteHealer = false;
		JawbreakerPickEffects = false;
		JawbreakerSetEffects = false;
	}

	public override void OnEnterWorld() {
		InspirationConsumed = 0;
	}

	public override void Load() {
		MonoModHooks.Add(
			typeof(BardItem).GetMethod(nameof(BardItem.ConsumeInspiration), BindingFlags.Static | BindingFlags.Public),
			static (Func<Player, int, bool, bool> orig, Player player, int cost, bool pay) => {
				if (orig(player, cost, pay) && pay) {
					if (player.TryGetModPlayer<ThoriumDLCPlayer>(out var dlcPlayer)
						&& player.TryGetModPlayer<ConfectionPlayer>(out var confectionPlayer)
						&& dlcPlayer.NeapoliniteBard
						&& player.whoAmI == Main.myPlayer) {
						dlcPlayer.InspirationConsumed += cost;
						confectionPlayer.Timer.Add(new(cost, 180, TimerDataType.ThoriumBard));
					}

					return true;
				}

				return false;
			});
	}

	public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
		var packet = Mod.GetPacket();
		packet.Write((byte)TheConfectionRebirth.MessageType.SyncThoriumDLCPlayer);
		packet.Write((byte)Player.whoAmI);
		packet.Write((byte)((RockinStarStage & 7) | ((YumdropKissStage & 7) << 4)));
		packet.Send(toWho, fromWho);
	}

	public void ReceivePlayerSync(BinaryReader reader) {
		var value = reader.ReadByte();
		RockinStarStage = value & 7;
		YumdropKissStage = (value >> 4) & 7;
	}

	public override void CopyClientState(ModPlayer targetCopy) {
		var clone = (ThoriumDLCPlayer)targetCopy;
		clone.RockinStarStage = RockinStarStage;
		clone.YumdropKissStage = YumdropKissStage;
	}

	public override void SendClientChanges(ModPlayer clientPlayer) {
		var clone = (ThoriumDLCPlayer)clientPlayer;

		if (RockinStarStage != clone.RockinStarStage || YumdropKissStage != clone.YumdropKissStage)
			SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
	}
}
