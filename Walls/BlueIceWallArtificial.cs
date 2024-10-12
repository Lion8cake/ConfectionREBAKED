using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class BlueIceWallArtificial : ModWall
    {
        public override void SetStaticDefaults()
        {
			WallID.Sets.Conversion.Ice[Type] = true;
			Main.wallHouse[Type] = true;
            DustType = ModContent.DustType<OrangeIceDust>();
            AddMapEntry(new Color(118, 72, 51));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.OrangeIceWall>());
        }
    }
}
