using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Walls
{
    public class HallowedBrickWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.HallowedBrickWall>());
            DustType = 84;
            AddMapEntry(new Color(59, 59, 73));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}