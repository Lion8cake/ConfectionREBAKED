using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls.GraveyardWalls
{
    public class BlueIceWallArtificial : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = ModContent.DustType<OrangeIceDust>();
            AddMapEntry(new Color(118, 72, 51));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.OrangeIceWall>());
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
