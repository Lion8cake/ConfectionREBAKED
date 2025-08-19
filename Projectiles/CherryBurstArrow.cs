using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using TheConfectionRebirth.Dusts;
using System;

namespace TheConfectionRebirth.Projectiles
{
	public class CherryBurstArrow : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.arrow = true;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 1200;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CherryDust>());

				int dustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default(Color), 1.5f);
				Dust dust = Main.dust[dustID];
				dust.velocity *= 0.9f;

				if (i % 2 == 0)
				{
					dustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default(Color), 2.5f);
					Dust dust2 = Main.dust[dustID];
					dust2.noGravity = true;
					dust2.velocity *= 3f;
					dustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default(Color), 1.5f);
					dust2 = Main.dust[dustID];
					dust2.velocity *= 2f;
				}
			}
			Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
			int goreID = Gore.NewGore(Projectile.GetSource_Death(), pos, default(Vector2), Main.rand.Next(61, 64));
			Gore gore = Main.gore[goreID];
			gore.velocity *= 0.3f;
			gore.velocity.X += Main.rand.Next(-1, 2);
			gore.velocity.Y += Main.rand.Next(-1, 2);
			Projectile.position.X += Projectile.width / 2;
			Projectile.position.Y += Projectile.height / 2;
			Projectile.width = 150;
			Projectile.height = 150;
			Projectile.position.X -= Projectile.width / 2;
			Projectile.position.Y -= Projectile.height / 2;
			Projectile.penetrate = -1;
			Projectile.maxPenetrate = 0;
			Projectile.Damage();
			if (Projectile.owner == Main.myPlayer)
			{
				int rand = Main.rand.Next(2, 6);
				for (int i = 0; i < rand; i++)
				{
					float velX = Main.rand.Next(-100, 101);
					velX += 0.01f;
					float velY = Main.rand.Next(-100, 101);
					velX -= 0.01f;
					float speed = (float)Math.Sqrt(velX * velX + velY * velY);
					speed = 8f / speed;
					velX *= speed;
					velY *= speed;
					int projID = Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X - Projectile.oldVelocity.X, Projectile.Center.Y - Projectile.oldVelocity.Y, velX, velY, ModContent.ProjectileType<CherryShard>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
					Projectile projectile = Main.projectile[projID];
					projectile.maxPenetrate = 0;
				}
			}
		}
	}
}
