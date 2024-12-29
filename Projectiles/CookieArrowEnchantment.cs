using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Projectiles
{
	public class CookieArrowEnchantment : GlobalProjectile
	{
		public override bool InstancePerEntity => true;

		public bool isEnchanted = false;

		public override bool PreDraw(Projectile projectile, ref Color lightColor)
		{
			if (isEnchanted)
			{
				Texture2D texture = (Texture2D)TextureAssets.Extra[98];
				Vector2 pos = projectile.Center - Main.screenPosition;
				Vector2 orig = texture.Size() / 2;
				Vector2 orig2 = new Vector2(texture.Width / 2, texture.Height / 3);
				Rectangle? frame = new Rectangle?();
				Color color = new Color(239, 219, 173, 127);
				Color color2 = new Color(168, 129, 74, 127);
				color *= 0.5f;
				color2 *= 0.5f;

				float scaler = (float)((double)Utils.GetLerpValue(15f, 30f, 25f, true) * (double)Utils.GetLerpValue(240f, 200f, 25f, true) * (1.0 + 0.200000002980232 * Math.Cos((double)Main.GlobalTimeWrappedHourly % 30.0 / 0.5 * 6.28318548202515 * 3.0)) * 0.800000011920929);
				Vector2 scale1 = new Vector2(1f, 2f) * 2 * scaler;
				Vector2 scale2 = new Vector2(0.6f, 0.7f) * 2 * scaler;
				Vector2 scale3 = new Vector2(projectile.scale * 1.4f, projectile.scale * 0.85f * 3f) * 2f * scaler;
				Vector2 scale4 = new Vector2(projectile.scale * 1.2f, projectile.scale * 1.8f) * 2f * scaler;

				Main.EntitySpriteDraw(texture, pos, frame, color, projectile.rotation, orig2, scale4, SpriteEffects.None);
				Main.EntitySpriteDraw(texture, pos, frame, color2, projectile.rotation, orig2, scale3, SpriteEffects.None);
				Main.EntitySpriteDraw(texture, pos, frame, color, 0f, orig, scale1, SpriteEffects.None);
				Main.EntitySpriteDraw(texture, pos, frame, color2, 0f, orig, scale1, SpriteEffects.None);
				Main.EntitySpriteDraw(texture, pos, frame, color, MathHelper.ToRadians(90f), orig, scale2, SpriteEffects.None);
				Main.EntitySpriteDraw(texture, pos, frame, color2, MathHelper.ToRadians(90f), orig, scale2, SpriteEffects.None);
			}
			return true;
		}

		public override void OnKill(Projectile projectile, int timeLeft)
		{
			if (isEnchanted)
			{
				Color color = new Color(168, 129, 74, 1);
				for (int i = 0; i < 8; i++)
				{
					float degree = 360 / 8 * i;
					float radians = MathHelper.ToRadians(degree);
					Vector2 velcoity = Vector2.One.RotatedBy(radians);
					int num = Dust.NewDust(projectile.Center, 1, 1, ModContent.DustType<TintableBakersDust>());
					Dust dust = Main.dust[num];
					dust.noGravity = true;
					dust.scale = 2f;
					dust.velocity = velcoity;
					dust.color = color;
				}
			}
		}
	}
}
