using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Walls.GraveyardWalls
{
    public class CookieWallArtificial : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(61, 39, 27));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.CookieWall>());
        }
    }
}