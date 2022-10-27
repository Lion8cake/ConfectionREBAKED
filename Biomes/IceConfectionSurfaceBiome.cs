using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Biomes
{
    public class IceConfectionSurfaceBiome : ModBiome
    {
        public override string BestiaryIcon => "TheConfectionRebirth/Biomes/BestiaryIcon3";

        public override string BackgroundPath => "TheConfectionRebirth/Biomes/ConfectionIceBiomeMapBackground";

        public override Color? BackgroundColor => base.BackgroundColor;

        public override string MapBackground => BackgroundPath;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Confection Ice Surface");
        }

        public override bool IsBiomeActive(Player player) => ModContent.GetInstance<ConfectionBiomeSurface>().IsBiomeActive(player) && player.ZoneSnow && (player.ZoneOverworldHeight || player.ZoneDirtLayerHeight || player.ZoneSkyHeight);
    }
}
