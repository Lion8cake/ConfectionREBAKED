using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Biomes
{
    public class SandConfectionSurfaceBiome : ModBiome
    {
        public override string BestiaryIcon => "Biomes/BestiaryIcon5";

        public override string BackgroundPath => "Biomes/ConfectionDesertBiomeMapBackground";

        public override Color? BackgroundColor => base.BackgroundColor;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Confection Desert");
        }
    }
}
