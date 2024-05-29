using Microsoft.Xna.Framework;
using System.Numerics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Dusts
{
	public class SugarDust : ModDust
	{
		public override bool Update(Dust dust) {
			dust.position += dust.velocity;
			
			if (dust.type == Type) {
				dust.scale += 0.005f;
				dust.velocity.Y *= 0.94f;
				dust.velocity.X *= 0.94f;
				float num105 = dust.scale * 0.8f;
				if (num105 > 1f) {
					num105 = 1f;
				}
				Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num105 * 0.2f, num105 * 0.8f, num105 * 0.6f);
			}
			dust.velocity.X *= 0.99f;
			dust.rotation += dust.velocity.X * 0.5f;

			if (dust.fadeIn > 0f && dust.fadeIn < 100f) {
				dust.scale += 0.03f;
				if (dust.scale > dust.fadeIn) {
					dust.fadeIn = 0f;
				}
			}
			dust.scale -= 0.01f;
			if (dust.noGravity) {
				dust.velocity *= 0.92f;
				if (dust.fadeIn == 0f) {
					dust.scale -= 0.04f;
				}
			}
			if (dust.position.Y > Main.screenPosition.Y + (float)Main.screenHeight) {
				dust.active = false;
			}
			float num17 = 0.1f;
			if ((double)Dust.dCount == 0.5) {
				dust.scale -= 0.001f;
			}
			if ((double)Dust.dCount == 0.6) {
				dust.scale -= 0.0025f;
			}
			if ((double)Dust.dCount == 0.7) {
				dust.scale -= 0.005f;
			}
			if ((double)Dust.dCount == 0.8) {
				dust.scale -= 0.01f;
			}
			if ((double)Dust.dCount == 0.9) {
				dust.scale -= 0.02f;
			}
			if ((double)Dust.dCount == 0.5) {
				num17 = 0.11f;
			}
			if ((double)Dust.dCount == 0.6) {
				num17 = 0.13f;
			}
			if ((double)Dust.dCount == 0.7) {
				num17 = 0.16f;
			}
			if ((double)Dust.dCount == 0.8) {
				num17 = 0.22f;
			}
			if ((double)Dust.dCount == 0.9) {
				num17 = 0.25f;
			}
			if (dust.scale < num17) {
				dust.active = false;
			}
			return false;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor) {
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 25);
		}
	}
}
