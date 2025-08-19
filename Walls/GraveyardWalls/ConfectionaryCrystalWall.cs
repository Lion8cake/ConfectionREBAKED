using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls.GraveyardWalls
{
    public class ConfectionaryCrystalWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = ModContent.DustType<SacchariteCrystals>();
            AddMapEntry(new Color(45, 104, 117));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.ConfectionaryCrystalsWall>());
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}