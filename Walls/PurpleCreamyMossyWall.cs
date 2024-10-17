using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class PurpleCreamyMossyWall : ModWall
    {
        public override void SetStaticDefaults()
        {
			Main.wallHouse[Type] = false;
            DustType = ModContent.DustType<CreamstoneDust>();
            AddMapEntry(new Color(48, 35, 52));
        }
    }
}