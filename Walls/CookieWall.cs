using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Walls
{
    public class CookieWall : ModWall
    {
        public override void SetStaticDefaults()
        {
			WallID.Sets.Conversion.Dirt[Type] = true;
			Main.wallHouse[Type] = false;
            AddMapEntry(new Color(61, 39, 27));
        }
    }
}