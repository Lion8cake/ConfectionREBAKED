using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class CreamstoneSapphireWall : ModWall
    {
        public override void SetStaticDefaults()
        {
			Main.wallHouse[Type] = false;
            DustType = ModContent.DustType<CreamstoneDust>();
			ConfectionIDs.Sets.IsExtraConfectionWall[Type] = true;
			AddMapEntry(new Color(74, 61, 43));
        }
    }
}