using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Biomes
{
    public class IceConfectionUndergroundBiome : ModBiome
    {
        public override string BestiaryIcon => "TheConfectionRebirth/Biomes/BestiaryIcon4";

        public override string BackgroundPath => "TheConfectionRebirth/Biomes/ConfectionUndergroundIceMapBackground";

        public override string MapBackground => BackgroundPath;
    }
}
