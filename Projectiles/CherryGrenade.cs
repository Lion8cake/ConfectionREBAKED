using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Projectiles
{
	public class CherryGrenade : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Type] = true;
			ProjectileID.Sets.Explosive[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 16;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
		}

		public override void OnSpawn(IEntitySource source)
		{
			if (Projectile.owner == Main.myPlayer)
			{
				Projectile.timeLeft = 180;
			}
		}

		public override bool PreAI()
		{
			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
			{
				Projectile.hide = true;
				PrepareBombToBlow();
			}
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] > 10f)
			{
				Projectile.ai[0] = 10f;
				if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
				{
					Projectile.velocity.X *= 0.97f;
					if ((double)Projectile.velocity.X > -0.01 && (double)Projectile.velocity.X < 0.01)
					{
						Projectile.velocity.X = 0f;
						Projectile.netUpdate = true;
					}
				}
				Projectile.velocity.Y += 0.2f;
			}
			Projectile.rotation += Projectile.velocity.X * 0.1f;
			return false;
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (Main.expertMode)
			{
				modifiers.FinalDamage /= 5;
			}
		}

		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			if (Main.expertMode)
			{
				modifiers.FinalDamage /= 5;
			}
		}

		public override void PrepareBombToBlow()
		{
			Projectile.Resize(128, 128);
			Projectile.knockBack = 8f;
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(in SoundID.Item14, Projectile.position);
			Projectile.position.X += Projectile.width / 2;
			Projectile.position.Y += Projectile.height / 2;
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.position.X -= Projectile.width / 2;
			Projectile.position.Y -= Projectile.height / 2;
			int type = DustID.Torch;
			for (int i = 0; i < 20; i++)
			{
				int dustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default(Color), 1.5f);
				Dust dust = Main.dust[dustID];
				dust.velocity *= 1.4f;
				if (i % 2 == 0)
				{
					dustID = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, type, 0f, 0f, 100, default(Color), 2.5f);
					dust = Main.dust[dustID];
					dust.noGravity = true;
					dust.velocity *= 5f;
					dustID = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, type, 0f, 0f, 100, default(Color), 1.5f);
					dust = Main.dust[dustID];
					dust.velocity *= 3f;
				}
			}
			Vector2 pos = Projectile.position;
			int goreID = Gore.NewGore(Projectile.GetSource_Death(), pos, default(Vector2), Main.rand.Next(61, 64));
			Gore gore = Main.gore[goreID];
			gore.velocity *= 0.4f;
			gore.velocity.X += 1f;
			gore.velocity.Y += 1f;
			goreID = Gore.NewGore(Projectile.GetSource_Death(), pos, default(Vector2), Main.rand.Next(61, 64));
			gore = Main.gore[goreID];
			gore.velocity *= 0.4f;
			gore.velocity.X -= 1f;
			gore.velocity.Y += 1f;
			goreID = Gore.NewGore(Projectile.GetSource_Death(), pos, default(Vector2), Main.rand.Next(61, 64));
			gore = Main.gore[goreID];
			gore.velocity *= 0.4f;
			gore.velocity.X += 1f;
			gore.velocity.Y -= 1f;
			goreID = Gore.NewGore(Projectile.GetSource_Death(), pos, default(Vector2), Main.rand.Next(61, 64));
			gore = Main.gore[goreID];
			gore.velocity *= 0.4f;
			gore.velocity.X -= 1f;
			gore.velocity.Y -= 1f;

			for (int i = 0; i < 8; i++)
			{
				float degree = 360 / 8 * i;
				float radians = MathHelper.ToRadians(degree);
				float speed = 6f;
				Vector2 speedSquared = new Vector2(speed, speed);
				Vector2 velocity = speedSquared.RotatedBy(radians);
				int projID = Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, velocity, ModContent.ProjectileType<CherryShard>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner, ai2: 30);
			}

			if (Projectile.owner == Main.myPlayer)
			{
				int radius = 4;
				Vector2 center = Projectile.Center;
				int minX = (int)(center.X / 16f - (float)radius);
				int maxX = (int)(center.X / 16f + (float)radius);
				int minY = (int)(center.Y / 16f - (float)radius);
				int maxY = (int)(center.Y / 16f + (float)radius);
				if (minX < 0)
				{
					minX = 0;
				}
				if (maxX > Main.maxTilesX)
				{
					maxX = Main.maxTilesX;
				}
				if (minY < 0)
				{
					minY = 0;
				}
				if (maxY > Main.maxTilesY)
				{
					maxY = Main.maxTilesY;
				}
				Projectile.ExplodeCrackedTiles(center, radius, minX, maxX, minY, maxY);
			}
		}
	}
}
