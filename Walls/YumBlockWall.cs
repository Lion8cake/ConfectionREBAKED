using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
	public class YumBlockWall : ModWall
	{
		public override void SetStaticDefaults() {
			Main.wallHouse[Type] = true;
			ItemDrop = ModContent.ItemType<Items.Placeable.YumBlockWall>();
			DustType = ModContent.DustType<YumDust>();
			AddMapEntry(new Color(64, 121, 94));
		}
		
		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}