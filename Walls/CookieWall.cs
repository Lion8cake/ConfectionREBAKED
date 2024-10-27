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
			ConfectionIDs.Sets.IsExtraConfectionWall[Type] = true;
			AddMapEntry(new Color(61, 39, 27));
        }
    }
}