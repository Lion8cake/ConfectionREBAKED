using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Backgrounds;

namespace TheConfectionRebirth.Biomes
{
    public class SandConfectionUndergroundBiome : ModBiome
    {
        public override string BestiaryIcon => "TheConfectionRebirth/Biomes/BestiaryIcon6";

        public override string BackgroundPath => "TheConfectionRebirth/Biomes/ConfectionUndergroundDesertMapBackground";

        public override string MapBackground => BackgroundPath;
    }
}
