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
		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 13;
		}

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

			if (++Projectile.frameCounter >= 5) {
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Projectile.type])
					Projectile.frame = 0;
			}
			LightColor(out float r, out float g, out float b);
			Lighting.AddLight(Projectile.position, r, g, b);
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

		private void LightColor(out float red, out float green, out float blue) {
			red = 0;
			green = 0;
			blue = 0;
			switch (Projectile.frame) {
				case 0:
					red = 1.92f;
					green = 0.26f;
					blue = 0.26f;
					break;
				case 1:
					red = 1.93f;
					green = 0.68f;
					blue = 0.26f;
					break;
				case 2:
					red = 2.53f;
					green = 0.91f;
					blue = 0.03f;
					break;
				case 3:
					red = 2.52f;
					green = 1.58f;
					blue = 0.03f;
					break;
				case 4:
					red = 1.99f;
					green = 1.67f;
					blue = 0.15f;
					break;
				case 5:
					red = 1.04f;
					green = 1.57f;
					blue = 0.15f;
					break;
				case 6:
					red = 0.23f;
					green = 1.07f;
					blue = 0.29f;
					break;
				case 7:
					red = 0.23f;
					green = 1.06f;
					blue = 1.06f;
					break;
				case 8:
					red = 0.29f;
					green = 0.8f;
					blue = 1.31f;
					break;
				case 9:
					red = 0.37f;
					green = 0.57f;
					blue = 2.27f;
					break;
				case 10:
					red = 0.66f;
					green = 0.37f;
					blue = 2.26f;
					break;
				case 11:
					red = 1.01f;
					green = 0.36f;
					blue = 1.62f;
					break;
				case 12:
					red = 1.62f;
					green = 0.36f;
					blue = 1.58f;
					break;
			}
		}
	}
}