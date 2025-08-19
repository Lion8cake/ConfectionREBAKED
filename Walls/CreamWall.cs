using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class CreamWall : ModWall
    {
        public override void SetStaticDefaults()
        {
			WallID.Sets.Conversion.Snow[Type] = true;
			Main.wallHouse[Type] = false;
            DustType = ModContent.DustType<CreamDust>();
			ConfectionIDs.Sets.IsExtraConfectionWall[Type] = true;
			AddMapEntry(new Color(109, 111, 116));
        }
    }
}