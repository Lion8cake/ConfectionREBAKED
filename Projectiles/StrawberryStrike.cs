using Microsoft.Xna.Framework;
using System;
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
            Projectile.timeLeft = 2;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
        }
        public override bool? CanCutTiles()
        {
			return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
			lightColor = Color.White;
			return true;

		}
        public override void AI()
		{
			const int unfadeTime = 30;
			const float maxDetectRadius = 16 * 40;
			const float projSpeed = 5f;
			const float maxAmount = 0.1f;

			Projectile.timeLeft = 2;

			NPC closestNPC = FindClosestNPC(maxDetectRadius);
			if (closestNPC != null)
			{
				Vector2 disp = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;

				Projectile.velocity = Projectile.velocity.RotatedBy(Utils.AngleLerp(0, disp.ToRotation() - Projectile.velocity.ToRotation(), maxAmount));
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + 0.45f;
			Projectile.alpha = 255 - (255 * (int)Projectile.localAI[0] / unfadeTime);
			Projectile.localAI[0] = Math.Min(Projectile.localAI[0] + 1, unfadeTime);

			if (StackableBuffData.StrawberryStrike.FindBuff(Main.player[Projectile.owner], out byte _) == -1)
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
