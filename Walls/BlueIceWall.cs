using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class BlueIceWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            WallID.Sets.Conversion.Ice[Type] = true;
            Main.wallHouse[Type] = false;
            DustType = ModContent.DustType<OrangeIceDust>();
			ConfectionIDs.Sets.IsExtraConfectionWall[Type] = true;
			AddMapEntry(new Color(118, 72, 51));
        }
    }
}
