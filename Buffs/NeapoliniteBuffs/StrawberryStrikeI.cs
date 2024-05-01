using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Buffs.NeapoliniteBuffs {
	public class StrawberryStrikeI : ModBuff {
		public override void SetStaticDefaults() {
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			if (player.ownedProjectileCounts[ModContent.ProjectileType<StrawberryStrike>()] < 1) {
				Projectile.NewProjectile(new EntitySource_Misc("Strawberry Strike I"), player.Center, Vector2.Zero, ModContent.ProjectileType<StrawberryStrike>(), 50, 8f, player.whoAmI);
			}
		}
	}
}
