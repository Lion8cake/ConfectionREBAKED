using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria;

namespace TheConfectionRebirth.Biomes
{
    public class SherbetWaterfallStyle : ConfectionModWaterfallStyle
    {
		public override bool PreDraw(int currentWaterfallData, int i, int j, SpriteBatch spriteBatch)
		{
			if (!Main.drewLava)
			{
				return false;
			}
			return true;
		}

		public override void ColorMultiplier(ref float r, ref float g, ref float b, float alpha)
		{
			r = TheConfectionRebirth.SherbR * alpha;
			g = TheConfectionRebirth.SherbG * alpha;
			b = TheConfectionRebirth.SherbB * alpha;
		}

		public override void AddLight(int i, int j)
		{
			float r = TheConfectionRebirth.SherbR / 255f;
			float g = TheConfectionRebirth.SherbG / 255f;
			float b = TheConfectionRebirth.SherbB / 255f;
			r *= 0.2f;
			g *= 0.2f;
			b *= 0.2f;
			Lighting.AddLight(i, j, r, g, b);
		}
	}
}