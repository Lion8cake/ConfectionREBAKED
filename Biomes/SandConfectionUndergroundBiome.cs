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

        public override Color? BackgroundColor => base.BackgroundColor;

		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ConfectionUnderground");

		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<ConfectionSnowSurfaceBackgroundStyle>();

        public override string MapBackground => BackgroundPath;

        public override bool IsBiomeActive(Player player) => ModContent.GetInstance<ConfectionBiomeTileCount>().confectionBlockCount >= 120 && player.ZoneUndergroundDesert;
    }
}
