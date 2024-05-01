using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class BlueFairyFlossWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = ModContent.DustType<FairyFlossDust3>();
            RegisterItemDrop(ModContent.ItemType<Items.Placeable.BlueFairyFlossWall>());
            AddMapEntry(new Color(39, 95, 126));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
