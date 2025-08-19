using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
    public class ShellBlock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            AddMapEntry(new Color(84, 28, 187));
            DustType = DustID.Gastropod;
        }

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
            r = 0.4f;
            g = 0;
            b = 0.25f;
		}
	}
}