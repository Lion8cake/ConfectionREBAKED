using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.ModSupport.Thorium.Buffs.NeapoliniteBuffs {
	public class RockinStarV : ModBuff {
		public Mod thorium;
		public override bool IsLoadingEnabled(Mod mod) {
			return ModLoader.TryGetMod("ThoriumMod", out thorium);
		}
		public override void SetStaticDefaults() {
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			if (ModLoader.TryGetMod("ThoriumMod", out Mod source) && source.TryFind("BardDamage", out DamageClass damageClass)) {
				player.GetDamage(damageClass) += 0.1f;
				player.GetCritChance(DamageClass.Generic) += 10f;
			}
		}
	}
}
