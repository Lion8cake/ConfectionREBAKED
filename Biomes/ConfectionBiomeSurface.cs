using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Backgrounds;

namespace TheConfectionRebirth.Biomes;

public class ConfectionBiomeSurface : ModBiome
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
            if (Main.LocalPlayer.ZoneSkyHeight || Main.LocalPlayer.ZoneOverworldHeight)
            {
                return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Confection");
            }
            return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ConfectionUnderground");
        }
    }

    public override string MapBackground
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
            if (Main.LocalPlayer.ZoneNormalUnderground && Main.LocalPlayer.ZoneSnow || Main.LocalPlayer.ZoneNormalCaverns && Main.LocalPlayer.ZoneSnow)
            {
                return "Biomes/ConfectionUndergroundIceMapBackground";
            }
            if (Main.LocalPlayer.ZoneDesert)
            {
                return "Biomes/ConfectionDesertBiomeMapBackground";
            }
            if (Main.LocalPlayer.ZoneNormalUnderground && Main.LocalPlayer.ZoneDesert || Main.LocalPlayer.ZoneNormalCaverns && Main.LocalPlayer.ZoneDesert)
            {
                return "Biomes/ConfectionUndergroundDesertMapBackground";
            }

            return "Biomes/ConfectionBiomeMapBackground";
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
        return ModContent.GetInstance<ConfectionBiomeTileCount>().confectionBlockCount >= 120;
    }
}
