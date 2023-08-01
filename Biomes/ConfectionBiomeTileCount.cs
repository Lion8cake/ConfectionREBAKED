using System;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;

namespace TheConfectionRebirth.Biomes
{
    public class ConfectionBiomeTileCount : ModSystem
    {
        public int confectionBlockCount;
        public int desertOverlaytileCount;

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            Main.SceneMetrics.SnowTileCount += tileCounts[ModContent.TileType<CreamBlock>()]
                + tileCounts[ModContent.TileType<CreamBlock>()]
                + tileCounts[ModContent.TileType<BlueIce>()];

            Main.SceneMetrics.SandTileCount += tileCounts[ModContent.TileType<Creamsand>()]
                + tileCounts[ModContent.TileType<HardenedCreamsand>()]
                + tileCounts[ModContent.TileType<Creamsandstone>()];

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
