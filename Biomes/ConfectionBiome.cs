using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Backgrounds;
using Terraria.ID;
using Terraria.Graphics.Capture;
using static Terraria.Graphics.Capture.CaptureBiome;

namespace TheConfectionRebirth.Biomes;

public class ConfectionBiome : ModBiome
{
    public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("TheConfectionRebirth/CreamWaterStyle");

    public override SceneEffectPriority Priority
	{
		get
		{
			if (Main.LocalPlayer.ZoneDesert)
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
			bool TOWMusicCheck = (bool)typeof(Main).GetField("swapMusic", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			if (TOWMusicCheck == false && !Main.drunkWorld || TOWMusicCheck == true && Main.drunkWorld) {
				if ((double)Main.LocalPlayer.position.Y >= Main.worldSurface * 16.0 + (double)(Main.screenHeight / 2) && (Main.remixWorld || !WorldGen.oceanDepths((int)(Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16, (int)(Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16))) {
					if (Main.remixWorld && (double)Main.LocalPlayer.position.Y >= Main.rockLayer * 16.0 + (double)(Main.screenHeight / 2)) {
						return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Confection");
					}
					else {
						return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ConfectionUnderground");
					}
				}
				else if (Main.dayTime) {

					if (Main._shouldUseStormMusic) {
						return MusicID.Monsoon;
					}
					else if (Main.cloudAlpha > 0f && !Main.gameMenu) {
						return MusicID.Rain;
					}
					else if (Main._shouldUseWindyDayMusic && !Main.remixWorld) {
						return MusicID.WindyDay;
					}
					else {
						return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Confection");
					}
				}
				else {
					if (Main.bloodMoon) {
						return MusicID.Eerie;
					}
					else {
						return MusicID.Night;
					}
				}
			}
			else if (TOWMusicCheck == true && !Main.drunkWorld || TOWMusicCheck == false && Main.drunkWorld) {
				if ((double)Main.player[Main.myPlayer].position.Y >= Main.worldSurface * 16.0 + (double)(Main.screenHeight / 2) && !WorldGen.oceanDepths((int)(Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16, (int)(Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16)) {
					if (Main.remixWorld && (double)Main.player[Main.myPlayer].position.Y >= Main.rockLayer * 16.0 + (double)(Main.screenHeight / 2)) {
						if (Main.cloudAlpha > 0f) {
							return MusicID.OtherworldlyRain;
						}
						return MusicID.OtherworldlyHallow;
					}
					return MusicID.OtherworldlyUGHallow;
				}
				else if (Main.dayTime) {
					if (Main.cloudAlpha > 0f && !Main.gameMenu) {
						return MusicID.OtherworldlyRain;
					}
					else {
						return MusicID.OtherworldlyHallow;
					}
				}
				else if (Main._shouldUseStormMusic) {
					if (Main.bloodMoon) {
						return MusicID.OtherworldlyEerie;
					}
					else {
						return MusicID.OtherworldlyRain;
					}
				}
				else {
					if (Main.bloodMoon) {
						return MusicID.OtherworldlyEerie;
					}
					else {
						return MusicID.OtherworldlyHallow;
					}
				}
			}
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

	//public override TileColorStyle TileColorStyle => TileColorStyle.Confection;
	//Why doesnt something like this exist, why must I have to IL edit to add my own tile colouring

	public override bool IsBiomeActive(Player player) => ModContent.GetInstance<ConfectionBiomeTileCount>().confectionBlockCount >= 125 && (player.ZoneOverworldHeight || player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || player.ZoneSkyHeight);
}
