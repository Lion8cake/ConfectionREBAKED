using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles
{
    public class CookieBlock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamGrass").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("Creamstone").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamWood").Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            AddMapEntry(new Color(153, 97, 60));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }

		public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor) {
			sightColor = new Color(210, 196, 145);
			return true;
		}
	}
}