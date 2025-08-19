using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
	public class BirdnanaLightPet : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			Main.projFrames[Type] = 4;
			Main.projPet[Type] = true;
			ProjectileID.Sets.LightPet[Type] = true;
		}

		public override void SetDefaults() 
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.timeLeft *= 5;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.aiStyle = -1;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (!player.active)
			{
				Projectile.active = false;
				return;
			}

			if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.BirdnanaLightPetBuff>()))
			{
				player.GetModPlayer<ConfectionPlayer>().lightnana = true;
			}
			if (player.GetModPlayer<ConfectionPlayer>().lightnana)
			{
				Projectile.timeLeft = 2;
			}

			//Movement
			Vector2 anchorPos = player.MountedCenter + new Vector2(player.direction * 34, -20f);
			Vector2 distance = anchorPos - Projectile.Center;
			float distanceSquared = distance.LengthSquared();

			if (distanceSquared > 1000f * 1000f || distanceSquared < 2f * 2f)
			{
				Projectile.Center = anchorPos;
				Projectile.velocity = Vector2.Zero;
			}

			if (distance != Vector2.Zero)
			{
				Projectile.velocity = distance * 0.1f * 2;
			}

			bool isGliding = Projectile.velocity.LengthSquared() > 10f * 10f;
			if (isGliding)
			{
				float rotationVel = Projectile.velocity.X * 0.04f + Projectile.velocity.Y * Projectile.spriteDirection * 0.01f;
				if (Math.Abs(Projectile.rotation - rotationVel) >= MathHelper.Pi)
				{
					Projectile.rotation += rotationVel < Projectile.rotation ? - MathHelper.TwoPi : MathHelper.TwoPi;
				}

				float rotationInertia = 12f;
				Projectile.rotation = (Projectile.rotation * (rotationInertia - 1f) + rotationVel) / rotationInertia;
			}
			else
			{
				if (Projectile.rotation > MathHelper.Pi)
				{
					Projectile.rotation -= MathHelper.TwoPi;
				}
				Projectile.rotation *= Projectile.rotation > -0.005f && Projectile.rotation < 0.005f ? 0f : 0.96f;
			}

			//Visuals
			Projectile.spriteDirection = Projectile.direction = player.direction;

			if (!Main.dedServ)
			{
				float r = 1.92f;
				float g = 1.38f;
				float b = 0.12f;
				Lighting.AddLight(Projectile.Center, r / 2f, g / 2f, b / 2f);
			}

			if (isGliding)
			{
				Projectile.frame = 0;
				if (Projectile.velocity.Y < -10f)
				{
					Projectile.frame = 3;
				}
			}
			else
			{
				Projectile.frameCounter++;
				if (Projectile.frameCounter > 6)
				{
					Projectile.frame++;
					Projectile.frameCounter = 0;
				}
				if (Projectile.frame > 3)
				{
					Projectile.frame = 0;
				}
			}
		}
	}
} 