using System;
using TheConfectionRebirth.Tiles;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Biomes
{
	public class ConfectionBiomeTileCount : ModSystem
	{
		public int confectionBlockCount;
		public int confectionSandBlockCount;

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) {
			confectionBlockCount = tileCounts[ModContent.TileType<Creamstone>()] 
				+ tileCounts[ModContent.TileType<CookieBlock>()] 
				+ tileCounts[ModContent.TileType<CreamGrass>()]
			    + tileCounts[ModContent.TileType<CreamBlock>()]
				+ tileCounts[ModContent.TileType<BlueIce>()];
			confectionSandBlockCount = tileCounts[ModContent.TileType<Creamsand>()]
				+ tileCounts[ModContent.TileType<HardenedCreamsand>()]
				+ tileCounts[ModContent.TileType<Creamsandstone>()];
		}
	}
}
