using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.ModSupport.Thorium.Buffs.NeapoliniteBuffs;

namespace TheConfectionRebirth.ModSupport.Thorium
{
	public class ItemUseDetection : GlobalItem
	{
		public override bool? UseItem(Item item, Player player) {
			ThoriumConfectionPlayer thoriumplayer = player.GetModPlayer<ThoriumConfectionPlayer>();

			if (ModLoader.TryGetMod("ThoriumMod", out Mod source) && source.TryFind("BardDamage", out DamageClass damageClass)) {
				if (thoriumplayer.NeapoliniteBardSet == true && item.DamageType == damageClass && !player.HasBuff(ModContent.BuffType<RockinStarV>())) {
					if (!player.HasBuff(ModContent.BuffType<RockinStarI>()) && !player.HasBuff(ModContent.BuffType<RockinStarII>()) && !player.HasBuff(ModContent.BuffType<RockinStarIII>()) && !player.HasBuff(ModContent.BuffType<RockinStarIV>())) {
						player.AddBuff(ModContent.BuffType<RockinStarI>(), 480);
						return null;
					}
					else if (player.HasBuff(ModContent.BuffType<RockinStarI>())) {
						player.ClearBuff(ModContent.BuffType<RockinStarI>());
						player.AddBuff(ModContent.BuffType<RockinStarII>(), 480);
						return null;
					}
					else if (player.HasBuff(ModContent.BuffType<RockinStarII>())) {
						player.ClearBuff(ModContent.BuffType<RockinStarII>());
						player.AddBuff(ModContent.BuffType<RockinStarIII>(), 480);
						return null;
					}
					else if (player.HasBuff(ModContent.BuffType<RockinStarIII>())) {
						player.ClearBuff(ModContent.BuffType<RockinStarIII>());
						player.AddBuff(ModContent.BuffType<RockinStarIV>(), 480);
						return null;
					}
					else if (player.HasBuff(ModContent.BuffType<RockinStarIV>())) {
						player.ClearBuff(ModContent.BuffType<RockinStarIV>());
						player.AddBuff(ModContent.BuffType<RockinStarV>(), 480);
						return null;
					}	
				}
			}
			if (ModLoader.TryGetMod("ThoriumMod", out Mod source2) && source2.TryFind("HealerDamage", out DamageClass damageClass2)) {
				if (thoriumplayer.NeapoliniteHealerSet == true && item.DamageType == damageClass2 && !player.HasBuff(ModContent.BuffType<YumdropKissV>())) {
					if (!player.HasBuff(ModContent.BuffType<YumdropKissI>()) && !player.HasBuff(ModContent.BuffType<YumdropKissII>()) && !player.HasBuff(ModContent.BuffType<YumdropKissIII>()) && !player.HasBuff(ModContent.BuffType<YumdropKissIV>())) {
						player.AddBuff(ModContent.BuffType<YumdropKissI>(), 480);
						return null;
					}
					else if (player.HasBuff(ModContent.BuffType<YumdropKissI>())) {
						player.ClearBuff(ModContent.BuffType<YumdropKissI>());
						player.AddBuff(ModContent.BuffType<YumdropKissII>(), 480);
						return null;
					}
					else if (player.HasBuff(ModContent.BuffType<YumdropKissII>())) {
						player.ClearBuff(ModContent.BuffType<YumdropKissII>());
						player.AddBuff(ModContent.BuffType<YumdropKissIII>(), 480);
						return null;
					}
					else if (player.HasBuff(ModContent.BuffType<YumdropKissIII>())) {
						player.ClearBuff(ModContent.BuffType<YumdropKissIII>());
						player.AddBuff(ModContent.BuffType<YumdropKissIV>(), 480);
						return null;
					}
					else if (player.HasBuff(ModContent.BuffType<YumdropKissIV>())) {
						player.ClearBuff(ModContent.BuffType<YumdropKissIV>());
						player.AddBuff(ModContent.BuffType<YumdropKissV>(), 480);
						return null;
					}
				}
			}
			return null;
		}
	}
}
