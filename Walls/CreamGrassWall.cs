using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class CreamGrassWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            WallID.Sets.Conversion.Grass[Type] = true;
            Main.wallHouse[Type] = false;
            DustType = ModContent.DustType<CreamGrassDust>();
			ConfectionIDs.Sets.IsNaturalConfectionWall[Type] = true;
			AddMapEntry(new Color(100, 85, 54));
        }
    }
}