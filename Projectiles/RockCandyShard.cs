using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Projectiles
{
	public class RockCandyShard : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 600;
			Projectile.aiStyle = -1;
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 0.785f;
			Projectile.velocity *= 0.95f;

			if ((Projectile.velocity.X < 0.01f && Projectile.velocity.X > -0.01f) || (Projectile.velocity.Y < 0.01f && Projectile.velocity.Y > -0.01f))
			{
				Projectile.Kill();
				return;
			}
			Lighting.AddLight(Projectile.Center, 0.91f / 7, 2.09f / 7, 2.34f / 7);
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 6; i++)
			{
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<SacchariteDust>());
			}
		}
	}
}
