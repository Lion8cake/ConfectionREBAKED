using Terraria.ModLoader;
using Terraria;
using TheConfectionRebirth.Dusts;
using Microsoft.Xna.Framework;
using System;

namespace TheConfectionRebirth.Projectiles
{
	public class ChipArrow : ModProjectile
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
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CookieDust>());
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			OnHit(target, target.position.X, target.position.Y);
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			OnHit(target, target.position.X, target.position.Y);
		}

		private void OnHit(Entity victim, float x, float y)
		{
			Player player = Main.player[Projectile.owner];
			int dir = player.direction;
			float spawnX = Main.screenPosition.X;
			if (dir < 0)
			{
				spawnX += (float)Main.screenWidth;
			}
			float spawnY = Main.screenPosition.Y;
			spawnY += (float)Main.rand.Next(Main.screenHeight);
			Vector2 pos = new(spawnX, spawnY);
			float velX = x - pos.X;
			float velY = y - pos.Y;
			velX += (float)Main.rand.Next(-50, 51) * 0.1f;
			velY += (float)Main.rand.Next(-50, 51) * 0.1f;
			float speed = (float)Math.Sqrt(velX * velX + velY * velY);
			speed = 24f / speed;
			velX *= speed;
			velY *= speed;
			Projectile.NewProjectile(player.GetSource_OnHit(victim), spawnX, spawnY, velX, velY, ModContent.ProjectileType<ChocolateChip>(), (int)(Projectile.damage * 0.75f), 0f, player.whoAmI);
		}
	}
}
