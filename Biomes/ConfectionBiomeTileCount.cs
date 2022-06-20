using System;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;

using AltLibrary.Common.AltBiomes;

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

            desertOverlaytileCount = tileCounts[ModContent.TileType<Creamsand>()]
                + tileCounts[ModContent.TileType<HardenedCreamsand>()]
                + tileCounts[ModContent.TileType<Creamsandstone>()];
        }
    }

    public class ConfectionTileCounter : ALBiomeTileCountModifier
    {
        public override int GetTileCount(ReadOnlySpan<int> TileCounts)
        {
            return TileCounts[ModContent.TileType<CookieBlock>()]
                + TileCounts[ModContent.TileType<Creamstone>()]
                + TileCounts[ModContent.TileType<CreamGrass>()]
                + TileCounts[ModContent.TileType<CreamBlock>()]
                + TileCounts[ModContent.TileType<BlueIce>()]
                + TileCounts[ModContent.TileType<Creamsand>()]
                + TileCounts[ModContent.TileType<HardenedCreamsand>()]
                + TileCounts[ModContent.TileType<Creamsandstone>()];
        }

        public override void OnReceiveModifiedTileCount(int TileCount)
        {
            ModContent.GetInstance<ConfectionBiomeTileCount>().confectionBlockCount = TileCount;
        }
    }
}
