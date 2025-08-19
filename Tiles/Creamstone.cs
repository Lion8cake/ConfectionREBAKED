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
			Tile Blockpos = Main.tile[i, j];
			if (WorldGen.genRand.NextBool(20) && !Blockpos.IsHalfBlock && !Blockpos.BottomSlope && !Blockpos.LeftSlope && !Blockpos.RightSlope && !Blockpos.TopSlope) {
				if (j > Main.rockLayer && WorldGen.genRand.NextBool(2)) {
					if (!Main.tile[i + 1, j].HasTile && Main.tile[i + 1, j].LiquidAmount == 0) {
						WorldGen.PlaceTile(i + 1, j, ModContent.TileType<SacchariteBlock>(), mute: true);
					}
					else if (!Main.tile[i, j - 1].HasTile && Main.tile[i, j - 1].LiquidAmount == 0) {
						WorldGen.PlaceTile(i, j - 1, ModContent.TileType<SacchariteBlock>(), mute: true);
					}
					else if (!Main.tile[i - 1, j].HasTile && Main.tile[i - 1, j].LiquidAmount == 0) {
						WorldGen.PlaceTile(i - 1, j, ModContent.TileType<SacchariteBlock>(), mute: true);
					}
				}
			}
		}

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

		public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor) {
			sightColor = new Color(210, 196, 145);
			return true;
		}
	}
}