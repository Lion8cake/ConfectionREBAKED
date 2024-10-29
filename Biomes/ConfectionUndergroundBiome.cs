using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Backgrounds;

namespace TheConfectionRebirth.Biomes
{
    public class ConfectionUndergroundBiome : ModBiome
    {
        public override string BestiaryIcon => "TheConfectionRebirth/Biomes/BestiaryIcon2";

        public override string BackgroundPath => "TheConfectionRebirth/Biomes/ConfectionUndergroundMapBackground";

		public override string MapBackground => BackgroundPath;
	}
}
