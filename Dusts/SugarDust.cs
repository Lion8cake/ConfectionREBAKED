using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Dusts
{
	public class SugarDust : ModDust
	{
		/*public override bool Update(Dust dust) {
			dust.scale += 0.005f;
			//dust.velocity.Y *= 0.94f;
			//dust.velocity.X *= 0.94f;
			float num105 = dust.scale * 0.8f;
			if (num105 > 1f) {
				num105 = 1f;
			}
			Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num105 * 0.3f, num105 * 0.6f, num105);
			return false;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor) {
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 25);
		}*/

		public override bool Update(Dust dust) {

			dust.scale += 0.045f;
			var num67 = dust.scale * 0.4f;
			if (num67 > 1f) {
				num67 = 1f;
			}
			Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num67 * 0.55f, num67 * 0.9f, num67 * 0.25f);
			return true;
		}
		public override Color? GetAlpha(Dust dust, Color lightColor) {
			return new Color(255, 255, 255, 50);
		}
	}
}
