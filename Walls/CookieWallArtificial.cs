using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Walls
{
    public class CookieWallArtificial : ModWall
    {
        public override void SetStaticDefaults()
        {
			WallID.Sets.Conversion.Dirt[Type] = true;
			Main.wallHouse[Type] = true;
            AddMapEntry(new Color(61, 39, 27));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.CookieWall>());
        }
    }
}