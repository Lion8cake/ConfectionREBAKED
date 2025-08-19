using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls.GraveyardWalls
{
    public class MeltingConfectionWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = ModContent.DustType<CreamDust>();
            AddMapEntry(new Color(124, 123, 111));

			RegisterItemDrop(ModContent.ItemType<Items.Placeable.MeltingConfectionWall>());
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}