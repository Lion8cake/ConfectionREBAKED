using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class CreamstoneBrickWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = ModContent.DustType<CreamDust>();
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.CreamstoneBrickWall>());
            AddMapEntry(new Color(90, 78, 51));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}