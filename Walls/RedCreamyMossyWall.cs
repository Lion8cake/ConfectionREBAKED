using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class RedCreamyMossyWall : ModWall
    {
        public override void SetStaticDefaults()
        {
			Main.wallHouse[Type] = false;
            DustType = ModContent.DustType<CreamstoneDust>();
			ConfectionIDs.Sets.IsExtraConfectionWall[Type] = true;
			AddMapEntry(new Color(43, 33, 32));
        }
    }
}