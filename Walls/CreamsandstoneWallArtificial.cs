using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Walls
{
    public class CreamsandstoneWallArtificial : ModWall
    {
        public override void SetStaticDefaults()
        {
            WallID.Sets.Conversion.HardenedSand[Type] = true;
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(54, 30, 24));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.CreamsandstoneWall>());
        }
    }
}