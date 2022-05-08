using TheConfectionRebirth.Dusts;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TheConfectionRebirth.Waters
{
	public class CreamWaterStyle : ModWaterStyle
	{
		public override int ChooseWaterfallStyle() => Find<ModWaterfallStyle>("TheConfectionRebirth/CreamWaterfallStyle").Slot;

		public override int GetSplashDust() => DustType<CreamWaterSplash>();

		public override int GetDropletGore() => Find<ModGore>("CreamDroplet").Type;

		public override void LightColorMultiplier(ref float r, ref float g, ref float b) {
			r = 1f;
			g = 1f;
			b = 1f;
		}

		public override Color BiomeHairColor()
			=> Color.White;
	}
}