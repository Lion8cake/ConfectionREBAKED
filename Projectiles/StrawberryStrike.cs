using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Buffs.NeapoliniteBuffs;

namespace TheConfectionRebirth.Projectiles
{
	public class StrawberryStrike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 10000000;
        }

		public override void AI()
		{
			float maxDetectRadius = 400f;
			float projSpeed = 5f;

			Projectile.rotation = 0.45f;

			NPC closestNPC = FindClosestNPC(maxDetectRadius);
			if (closestNPC == null)
				return;

			Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
			Projectile.rotation = Projectile.velocity.ToRotation() + 0.45f;

			if (!Main.player[Projectile.owner].HasBuff(ModContent.BuffType<StrawberryStrikeI>()) && !Main.player[Projectile.owner].HasBuff(ModContent.BuffType<StrawberryStrikeII>()) && !Main.player[Projectile.owner].HasBuff(ModContent.BuffType<StrawberryStrikeIII>()) && !Main.player[Projectile.owner].HasBuff(ModContent.BuffType<StrawberryStrikeIV>()) && !Main.player[Projectile.owner].HasBuff(ModContent.BuffType<StrawberryStrikeV>()))
            {
				Projectile.Kill();
            }
		}

		public NPC FindClosestNPC(float maxDetectDistance)
		{
			NPC closestNPC = null;

			float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

			for (int k = 0; k < Main.maxNPCs; k++)
			{
				NPC target = Main.npc[k];
				if (target.CanBeChasedBy())
				{
					float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

					if (sqrDistanceToTarget < sqrMaxDetectDistance)
					{
						sqrMaxDetectDistance = sqrDistanceToTarget;
						closestNPC = target;
					}
				}
			}

			return closestNPC;
		}
	}
}