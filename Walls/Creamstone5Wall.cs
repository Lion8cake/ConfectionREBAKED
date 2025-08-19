using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class Creamstone5Wall : ModWall
    {
        public override void SetStaticDefaults()
        {
			WallID.Sets.Conversion.NewWall4[Type] = true;
			Main.wallHouse[Type] = false;
            DustType = ModContent.DustType<CreamstoneDust>();
			ConfectionIDs.Sets.IsExtraConfectionWall[Type] = true;
			AddMapEntry(new Color(124, 123, 111));
        }
    }
}