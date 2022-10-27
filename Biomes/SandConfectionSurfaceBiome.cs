using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Backgrounds;

namespace TheConfectionRebirth.Biomes
{
    public class SandConfectionSurfaceBiome : ModBiome
    {
        public override string BestiaryIcon => "TheConfectionRebirth/Biomes/BestiaryIcon5";

        public override string BackgroundPath => "TheConfectionRebirth/Biomes/ConfectionDesertBiomeMapBackground";

        public override Color? BackgroundColor => base.BackgroundColor;

		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<ConfectionSandSurfaceBackgroundStyle>();

        public override string MapBackground => BackgroundPath;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Confection Desert");
        }

        public override bool IsBiomeActive(Player player) => ModContent.GetInstance<ConfectionBiomeSurface>().IsBiomeActive(player) && player.ZoneDesert && (player.ZoneOverworldHeight || player.ZoneDirtLayerHeight || player.ZoneSkyHeight);
    }
}
