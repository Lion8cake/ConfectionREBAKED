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
<<<<<<< HEAD
<<<<<<< Updated upstream
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<Creamstone>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamstoneBrick>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CookieBlock>()] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<CreamGrass>()] = true;
=======
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("Creamstone").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamstoneBrick").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CookieBlock").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamGrass").Type] = true;
>>>>>>> 21d961fc5a6b7f1a1395f5a436ef383fb42b52eb
            ItemDrop = ModContent.ItemType<Items.Placeable.CreamWood>();
=======
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("Creamstone").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamstoneBrick").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CookieBlock").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamGrass").Type] = true;
>>>>>>> Stashed changes
            AddMapEntry(new Color(153, 97, 60));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}