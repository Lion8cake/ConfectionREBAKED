using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class PinkFairyFlossWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.PinkFairyFlossWall>());
            DustType = ModContent.DustType<FairyFlossDust>();
            AddMapEntry(new Color(126, 71, 127));
        }
    }
}