using System;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;

namespace TheConfectionRebirth.Biomes
{
    public class ConfectionBiomeTileCount : ModSystem
    {
        public int confectionBlockCount;
        public int snowpylonConfectionCount;
		public int desertpylonConfectionCount;

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
			snowpylonConfectionCount = tileCounts[ModContent.TileType<CreamBlock>()]
				+ tileCounts[ModContent.TileType<BlueIce>()];

			desertpylonConfectionCount = tileCounts[ModContent.TileType<Creamsand>()]
				+ tileCounts[ModContent.TileType<Creamsandstone>()]
				+ tileCounts[ModContent.TileType<HardenedCreamsand>()];

			confectionBlockCount = tileCounts[ModContent.TileType<CookieBlock>()]
                + tileCounts[ModContent.TileType<Creamstone>()]
                + tileCounts[ModContent.TileType<CreamGrass>()]
                + tileCounts[ModContent.TileType<CreamBlock>()]
                + tileCounts[ModContent.TileType<BlueIce>()]
                + tileCounts[ModContent.TileType<Creamsand>()]
                + tileCounts[ModContent.TileType<HardenedCreamsand>()]
                + tileCounts[ModContent.TileType<Creamsandstone>()];
        }
    }
}
