
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Dusts
{
	public class TintableBakersDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noLight = false;
			dust.noGravity = true;
		}

		public override bool MidUpdate(Dust dust)
		{
			if (!dust.noLight)
			{
				float r = dust.color.R * 0.01f;
				float g = dust.color.G * 0.01f;
				float b = dust.color.B * 0.01f;
				r /= 4; 
				g /= 4; 
				b /= 4;
				Lighting.AddLight(dust.position, r * dust.scale, g * dust.scale, b * dust.scale);
			}
			return true;
		}

		public override bool PreDraw(Dust dust)
		{
			Color color = dust.color;
			color.A = 0;
			Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Texture), dust.position - Main.screenPosition, (Rectangle?)dust.frame, color, dust.GetVisualRotation(), new Vector2(4f, 4f), dust.scale, (SpriteEffects)0, 0f);
			return false;
		}
	}
}
