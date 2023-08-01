using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Dusts
{
    public class NeapoliniteStrikeDust : ModDust
    {
		public override void OnSpawn(Dust dust) {
			dust.noLight = true;
			dust.scale *= 1f;
		}

		public override bool Update(Dust dust) {
			dust.rotation += 0.1f * dust.scale;
			Color color2 = Lighting.GetColor((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f));
			float num86 = color2.R / 270f;
			float num87 = color2.G / 270f;
			float num88 = color2.B / 270f;
			float num89 = dust.color.R / 255f;
			float num90 = dust.color.G / 255f;
			float num92 = dust.color.B / 255f;
			num86 *= dust.scale * 1.07f * num89;
			num87 *= dust.scale * 1.07f * num90;
			num88 *= dust.scale * 1.07f * num92;
			if (dust.alpha < 255) {
				dust.scale += 0.09f;
				if (dust.scale >= 1f) {
					dust.scale = 1f;
					dust.alpha = 255;
				}
			}
			else {
				if ((double)dust.scale < 0.8) {
					dust.scale -= 0.01f;
				}
				if ((double)dust.scale < 0.5) {
					dust.scale -= 0.01f;
				}
			}
			if ((double)num86 < 0.05 && (double)num87 < 0.05 && (double)num88 < 0.05) {
				dust.active = false;
			}
			if (dust.customData != null && dust.customData is Player) {
				Player player8 = (Player)dust.customData;
				dust.position += player8.position - player8.oldPosition;
			}
			return false;
		}
	}
}