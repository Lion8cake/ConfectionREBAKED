using Microsoft.Xna.Framework;
using Terraria.Audio;
using TheConfectionRebirth;
using TheConfectionRebirth.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace TheConfectionRebirth.Projectiles
{
	public class CherryShard : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = 2;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.ai[2] > 0)
			{
				Projectile.ai[2]--;
				return false;
			}
			return true;
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(in SoundID.Item14, Projectile.position);
			for (int i = 0; i < 7; i++)
			{
				int dustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default(Color), 1.5f);
				Dust dust = Main.dust[dustID];
				dust.velocity *= 0.8f;

				if (i % 3 == 0)
				{
					dustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default(Color), 2.5f);
					dust = Main.dust[dustID];
					dust.noGravity = true;
					dust.velocity *= 2.5f;
					dustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default(Color), 1.5f);
					dust = Main.dust[dustID];
					dust.velocity *= 1.5f;
				}
			}
			Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
			int goreID = Gore.NewGore(Projectile.GetSource_Death(), pos, default(Vector2), Main.rand.Next(61, 64));
			Gore gore = Main.gore[goreID];
			gore.velocity *= 0.2f;
			gore.velocity.X += Main.rand.Next(-1, 2);
			gore.velocity.Y += Main.rand.Next(-1, 2);
			Projectile.position.X += Projectile.width / 2;
			Projectile.position.Y += Projectile.height / 2;
			Projectile.width = 100;
			Projectile.height = 100;
			Projectile.position.X -= Projectile.width / 2;
			Projectile.position.Y -= Projectile.height / 2;
			Projectile.penetrate = -1;
			Projectile.Damage();
		}
	}
}