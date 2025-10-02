using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;
using TheConfectionRebirth.Backgrounds;
using static Terraria.Graphics.Capture.CaptureBiome;

namespace TheConfectionRebirth.Biomes;

public class ConfectionBiome : ModBiome
{
    public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("TheConfectionRebirth/CreamWaterStyle");

    public override SceneEffectPriority Priority
	{
		get
		{
			if (Main.LocalPlayer.ZoneDesert && !Main.LocalPlayer.ZoneBeach)
			{
				return SceneEffectPriority.BiomeHigh;
			}
			return SceneEffectPriority.BiomeMedium;
		}
	}

	public override int BiomeTorchItemType => ModContent.ItemType<Items.Placeable.ConfectionTorch>();
	public override int BiomeCampfireItemType => ModContent.ItemType<Items.Placeable.ConfectionCampfire>();

	public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle
	{
		get
		{
			if (Main.LocalPlayer.ZoneDesert) {
				return ModContent.GetInstance<ConfectionSandSurfaceBackgroundStyle>();
			}
			else if (Main.LocalPlayer.ZoneSnow) {
				return ModContent.GetInstance<ConfectionSnowSurfaceBackgroundStyle>();
			}
			return ModContent.GetInstance<ConfectionSurfaceBackgroundStyle>();
		}
	}

