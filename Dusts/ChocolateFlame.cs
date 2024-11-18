using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Dusts
{
    public class ChocolateFlame : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
			dust.velocity.Y = (float)Main.rand.Next(-10, 6) * 0.1f;
			dust.velocity.X *= 0.3f;
			dust.scale *= 0.7f;
		}
		public override bool MidUpdate(Dust dust)
		{
			if (!dust.noGravity)
			{
				dust.velocity.Y += 0.05f;
			}
			else
			{
				dust.velocity.Y = 0f;
			}
			if (!dust.noLight && !dust.noLightEmittence)
			{
				float num66 = dust.scale * 1.4f;
				if (num66 > 1f)
				{
					num66 = 1f;
				}
				Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num66 * 1.23f, num66 * 0.78f, num66 * 0.55f);
			}
			return false;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			Color newColor = lightColor;
			newColor = Color.Lerp(newColor, Color.White, 0.8f);
			return new Color((int)newColor.R, (int)newColor.G, (int)newColor.B, 25);
		}
	}
}