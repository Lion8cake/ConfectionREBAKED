using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Biomes
{
    public class IceConfectionUndergroundBiome : ModBiome
    {
        public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.Find<ModUndergroundBackgroundStyle>("TheConfectionRebirth/ConfectionSnowUGBackgroundStyle");

        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/UndergorundConfection");

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;

        public override string BestiaryIcon => "Biomes/BestiaryIcon4";
        public override string BackgroundPath => "Biomes/ConfectionUndergroundIceMapBackground";
        public override Color? BackgroundColor => base.BackgroundColor;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Confection Cave Ice");
        }

        public override bool IsBiomeActive(Player player)
        {
            return (player.ZoneRockLayerHeight || player.ZoneDirtLayerHeight) &&
                ModContent.GetInstance<ConfectionBiomeTileCount>().iceConfectionBlockCount >= 120 &&
                Math.Abs(player.position.ToTileCoordinates().X - Main.maxTilesX / 2) < Main.maxTilesX / 6;
        }
    }
}
