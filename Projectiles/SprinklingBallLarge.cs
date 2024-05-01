using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
    public class SprinklingBallLarge : ModProjectile
    {
		public override void SetStaticDefaults()
		{
            Main.projFrames[Type] = 4;
		}

		public override void SetDefaults()
        {
			Projectile.width = 12;
            Projectile.height = 12;
            Projectile.hostile = true;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : -1;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
            }
			Projectile.localAI[1] = 0f;
			Projectile.velocity.Y += 0.205f;
			if (Projectile.velocity.Y < -10f) {
				Projectile.velocity.Y = -10f;
			}
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ai[0] += 0.1f;
            Projectile.velocity *= 0.75f;
        }
	}
}