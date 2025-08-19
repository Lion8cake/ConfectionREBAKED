using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Dusts
{
	public class FairyFlossRainDust : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.noGravity = true;
			dust.noLight = true;
			dust.scale *= 1f;
		}

		public override bool Update(Dust dust) {
			dust.position += dust.velocity;

			if (dust.type == Type) {
				dust.velocity.Y *= 0.98f;
				dust.velocity.X *= 0.98f;
			}

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
	}
}
