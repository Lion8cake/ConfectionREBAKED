using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.ModSupport.Thorium.Buffs;
using ThoriumMod;

namespace TheConfectionRebirth.ModSupport.Thorium.Items;

[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
internal abstract class NeapoliniteItemUseDetection<TDamage, TBuff> : GlobalItem where TDamage : DamageClass where TBuff : NeapoliniteBuff {
	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.DamageType.CountsAsClass<TDamage>();
	}

	public abstract bool IsSetActive(Player player);

	public override bool? UseItem(Item item, Player player) {
		if (IsSetActive(player)) {
			player.AddBuff(ModContent.BuffType<TBuff>(), 480);
		}

		return null;
	}
}

[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
internal sealed class NeapoliniteBardItemUseDetection : NeapoliniteItemUseDetection<BardDamage, RockinStar> {
	public override bool IsSetActive(Player player) {
		return player.GetModPlayer<ThoriumDLCPlayer>().NeapoliniteBard;
	}
}
