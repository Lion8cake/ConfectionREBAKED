using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
    public class DefenciveSprinkle : ModProjectile
    {
		public override void SetStaticDefaults()
		{
            Main.projFrames[Type] = 20;
		}

		public override void SetDefaults()
        {
			Projectile.width = 8;
            Projectile.height = 8;
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
			Projectile.velocity.Y += 0.205f;
			if (Projectile.velocity.Y < -5f) {
				Projectile.velocity.Y = -5f;
			}
		}
	}
}