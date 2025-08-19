using TheConfectionRebirth.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;

namespace TheConfectionRebirth.Projectiles
{
	public class MeawzerSummonLazer : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 600;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 6;
		}

		public override bool PreAI()
		{
			if (Projectile.ai[0] == 0f)
			{
				Projectile.ai[0] = 1f;
				SoundEngine.PlaySound(SoundID.Item67 with
				{
					Volume = 0.2f
				}, Projectile.Center);
				if (Main.rand.NextBool(4))
					SoundEngine.PlaySound(SoundID.Item57 with
					{
						Volume = 0.4f
					}, Projectile.Center);
			}
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}

			float value = MathHelper.Lerp(0, 255, (float)Projectile.ai[1] / 255);
			Projectile.ai[1] += 15;
			if (Projectile.ai[1] > 255)
				Projectile.ai[1] = 255;
			Lighting.AddLight(Projectile.Center, new Vector3(value *= 0.015f) / 2f);
			Projectile.alpha = 255 - (int)Projectile.ai[1];
			return false;
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCDeath3 with
			{
				Volume = 0.5f
			}, Projectile.Center);
			for (int i = 0; i < 14; i++)
			{
				Vector2 position = Projectile.Center + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / 14 * i));
				Dust dust = Dust.NewDustPerfect(position, 66);
				dust.noGravity = true;
				dust.velocity = Vector2.Normalize(dust.position - Projectile.Center) * 2.75f;
				dust.noLight = false;
				dust.fadeIn = 1f;
				dust.scale = 0.5f;
			}
		}
	}
}