using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Backgrounds;

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
