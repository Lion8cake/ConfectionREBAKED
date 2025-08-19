using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs.NeapoliniteBuffs {
	public class StrawberryStrikeI : ModBuff {
		public override void SetStaticDefaults() {
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.GetModPlayer<ConfectionPlayer>().strawberrySpawnStrawTimer++;
			player.GetModPlayer<ConfectionPlayer>().mageStrawberry = 0;
			player.GetModPlayer<ConfectionPlayer>().neapolinitePowerLevel = 1;
		}
	}
}
