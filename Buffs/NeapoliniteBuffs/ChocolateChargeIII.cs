using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs.NeapoliniteBuffs {
	public class ChocolateChargeIII : ModBuff {
		public override void SetStaticDefaults() {
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.GetDamage(DamageClass.Ranged) += 0.12f;
			player.GetCritChance(DamageClass.Generic) += 6f;
			player.GetModPlayer<ConfectionPlayer>().neapolinitePowerLevel = 3;
		}
	}
}
