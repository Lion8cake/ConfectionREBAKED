using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
    public class SacchariteBlock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileLighted[Type] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<Creamstone>()] = true;
            ItemDrop = ModContent.ItemType<Items.Placeable.Saccharite>();
            AddMapEntry(new Color(32, 174, 221));
            DustType = ModContent.DustType<SacchariteCrystals>();
            HitSound = SoundID.Item27;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void RandomUpdate(int i, int j)
        {
            Tile tileBelow = Framing.GetTileSafely(i, j + 1);
            if (j > Main.rockLayer)
            {
                if (WorldGen.genRand.NextBool(20) && !tileBelow.HasTile && tileBelow.LiquidType != LiquidID.Lava)
                {
                    bool placeSaccharite = false;
                    int yTest = j;
                    while (yTest > j - 10)
                    {
                        Tile testTile = Framing.GetTileSafely(i, yTest);
                        if (testTile.BottomSlope)
                        {
                            break;
                        }
                        placeSaccharite = true;
                        break;
                    }
                    if (placeSaccharite)
                    {
                        tileBelow.TileType = Type;
                        tileBelow.HasTile = true;
                        WorldGen.SquareTileFrame(i, j + 1, true);
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendTileSquare(-1, i, j + 1, 3, TileChangeType.None);
                        }
                    }
                    if (placeSaccharite)
                    {
                        tileBelow.TileType = Type;
                        tileBelow.HasTile = true;
                        WorldGen.SquareTileFrame(i, j + 1, true);
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendTileSquare(+1, i, j - 1, 3, TileChangeType.None);
                        }
                    }
                }
            }
        }
    }
}