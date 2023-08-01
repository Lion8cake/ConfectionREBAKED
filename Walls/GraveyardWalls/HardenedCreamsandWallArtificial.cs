using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Walls.GraveyardWalls
{
    public class HardenedCreamsandWallArtificial : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(35, 21, 19));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.HardenedCreamsandWall>());
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}