using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs.NeapoliniteBuffs {
	public class ChocolateChargeI : ModBuff {
		public override void SetStaticDefaults() {
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.GetDamage(DamageClass.Ranged) += 0.04f;
			player.GetCritChance(DamageClass.Generic) += 2f;
			player.GetModPlayer<ConfectionPlayer>().neapolinitePowerLevel = 1;
		}
	}
}
