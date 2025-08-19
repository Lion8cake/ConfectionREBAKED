using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Projectiles
{
	public class ParfaitSpike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 2;
		}

		public override void SetDefaults()
        {
			Projectile.alpha = 255;
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.aiStyle = -1;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
		}

		public override bool? CanCutTiles() {
			return false;
		}

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.frame = Main.rand.Next(0, 2);
		}

		public override void AI()
		{
			if (Projectile.alpha == 0) {
				int num215 = Dust.NewDust(Projectile.oldPosition, Projectile.width, Projectile.height, ModContent.DustType<CreamDust>(), 0f, 0f, 100, default(Color), 0.5f);
				Main.dust[num215].noGravity = true;
				Main.dust[num215].noLight = true;
				Dust obj31 = Main.dust[num215];
				obj31.velocity *= 0.15f;
				Main.dust[num215].fadeIn = 0.8f;
			}
			Projectile.alpha -= 50;
			if (Projectile.alpha < 0) {
				Projectile.alpha = 0;
			}
			if (Projectile.ai[1] == 0f) {
				Projectile.ai[1] = 1f;
				SoundEngine.PlaySound(in SoundID.Item17, Projectile.position);
			}
			
			bool flag3 = true;
			bool flag4 = false;
			if (flag3) {
				Projectile.ai[0] += 1f;
			}
			if (Projectile.ai[0] >= 15f) {
				Projectile.ai[0] = 15f;
				Projectile.velocity.Y += 0.05f;
			}
			if (Projectile.ai[0] >= 15f) {
				Projectile.ai[0] = 15f;
				if (flag4) {
					Projectile.velocity.Y -= 0.1f;
				}
				else {
					Projectile.velocity.Y += 0.1f;
				}
			}
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
			bool flag7 = true;
			if (flag7) {
				if (flag4 && Projectile.velocity.Y < -16f) {
					Projectile.velocity.Y = -16f;
				}
				if (Projectile.velocity.Y > 16f) {
					Projectile.velocity.Y = 16f;
				}
			}
		}
	}
}
