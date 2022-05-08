using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
	public class CreamwoodFence : ModWall
	{
		public override void SetStaticDefaults() {
			Main.wallHouse[Type] = true;
			ItemDrop = ModContent.ItemType<Items.Placeable.CreamwoodFence>();
			DustType = ModContent.DustType<CreamwoodDust>();
			AddMapEntry(new Color(74, 61, 43));
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}