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
            DustType = ModContent.DustType<CreamstoneDust>();
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.CreamstoneBrickWall>());
            AddMapEntry(new Color(90, 78, 51));
        }
    }
}