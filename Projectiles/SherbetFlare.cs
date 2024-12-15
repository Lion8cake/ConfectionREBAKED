using TheConfectionRebirth.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace TheConfectionRebirth.Projectiles
{
	public class SherbetFlare : ModProjectile
	{
		public override void SetDefaults() {
			Projectile.netImportant = true;
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.aiStyle = 33;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 36000;
			DrawOriginOffsetY = -10;
		}

		public override bool PreAI() {
			if (Projectile.alpha > 0) {
				Projectile.alpha -= 50;
				if (Projectile.alpha < 0) {
					Projectile.alpha = 0;
				}
			}
			float num279 = 4f;
			float num280 = Projectile.ai[0];
			float num281 = Projectile.ai[1];
			if (num280 == 0f && num281 == 0f) {
				num280 = 1f;
			}
			float num282 = (float)Math.Sqrt(num280 * num280 + num281 * num281);
			num282 = num279 / num282;
			num280 *= num282;
			num281 *= num282;
			if (Projectile.alpha < 70) {
				short num283 = (short)ModContent.DustType<SherbetDust>();
				int num284 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y - 2f), 6, 6, num283, Projectile.velocity.X, Projectile.velocity.Y, 100, default(Color), 1.6f);
				Main.dust[num284].noGravity = true;
				Main.dust[num284].position.X -= num280 * 1f;
				Main.dust[num284].position.Y -= num281 * 1f;
				Main.dust[num284].velocity.X -= num280;
				Main.dust[num284].velocity.Y -= num281;
				Main.dust[num284].color = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.6f % 1f, 1f, 0.5f);
				Dust dust155 = Main.dust[num284];
				Dust dust212 = dust155;
				dust212.scale *= 0.5f;
				dust155 = Main.dust[num284];
				dust212 = dust155;
				dust212.velocity *= 0.75f;
			}
			if (Projectile.localAI[0] == 0f) {
				Projectile.ai[0] = Projectile.velocity.X;
				Projectile.ai[1] = Projectile.velocity.Y;
				Projectile.localAI[1] += 1f;
				if (Projectile.localAI[1] >= 30f) {
					Projectile.velocity.Y += 0.09f;
					Projectile.localAI[1] = 30f;
				}
			}
			else {
				if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height)) {
					Projectile.localAI[0] = 0f;
					Projectile.localAI[1] = 30f;
				}
				Projectile.damage = 0;
			}
			if (Projectile.velocity.Y > 16f) {
				Projectile.velocity.Y = 16f;
			}
			Projectile.rotation = (float)Math.Atan2(Projectile.ai[1], Projectile.ai[0]) + 1.57f;

			Lighting.AddLight(Projectile.position, (float)(TheConfectionRebirth.SherbR) / 255, (float)(TheConfectionRebirth.SherbG) / 255, (float)(TheConfectionRebirth.SherbB) / 255);
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if (Main.rand.NextBool(3)) {
				target.AddBuff(24, 600);
			}
			else {
				target.AddBuff(24, 300);
			}
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info) {
			if (Main.rand.NextBool(3)) {
				target.AddBuff(24, 600, false);
			}
			else {
				target.AddBuff(24, 300, false);
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((byte)TheConfectionRebirth.SherbR, (byte)TheConfectionRebirth.SherbG, (byte)TheConfectionRebirth.SherbB, byte.MaxValue);
		}
	}
}