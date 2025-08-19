using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class CrackingConfectionWall : ModWall
    {
        public override void SetStaticDefaults()
        {
			WallID.Sets.Conversion.NewWall1[Type] = true;
			Main.wallHouse[Type] = true;
            DustType = ModContent.DustType<CreamstoneDust>();
            AddMapEntry(new Color(58, 48, 35));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.CrackedConfectionWall>());
        }
    }
}