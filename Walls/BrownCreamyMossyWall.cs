using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class BrownCreamyMossyWall : ModWall
    {
        public override void SetStaticDefaults()
        {
			Main.wallHouse[Type] = false;
            DustType = ModContent.DustType<CreamstoneDust>();
			ConfectionIDs.Sets.IsExtraConfectionWall[Type] = true;
			AddMapEntry(new Color(49, 48, 36));
        }
    }
}