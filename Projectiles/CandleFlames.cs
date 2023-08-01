using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System;
using Terraria.GameContent;

namespace TheConfectionRebirth.Projectiles
{
	public class CandleFlames : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.alpha = 255; 
			Projectile.friendly = true; 
			Projectile.hostile = false; 
			Projectile.penetrate = 3; 
			Projectile.timeLeft = 90; 
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.extraUpdates = 2;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}

		public override void AI() {
			Projectile.localAI[0] += 1f;
			int num = 60;
			int num2 = 12;
			int num3 = num + num2;
			if (Projectile.localAI[0] >= (float)num3) {
				Projectile.Kill();
			}
			if (Projectile.localAI[0] >= (float)num) {
				Projectile.velocity *= 0.95f;
			}
			bool flag = Projectile.ai[0] == 1f;
			int num4 = 50;
			int num5 = num4;
			if (flag) {
				num4 = 0;
				num5 = num;
			}
			if (Projectile.localAI[0] < (float)num5 && Main.rand.NextFloat() < 0.25f) {
				short num6 = (short)(flag ? 135 : 6);
				Dust dust = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(60f, 60f) * Utils.Remap(Projectile.localAI[0], 0f, 72f, 0.5f, 1f), 4, 4, num6, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100);
				if (Main.rand.Next(4) == 0) {
					dust.noGravity = true;
					dust.scale *= 3f;
					dust.velocity.X *= 2f;
					dust.velocity.Y *= 2f;
				}
				else {
					dust.scale *= 1.5f;
				}
				dust.scale *= 1.5f;
				dust.velocity *= 1.2f;
				dust.velocity += Projectile.velocity * 1f * Utils.Remap(Projectile.localAI[0], 0f, (float)num * 0.75f, 1f, 0.1f) * Utils.Remap(Projectile.localAI[0], 0f, (float)num * 0.1f, 0.1f, 1f);
				dust.customData = 1;
			}
			if (num4 > 0 && Projectile.localAI[0] >= (float)num4 && Main.rand.NextFloat() < 0.5f) {
				Vector2 center = Main.player[Projectile.owner].Center;
				Vector2 vector = (Projectile.Center - center).SafeNormalize(Vector2.Zero).RotatedByRandom(0.19634954631328583) * 7f;
				short num7 = 31;
				Dust dust2 = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(50f, 50f) - vector * 2f, 4, 4, num7, 0f, 0f, 150, new Color(80, 80, 80));
				dust2.noGravity = true;
				dust2.velocity = vector;
				dust2.scale *= 1.1f + Main.rand.NextFloat() * 0.2f;
				dust2.customData = -0.3f - 0.15f * Main.rand.NextFloat();
			}

			if (Projectile.wet && !Projectile.lavaWet) {
				Projectile.Kill();
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			Projectile.damage = (int)(Projectile.damage * 0.8);
			target.AddBuff(BuffID.OnFire3, 1200);
        }

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			Projectile.damage = (int)(Projectile.damage * 0.8);
			target.AddBuff(BuffID.OnFire3, 999999999, false);
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			int num = (int)Utils.Remap(Projectile.localAI[0], 0f, 72f, 10f, 40f);
			hitbox.Inflate(num, num);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			if (!projHitbox.Intersects(targetHitbox)) {
				return false;
			}
			return Collision.CanHit(Projectile.Center, 0, 0, projHitbox.Center.ToVector2(), 0, 0);
		}

		public override bool PreDraw(ref Color lightColor) {
			bool flag = Projectile.ai[0] == 1f;
			float num = 60f;
			float num10 = 12f;
			float fromMax = num + num10;
			Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(255, 80, 20, 200);
			Color color2 = new Color(255, 255, 20, 70);
			Color color3 = Color.Lerp(new Color(255, 80, 20, 100), color2, 0.25f);
			Color color4 = new Color(80, 80, 80, 100);
			float num11 = 0.35f;
			float num12 = 0.7f;
			float num13 = 0.85f;
			float num14 = ((Projectile.localAI[0] > num - 10f) ? 0.175f : 0.2f);
			if (flag) {
				color = new Color(95, 120, 255, 200);
				color2 = new Color(50, 180, 255, 70);
				color3 = new Color(95, 160, 255, 100);
				color4 = new Color(33, 125, 202, 100);
			}
			int verticalFrames = 7;
			float num15 = Utils.Remap(Projectile.localAI[0], num, fromMax, 1f, 0f);
			float num2 = Math.Min(Projectile.localAI[0], 20f);
			float num3 = Utils.Remap(Projectile.localAI[0], 0f, fromMax, 0f, 1f);
			float num4 = Utils.Remap(num3, 0.2f, 0.5f, 0.25f, 1f);
			Rectangle rectangle = ((!flag) ? value.Frame(1, verticalFrames, 0, 3) : value.Frame(1, verticalFrames, 0, (int)Utils.Remap(num3, 0.5f, 1f, 3f, 5f)));
			/*if (!(num3 < 1f)) {
				return;
			}*/
			for (int i = 0; i < 2; i++) {
				for (float num5 = 1f; num5 >= 0f; num5 -= num14) {
					Color val = ((num3 < 0.1f) ? Color.Lerp(Color.Transparent, color, Utils.GetLerpValue(0f, 0.1f, num3, clamped: true)) : ((num3 < 0.2f) ? Color.Lerp(color, color2, Utils.GetLerpValue(0.1f, 0.2f, num3, clamped: true)) : ((num3 < num11) ? color2 : ((num3 < num12) ? Color.Lerp(color2, color3, Utils.GetLerpValue(num11, num12, num3, clamped: true)) : ((num3 < num13) ? Color.Lerp(color3, color4, Utils.GetLerpValue(num12, num13, num3, clamped: true)) : ((!(num3 < 1f)) ? Color.Transparent : Color.Lerp(color4, Color.Transparent, Utils.GetLerpValue(num13, 1f, num3, clamped: true))))))));
					float num6 = (1f - num5) * Utils.Remap(num3, 0f, 0.2f, 0f, 1f);
					Vector2 vector = Projectile.Center - Main.screenPosition + Projectile.velocity * (0f - num2) * num5;
					Color color5 = val * num6;
					Color color6 = color5;
					if (!flag) {
						color6.G = (byte)(int)(color6.G / 2);
						color6.B = (byte)(int)(color6.B / 2);
						color6.A = ((byte)Math.Min((float)(int)(color5.A) + 80f * num6, 255f));
						Utils.Remap(Projectile.localAI[0], 20f, fromMax, 0f, 1f);
					}
					float num7 = 1f / num14 * (num5 + 1f);
					float num8 = Projectile.rotation + num5 * ((float)Math.PI / 2f) + Main.GlobalTimeWrappedHourly * num7 * 2f;
					float num9 = Projectile.rotation - num5 * ((float)Math.PI / 2f) - Main.GlobalTimeWrappedHourly * num7 * 2f;
					switch (i) {
						case 0:
							Main.EntitySpriteDraw(value, vector + Projectile.velocity * (0f - num2) * num14 * 0.5f, rectangle, color6 * num15 * 0.25f, num8 + (float)Math.PI / 4f, rectangle.Size() / 2f, num4, (SpriteEffects)0);
							Main.EntitySpriteDraw(value, vector, rectangle, color6 * num15, num9, rectangle.Size() / 2f, num4, (SpriteEffects)0);
							break;
						case 1:
							if (!flag) {
								Main.EntitySpriteDraw(value, vector + Projectile.velocity * (0f - num2) * num14 * 0.2f, rectangle, color5 * num15 * 0.25f, num8 + (float)Math.PI / 2f, rectangle.Size() / 2f, num4 * 0.75f, (SpriteEffects)0);
								Main.EntitySpriteDraw(value, vector, rectangle, color5 * num15, num9 + (float)Math.PI / 2f, rectangle.Size() / 2f, num4 * 0.75f, (SpriteEffects)0);
							}
							break;
					}
				}
			}
			return false;
		}
	}

	/*public class spaz : GlobalProjectile {
		public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) {
			return entity.type == 101;
		}

		public override void SetDefaults(Projectile entity) {
			entity.aiStyle = -1;
		}

		public override void ModifyDamageHitbox(Projectile projectile, ref Rectangle hitbox) {
			int num = (int)Utils.Remap(projectile.localAI[0], 0f, 72f, 10f, 40f);
			hitbox.Inflate(num, num);
		}

		public override bool PreDraw(Projectile projectile, ref Color lightColor) {
			bool flag = projectile.ai[0] == 1f;
			float num = 60f;
			float num10 = 12f;
			float fromMax = num + num10;
			Texture2D value = TextureAssets.Projectile[85].Value;
			Color color = new Color(55, 200, 26, 200);
			Color color2 = new Color(218, 253, 9, 70);
			Color color3 = Color.Lerp(new Color(55, 200, 26, 100), color2, 0.25f);
			Color color4 = new Color(80, 80, 80, 100);
			float num11 = 0.35f;
			float num12 = 0.7f;
			float num13 = 0.85f;
			float num14 = ((projectile.localAI[0] > num - 10f) ? 0.175f : 0.2f);
			if (flag) {
				color = new Color(95, 120, 255, 200);
				color2 = new Color(50, 180, 255, 70);
				color3 = new Color(95, 160, 255, 100);
				color4 = new Color(33, 125, 202, 100);
			}
			int verticalFrames = 7;
			float num15 = Utils.Remap(projectile.localAI[0], num, fromMax, 1f, 0f);
			float num2 = Math.Min(projectile.localAI[0], 20f);
			float num3 = Utils.Remap(projectile.localAI[0], 0f, fromMax, 0f, 1f);
			float num4 = Utils.Remap(num3, 0.2f, 0.5f, 0.25f, 1f);
			Rectangle rectangle = ((!flag) ? value.Frame(1, verticalFrames, 0, 3) : value.Frame(1, verticalFrames, 0, (int)Utils.Remap(num3, 0.5f, 1f, 3f, 5f)));
			/*if (!(num3 < 1f)) {
				return;
			}*/
			/*for (int i = 0; i < 2; i++) {
				for (float num5 = 1f; num5 >= 0f; num5 -= num14) {
					Color val = ((num3 < 0.1f) ? Color.Lerp(Color.Transparent, color, Utils.GetLerpValue(0f, 0.1f, num3, clamped: true)) : ((num3 < 0.2f) ? Color.Lerp(color, color2, Utils.GetLerpValue(0.1f, 0.2f, num3, clamped: true)) : ((num3 < num11) ? color2 : ((num3 < num12) ? Color.Lerp(color2, color3, Utils.GetLerpValue(num11, num12, num3, clamped: true)) : ((num3 < num13) ? Color.Lerp(color3, color4, Utils.GetLerpValue(num12, num13, num3, clamped: true)) : ((!(num3 < 1f)) ? Color.Transparent : Color.Lerp(color4, Color.Transparent, Utils.GetLerpValue(num13, 1f, num3, clamped: true))))))));
					float num6 = (1f - num5) * Utils.Remap(num3, 0f, 0.2f, 0f, 1f);
					Vector2 vector = projectile.Center - Main.screenPosition + projectile.velocity * (0f - num2) * num5;
					Color color5 = val * num6;
					Color color6 = color5;
					if (!flag) {
						color6.G = (byte)(int)(color6.G / 2);
						color6.B = (byte)(int)(color6.B / 2);
						color6.A = ((byte)Math.Min((float)(int)(color5.A) + 80f * num6, 255f));
						Utils.Remap(projectile.localAI[0], 20f, fromMax, 0f, 1f);
					}
					float num7 = 1f / num14 * (num5 + 1f);
					float num8 = projectile.rotation + num5 * ((float)Math.PI / 2f) + Main.GlobalTimeWrappedHourly * num7 * 2f;
					float num9 = projectile.rotation - num5 * ((float)Math.PI / 2f) - Main.GlobalTimeWrappedHourly * num7 * 2f;
					switch (i) {
						case 0:
							Main.EntitySpriteDraw(value, vector + projectile.velocity * (0f - num2) * num14 * 0.5f, rectangle, color6 * num15 * 0.25f, num8 + (float)Math.PI / 4f, rectangle.Size() / 2f, num4, (SpriteEffects)0);
							Main.EntitySpriteDraw(value, vector, rectangle, color6 * num15, num9, rectangle.Size() / 2f, num4, (SpriteEffects)0);
							break;
						case 1:
							if (!flag) {
								Main.EntitySpriteDraw(value, vector + projectile.velocity * (0f - num2) * num14 * 0.2f, rectangle, color5 * num15 * 0.25f, num8 + (float)Math.PI / 2f, rectangle.Size() / 2f, num4 * 0.75f, (SpriteEffects)0);
								Main.EntitySpriteDraw(value, vector, rectangle, color5 * num15, num9 + (float)Math.PI / 2f, rectangle.Size() / 2f, num4 * 0.75f, (SpriteEffects)0);
							}
							break;
					}
				}
			}
			return false;
		}

		public override void AI(Projectile projectile) {
			projectile.localAI[0] += 1f;
			int num = 60;
			int num2 = 12;
			int num3 = num + num2;
			if (projectile.localAI[0] >= (float)num3) {
				projectile.Kill();
			}
			if (projectile.localAI[0] >= (float)num) {
				projectile.velocity *= 0.95f;
			}
			bool flag = projectile.ai[0] == 1f;
			int num4 = 50;
			int num5 = num4;
			if (flag) {
				num4 = 0;
				num5 = num;
			}
			if (projectile.localAI[0] < (float)num5 && Main.rand.NextFloat() < 0.25f) {
				short num6 = (short)(flag ? 135 : 75);
				/*Dust dust = Dust.NewDustDirect(projectile.Center + Main.rand.NextVector2Circular(60f, 60f) * Utils.Remap(projectile.localAI[0], 0f, 72f, 0.5f, 1f), 4, 4, num6, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100);
				if (Main.rand.Next(4) == 0) {
					dust.noGravity = true;
					dust.scale *= 3f;
					dust.velocity.X *= 2f;
					dust.velocity.Y *= 2f;
				}
				else {
					dust.scale *= 1.5f;
				}
				dust.scale *= 1.5f;
				dust.velocity *= 1.2f;
				dust.velocity += projectile.velocity * 1f * Utils.Remap(projectile.localAI[0], 0f, (float)num * 0.75f, 1f, 0.1f) * Utils.Remap(projectile.localAI[0], 0f, (float)num * 0.1f, 0.1f, 1f);
				dust.customData = 1;*/
			/*}
			if (num4 > 0 && projectile.localAI[0] >= (float)num4 && Main.rand.NextFloat() < 0.5f) {
				Vector2 center = Main.player[projectile.owner].Center;
				Vector2 vector = (projectile.Center - center).SafeNormalize(Vector2.Zero).RotatedByRandom(0.19634954631328583) * 7f;
				short num7 = 31;
				Dust dust2 = Dust.NewDustDirect(projectile.Center + Main.rand.NextVector2Circular(50f, 50f) - vector * 2f, 4, 4, num7, 0f, 0f, 150, new Color(80, 80, 80));
				dust2.noGravity = true;
				dust2.velocity = vector;
				dust2.scale *= 1.1f + Main.rand.NextFloat() * 0.2f;
				dust2.customData = -0.3f - 0.15f * Main.rand.NextFloat();
			}
		}
	}*/
}