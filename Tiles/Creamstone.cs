using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
    public class Creamstone : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CookieBlock").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamGrass").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("HallowedOre").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("NeapoliniteOre").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamstoneBrick").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("SacchariteBlock").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamWood").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamBlock").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("BlueIce").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamstoneRuby").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamstoneSaphire").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamstoneDiamond").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamstoneAmethyst").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamstoneTopaz").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamstoneEmerald").Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            Main.tileStone[Type] = true;
            TileID.Sets.Conversion.Stone[Type] = true;
            TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
            DustType = ModContent.DustType<CreamDust>();
            AddMapEntry(new Color(188, 168, 120));

            HitSound = SoundID.Tink;
            MinPick = 65;
        }
	
		public override void RandomUpdate(int i, int j)
		{
			if (Main.rand.NextBool(12))
			{
				if (j > Main.rockLayer) {
					if (Main.tile[i, j + 1].TileType == 0 && Main.rand.NextBool(2)) {
						WorldGen.PlaceTile(i, j + 1, ModContent.TileType<SacchariteBlock>(), mute: true);
					}
					if (Main.tile[i, j - 1].TileType == 0 && Main.rand.NextBool(2)) {
						WorldGen.PlaceTile(i, j - 1, ModContent.TileType<SacchariteBlock>(), mute: true);
					}
				}
			}
		}

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}