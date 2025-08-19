using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
    public class SprinklerBall : ModProjectile
    {
		public override void SetStaticDefaults()
		{
            Main.projFrames[Type] = 4;
		}

		public override void SetDefaults()
        {
			Projectile.width = 18;
            Projectile.height = 18;
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
			if (Projectile.velocity.Y < -10f) {
				Projectile.velocity.Y = -10f;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects spriteEffects = 0;
			if (Projectile.spriteDirection == 1)
			{
				spriteEffects = (SpriteEffects)1;
			}
			Rectangle frame = new Rectangle(0, 0, TextureAssets.Projectile[Projectile.type].Width() / 3, TextureAssets.Projectile[Projectile.type].Height() / 4);
			frame = new Rectangle((int)(frame.Width * Projectile.ai[0]), frame.Height * Projectile.frame, frame.Width, frame.Height);
			Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, (Rectangle?)frame, Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(frame.Width / 2, frame.Height / 2), Projectile.scale, spriteEffects, 0f);
			return false;
		}
	}
}