using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class CreamVines : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileCut[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileNoFail[Type] = true;

			TileID.Sets.TileCutIgnore.Regrowth[Type] = true;
			TileID.Sets.IsVine[Type] = true;
			TileID.Sets.ReplaceTileBreakDown[Type] = true;
			TileID.Sets.VineThreads[Type] = true;
			//TileID.Sets.DrawFlipMode[Type] = 1;

			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;
			ConfectionIDs.Sets.Confection[Type] = true;
			ConfectionIDs.Sets.IsNaturalConfectionTile[Type] = true;

			DustType = ModContent.DustType<CreamGrassDust>();
			HitSound = SoundID.Grass;
			AddMapEntry(new Color(200, 170, 108));
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) {
			Main.instance.TilesRenderer.CrawlToTopOfVineAndAddSpecialPoint(j, i);
			return false;
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) {
			offsetY = -2;
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) {
			if (i % 2 == 0) {
				spriteEffects = (SpriteEffects)1;
			}
		}

		public override void RandomUpdate(int i, int j) {
			if (i > Main.worldSurface) {
				if (ConfectionWorldGeneration.GrowMoreVines(i, j)) {
					int maxValue3 = 60;
					if (Main.tile[i, j].TileType == ModContent.TileType<CreamVines>()) {
						maxValue3 = 20;
					}
					if (WorldGen.genRand.NextBool(maxValue3) && !Main.tile[i, j + 1].HasTile && Main.tile[i, j + 1].LiquidType != LiquidID.Lava) {
						bool flag10 = false;
						for (int num35 = j; num35 > j - 10; num35--) {
							if (Main.tile[i, num35].BottomSlope) {
								flag10 = false;
								break;
							}
							if (Main.tile[i, num35].HasTile && Main.tile[i, num35].TileType == Type && !Main.tile[i, num35].BottomSlope) {
								flag10 = true;
								break;
							}
						}
						if (flag10) {
							int num36 = j + 1;
							Main.tile[i, num36].TileType = (ushort)ModContent.TileType<CreamVines>();
							Tile tile = Main.tile[i, num36];
							tile.HasTile = true;
							Main.tile[i, num36].CopyPaintAndCoating(Main.tile[i, j]);
							WorldGen.SquareTileFrame(i, num36);
							if (Main.netMode == 2) {
								NetMessage.SendTileSquare(-1, i, num36);
							}
						}
					}
				}
			}
		}
	}
}
