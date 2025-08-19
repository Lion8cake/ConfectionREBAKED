using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class MeltingConfectionWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            WallID.Sets.Conversion.NewWall4[Type] = true;
            Main.wallHouse[Type] = true;
            DustType = ModContent.DustType<CreamstoneDust>();
            AddMapEntry(new Color(124, 123, 111));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.MeltingConfectionWall>());
        }
    }
}