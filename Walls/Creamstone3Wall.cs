using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class Creamstone3Wall : ModWall
    {
        public override void SetStaticDefaults()
        {
			WallID.Sets.Conversion.NewWall2[Type] = true;
			Main.wallHouse[Type] = false;
            DustType = ModContent.DustType<SacchariteDust>();
			ConfectionIDs.Sets.IsExtraConfectionWall[Type] = true;
			AddMapEntry(new Color(45, 104, 117));
        }
    }
}