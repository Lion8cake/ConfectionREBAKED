using AvalonTesting.Items.Weapons.Throw;
using AvalonTesting.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AvalonTesting.Players;
using AvalonTesting;
using TheConfectionRebirth.ModSupport.ExxoAvalonOrigins.Tiles;

namespace TheConfectionRebirth.ModSupport.ExxoAvalonOrigins;

public class AvalonGlobalTile : GlobalTile
{
    public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (Main.tile[i, j - 1].TileType == ModContent.TileType<ConfectedAltar>() && Main.tile[i, j].TileType != ModContent.TileType<ConfectedAltar>())
        {
            fail = true;
        }
    }

    public override bool Slope(int i, int j, int type)
    {
        if (Main.tile[i, j - 1].HasTile && Main.tile[i, j - 1].TileType == ModContent.TileType<ConfectedAltar>())
        {
            return false;
        }

        return base.Slope(i, j, type);
    }
}
