using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Walls
{
	public class CreamsandstoneWall : ModWall
	{
		public override void SetStaticDefaults() {
			Main.wallHouse[Type] = false;
			AddMapEntry(new Color(74, 61, 43));
		}
	}
}