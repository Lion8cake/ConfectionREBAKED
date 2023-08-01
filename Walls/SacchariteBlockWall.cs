using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class SacchariteBlockWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.SacchariteBlockWall>());
            DustType = ModContent.DustType<SacchariteCrystals>();
            AddMapEntry(new Color(72, 120, 123));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}