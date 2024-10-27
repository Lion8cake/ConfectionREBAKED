using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Walls
{
    public class CookieStonedWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            AddMapEntry(new Color(70, 45, 28));
			ConfectionIDs.Sets.IsExtraConfectionWall[Type] = true;
		}
    }
}