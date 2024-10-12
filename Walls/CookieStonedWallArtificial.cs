using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Walls
{
    public class CookieStonedWallArtificial : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(70, 45, 28));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.CookieStonedWall>());
        }
    }
}