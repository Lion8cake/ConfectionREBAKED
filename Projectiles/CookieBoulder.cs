using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Golf;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.Utilities;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Projectiles
{
    public class CookieBoulder : ModProjectile
    {
		public override void SetDefaults()
        {
			Projectile.width = 38;
			Projectile.height = 40;
			Projectile.aiStyle = 14;
			Projectile.friendly = true;
			Projectile.penetrate = 6;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.ignoreWater = true;
			Projectile.extraUpdates = 1;
			Projectile.scale = 0.95f;
		}

		public override bool PreAI()
		{
			if (Math.Abs(Projectile.velocity.X) <= 0f && Math.Abs(Projectile.velocity.Y) <= 0)
			{
				Projectile.Kill();
			}
			return true;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 28; 
			height = 28;
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if ((Projectile.velocity.X != oldVelocity.X && (oldVelocity.X < -3f || oldVelocity.X > 3f)) || (Projectile.velocity.Y != oldVelocity.Y && (oldVelocity.Y < -3f || oldVelocity.Y > 3f)))
			{
				for (int n = 0; n < 4; n++)
				{
					Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
				}
				SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			}
			return true;
		}

		public override void OnKill(int timeLeft)
        {
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
			for (int i = 0; i < 15; i++)
			{
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CookieDust>());
				dust.velocity *= 1.2f;
				dust.velocity.Y -= 0.8f;
			}
		}
	}
}