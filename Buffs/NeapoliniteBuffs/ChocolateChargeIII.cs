using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Buffs.NeapoliniteBuffs {
	public class ChocolateChargeIII : ModBuff {
		public override void SetStaticDefaults() {
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.GetDamage(DamageClass.Ranged) += 0.12f;
			player.GetCritChance(DamageClass.Generic) += 6f;
		}
	}
}
