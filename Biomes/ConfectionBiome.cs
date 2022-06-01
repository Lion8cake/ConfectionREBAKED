using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Backgrounds;

namespace TheConfectionRebirth.Biomes
{
    public class ConfectionBiome : ModBiome
    {
        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("TheConfectionRebirth/CreamWaterStyle");

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;

        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle
        {
            get
            {
                if (Main.LocalPlayer.ZoneDesert)
                {
                    return ModContent.GetInstance<ConfectionSandSurfaceBackgroundStyle>();
                }

                else if (Main.LocalPlayer.ZoneSnow)
                {
                    return ModContent.GetInstance<ConfectionSnowSurfaceBackgroundStyle>();
                }

                return ModContent.GetInstance<ConfectionSurfaceBackgroundStyle>();
            }
        }

        public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle
        {
            get
            {
                if (Main.LocalPlayer.ZoneSnow)
                {
                    return ModContent.GetInstance<ConfectionSnowUGBackgroundStyle>();
                }

                return ModContent.GetInstance<ConfectionUGBackgroundStyle>();
            }
        }

        public override int Music
        {
            get
            {
                if (Main.LocalPlayer.ZoneNormalUnderground || Main.LocalPlayer.ZoneNormalCaverns)
                {
                    return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ConfectionUnderground");
                }

                return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Confection");
            }
        }

        public override string BestiaryIcon
        {
            get
            {
                if (Main.LocalPlayer.ZoneNormalUnderground || Main.LocalPlayer.ZoneNormalCaverns)
                {
                    return "Biomes/BestiaryIcon2";
                }
                if (Main.LocalPlayer.ZoneSnow)
                {
                    return "Biomes/BestiaryIcon3";
                }
                if (Main.LocalPlayer.ZoneNormalUnderground || Main.LocalPlayer.ZoneNormalCaverns || Main.LocalPlayer.ZoneSnow)
                {
                    return "Biomes/BestiaryIcon4";
                }
                if (Main.LocalPlayer.ZoneDesert)
                {
                    return "Biomes/BestiaryIcon5";
                }
                if (Main.LocalPlayer.ZoneNormalUnderground || Main.LocalPlayer.ZoneNormalCaverns || Main.LocalPlayer.ZoneDesert)
                {
                    return "Biomes/BestiaryIcon6";
                }

                return "Biomes/BestiaryIcon1";
            }
        }

        public override string BackgroundPath
        {
            get
            {
                if (Main.LocalPlayer.ZoneNormalUnderground || Main.LocalPlayer.ZoneNormalCaverns)
                {
                    return "Biomes/ConfectionUndergroundMapBackground";
                }
                if (Main.LocalPlayer.ZoneSnow)
                {
                    return "Biomes/ConfectionIceBiomeMapBackground";
                }
                if (Main.LocalPlayer.ZoneNormalUnderground || Main.LocalPlayer.ZoneNormalCaverns || Main.LocalPlayer.ZoneSnow)
                {
                    return "Biomes/ConfectionUndergroundIceMapBackground";
                }
                if (Main.LocalPlayer.ZoneDesert)
                {
                    return "Biomes/ConfectionDesertBiomeMapBackground";
                }
                if (Main.LocalPlayer.ZoneNormalUnderground || Main.LocalPlayer.ZoneNormalCaverns || Main.LocalPlayer.ZoneDesert)
                {
                    return "Biomes/ConfectionUndergroundDesertMapBackground";
                }

                return "Biomes/ConfectionBiomeMapBackground";
            }
        }

        public override Color? BackgroundColor => base.BackgroundColor;

        /*public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Confection");
        }*/

        public override void SetStaticDefaults()
        {
            if (Main.LocalPlayer.ZoneNormalUnderground || Main.LocalPlayer.ZoneNormalCaverns)
            {
                DisplayName.SetDefault("The Confection Underground");
            }
            if (Main.LocalPlayer.ZoneSnow)
            {
                DisplayName.SetDefault("The Confection Snow");
            }
            if (Main.LocalPlayer.ZoneNormalUnderground || Main.LocalPlayer.ZoneNormalCaverns || Main.LocalPlayer.ZoneSnow)
            {
                DisplayName.SetDefault("The Confection Snow Underground");
            }
            if (Main.LocalPlayer.ZoneDesert)
            {
                DisplayName.SetDefault("The Confection Sand");
            }
            if (Main.LocalPlayer.ZoneNormalUnderground || Main.LocalPlayer.ZoneNormalCaverns || Main.LocalPlayer.ZoneDesert)
            {
                DisplayName.SetDefault("The Confection Sand Underground");
            }
            DisplayName.SetDefault("The Confection");
        }

        public override bool IsBiomeActive(Player player)
        {
            bool b1 = ModContent.GetInstance<ConfectionBiomeTileCount>().confectionBlockCount >= 120;

            bool b2 = Math.Abs(player.position.ToTileCoordinates().X - Main.maxTilesX / 2) < Main.maxTilesX / 6;

            return b1 && b2;
        }
    }
}
