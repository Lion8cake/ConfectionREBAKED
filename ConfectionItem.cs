using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth {
	public class ConfectionItem : GlobalItem {
		public override void OnConsumeMana(Item item, Player player, int manaConsumed) {
			const float radius = 16 * 30;

			ConfectionPlayer playerFuncs = player.GetModPlayer<ConfectionPlayer>();
			if (playerFuncs.StrawberryStrikeOnCooldown)
				return;

			Vector2 velocity = Main.MouseWorld - player.Center;
			velocity.Normalize();
			velocity *= 5;
			const float rotPerIter = MathF.PI / 6;
			StackableBuffData.StrawberryStrike.FindBuff(player, out byte rank);
			float initialRot = (rank - 1) * -rotPerIter / 2;
			while (rank > 0) {
				Vector2 vel = velocity.RotatedBy(initialRot);
				Projectile.NewProjectile(item.GetSource_FromThis(), player.Center + Main.rand.NextVector2Circular(radius, radius), vel, ModContent.ProjectileType<StrawberryStrike>(), item.damage / 2, 8f, player.whoAmI);
				rank--;
				initialRot += rotPerIter;
			}
			playerFuncs.StrawberryStrikeOnCooldown = true;
			playerFuncs.Timer.Add(new(0, 60, TimerDataType.StrawberryStrikeDelay));
		}
	}
}
