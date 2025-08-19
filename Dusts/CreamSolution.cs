using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Dusts
{
    public class CreamSolution : ModDust
    {
		public override bool Update(Dust dust)
		{
			float scale = dust.scale * 0.1f;
			if (scale > 1f)
			{
				scale = 1f;
			}
			Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), scale, scale * 0.8f, scale * 0.4f);
			return true;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return new Color(200, 200, 200, 0);
		}
	}
}