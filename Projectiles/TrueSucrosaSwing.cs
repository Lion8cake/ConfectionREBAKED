using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
	public class TrueSucrosaSwing : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.extraUpdates = 1;
			Projectile.timeLeft = 360;
			Projectile.hide = true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.ai[0] += 1f;
			float opacity = Utils.GetLerpValue(0f, 7f, Projectile.ai[0], true) * Utils.GetLerpValue(16f, 12f, Projectile.ai[0], true);
			Projectile.Opacity = opacity;
			Projectile.Center = player.RotatedRelativePoint(player.MountedCenter, false, false) + Projectile.velocity * (Projectile.ai[0] - 1f);
			Projectile.spriteDirection = ((Vector2.Dot(Projectile.velocity, Vector2.UnitX) < 0f) ? -1 : 1);
			if (Projectile.ai[0] >= 16f)
			{
				Projectile.Kill();
				return;
			}
			player.heldProj = Projectile.whoAmI;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 vector = Projectile.Center - Main.screenPosition;
			Asset<Texture2D> asset = TextureAssets.Projectile[Projectile.type];
			Rectangle rectangle = asset.Frame(1, 4, 0, 0, 0, 0);
			Vector2 origin = rectangle.Size() / 2f;
			float num = Projectile.scale * 1.1f;
			SpriteEffects effects = (Projectile.ai[0] >= 0f) ? SpriteEffects.None : SpriteEffects.FlipVertically;
			float num2 = Projectile.localAI[0] / Projectile.ai[1];
			float num3 = Utils.Remap(num2, 0f, 0.6f, 0f, 1f, true) * Utils.Remap(num2, 0.6f, 1f, 1f, 0f, true);
			float num4 = 0.975f;
			float amount = num3;
			float num5 = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);
			num5 = Utils.Remap(num5, 0.2f, 1f, 0f, 1f, true);
			Color value = Color.Lerp(new Color(224, 92, 165), new Color(203, 58, 128), amount);
			Main.spriteBatch.Draw(asset.Value, vector, new Rectangle?(rectangle), value * num5 * num3, Projectile.rotation + Projectile.ai[0] * MathHelper.PiOver4 * -1f * (1f - num2), origin, num, effects, 0f);
			Color value2 = Color.Lerp(new Color(177, 112, 71), new Color(27, 134, 169), amount);
			Color color = Color.Lerp(new Color(153, 96, 62), new Color(27, 124, 169), amount);
			Color value3 = Color.White * num3 * 0.5f;
			value3.A = (byte)(value3.A * (1f - num5));
			Color value4 = value3 * num5 * 0.5f;
			value4.G = (byte)(value4.G * num5);
			value4.B = (byte)(value4.R * (0.25f + num5 * 0.75f));
			Main.spriteBatch.Draw(asset.Value, vector, new Rectangle?(rectangle), value4 * 0.15f, Projectile.rotation + Projectile.ai[0] * 0.01f, origin, num, effects, 0f);
			Main.spriteBatch.Draw(asset.Value, vector, new Rectangle?(rectangle), color * num5 * num3 * 0.3f, Projectile.rotation, origin, num, effects, 0f);
			Main.spriteBatch.Draw(asset.Value, vector, new Rectangle?(rectangle), value2 * num5 * num3 * 0.5f, Projectile.rotation, origin, num * num4, effects, 0f);
			Main.spriteBatch.Draw(asset.Value, vector, new Rectangle?(asset.Frame(1, 4, 0, 3, 0, 0)), Color.White * 0.6f * num3, Projectile.rotation + Projectile.ai[0] * 0.01f, origin, num, effects, 0f);
			Main.spriteBatch.Draw(asset.Value, vector, new Rectangle?(asset.Frame(1, 4, 0, 3, 0, 0)), Color.White * 0.5f * num3, Projectile.rotation + Projectile.ai[0] * -0.05f, origin, num * 0.8f, effects, 0f);
			Main.spriteBatch.Draw(asset.Value, vector, new Rectangle?(asset.Frame(1, 4, 0, 3, 0, 0)), Color.White * 0.4f * num3, Projectile.rotation + Projectile.ai[0] * -0.1f, origin, num * 0.6f, effects, 0f);
			float scaleFactor = num * 0.75f;
			for (float num6 = 0f; num6 < 12f; num6 += 1f)
			{
				float num7 = Projectile.rotation + Projectile.ai[0] * num6 * -MathHelper.TwoPi * 0.025f + Utils.Remap(num2, 0f, 0.6f, 0f, 0.95504415f, true) * Projectile.ai[0];
				Vector2 drawpos = vector + num7.ToRotationVector2() * (asset.Width() * 0.5f - 6f) * num;
				float scale = num6 / 12f;
				DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawpos, new Color(255, 255, 255, 0) * num3 * scale, color, num2, 0f, 0.5f, 0.5f, 1f, num7, new Vector2(0f, Utils.Remap(num2, 0f, 1f, 3f, 0f, true)) * scaleFactor, Vector2.One * scaleFactor);
			}
			Vector2 drawpos2 = vector + (Projectile.rotation + Utils.Remap(num2, 0f, 0.6f, 0f, 0.95504415f, true) * Projectile.ai[0]).ToRotationVector2() * (asset.Width() * 0.5f - 4f) * num;
			DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawpos2, new Color(255, 255, 255, 0) * num3 * 0.5f, color, num2, 0f, 0.5f, 0.5f, 1f, 0f, new Vector2(2f, Utils.Remap(num2, 0f, 1f, 4f, 1f, true)) * scaleFactor, Vector2.One * scaleFactor);
			return false;
		}

		private static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawpos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness)
		{
			Texture2D value = TextureAssets.Extra[98].Value;
			Color color = shineColor * opacity * 0.5f;
			color.A = 0;
			Vector2 origin = value.Size() / 2f;
			Color color2 = drawColor * 0.5f;
			float num = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, true);
			Vector2 vector = new Vector2(fatness.X * 0.5f, scale.X) * num;
			Vector2 vector2 = new Vector2(fatness.Y * 0.5f, scale.Y) * num;
			color *= num;
			color2 *= num;
			Main.EntitySpriteDraw(value, drawpos, null, color, MathHelper.PiOver2 + rotation, origin, vector, dir, 0);
			Main.EntitySpriteDraw(value, drawpos, null, color, 0f + rotation, origin, vector2, dir, 0);
			Main.EntitySpriteDraw(value, drawpos, null, color2, MathHelper.PiOver2 + rotation, origin, vector * 0.6f, dir, 0);
			Main.EntitySpriteDraw(value, drawpos, null, color2, 0f + rotation, origin, vector2 * 0.6f, dir, 0);
		}
	}
}
