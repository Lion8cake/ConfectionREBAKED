using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class CreamstoneWallArtificial : ModWall
    {
        public override void SetStaticDefaults()
        {
            WallID.Sets.Conversion.Stone[Type] = true;
            Main.wallHouse[Type] = true;
            DustType = ModContent.DustType<CreamstoneDust>();
            AddMapEntry(new Color(74, 61, 43));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.CreamstoneWall>());
        }
    }
}