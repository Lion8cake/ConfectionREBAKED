using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls.GraveyardWalls
{
    public class CreamWallArtificial : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = ModContent.DustType<CreamSnowDust>();
            AddMapEntry(new Color(109, 111, 116));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.CreamWall>());
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}