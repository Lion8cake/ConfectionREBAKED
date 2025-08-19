using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Walls
{
    public class CreamsandstoneWall : ModWall
    {
        public override void SetStaticDefaults()
        {
			WallID.Sets.Conversion.HardenedSand[Type] = true;
			WallID.Sets.AllowsUndergroundDesertEnemiesToSpawn[Type] = true;
			Main.wallHouse[Type] = false;
			ConfectionIDs.Sets.IsNaturalConfectionWall[Type] = true;
			AddMapEntry(new Color(54, 30, 24));
        }
    }
}