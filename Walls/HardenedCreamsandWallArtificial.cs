using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Walls
{
    public class HardenedCreamsandWallArtificial : ModWall
    {
        public override void SetStaticDefaults()
        {
            WallID.Sets.Conversion.Sandstone[Type] = true;
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(35, 21, 19));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.HardenedCreamsandWall>());
        }
    }
}