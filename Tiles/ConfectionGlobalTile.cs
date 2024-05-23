using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheConfectionRebirth.Tiles {
	public class ConfectionGlobalTile : GlobalTile {
		public override void SetStaticDefaults() { //Daybloom does not appear here yet, a complete rewrite of modded herbs is required first
			TileObjectData tileObjectData = TileObjectData.GetTileData(TileID.Sunflower, 0);
			tileObjectData.AnchorValidTiles = tileObjectData.AnchorValidTiles.Append(ModContent.TileType<CreamGrass>()).ToArray();
			tileObjectData.AnchorValidTiles = tileObjectData.AnchorValidTiles.Append(ModContent.TileType<CreamGrassMowed>()).ToArray();
			TileObjectData tileObjectData2 = TileObjectData.GetTileData(TileID.Pumpkins, 0);
			tileObjectData2.AnchorValidTiles = tileObjectData.AnchorValidTiles.Append(ModContent.TileType<CreamGrass>()).ToArray();
			tileObjectData2.AnchorValidTiles = tileObjectData.AnchorValidTiles.Append(ModContent.TileType<CreamGrassMowed>()).ToArray();
			TileObjectData tileObjectData3 = TileObjectData.GetTileData(TileID.FallenLog, 0);
			tileObjectData3.AnchorValidTiles = tileObjectData.AnchorValidTiles.Append(ModContent.TileType<CreamGrass>()).ToArray();
			tileObjectData3.AnchorValidTiles = tileObjectData.AnchorValidTiles.Append(ModContent.TileType<CreamGrassMowed>()).ToArray();
		}

		public override void Unload() {
			TileObjectData tileObjectData = TileObjectData.GetTileData(TileID.Sunflower, 0);
			tileObjectData.AnchorValidTiles = tileObjectData.AnchorValidTiles.Except(new int[] { ModContent.TileType<CreamGrass>(), ModContent.TileType<CreamGrassMowed>() }).ToArray();
			TileObjectData tileObjectData2 = TileObjectData.GetTileData(TileID.Pumpkins, 0);
			tileObjectData2.AnchorValidTiles = tileObjectData.AnchorValidTiles.Except(new int[] { ModContent.TileType<CreamGrass>(), ModContent.TileType<CreamGrassMowed>() }).ToArray();
			TileObjectData tileObjectData3 = TileObjectData.GetTileData(TileID.FallenLog, 0);
			tileObjectData3.AnchorValidTiles = tileObjectData.AnchorValidTiles.Except(new int[] { ModContent.TileType<CreamGrass>(), ModContent.TileType<CreamGrassMowed>() }).ToArray();
		}

		public override void RandomUpdate(int i, int j, int type) {
			if (ConfectionIDs.Sets.CanGrowSaccharite[type]) {
				Tile Blockpos = Main.tile[i, j];
				if (j > Main.rockLayer) {
					for (int NearSaccX = i + 4; NearSaccX > i - 4; --NearSaccX) {
						for (int NearSaccY = j + 4; NearSaccY > j - 4; --NearSaccY) {
							if (Main.tile[NearSaccX, NearSaccY].TileType == ModContent.TileType<SacchariteBlock>()) {
								return;
							}
						}
					}
					if (WorldGen.genRand.NextBool(40) && !Blockpos.IsHalfBlock && !Blockpos.BottomSlope && !Blockpos.LeftSlope && !Blockpos.RightSlope && !Blockpos.TopSlope) {
						if (!Main.tile[i + 1, j].HasTile && Main.tile[i + 1, j].LiquidAmount == 0) {
							WorldGen.PlaceTile(i + 1, j, ModContent.TileType<SacchariteBlock>(), mute: true);
						}
						else if (!Main.tile[i, j - 1].HasTile && Main.tile[i, j - 1].LiquidAmount == 0) {
							WorldGen.PlaceTile(i, j - 1, ModContent.TileType<SacchariteBlock>(), mute: true);
						}
						else if (!Main.tile[i, j + 1].HasTile && Main.tile[i, j + 1].LiquidAmount == 0) {
							WorldGen.PlaceTile(i, j + 1, ModContent.TileType<SacchariteBlock>(), mute: true);
						}
						else if (!Main.tile[i - 1, j].HasTile && Main.tile[i - 1, j].LiquidAmount == 0) {
							WorldGen.PlaceTile(i - 1, j, ModContent.TileType<SacchariteBlock>(), mute: true);
						}
					}
				}
			}

			if (j >= Main.worldSurface) {
				if (Main.tile[i, j].LiquidAmount > 32) {
					if (WorldGen.genRand.NextBool(600)) {
						WorldGen.PlaceTile(i, j, ModContent.TileType<CreamCattails>(), mute: true);
						if (Main.netMode == NetmodeID.Server) {
							NetMessage.SendTileSquare(-1, i, j);
						}
					}
					else if (WorldGen.genRand.NextBool(600)) {
						WorldGen.PlaceTile(i, j, ModContent.TileType<CreamLilyPads>(), mute: true);
						if (Main.netMode == 2) {
							NetMessage.SendTileSquare(-1, i, j);
						}
					}
				}
				if (TileID.Sets.Conversion.Sand[Main.tile[i, j].TileType]) {
					if (!WorldGen.genRand.NextBool(20)) {
						ConfectionWorldGeneration.PlantSeaOat(i, j - 1);
					}
				}
				bool[] sand = TileID.Sets.Conversion.Sand;
				Tile tile = Main.tile[i, j];
				int num22 = j - 1;
				if (sand[tile.TileType]) {
					tile = Main.tile[i, num22];
					if (!tile.HasTile) {
						if (WorldGen.genRand.NextBool(25)) {
							WorldGen.PlaceOasisPlant(i, num22, 530);
							tile = Main.tile[i, num22];
							if (tile.TileType == ModContent.TileType<CreamOasisPlants>() && Main.netMode == 2) {
								NetMessage.SendTileSquare(-1, i - 1, num22 - 1, 3, 2);
							}
						}
					}
				}
			}
		}

		public override bool? IsTileBiomeSightable(int i, int j, int type, ref Color sightColor) {
			if (ConfectionIDs.Sets.ConfectionBiomeSight[type]) {
				sightColor = new Color(210, 196, 145);
				return true;
			}
			else
				return null;
		}

		public override bool TileFrame(int i, int j, int type, ref bool resetFrame, ref bool noBreak) {
			Tile tile = Main.tile[i, j];
			Tile tileBelow = Main.tile[i, j + 1];
			Tile tileAbove = Main.tile[i, j - 1];
			Tile tileLeft = Main.tile[i + 1, j + 1];
			Tile tileRight = Main.tile[i - 1, j + 1];
			if (tile.TileType == TileID.SeaOats && tileBelow.TileType == ModContent.TileType<Creamsand>()) {
				tile.TileType = (ushort)ModContent.TileType<CreamSeaOats>();
			}
			if (tile.TileType == ModContent.TileType<CreamSeaOats>() && tileBelow.TileType != ModContent.TileType<Creamsand>()) {
				tile.TileType = (ushort)TileID.SeaOats;
			}
			if (tile.TileType == TileID.OasisPlants && (Main.tile[i - 1, j + 1].TileType == ModContent.TileType<Creamsand>() || Main.tile[i - 2, j + 1].TileType == ModContent.TileType<Creamsand>() || tileBelow.TileType == ModContent.TileType<Creamsand>()) && tile.TileFrameX % 54 / 18 == 0 && tile.TileFrameY % 36 / 18 == 1) {
				for (int m = i - 1; m < i + 2; m++) {
					for (int n = j - 2; n < j; n++) {
						tile = Main.tile[m, n];
						//if (tile.HasTile) {
							tile.TileType = (ushort)ModContent.TileType<CreamOasisPlants>();
						//}
					}
				}
				//tile.TileColor = PaintID.RedPaint;
			}
			tile = Main.tile[i, j];
			/*if ((tile.TileType == ModContent.TileType<CreamOasisPlants>() || tileLeft.TileType == ModContent.TileType<CreamOasisPlants>() || tileRight.TileType == ModContent.TileType<CreamOasisPlants>()) && tileBelow.TileType != ModContent.TileType<Creamsand>()) {
				tile.TileType = (ushort)TileID.OasisPlants;
				tileAbove.TileType = (ushort)TileID.OasisPlants;
			}*/
			return true;
		}
	}
}
