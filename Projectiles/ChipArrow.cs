using TheConfectionRebirth.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TheConfectionRebirth.Projectiles
{
	public class ChipArrow : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
		}

		public override void AI() {
			Projectile.ai[0] += 1f;
			Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : -1;
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.velocity.Y > 16f) {
				Projectile.velocity.Y = 16f;
			}
			if (Projectile.spriteDirection == -1) {
				Projectile.rotation += MathHelper.Pi;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
			Projectile.ai[0] += 0.1f;
			Projectile.velocity *= 0.75f;
		}
		
		public override void Kill(int timeLeft)
		{
			if (Main.myPlayer != Projectile.owner)
			{
				return;
			}
			int choice = Main.rand.Next(1);
			if (choice == 0)
			{
				Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y, -8 + Main.rand.Next(0, 17), -8 + Main.rand.Next(0, 17), ModContent.ProjectileType<ChocolateChip>(), 24, 1f, Main.myPlayer, 0f, 0f);
			}
			
			int num = Main.rand.Next(1);
			if (num == 0)
			{
				Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y, -8 + Main.rand.Next(0, 17), -8 + Main.rand.Next(0, 17), ModContent.ProjectileType<ChocolateChip>(), 24, 1f, Main.myPlayer, 0f, 0f);
			}
			
			int num2 = Main.rand.Next(1);
			if (num2 == 0)
			{
				Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y, -8 + Main.rand.Next(0, 17), -8 + Main.rand.Next(0, 17), ModContent.ProjectileType<ChocolateChip>(), 24, 1f, Main.myPlayer, 0f, 0f);
			}
			{
				int num3 = Main.rand.Next(1);
				if (num3 == 0)
				{
					Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y, -8 + Main.rand.Next(0, 17), -8 + Main.rand.Next(0, 17), ModContent.ProjectileType<ChocolateChip>(), 24, 1f, Main.myPlayer, 0f, 0f);
				}
				
				int num4 = Main.rand.Next(1);
				if (num4 == 0)
				{
					Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center.X, Projectile.Center.Y, -8 + Main.rand.Next(0, 17), -8 + Main.rand.Next(0, 17), ModContent.ProjectileType<ChocolateChip>(), 24, 1f, Main.myPlayer, 0f, 0f);
				}
			}
		}
	}
}