using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class CreamwoodFence : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.CreamwoodFence>());
            DustType = ModContent.DustType<CreamwoodDust>();
            AddMapEntry(new Color(61, 39, 27));
        }
    }
}