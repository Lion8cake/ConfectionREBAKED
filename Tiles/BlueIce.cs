using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
    public class BlueIce : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CookieBlock").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamGrass").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("HallowedOre").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("NeapoliniteOre").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamstoneBrick").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamWood").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamBlock").Type] = true;
            TheConfectionRebirth.tileMerge[Type, 161] = true;
            TheConfectionRebirth.tileMerge[Type, 163] = true;
            TheConfectionRebirth.tileMerge[Type, 164] = true;
            TheConfectionRebirth.tileMerge[Type, 200] = true;
            TheConfectionRebirth.tileMerge[Type, 147] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            TileID.Sets.Ices[Type] = true;
            TileID.Sets.IcesSlush[Type] = true;
            TileID.Sets.IcesSnow[Type] = true;
            TileID.Sets.Conversion.Ice[Type] = true;
            DustType = ModContent.DustType<OrangeIceDust>();
            AddMapEntry(new Color(237, 145, 103));

            HitSound = SoundID.Item50;
        }

        public override void FloorVisuals(Player player)
        {
            player.slippy = true;
        }

		public override void RandomUpdate(int i, int j) {
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