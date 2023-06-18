using TheConfectionRebirth.ModSupport.ExxoAvalonOrigins.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.ModSupport.ExxoAvalonOrigins.World;

class Utils
{
    public static void PlaceConfectedAltar(int x, int y, int style = 0)
    {
        if (x < 5 || x > Main.maxTilesX - 5 || y < 5 || y > Main.maxTilesY - 5)
        {
            return;
        }
        bool placeOrNot = true;
        int num = y - 1;
        for (int i = x - 1; i < x + 2; i++)
        {
            for (int j = num; j < y + 1; j++)
            {
                if (Main.tileCut[Main.tile[i, j].TileType] || Main.tile[i, j].TileType is TileID.SmallPiles or TileID.LargePiles or TileID.LargePiles2 or TileID.Stalactite)
                {
                    WorldGen.KillTile(i, j, noItem: true);
                }
            }
            for (int j2 = num; j2 < y + 1; j2++)
            {
                if (Main.tile[i, j2].HasTile) placeOrNot = false;
            }
        }
        if (placeOrNot)
        {
            short num2 = (short)(54 * style);
            Tile t0 = Main.tile[x - 1, y - 1];
            t0.HasTile = true;
            Main.tile[x - 1, y - 1].TileFrameY = 0;
            Main.tile[x - 1, y - 1].TileFrameX = num2;
            Main.tile[x - 1, y - 1].TileType = (ushort)ModContent.TileType<ConfectedAltar>();
            Tile t = Main.tile[x, y - 1];
            t.HasTile = true;
            Main.tile[x, y - 1].TileFrameY = 0;
            Main.tile[x, y - 1].TileFrameX = (short)(num2 + 18);
            Main.tile[x, y - 1].TileType = (ushort)ModContent.TileType<ConfectedAltar>();
            Tile t2 = Main.tile[x + 1, y - 1];
            t2.HasTile = true;
            Main.tile[x + 1, y - 1].TileFrameY = 0;
            Main.tile[x + 1, y - 1].TileFrameX = (short)(num2 + 36);
            Main.tile[x + 1, y - 1].TileType = (ushort)ModContent.TileType<ConfectedAltar>();
            Tile t3 = Main.tile[x - 1, y];
            t3.HasTile = true;
            Main.tile[x - 1, y].TileFrameY = 18;
            Main.tile[x - 1, y].TileFrameX = num2;
            Main.tile[x - 1, y].TileType = (ushort)ModContent.TileType<ConfectedAltar>();
            Tile t4 = Main.tile[x, y];
            t4.HasTile = true;
            Main.tile[x, y].TileFrameY = 18;
            Main.tile[x, y].TileFrameX = (short)(num2 + 18);
            Main.tile[x, y].TileType = (ushort)ModContent.TileType<ConfectedAltar>();
            Tile t5 = Main.tile[x + 1, y];
            t5.HasTile = true;
            Main.tile[x + 1, y].TileFrameY = 18;
            Main.tile[x + 1, y].TileFrameX = (short)(num2 + 36);
            Main.tile[x + 1, y].TileType = (ushort)ModContent.TileType<ConfectedAltar>();
        }
    }
}
