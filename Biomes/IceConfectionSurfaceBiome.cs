using TheConfectionRebirth.Backgrounds;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Biomes
{
	public class IceConfectionSurfaceBiome : ModBiome
	{
		public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("TheConfectionRebirth/CreamWaterStyle");
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("TheConfectionRebirth/ConfectionSnowSurfaceBackgroundStyle");


		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sound/Music/Confection");

		public override string BestiaryIcon => "Biomes/BestiaryIcon3";
		public override string BackgroundPath => "Biomes/ConfectionIceBiomeMapBackground";
		public override Color? BackgroundColor => base.BackgroundColor;

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Confection Ice");
		}

		public override bool IsBiomeActive(Player player) {
			bool b1 = ModContent.GetInstance<ConfectionBiomeTileCount>().iceConfectionBlockCount >= 120;

			bool b2 = Math.Abs(player.position.ToTileCoordinates().X - Main.maxTilesX / 2) < Main.maxTilesX / 6;

			bool b3 = player.ZoneSkyHeight || player.ZoneOverworldHeight;
			return b1 && b2 && b3;
		}
	}
}
