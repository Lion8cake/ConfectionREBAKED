using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Biomes
{
    public class IceConfectionUndergroundBiome : ModBiome
    {
        public override string BestiaryIcon => "Biomes/BestiaryIcon4";

        public override string BackgroundPath => "Biomes/ConfectionUndergroundIceMapBackground";

        public override Color? BackgroundColor => base.BackgroundColor;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Confection Ice");
        }
    }
}