	public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle
    {
		get
		{
			double num2 = Main.maxTilesY - 330;
			double num3 = (int)((num2 - Main.worldSurface) / 6.0) * 6;
			num2 = Main.worldSurface + num3 - 5.0;
			if (WorldGen.oceanDepths((int)(Main.screenPosition.X + (float)(Main.screenHeight / 2)) / 16, (int)(Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16)) {
				return ModContent.GetInstance<ConfectionUndergroundOceanBackgroundStyle>();
			}
			else if ((double)(Main.screenPosition.Y / 16f) > Main.rockLayer + 60.0 && (double)(Main.screenPosition.Y / 16f) < num2 - 60.0) {
				if (Main.player[Main.myPlayer].ZoneSnow) {
					return ModContent.GetInstance<ConfectionUndergroundSnowBackgroundStyle>();
				}
				return ModContent.GetInstance<ConfectionUndergroundBackgroundStyle>();
			}
			return null;
		}
	}

	public override int Music
	{
		get
		{
			if (Main.swapMusic)
			{
				if (Main.drunkWorld && !Main.remixWorld)
				{
					return UpdateAudio_DecideOnNewConfectionMusic();
				}
				else
				{
					return UpdateAudio_DecideOnTOWConfectionMusic();
				}
			}
			else if (!Main.gameMenu && Main.drunkWorld && !Main.remixWorld)
			{
				return UpdateAudio_DecideOnTOWConfectionMusic();
			}
			else
			{
				return UpdateAudio_DecideOnNewConfectionMusic();
			}
		}
	}

	private int UpdateAudio_DecideOnTOWConfectionMusic()
	{
		int newMusic = MusicID.OtherworldlyHallow;
		if ((double)Main.LocalPlayer.position.Y >= Main.worldSurface * 16.0 + (double)(Main.screenHeight / 2) && !WorldGen.oceanDepths((int)(Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16, (int)(Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16))
		{
			if (Main.remixWorld && (double)Main.LocalPlayer.position.Y >= Main.rockLayer * 16.0 + (double)(Main.screenHeight / 2))
			{
				if (Main.LocalPlayer.ZoneUndergroundDesert)
				{
					newMusic = MusicID.OtherworldlyDesert;
				}
				else if (Main.cloudAlpha > 0f)
				{
					newMusic = MusicID.OtherworldlyRain;
				}
				else
				{
					newMusic = MusicID.OtherworldlyHallow;
				}
			}
			else
			{
				newMusic = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/OtherworldlyConfectionUnderground");
			}
		}
		else if (Main.dayTime)
		{
			if (Main.cloudAlpha > 0f && !Main.gameMenu)
			{
				newMusic = MusicID.OtherworldlyRain;
			}
			else
			{
				newMusic = MusicID.OtherworldlyHallow;
			}
		}
		else if (Main._shouldUseStormMusic)
		{
			if (Main.bloodMoon)
			{
				newMusic = MusicID.OtherworldlyEerie;
			}
			else
			{
				newMusic = MusicID.OtherworldlyRain;
			}
		}
		else if (WorldGen.oceanDepths((int)(Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16, (int)(Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16))
		{
			if (Main.bloodMoon)
			{
				newMusic = MusicID.OtherworldlyEerie;
			}
			else
			{
				newMusic = MusicID.OtherworldlyOcean;
			}
		}
		else if (Main.LocalPlayer.ZoneDesert)
		{
			newMusic = MusicID.OtherworldlyDesert;
		}
		else if (Main.remixWorld)
		{
			newMusic = MusicID.OtherworldlySpace;
		}
		else if (!Main.dayTime)
		{
			if (Main.bloodMoon)
			{
				newMusic = MusicID.OtherworldlyEerie;
			}
			else if (Main.cloudAlpha > 0f && !Main.gameMenu)
			{
				newMusic = MusicID.OtherworldlyNight;
			}
			else
			{
				newMusic = MusicID.OtherworldlyNight;
			}
		}
		return newMusic;
	}

	private int UpdateAudio_DecideOnNewConfectionMusic()
	{
		int newMusic = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Confection");
		bool flag10 = Main.LocalPlayer.townNPCs > 2f;
		if (Main.SceneMetrics.ShadowCandleCount > 0 || Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].type == ItemID.ShadowCandle)
		{
			flag10 = false;
		}
		if ((double)Main.LocalPlayer.position.Y >= Main.worldSurface * 16.0 + (double)(Main.screenHeight / 2) && (Main.remixWorld || !WorldGen.oceanDepths((int)(Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16, (int)(Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16)))
		{
			if (Main.remixWorld && (double)Main.LocalPlayer.position.Y >= Main.rockLayer * 16.0 + (double)(Main.screenHeight / 2))
			{
				newMusic = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Confection");
			}
			else
			{
				newMusic = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ConfectionUnderground");
			}
		}
		else if (Main.dayTime)
		{
			if (Main._shouldUseStormMusic)
			{
				newMusic = MusicID.Monsoon;
			}
			else if (Main.cloudAlpha > 0f && !Main.gameMenu)
			{
				newMusic = MusicID.Rain;
			}
			else if (Main._shouldUseWindyDayMusic && !Main.remixWorld)
			{
				newMusic = MusicID.MorningRain;
			}
			else
			{
				newMusic = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Confection");
			}
		}
		else if (Main._shouldUseStormMusic)
		{
			if (Main.bloodMoon)
			{
				newMusic = MusicID.Eerie;
			}
			else
			{
				newMusic = MusicID.Monsoon;
			}
		}
		else if (WorldGen.oceanDepths((int)(Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16, (int)(Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16))
		{
			if (Main.bloodMoon)
			{
				newMusic = MusicID.Eerie;
			}
			else if (flag10)
			{
				if (Main.dayTime)
				{
					newMusic = MusicID.TownDay;
				}
				else
				{
					newMusic = MusicID.TownNight;
				}
			}
			else
			{
				newMusic = (Main.dayTime ? MusicID.Ocean : MusicID.OceanNight);
			}
		}
		else if (Main.LocalPlayer.ZoneDesert)
		{
			if ((double)Main.LocalPlayer.position.Y >= Main.worldSurface * 16.0)
			{
				int num6 = (int)(Main.LocalPlayer.Center.X / 16f);
				int num7 = (int)(Main.LocalPlayer.Center.Y / 16f);
				if (WorldGen.InWorld(num6, num7) && (WallID.Sets.Conversion.Sandstone[Main.tile[num6, num7].WallType] || WallID.Sets.Conversion.HardenedSand[Main.tile[num6, num7].WallType]))
				{
					newMusic = MusicID.UndergroundDesert;
				}
				else
				{
					newMusic = MusicID.Desert;
				}
			}
			else
			{
				newMusic = MusicID.Desert;
			}
		}
		else if (Main.remixWorld)
		{
			newMusic = (Main.dayTime ? MusicID.SpaceDay : MusicID.Space);
		}
		else if (!Main.dayTime)
		{
			if (Main.bloodMoon)
			{
				newMusic = MusicID.Eerie;
			}
			else if (Main.cloudAlpha > 0f && !Main.gameMenu)
			{
				newMusic = MusicID.Rain;
			}
			else
			{
				newMusic = MusicID.Night;
			}
		}
		return newMusic;
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

	//public override TileColorStyle TileColorStyle => TileColorStyle.Confection;
	//Why doesnt something like this exist, why must I have to IL edit to add my own tile colouring

	public override bool IsBiomeActive(Player player) => InModBiome(player);

	public static bool InModBiome(Player player) => CheckIfTileCountisNull() >= 125 && (player.ZoneOverworldHeight || player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || player.ZoneSkyHeight);

	private static int CheckIfTileCountisNull()
	{
		if (ModContent.GetInstance<ConfectionBiomeTileCount>() != null)
		{
			return ModContent.GetInstance<ConfectionBiomeTileCount>().confectionBlockCount;
		}
		return 0;
	}
}
