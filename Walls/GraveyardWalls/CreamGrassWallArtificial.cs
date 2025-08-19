using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls.GraveyardWalls
{
    public class CreamGrassWallArtificial : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = ModContent.DustType<CreamDust>();
            AddMapEntry(new Color(100, 85, 54));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.CreamgrassWall>());
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}