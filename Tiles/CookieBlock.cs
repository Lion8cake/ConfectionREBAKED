using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles
{
    public class CookieBlock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamGrass").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("Creamstone").Type] = true;
            Main.tileMerge[Type][Mod.Find<ModTile>("CreamWood").Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            ItemDrop = ModContent.ItemType<Items.Placeable.CookieBlock>();
            AddMapEntry(new Color(153, 97, 60));
        }

        /*public override void RandomUpdate(int i, int j)
        {
		Tile up = Main.tile[i, j - 1];
		Tile down = Main.tile[i, j + 1];
		Tile left = Main.tile[i - 1, j];
		Tile right = Main.tile[i + 1, j];
		if (WorldGen.genRand.Next(3) == 0 && (up.type == ModContent.TileType<CreamGrass>() || down.type == ModContent.TileType<CreamGrass>() || left.type == ModContent.TileType<CreamGrass>() || right.type == ModContent.TileType<CreamGrass>()))
		{
			WorldGen.SpreadGrass(i, j, base.Type, ModContent.TileType<CreamGrass>(), repeat: false, 0);
		}
	}*/

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
    }
}