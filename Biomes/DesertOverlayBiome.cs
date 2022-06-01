using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;
using TheConfectionRebirth.Backgrounds;

namespace TheConfectionRebirth.Biomes
{
	public class DesertOverlayBiome : ModBiome
	{
		public override ModWaterStyle WaterStyle => ModContent.GetInstance<CreamWaterStyle>();
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<ConfectionSandSurfaceBackgroundStyle>();

		public override bool IsBiomeActive(Player player) {
			bool b1 = ModContent.GetInstance<ConfectionBiomeTileCount>().desertOverlaytileCount >= 1200;

			bool b2 = Math.Abs(player.position.ToTileCoordinates().X - Main.maxTilesX / 2) < Main.maxTilesX / 6;

			bool b3 = player.ZoneSkyHeight || player.ZoneOverworldHeight;
			return b1 && b2 && b3;
		}
	}
}
