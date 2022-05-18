using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Biomes
{
    public class ConfectionUndergroundBiome : ModBiome
    {
        public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.Find<ModUndergroundBackgroundStyle>("TheConfectionRebirth/ConfectionUGBackgroundStyle");

        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/UndergorundConfection");

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;

        public override string BestiaryIcon => "Biomes/BestiaryIcon2";
        public override string BackgroundPath => "Biomes/ConfectionUndergroundMapBackground";
        public override Color? BackgroundColor => base.BackgroundColor;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Confection Underground");
        }

        public override bool IsBiomeActive(Player player)
        {
            return (player.ZoneRockLayerHeight || player.ZoneDirtLayerHeight) &&
                ModContent.GetInstance<ConfectionBiomeTileCount>().confectionBlockCount >= 40 &&
                Math.Abs(player.position.ToTileCoordinates().X - Main.maxTilesX / 2) < Main.maxTilesX / 6;
        }
    }
}
