using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace TheConfectionRebirth.Projectiles
{
	public class MeawzerPetProjectile : ModProjectile
	{
		public override void SetStaticDefaults() {
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, 0, 1)
				.WithOffset(-6, -8)
				.WithSpriteDirection(1)
				.WithCode(DelegateMethods.CharacterPreview.Float);
		}

		public override void SetDefaults() {
			Projectile.netImportant = true;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft *= 5;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.manualDirectionChange = true;
			Projectile.width = 44;
			Projectile.height = 40;
		}

		public override void AI() {
			Player player = Main.player[Projectile.owner];

			if (!player.dead && player.HasBuff(ModContent.BuffType<Buffs.MeawzerPet>())) {
				Projectile.timeLeft = 2;
			}
			if (player.dead) {
				Projectile.Kill();
				return;
			}
			Projectile.direction = (Projectile.spriteDirection = player.direction);
			float num1037 = 30f;
			float y7 = -20f;
			int num1038 = player.direction;
			num1037 = -40f;
			y7 = -40f;
			if (player.ownedProjectileCounts[895] > 0) {
				num1037 = 40f;
			}
			Vector2 vector66 = new((float)num1038 * num1037, y7);
			Vector2 vector67 = player.MountedCenter + vector66;
			float num1039 = Vector2.Distance(Projectile.Center, vector67);
			if (num1039 > 1000f) {
				Projectile.Center = player.Center + vector66;
			}
			Vector2 vector69 = vector67 - Projectile.Center;
			float num1040 = 4f;
			if (num1039 < num1040) {
				Projectile.velocity *= 0.25f;
			}
			if (vector69 != Vector2.Zero) {
				if (vector69.Length() < num1040) {
					Projectile.velocity = vector69;
				}
				else {
					Projectile.velocity = vector69 * 0.1f;
				}
			}
			if (num1039 > 50f) {
				Projectile.direction = (Projectile.spriteDirection = 1);
				if (Projectile.velocity.X < 0f) {
					Projectile.direction = (Projectile.spriteDirection = -1);
				}
			}
			if (Projectile.velocity.Length() > 6f) {
				float num1055 = Projectile.velocity.X * 0.05f;
				if (Math.Abs(Projectile.rotation - num1055) >= (float)Math.PI) {
					if (num1055 < Projectile.rotation) {
						Projectile.rotation -= (float)Math.PI * 2f;
					}
					else {
						Projectile.rotation += (float)Math.PI * 2f;
					}
				}
				float num1056 = 12f;
				Projectile.rotation = (Projectile.rotation * (num1056 - 1f) + num1055) / num1056;
			}
			if (Projectile.rotation > (float)Math.PI) {
				Projectile.rotation -= (float)Math.PI * 2f;
			}
			if (Projectile.rotation > -0.005f && Projectile.rotation < 0.005f) {
				Projectile.rotation = 0f;
			}
			else {
				Projectile.rotation *= 0.96f;
			}

			float f3 = Projectile.localAI[0] % ((float)Math.PI * 2f) - (float)Math.PI;
			float num1075 = (float)Math.IEEERemainder(Projectile.localAI[1], 1.0);
			if (num1075 < 0f) {
				num1075 += 1f;
			}
			float num4 = (float)Math.Floor(Projectile.localAI[1]);
			float max = 0.999f;
			float num5 = 0f;
			int num6 = 0;
			float amount2 = 0.1f;
			bool flag64 = player.velocity.Length() > 3f;
			int num7 = -1;
			int num8 = -1;
			float num9 = 300f;
			float num10 = 500f;
			for (int num11 = 0; num11 < 200; num11++) {
				NPC nPC9 = Main.npc[num11];
				if (!nPC9.active || !nPC9.chaseable || nPC9.dontTakeDamage || nPC9.immortal) {
					continue;
				}
				float num12 = Projectile.Distance(nPC9.Center);
				if (nPC9.friendly || nPC9.lifeMax <= 5) {
					if (num12 < num9 && !flag64) {
						num9 = num12;
						num8 = num11;
					}
				}
				else if (num12 < num10) {
					num10 = num12;
					num7 = num11;
				}
			}
			if (flag64) {
				num5 = Projectile.AngleTo(Projectile.Center + player.velocity);
				num6 = 1;
				num1075 = MathHelper.Clamp(num1075 + 0.05f, 0f, max);
				num4 += (float)Math.Sign(-10f - num4);
			}
			if (num7 != -1) {
				num5 = Projectile.AngleTo(Main.npc[num7].Center);
				num6 = 2;
				num1075 = MathHelper.Clamp(num1075 + 0.05f, 0f, max);
				num4 += (float)Math.Sign(-12f - num4);
			}
			else if (num8 != -1) {
				num5 = Projectile.AngleTo(Main.npc[num8].Center);
				num6 = 3;
				num1075 = MathHelper.Clamp(num1075 + 0.05f, 0f, max);
				num4 += (float)Math.Sign(6f - num4);
			}
			else if (Projectile.ai[0] > 0f) {
				num5 = Projectile.ai[1];
				num1075 = MathHelper.Clamp(num1075 + (float)Math.Sign(0.75f - num1075) * 0.05f, 0f, max);
				num6 = 4;
				num4 += (float)Math.Sign(10f - num4);
				if (Main.rand.Next(10) == 0) {
					int num13 = Dust.NewDust(Projectile.Center + f3.ToRotationVector2() * 6f * num1075 - Vector2.One * 4f, 8, 8, 204, 0f, 0f, 150, default(Color), 0.3f);
					Main.dust[num13].fadeIn = 0.75f;
					Dust dust20 = Main.dust[num13];
					Dust dust212 = dust20;
					dust212.velocity *= 0.1f;
				}
			}
			else {
				num5 = ((player.direction == 1) ? 0f : 3.14160275f);
				num1075 = MathHelper.Clamp(num1075 + (float)Math.Sign(0.75f - num1075) * 0.05f, 0f, max);
				num4 += (float)Math.Sign(0f - num4);
				amount2 = 0.12f;
			}
			Vector2 value11 = num5.ToRotationVector2();
			num5 = Vector2.Lerp(f3.ToRotationVector2(), value11, amount2).ToRotation();
			Projectile.localAI[0] = num5 + (float)num6 * ((float)Math.PI * 2f) + (float)Math.PI;
			Projectile.localAI[1] = num4 + num1075;
		}
	}
}
