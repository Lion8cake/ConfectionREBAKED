using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class CreamWallArtificial : ModWall
    {
        public override void SetStaticDefaults()
        {
            WallID.Sets.Conversion.Snow[Type] = true;
            Main.wallHouse[Type] = true;
            DustType = ModContent.DustType<CreamDust>();
            AddMapEntry(new Color(109, 111, 116));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.CreamWall>());
        }
    }
}