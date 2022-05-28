using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Backgrounds;

namespace TheConfectionRebirth.Biomes
{
    public class IceConfectionSurfaceBiome : ModBiome
    {
        public override string BestiaryIcon => "Biomes/BestiaryIcon3";

        public override string BackgroundPath => "Biomes/ConfectionIceBiomeMapBackground";

        public override Color? BackgroundColor => base.BackgroundColor;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Confection Ice Surface");
        }
    }
}
