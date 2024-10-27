using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class Creamstone2Wall : ModWall
    {
        public override void SetStaticDefaults()
        {
			WallID.Sets.Conversion.NewWall1[Type] = true;
			Main.wallHouse[Type] = false;
            DustType = ModContent.DustType<CreamstoneDust>();
			ConfectionIDs.Sets.IsExtraConfectionWall[Type] = true;
			AddMapEntry(new Color(58, 48, 35));
        }
    }
}