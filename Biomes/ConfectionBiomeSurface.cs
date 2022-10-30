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
				return ModContent.GetInstance<SandConfectionSurfaceBiome>().SurfaceBackgroundStyle;
			else if (Main.LocalPlayer.ZoneSnow)
				return ModContent.GetInstance<IceConfectionSurfaceBiome>().SurfaceBackgroundStyle;

			return ModContent.GetInstance<ConfectionSurfaceBackgroundStyle>();
		}
	}

	public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.GetInstance<ConfectionUGBackgroundStyle>();

	public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Confection");

	public override string MapBackground => BackgroundPath;

	public override string BackgroundPath => "TheConfectionRebirth/Biomes/ConfectionBiomeMapBackground";

	public override string BestiaryIcon => "TheConfectionRebirth/Biomes/BestiaryIcon1";

	public override Color? BackgroundColor => base.BackgroundColor;

	public override bool IsBiomeActive(Player player) => ModContent.GetInstance<ConfectionBiomeTileCount>().confectionBlockCount >= 120 && (player.ZoneOverworldHeight || player.ZoneDirtLayerHeight || player.ZoneSkyHeight);
}
