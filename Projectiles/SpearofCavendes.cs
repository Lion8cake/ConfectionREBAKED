using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent;
using System;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheConfectionRebirth.Projectiles
{
    public class SpearofCavendes : ModProjectile
    {
		protected virtual float HoldoutRangeMin => 98f;
		protected virtual float HoldoutRangeMax => 200f;

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 19;
            Projectile.penetrate = -1;
            Projectile.scale = 1.3f;
            Projectile.alpha = 0;

            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
        }

		public override bool PreAI() {
			Player player = Main.player[Projectile.owner];
			int duration = player.itemAnimationMax;

			player.heldProj = Projectile.whoAmI;

			if (Projectile.timeLeft > duration) {
				Projectile.timeLeft = duration;
			}

			Projectile.velocity = Vector2.Normalize(Projectile.velocity);

			float halfDuration = duration * 0.5f;
			float progress;

			if (Projectile.timeLeft < halfDuration) {
				progress = Projectile.timeLeft / halfDuration;
			}
			else {
				progress = (duration - Projectile.timeLeft) / halfDuration;
			}

			Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * this.HoldoutRangeMin, Projectile.velocity * this.HoldoutRangeMax, progress);

			if (Projectile.spriteDirection == -1) {
				Projectile.rotation += MathHelper.ToRadians(45f);
			}
			else {
				Projectile.rotation += MathHelper.ToRadians(135f);
			}

			if (Projectile.timeLeft == (int)halfDuration) {
				Projectile.NewProjectile(new EntitySource_Misc(""), Projectile.Center, Projectile.velocity * 15, ModContent.ProjectileType<SpearofCavendesBannana>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
			}
			SpearExtraLength(out var _);
			return false;
		}

		public bool SpearExtraLength(out Rectangle extensionBox) {
			extensionBox = default(Rectangle);
			Player player = Main.player[Projectile.owner];
			if (player.itemAnimation < player.itemAnimationMax / 3) {
				return false;
			}
			int itemAnimationMax = player.itemAnimationMax;
			int itemAnimation = player.itemAnimation;
			int num = player.itemAnimationMax / 3;
			float num2 = Utils.Remap(itemAnimation, itemAnimationMax, num, 0f, 1f);
			float num3 = 10f;
			float num4 = 30f;
			float num5 = 10f;
			float num6 = 10f;
			num4 = 60f;
			num6 = 25f; 
			num4 *= 1f / player.GetAttackSpeed(DamageClass.Melee);
			float num7 = num3 + num4 * num2;
			float num8 = num5 + num6 * num2;
			float f = Projectile.velocity.ToRotation();
			Vector2 center = Projectile.Center + f.ToRotationVector2() * num7;
			extensionBox = Utils.CenteredRectangle(center, new Vector2(num8, num8));
			return true;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			Rectangle val;
			Vector2 center = Projectile.Center;
			if (SpearExtraLength(out var extensionBox)) {
				Vector2 vector2 = extensionBox.Center.ToVector2();
				float num9 = Vector2.Distance(vector2, center);
				Vector2 size = extensionBox.Size();
				float num10 = MathHelper.Max(extensionBox.Width, extensionBox.Height);
				if (num10 < 12f) {
					num10 = 12f;
				}
				for (float num11 = num10; num11 < num9; num11 += num10) {
					val = Utils.CenteredRectangle(Vector2.Lerp(center, vector2, num11 / num9), size);
					if (val.Intersects(targetHitbox)) {
						return true;
					}
				}
				if (extensionBox.Intersects(targetHitbox)) {
					return true;
				}
			}
			return false;
		}

		public override bool PreDraw(ref Color lightColor) {
			SpriteEffects dir = (SpriteEffects)0;
			float num = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 2.355f;
			Asset<Texture2D> asset = TextureAssets.Projectile[Projectile.type];
			Player player = Main.player[Projectile.owner];
			Rectangle value = asset.Frame();
			Rectangle rect = Projectile.getRect();
			Vector2 vector = Vector2.Zero;
			if (player.direction > 0) {
				dir = (SpriteEffects)1;
				vector.X = asset.Width();
				num -= (float)Math.PI / 2f;
			}
			if (player.gravDir == -1f) {
				if (Projectile.direction == 1) {
					dir = (SpriteEffects)3;
					vector = new((float)asset.Width(), (float)asset.Height());
					num -= (float)Math.PI / 2f;
				}
				else if (Projectile.direction == -1) {
					dir = (SpriteEffects)2;
					vector = new(0f, (float)asset.Height());
					num += (float)Math.PI / 2f;
				}
			}
			Vector2.Lerp(vector, value.Center.ToVector2(), 0.25f);
			float num2 = 0f;
			Vector2 vector2 = Projectile.Center + new Vector2(0f, Projectile.gfxOffY);
			if (SpearExtraLength(out var extensionBox)) {
				Vector2 value2 = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false);
				Vector2 val = extensionBox.Size();
				float num8 = val.Length();
				val = Projectile.Hitbox.Size();
				float num3 = num8 / val.Length();
				_ = new Color(255, 255, 255, 0) * 1f;
				float num4 = Utils.Remap(player.itemAnimation, player.itemAnimationMax, (float)player.itemAnimationMax / 3f, 0f, 1f);
				float num5 = Utils.Remap(num4, 0f, 0.3f, 0f, 1f) * Utils.Remap(num4, 0.3f, 1f, 1f, 0f);
				num5 = 1f - (1f - num5) * (1f - num5);
				Vector2 vector3 = extensionBox.Center.ToVector2() + new Vector2(0f, Projectile.gfxOffY);
				Vector2.Lerp(value2, vector3, 1.1f);
				Texture2D value3 = TextureAssets.Extra[98].Value;
				Vector2 origin = value3.Size() / 2f;
				Color color = new Color(255, 255, 255, 0) * 0.5f;
				color = new(230, 196, 125, 25);
				float num6 = num - (float)Math.PI / 4f * (float)Projectile.spriteDirection;
				if (player.gravDir < 0f) {
					if (player.direction > 0) {
						num6 -= (float)Math.PI / 2f * ((float)Projectile.spriteDirection * MathHelper.Pi);
					}
					else {
					num6 -= (float)Math.PI / 2f * (float)Projectile.spriteDirection;
					}
				}
				if (player.direction > 0) {
					num6 = num - (float)Math.PI / 4f * ((float)Projectile.spriteDirection * MathHelper.Pi);
				}
				Main.EntitySpriteDraw(value3, Vector2.Lerp(vector3, vector2, 0.5f) - Main.screenPosition, null, color * num5, num6, origin, new Vector2(num5 * num3, num3) * Projectile.scale * num3, dir);
				Main.EntitySpriteDraw(value3, Vector2.Lerp(vector3, vector2, 1f) - Main.screenPosition, null, color * num5, num6, origin, new Vector2(num5 * num3, num3 * 1.5f) * Projectile.scale * num3, dir);
				Main.EntitySpriteDraw(value3, Vector2.Lerp(value2, vector2, num4 * 1.5f - 0.5f) - Main.screenPosition + new Vector2(0f, 2f), null, color * num5, num6, origin, new Vector2(num5 * num3 * 1f * num5, num3 * 2f * num5) * Projectile.scale * num3, dir);
				for (float num7 = 0.4f; num7 <= 1f; num7 += 0.1f) {
					Vector2 vector4 = Vector2.Lerp(value2, vector3, num7 + 0.2f);
					Main.EntitySpriteDraw(value3, vector4 - Main.screenPosition + new Vector2(0f, 2f), null, color * num5 * 0.75f * num7, num6, origin, new Vector2(num5 * num3 * 1f * num5, num3 * 2f * num5) * Projectile.scale * num3, dir);
				}
				extensionBox.Offset((int)(0f - Main.screenPosition.X), (int)(0f - Main.screenPosition.Y));
			}
			Main.EntitySpriteDraw(asset.Value, vector2 - Main.screenPosition, value, Projectile.GetAlpha(Lighting.GetColor((int)((double)Projectile.position.X + (double)Projectile.width * 0.5) / 16, (int)(((double)Projectile.position.Y + (double)Projectile.height * 0.5) / 16.0))), num, vector, Projectile.scale, dir);
			rect.Offset((int)(0f - Main.screenPosition.X), (int)(0f - Main.screenPosition.Y));
			Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, rect, Color.White * num2);
			return false;
		}
	}
}
