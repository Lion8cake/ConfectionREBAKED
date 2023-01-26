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
	public class BananawarpPeel : ModProjectile
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
			Player player = Main.LocalPlayer;
			ConfectionPlayer modPlayer = player.GetModPlayer<ConfectionPlayer>();
			int tileX = (int)((Main.mouseX + Main.screenPosition.X) / 16);
			int tileY = (int)((Main.mouseY + Main.screenPosition.Y) / 16);
			if (modPlayer.BananawarpPeelWarp == null)
			{
				Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y + -12f, 0, 0, ModContent.ProjectileType<PeelWarp>(), 0, 0, Main.myPlayer);
			}
			else if (modPlayer.BananawarpPeelWarp != null && player.ownedProjectileCounts[ModContent.ProjectileType<PeelWarp2>()] == 0)
			{
				Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y + -12f, 0, 0, ModContent.ProjectileType<PeelWarp2>(), 1, 0, Main.myPlayer);
			}
			else if (modPlayer.BananawarpPeelWarp != null && player.ownedProjectileCounts[ModContent.ProjectileType<PeelWarp2>()] == 1)
            {
				for (int k = 0; k < 5; k++)
				{
					Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<BananaWarpDust>(), Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
				}
			}
		}
	}
}
