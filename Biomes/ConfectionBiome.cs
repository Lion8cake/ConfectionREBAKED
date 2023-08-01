using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Backgrounds;

namespace TheConfectionRebirth.Biomes;

public class ConfectionBiome : ModBiome
{
    public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("TheConfectionRebirth/CreamWaterStyle");

    public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;

	public override int BiomeTorchItemType => ModContent.ItemType<Items.Placeable.ConfectionTorch>();
	public override int BiomeCampfireItemType => ModContent.ItemType<Items.Placeable.ConfectionCampfire>();

	public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle
	{
		get
		{
			if (Main.LocalPlayer.ZoneDesert)
				return ModContent.GetInstance<SandConfectionSurfaceBiome>().SurfaceBackgroundStyle;
			else if (Main.LocalPlayer.ZoneSnow)
				return ModContent.GetInstance<IceConfectionSurfaceBiome>().SurfaceBackgroundStyle;
			else if (ConfectionWorldGeneration.confectionBG == 0) {
				return ModContent.GetInstance<ConfectionSurfaceBackgroundStyle>();
			}
			else if (ConfectionWorldGeneration.confectionBG == 1) {
				return ModContent.GetInstance<ConfectionSurface1BackgroundStyle>();
			}
			else if (ConfectionWorldGeneration.confectionBG == 2) {
				return ModContent.GetInstance<ConfectionSurface2BackgroundStyle>();
			}
			else if (ConfectionWorldGeneration.confectionBG == 3) {
				return ModContent.GetInstance<ConfectionSurface3BackgroundStyle>();
			}
			return ModContent.GetInstance<ConfectionSurfaceBackgroundStyle>();
		}
	}

	public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle
    {
		get
		{
			if (Main.LocalPlayer.ZoneSnow)
				return ModContent.GetInstance<IceConfectionSurfaceBiome>().UndergroundBackgroundStyle;

			return ModContent.GetInstance<Backgrounds.IceConfectionUndergroundBiome>();
		}
	}

	public override int Music
	{
		get
		{
			if (Main.LocalPlayer.ZoneRockLayerHeight)
				return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ConfectionUnderground");

			return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Confection");
		}
	}

	public override string MapBackground => BackgroundPath;

	public override string BackgroundPath
    {
		get
		{
			if (Main.LocalPlayer.ZoneRockLayerHeight && !Main.LocalPlayer.ZoneSnow && !Main.LocalPlayer.ZoneDesert)
				return "TheConfectionRebirth/Biomes/ConfectionUndergroundMapBackground";
			else if (Main.LocalPlayer.ZoneSnow && !Main.LocalPlayer.ZoneRockLayerHeight)
				return "TheConfectionRebirth/Biomes/ConfectionIceBiomeMapBackground";
			else if (Main.LocalPlayer.ZoneRockLayerHeight && Main.LocalPlayer.ZoneSnow)
				return "TheConfectionRebirth/Biomes/ConfectionUndergroundIceMapBackground";
			else if (Main.LocalPlayer.ZoneDesert && !Main.LocalPlayer.ZoneRockLayerHeight)
				return "TheConfectionRebirth/Biomes/ConfectionDesertBiomeMapBackground";
			else if (Main.LocalPlayer.ZoneRockLayerHeight && Main.LocalPlayer.ZoneDesert)
				return "TheConfectionRebirth/Biomes/ConfectionUndergroundDesertMapBackground";

			return "TheConfectionRebirth/Biomes/ConfectionBiomeMapBackground";
		}
	}

	public override string BestiaryIcon => "TheConfectionRebirth/Biomes/BestiaryIcon1";

	public override Color? BackgroundColor => base.BackgroundColor;

	public override bool IsBiomeActive(Player player) => ModContent.GetInstance<ConfectionBiomeTileCount>().confectionBlockCount >= 120 && (player.ZoneOverworldHeight || player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || player.ZoneSkyHeight);
}
