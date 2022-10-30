using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TheConfectionRebirth.Dusts;
using Terraria.DataStructures;

namespace TheConfectionRebirth.Projectiles
{
	public class SugarWaterBottle : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 2;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
		}
	
		public override void AI()
		{
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] >= 10f)
			{
				Projectile.velocity.Y += 0.1f;
				Projectile.velocity.X *= 0.998f;
			}
		}
	
		public override void Kill(int timeLeft)
		{
			if (Projectile.owner == Main.myPlayer)
			{
				SoundEngine.PlaySound(SoundID.Shatter, Projectile.position);
				for (int index = 0; index < 5; index++)
				{
					Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 13);
				}
				for (int index2 = 0; index2 < 30; index2++)
				{
					int index3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CreamWaterSplash>(), 0f, -2f, 0, default(Color), 1.1f);
					Dust obj = Main.dust[index3];
					obj.alpha = 100;
					obj.velocity.X *= 1.5f;
					obj.velocity *= 3f;
				}
				Projectile.NewProjectile(new EntitySource_Misc("Sugar water from the bottle"), Projectile.Center.X, Projectile.Center.Y, -8 + Main.rand.Next(0, 17), -8 + Main.rand.Next(0, 17), ModContent.ProjectileType<SugarWater>(), 24, 1f, Main.myPlayer, 0f, 0f);
			}
		}
	}
}
