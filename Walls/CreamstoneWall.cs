using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class CreamstoneWall : ModWall
    {
        public override void SetStaticDefaults()
        {
			WallID.Sets.Conversion.Stone[Type] = true;
			Main.wallHouse[Type] = false;
            DustType = ModContent.DustType<CreamstoneDust>();
            AddMapEntry(new Color(74, 61, 43));
        }
    }
}