using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TheConfectionRebirth.Dusts;
using Terraria.DataStructures;
using Mono.Cecil;

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
	
		public override void OnKill(int timeLeft)
		{
			Player player = Main.LocalPlayer;
			if (player.ownedProjectileCounts[ModContent.ProjectileType<DimensionalWarp>()] <= 0)
			{
				Projectile.NewProjectile(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (Projectile.width / 2), Projectile.position.Y - 8), Vector2.Zero, ModContent.ProjectileType<DimensionalWarp>(), 0, 0, Projectile.owner, 0f);
			}
			else if (player.ownedProjectileCounts[ModContent.ProjectileType<DimensionalWarp>()] == 1) 
			{
				Projectile.NewProjectile(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (Projectile.width / 2), Projectile.position.Y - 8), Vector2.Zero, ModContent.ProjectileType<DimensionalWarp>(), 0, 0, Projectile.owner, 1f);
			}
			else
			{
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile projectile = Main.projectile[i];
					if (projectile.active && (projectile.ai[0] == 1 || projectile.ai[0] == 3) && projectile.type == ModContent.ProjectileType<DimensionalWarp>())
					{
						projectile.Kill();
						Projectile.NewProjectile(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (Projectile.width / 2), Projectile.position.Y - 8), Vector2.Zero, ModContent.ProjectileType<DimensionalWarp>(), 0, 0, Projectile.owner, 1f);
					}
				}
			}
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<BananaWarpDust>(), Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
			}
		}
	}
}
