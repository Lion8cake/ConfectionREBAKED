using System;
using TheConfectionRebirth.Tiles;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Biomes
{
	public class ConfectionSnowBiomeTileCount : ModSystem
	{
		public int confectionSnowBlockCount;

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) {
			confectionSnowBlockCount = tileCounts[ModContent.TileType<CreamBlock>()]
				+ tileCounts[ModContent.TileType<BlueIce>()];
		}
	}
}
