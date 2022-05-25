using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles
{
    public class CreamGrass : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            this.SetModTree(new Trees.CreamTree());
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamGrass").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CookieBlock").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("Creamstone").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamGrassMowed").Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBrick[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            TileID.Sets.Grass[Type] = false;
            TileID.Sets.ChecksForMerge[Type] = true;
            AddMapEntry(new Color(235, 207, 150));
            SoundType = 0;
            SoundStyle = 2;
            ItemDrop = Mod.Find<ModItem>("CookieBlock").Type;
        }

        private bool SpawnGrass(int i, int j)
        {
            if (Main.tile[i, j - 1].TileType == 0 && Main.rand.Next(4) == 0)
            {
                WorldGen.PlaceTile(i, j - 1, ModContent.TileType<CreamGrass_Foliage>(), mute: true);
                return true;
            }
            if (Main.tile[i, j - 1].TileType == 0 && Main.rand.Next(18) == 0)
            {
                WorldGen.PlaceTile(i, j - 1, ModContent.TileType<YumDrop>(), mute: true);
                return true;
            }
            return false;
        }

        public override void RandomUpdate(int i, int j)
        {
            if (Main.rand.Next(8) == 0)
            {
                bool spawned = false;
                if (!spawned)
                {
                    spawned = SpawnGrass(i, j);
                }
            }

            Tile tile = Framing.GetTileSafely(i, j);
            Tile tileBelow = Framing.GetTileSafely(i, j + 1);
            Tile tileAbove = Framing.GetTileSafely(i, j - 1);
            if (WorldGen.genRand.NextBool(15) && !tileBelow.HasTile && !tileBelow.CheckingLiquid && !tile.IsHalfBlock)
            {
                tileBelow.TileType = (ushort)ModContent.TileType<CreamVines>();
                WorldGen.SquareTileFrame(i, j + 1);
                if (Main.netMode == 2)
                {
                    NetMessage.SendTileSquare(-1, i, j + 1, 3);
                }
            }
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail && !effectOnly)
            {
                Main.tile[i, j].TileType = (ushort)ModContent.TileType<CookieBlock>();
            }
        }
    }
}