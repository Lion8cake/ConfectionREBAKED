/*using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.WorldBuilding;
using Terraria.IO;
using TheConfectionRebirth.Tiles;

namespace TheConfectionRebirth.ModSupport.ExxoAvalonOrigins.World;

class ConfectedAltars
{
    public static void Generate()
    {
        Main.rand ??= new UnifiedRandom((int)DateTime.Now.Ticks);

        for (int a = 0; a < (int)(Main.maxTilesX * Main.maxTilesY * 0.3); a++)
        {
            int k = Main.rand.Next(100, Main.maxTilesX - 100);
            int l = Main.rand.Next((int)Main.rockLayer, Main.maxTilesY - 150);
            if (Main.tile[k, l].HasTile && (Main.tile[k - 1, l].HasTile && !Main.tile[k - 1, l].IsHalfBlock && Main.tile[k - 1, l].Slope == SlopeType.Solid) &&
                (Main.tile[k + 1, l].HasTile && !Main.tile[k + 1, l].IsHalfBlock && Main.tile[k + 1, l].Slope == SlopeType.Solid) &&
                Main.tile[k, l - 1].TileType != TileID.Containers && Main.tile[k, l - 1].TileType != TileID.Containers2 &&
                (Main.tile[k, l].TileType == ModContent.TileType<Creamstone>() || Main.tile[k, l].TileType == ModContent.TileType<Creamsand>() ||
                Main.tile[k, l].TileType == ModContent.TileType<Creamsandstone>() || Main.tile[k, l].TileType == ModContent.TileType<HardenedCreamsand>() ||
                Main.tile[k, l].TileType == ModContent.TileType<CreamGrass>() || Main.tile[k, l].TileType == ModContent.TileType<BlueIce>()) &&
                l < Main.maxTilesY - 200 && Main.rand.NextBool(2))
            {
                Utils.PlaceConfectedAltar(k, l - 1);
            }
        }
    }
    public static void Method(GenerationProgress progress, GameConfiguration configuration)
    {
        Main.rand ??= new UnifiedRandom((int)DateTime.Now.Ticks);

        for (int a = 0; a < (int)(Main.maxTilesX * Main.maxTilesY * 0.3); a++)
        {
            int k = Main.rand.Next(100, Main.maxTilesX - 100);
            int l = Main.rand.Next((int)Main.worldSurface, Main.maxTilesY - 150);
            if ((Main.tile[k, l].HasTile && Main.tileSolid[Main.tile[k, l].TileType] && !Main.tile[k, l].IsHalfBlock && Main.tile[k, l].Slope == SlopeType.Solid) &&
                (Main.tile[k - 1, l].HasTile && Main.tileSolid[Main.tile[k - 1, l].TileType] && !Main.tile[k - 1, l].IsHalfBlock && Main.tile[k - 1, l].Slope == SlopeType.Solid) &&
                (Main.tile[k + 1, l].HasTile && Main.tileSolid[Main.tile[k + 1, l].TileType] && !Main.tile[k + 1, l].IsHalfBlock && Main.tile[k + 1, l].Slope == SlopeType.Solid) &&
                Main.tile[k, l - 1].TileType != TileID.Containers && Main.tile[k, l - 1].TileType != TileID.Containers2 &&
                (Main.tile[k, l].TileType == ModContent.TileType<Creamstone>() || Main.tile[k, l].TileType == ModContent.TileType<Creamsand>() ||
                Main.tile[k, l].TileType == ModContent.TileType<HardenedCreamsand>() || Main.tile[k, l].TileType == ModContent.TileType<Creamsandstone>() ||
                Main.tile[k, l].TileType == ModContent.TileType<CreamGrass>() || Main.tile[k, l].TileType == ModContent.TileType<BlueIce>()) &&
                l < Main.maxTilesY - 200 && Main.rand.NextBool(2))
            {
                Utils.PlaceConfectedAltar(k, l - 1);
            }
        }
    }
}*/
