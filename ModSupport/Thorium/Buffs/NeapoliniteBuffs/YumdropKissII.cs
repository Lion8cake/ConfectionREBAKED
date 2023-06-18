using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.ModSupport.Thorium.Buffs.NeapoliniteBuffs {
	public class YumdropKissII : ModBuff 
	{
		public Mod thorium;
		public override bool IsLoadingEnabled(Mod mod) {
			return ModLoader.TryGetMod("ThoriumMod", out thorium);
		}
		public override void SetStaticDefaults() {
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		[JITWhenModsEnabled("ThoriumMod")]
		public override void Update(Player player, ref int buffIndex) {
			if (ModLoader.TryGetMod("ThoriumMod", out Mod source) && source.TryFind("HealerDamage", out DamageClass damageClass)) {
				ThoriumMod.ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumMod.ThoriumPlayer>();
				player.GetDamage(damageClass) += 0.02f;
				player.GetCritChance(DamageClass.Generic) += 4f;
				thoriumPlayer.healBonus += 2;
			}
		}
	}
}
