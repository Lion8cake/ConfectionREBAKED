using System;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;
using Terraria;
using Terraria.ObjectData;

namespace TheConfectionRebirth.Biomes
{
    public class ConfectionBiomeTileCount : ModSystem
    {
        public int confectionBlockCount;
        public int iceConfectionBlockCount;
        public int desertConfectionBlockCount;

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            confectionBlockCount = tileCounts[ModContent.TileType<CookieBlock>()]
                + tileCounts[ModContent.TileType<Creamstone>()]
                + tileCounts[ModContent.TileType<CreamGrass>()];
            
            desertConfectionBlockCount = tileCounts[ModContent.TileType<Creamsand>()]
                + tileCounts[ModContent.TileType<HardenedCreamsand>()]
                + tileCounts[ModContent.TileType<Creamsandstone>()];
        }

        /*private void ExportTileCountstoMain()
        {
            SnowTileCount += tileCounts[ModContent.TileType<CreamBlock>()]
                + tileCounts[ModContent.TileType<CreamBlock>()]
                + tileCounts[ModContent.TileType<BlueIce>()];
        }*/
    }
}
