using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TheConfectionRebirth.Projectiles
{
	public class ToothFairy : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 6;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = false;
		}

		public override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.tileCollide = false;
			Projectile.minionSlots = 0;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Color.White;
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (player.dead)
			{
				player.GetModPlayer<ConfectionPlayer>().toothfairyMinion = false;
			}
			if (player.GetModPlayer<ConfectionPlayer>().toothfairyMinion)
			{
				Projectile.timeLeft = 2;
			}
			TryAttack();
			Animate();
			IdlePosition();
			//Lighting.AddLight(Projectile.Center, 2f, 0.5f, 0.25f);
		}

		public void Attack(Vector2 displacement, Vector2 direction, int target)
		{
			if (Main.myPlayer != Projectile.owner)
				return;
			if (Projectile.spriteDirection == -1)
			{
				displacement.X = -displacement.X;
				direction.X = -direction.X;
			}
			int ai1 = Main.rand.Next(7);
			int damage = Projectile.damage;
			int damageScaling = Math.Max(0, Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<ToothFairyCrystal>()] - 1);
			damage = (int)(damage * (1.5f + (float)damageScaling * 0.4f));
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + displacement, direction, ModContent.ProjectileType<ToothFairyTooth>(), damage, Projectile.knockBack, Projectile.owner, target, ai1);
		}

		public void IdlePosition()
		{
			Player player = Main.player[Projectile.owner];
			Vector2 pos = player.Center;
			float ai0 = Projectile.ai[0];
			if (ai0 == 0)
			{
				pos.X -= 60 * player.direction;
				pos.Y -= 24;
			}
			else
			{
				pos.X -= 8 * player.direction;
				pos.Y -= 96;
				pos.X += MathF.Cos((40 - ai0) * MathF.PI / 20) * 24 * player.direction;
				pos.Y += MathF.Abs(MathF.Sin((40 - ai0) * MathF.PI / 20)) * 16;
			}
			Vector2 diff = pos - Projectile.Center;
			float len = diff.Length();
			if (len < 4 || len > 1600)
			{
				Projectile.velocity = diff + player.velocity;
			}
			else if (len < 160)
			{
				Projectile.velocity = diff * 4 / len + player.velocity;
			}
			else
			{
				Projectile.velocity = diff / 40 + player.velocity;
			}
			Projectile.direction = player.direction;
			Projectile.spriteDirection = player.direction;

		}
		public void TryAttack()
		{
			if (Projectile.ai[0] == 0)
			{
				int target = -1; Projectile.Minion_FindTargetInRange(1400, ref target, false);
				if (target != -1)
				{
					Attack(new(-8, 0), new(-8, 0), target);
					Attack(new(-8, 0), new(-6, 3), target);
					Attack(new(-8, 0), new(-3, -3), target);
					Projectile.ai[0] = 39;
				}
			}
			else
			{
				if (Projectile.ai[0] == 20)
				{
					int target = -1; Projectile.Minion_FindTargetInRange(1400, ref target, false);
					if (target != -1) Attack(new(8, 0), new(8, 0), target);
				}
				Projectile.ai[0] -= 1;
			}
		}

		private static void MoveSingleCrownPiece(Projectile Projectile, Projectile proj, Vector2 disp, float rotated)
		{
			Vector2 pos = Projectile.Center + disp.RotatedBy(rotated);
			proj.Center = pos;
		}

		public void Animate()
		{
			float ai0 = Projectile.ai[0];
			if (ai0 == 0)
			{
				Projectile.frameCounter += 1;
				Projectile.frameCounter %= 40;
				int frame = Projectile.frameCounter / 10;
				switch (frame)
				{
					case 1:
					case 3:
						Projectile.frame = 1;
						break;
					case 0:
					case 2:
						Projectile.frame = frame;
						break;
				}
			}
			else
			{
				int frame = (int)(ai0 / 10);
				switch (frame)
				{
					case 1:
						Projectile.frame = 5;
						break;
					case 0:
					case 2:
						Projectile.frame = 4;
						break;
					case 3:
						Projectile.frame = 3;
						break;
				}
			}
		}
	}
}
