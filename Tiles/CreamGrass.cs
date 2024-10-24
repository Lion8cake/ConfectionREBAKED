using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class CreamGrass : ModTile {
		public override void SetStaticDefaults() {
			Main.tileBrick[Type] = true;
			Main.tileShine[Type] = 9000;
			Main.tileLighted[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			TileID.Sets.Conversion.MergesWithDirtInASpecialWay[Type] = true;
			TileID.Sets.Conversion.Grass[Type] = true;
			TileID.Sets.ForcedDirtMerging[Type] = true;
			TileID.Sets.CanBeDugByShovel[Type] = true;
			TileID.Sets.ResetsHalfBrickPlacementAttempt[Type] = true;
			TileID.Sets.DoesntPlaceWithTileReplacement[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			TileID.Sets.SpreadOverground[Type] = true;
			TileID.Sets.SpreadUnderground[Type] = true;
			TileID.Sets.Grass[Type] = true;
			TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
			TileID.Sets.GrassSpecial[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true; 
			ConfectionIDs.Sets.Confection[Type] = true;

			Main.tileMerge[Type][ModContent.TileType<Creamstone>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CookieBlock>()] = true;

			AddMapEntry(new Color(235, 207, 150));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.CookieBlock>());
			DustType = ModContent.DustType<CreamGrassDust>();
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem) 
		{
			if(fail && !effectOnly) 
			{
				Main.tile[i, j].TileType = (ushort)ModContent.TileType<CookieBlock>();
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
				if (Main.tile[i, j].HasUnactuatedTile)
				{
					int num = i - 1;
					int num11 = i + 2;
					int num22 = j - 1;
					int num33 = j + 2;
					CreamGrassGrowth(i, j, num, num11, num22, num33);
				}
			}
		}

		public static void CreamGrassGrowth(int i, int j, int minI, int maxI, int minJ, int maxJ) {
			if (i > Main.worldSurface) {
				int num2 = Main.tile[i, j].TileType;
				if (!Main.tile[i, minJ].HasTile && Main.tile[i, minJ].LiquidAmount == 0) {
					int num9 = -1;
					if (num2 == ModContent.TileType<CreamGrass>() && WorldGen.genRand.NextBool(10)) {
						num9 = ModContent.TileType<CreamGrass_Foliage>();
					}
					if (num9 != -1 && WorldGen.PlaceTile(i, minJ, num9, mute: true)) {
						Main.tile[i, minJ].CopyPaintAndCoating(Main.tile[i, j]);
						if (Main.netMode == 2) {
							NetMessage.SendTileSquare(-1, i, minJ);
						}
					}
				}
				bool flag7 = false;
				if (num2 == ModContent.TileType<CreamGrassMowed>()) {
					num2 = ModContent.TileType<CreamGrass>();
				}
				int grass = num2;
				bool flag8 = WorldGen.AllowedToSpreadInfections && num2 == ModContent.TileType<CreamGrass>() && WorldGen.InWorld(i, j, 10);
				for (int num11 = minI; num11 < maxI; num11++) {
					for (int num13 = minJ; num13 < maxJ; num13++) {
						if (!WorldGen.InWorld(num11, num13, 10) || (i == num11 && j == num13) || !Main.tile[num11, num13].HasTile) {
							continue;
						}
						int type2 = Main.tile[num11, num13].TileType;
						TileColorCache color3 = Main.tile[i, j].BlockColorAndCoating();
						if (type2 == 0 || ((num2 == ModContent.TileType<CreamGrass>() || num2 == ModContent.TileType<CreamGrassMowed>()) && (type2 == 2 || type2 == 477 || type2 == 23 || type2 == 199))) {
							WorldGen.SpreadGrass(num11, num13, 0, grass, repeat: false, color3);
							if (Main.tile[num11, num13 - 1].TileType == 27) {
								if (num2 == ModContent.TileType<CreamGrass>()) {
									WorldGen.SpreadGrass(num11, num13, 2, grass, repeat: false, color3);
								}
								if (num2 == ModContent.TileType<CreamGrassMowed>()) {
									WorldGen.SpreadGrass(num11, num13, 477, grass, repeat: false, color3);
								}
								if (num2 == ModContent.TileType<CreamGrass>()) {
									WorldGen.SpreadGrass(num11, num13, 477, ModContent.TileType<CreamGrassMowed>(), repeat: false, color3);
								}
								if ((num2 == ModContent.TileType<CreamGrassMowed>() || num2 == ModContent.TileType<CreamGrass>()) && WorldGen.AllowedToSpreadInfections) {
									WorldGen.SpreadGrass(num11, num13, 23, ModContent.TileType<CreamGrass>(), repeat: false, color3);
								}
								if ((num2 == ModContent.TileType<CreamGrassMowed>() || num2 == ModContent.TileType<CreamGrass>()) && WorldGen.AllowedToSpreadInfections) {
									WorldGen.SpreadGrass(num11, num13, 199, ModContent.TileType<CreamGrass>(), repeat: false, color3);
								}
							}
							if (Main.tile[num11, num13].TileType == num2) {
								WorldGen.SquareTileFrame(num11, num13);
								flag7 = true;
							}
						}
					}
				}
				if (Main.netMode == 2 && flag7) {
					NetMessage.SendTileSquare(-1, i, j, 3);
				}
			}
		}
	}
}
