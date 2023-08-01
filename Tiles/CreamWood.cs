using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles
{
    public class CreamWood : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<Creamstone>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamstoneBrick>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CookieBlock>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamGrass>()] = true;
            AddMapEntry(new Color(153, 97, 60));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}