using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class SherbetWall : ModWall
    {
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = ModContent.DustType<SherbetDust>();
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.SherbetWall>());
			AddMapEntry(new Color(98, 39, 32));
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}


		public override void AnimateWall(ref byte frame, ref byte frameCounter)
		{
			frameCounter++;
			if (frameCounter > 5)
			{
				frameCounter = 0;
				frame++;
				if (frame > 12)
				{
					frame = 0;
				}
			}
		}
	}
}