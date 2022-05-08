using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
	public class PastryBlockWall : ModWall
	{
		public override void SetStaticDefaults() {
			Main.wallHouse[Type] = true;
			ItemDrop = ModContent.ItemType<Items.Placeable.PastryBlockWall>();
			DustType = ModContent.DustType<PastryDust>();
			AddMapEntry(new Color(42, 14, 93));
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}