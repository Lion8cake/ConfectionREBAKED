using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles
{
	public class CreamWood : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileMerge[Type][Mod.Find<ModTile>("Creamstone").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("CreamstoneBrick").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("CookieBlock").Type] = true;
			Main.tileMerge[Type][Mod.Find<ModTile>("CreamGrass").Type] = true;
			ItemDrop = ModContent.ItemType<Items.Placeable.CreamWood>();
			AddMapEntry(new Color(153, 97, 60));
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}