using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Biomes
{
    public class ConfectionUndergroundBiome : ModBiome
    {
        public override string BestiaryIcon => "Biomes/BestiaryIcon2";

        public override string BackgroundPath => "Biomes/ConfectionUndergroundMapBackground";

        public override Color? BackgroundColor => base.BackgroundColor;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Confection Underground");
        }
    }
}
