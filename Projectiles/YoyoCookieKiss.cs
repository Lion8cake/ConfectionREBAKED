using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Buffs.NeapoliniteBuffs;

namespace TheConfectionRebirth.Projectiles
{
	public class YoyoCookieKiss : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		public override void SetDefaults() {
			Projectile.width = 14;
			Projectile.height = 14;

			Projectile.aiStyle = 0;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 500;
		}

		public override void AI() {
			float maxDetectRadius = 250f;
			float projSpeed = 7f;

			NPC closestNPC = FindClosestNPC(maxDetectRadius);
			if (closestNPC == null) {
				Projectile.velocity.Y += 1f;
				Projectile.tileCollide = true;
			}
			else {
				Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
				Projectile.tileCollide = false;
			}
		}

		public NPC FindClosestNPC(float maxDetectDistance) {
			NPC closestNPC = null;

			float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

			for (int k = 0; k < Main.maxNPCs; k++) {
				NPC target = Main.npc[k];
				if (target.CanBeChasedBy()) {
					float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

					if (sqrDistanceToTarget < sqrMaxDetectDistance) {
						sqrMaxDetectDistance = sqrDistanceToTarget;
						closestNPC = target;
					}
				}
			}

			return closestNPC;
		}

		public override bool? CanCutTiles() {
			return false;
		}
	}
}
