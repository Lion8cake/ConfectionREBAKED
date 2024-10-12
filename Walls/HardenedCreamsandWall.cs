using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Walls
{
    public class HardenedCreamsandWall : ModWall
    {
        public override void SetStaticDefaults()
        {
			WallID.Sets.Conversion.Sandstone[Type] = true;
            WallID.Sets.AllowsUndergroundDesertEnemiesToSpawn[Type] = true;
			Main.wallHouse[Type] = false;
            AddMapEntry(new Color(35, 21, 19));
        }
    }
}