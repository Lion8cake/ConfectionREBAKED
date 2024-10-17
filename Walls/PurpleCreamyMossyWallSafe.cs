using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class PurpleCreamyMossyWallSafe : ModWall
    {
        public override void SetStaticDefaults()
        {
			Main.wallHouse[Type] = true;
            DustType = ModContent.DustType<CreamstoneDust>();
            AddMapEntry(new Color(48, 35, 52));
        }
    }
}