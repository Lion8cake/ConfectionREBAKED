using Microsoft.CodeAnalysis.CSharp.Syntax;
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
using TheConfectionRebirth.Tiles.Trees;

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
			int num = i - 1;
			int num11 = i + 2;
			int num22 = j - 1;
			int num33 = j + 2;
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
				if (Main.tile[i, j].LiquidAmount > 32 && Main.tile[i, j].LiquidType == LiquidID.Water && !Main.tile[i, j].HasTile) {
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
				if (i > Main.worldSurface) {
					if (Main.tile[i, j].HasUnactuatedTile) {
						GrassOverrideGrowth(i, j, num, num11, num22, num33); //Allows other grasses to overgrow creamgrass (there isnt really a dynamic way of doing this)
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

		public static void MergesWithCreamGems(ushort Type)
		{
			Main.tileMerge[Type][ModContent.TileType<CreamstoneAmethyst>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneTopaz>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneSaphire>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneEmerald>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneRuby>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneDiamond>()] = true;
			Main.tileMerge[ModContent.TileType<CreamstoneAmethyst>()][Type] = true;
			Main.tileMerge[ModContent.TileType<CreamstoneTopaz>()][Type] = true;
			Main.tileMerge[ModContent.TileType<CreamstoneSaphire>()][Type] = true;
			Main.tileMerge[ModContent.TileType<CreamstoneEmerald>()][Type] = true;
			Main.tileMerge[ModContent.TileType<CreamstoneRuby>()][Type] = true;
			Main.tileMerge[ModContent.TileType<CreamstoneDiamond>()][Type] = true;
		}

		public override bool TileFrame(int i, int j, int type, ref bool resetFrame, ref bool noBreak) {
			Tile tile = Main.tile[i, j];
			Tile tileBelow = Main.tile[i, j + 1];
			if (tile.TileType == TileID.SeaOats && tileBelow.TileType == ModContent.TileType<Creamsand>()) {
				tile.TileType = (ushort)ModContent.TileType<CreamSeaOats>();
			}
			if (tile.TileType == ModContent.TileType<CreamSeaOats>() && tileBelow.TileType != ModContent.TileType<Creamsand>()) {
				tile.TileType = (ushort)TileID.SeaOats;
			}
			/*if (TileID.Sets.IsVine[tile.TileType]) {
				int num = tile.TileType;
				int up = -1;
				Tile tile10 = Main.tile[i, j - 1];
				int nullium = 0;
				WorldGen.TileMergeAttempt(0, TileID.Sets.Dirt, ref up, ref nullium, ref nullium, ref nullium, ref nullium, ref nullium, ref nullium, ref nullium);
				up = ((tile10 == null) ? num : ((!tile10.HasUnactuatedTile) ? (-1) : ((!tile10.BottomSlope) ? tile10.TileType : (-1))));
				if (num != up) {
					bool num48 = up == 60 || up == 62;
					bool num49 = up == 109 || up == 115;
					bool flag17 = up == 23 || up == 636 || up == 661;
					bool flag2 = up == 199 || up == 205 || up == 662;
					bool flag3 = up == 2 || up == 52;
					bool flag4 = up == 382;
					bool num50 = up == 70 || up == 528;
					bool num51 = up == 633 || up == 638;
					ushort num37 = 0;
					if (num51) {
						num37 = 638;
					}
					if (num50) {
						num37 = 528;
					}
					if (num49) {
						num37 = 115;
					}
					if (num48) {
						num37 = 62;
					}
					if (flag17) {
						num37 = 636;
					}
					if (flag2) {
						num37 = 205;
					}
					if (flag3 && num != 382) {
						num37 = 52;
					}
					if (flag4) {
						num37 = 382;
					}
					if (up == ModContent.TileType<CreamGrass>() || up == ModContent.TileType<CreamVines>()) {
						num37 = (ushort)ModContent.TileType<CreamVines>();
					}
					if (num37 != 0 && num37 != num) {
						tile.TileType = num37;
						WorldGen.SquareTileFrame(i, j);
						return false;
					}
				}
				if (up != num) {
					bool flag5 = false;
					//if (num == ModContent.TileType<CreamVines>() && up != ModContent.TileType<CreamGrass>()) {
					//	flag5 = true;
					//}
					if (up == -1) {
						flag5 = true;
					}
					if (num == 52 && up != 2 && up != 192) {
						flag5 = true;
					}
					if (num == 382 && up != 2 && up != 192) {
						flag5 = true;
					}
					if (num == 62 && up != 60) {
						flag5 = true;
					}
					if (num == 115 && up != 109) {
						flag5 = true;
					}
					if (num == 528 && up != 70) {
						flag5 = true;
					}
					if (num == 636 && up != 23 && up != 661) {
						flag5 = true;
					}
					if (num == 205 && up != 199 && up != 662) {
						flag5 = true;
					}
					if (num == 638 && up != 633) {
						flag5 = true;
					}
					if (flag5) {
						WorldGen.KillTile(i, j);
					}
				}
				return false;
			}*/
			return true;
		}

		public override void NearbyEffects(int i, int j, int type, bool closer) {
			if (type == TileID.Trees) {
				WorldGen.GetTreeBottom(i, j, out var x, out var y);
				Tile tilebelow = Main.tile[x, y + 1];
				Tile tilecurrent = Main.tile[x, y];
				if (tilebelow.TileType == ModContent.TileType<CreamGrass>() || tilebelow.TileType == ModContent.TileType<CreamGrassMowed>() || tilebelow.TileType == ModContent.TileType<CreamTree>() || tilecurrent.TileType == ModContent.TileType<CreamGrass>() || tilecurrent.TileType == ModContent.TileType<CreamGrassMowed>() || tilecurrent.TileType == ModContent.TileType<CreamTree>()) 
				{
					Main.tile[i, j].TileType = (ushort)ModContent.TileType<CreamTree>();
				}
				if (tilebelow.TileType == ModContent.TileType<CreamBlock>() || tilebelow.TileType == ModContent.TileType<CreamSnowTree>() || tilecurrent.TileType == ModContent.TileType<CreamBlock>() || tilecurrent.TileType == ModContent.TileType<CreamSnowTree>())
				{
					Main.tile[i, j].TileType = (ushort)ModContent.TileType<CreamSnowTree>();
				}
			}
			if (type == TileID.PalmTree)
			{
				WorldGen.GetTreeBottom(i, j, out var x, out var y);
				Tile tilebelow = Main.tile[x, y + 1];
				Tile tilecurrent = Main.tile[x, y];
				if (tilebelow.TileType == ModContent.TileType<Creamsand>() || tilebelow.TileType == ModContent.TileType<CreamPalmTree>() || tilecurrent.TileType == ModContent.TileType<Creamsand>() || tilecurrent.TileType == ModContent.TileType<CreamPalmTree>())
				{
					Main.tile[i, j].TileType = (ushort)ModContent.TileType<CreamPalmTree>();
				}
			}
			Tile tile = Main.tile[i, j];
			Tile tileBelow = Main.tile[i, j + 1];
			if (tile.TileType == TileID.OasisPlants && (Main.tile[i + 1, j + 1].TileType == ModContent.TileType<Creamsand>() || Main.tile[i + 2, j + 1].TileType == ModContent.TileType<Creamsand>() || tileBelow.TileType == ModContent.TileType<Creamsand>()) && tile.TileFrameX % 54 / 18 == 0 && tile.TileFrameY % 36 / 18 == 1) {
				for (int m = i; m < i + 3; m++) {
					for (int n = j - 1; n < j + 1; n++) {
						tile = Main.tile[m, n];
						if (tile.HasTile) {
							tile.TileType = (ushort)ModContent.TileType<CreamOasisPlants>();
						}
					}
				}
			}
			tile = Main.tile[i, j];
			if (tile.TileType == ModContent.TileType<CreamOasisPlants>() && Main.tile[i + 1, j + 1].TileType != ModContent.TileType<Creamsand>() && Main.tile[i + 2, j + 1].TileType != ModContent.TileType<Creamsand>() && tileBelow.TileType != ModContent.TileType<Creamsand>() && tile.TileFrameX % 54 / 18 == 0 && tile.TileFrameY % 36 / 18 == 1) {
				for (int m = i; m < i + 3; m++) {
					for (int n = j - 1; n < j + 1; n++) {
						tile = Main.tile[m, n];
						if (tile.HasTile) {
							tile.TileType = TileID.OasisPlants;
						}
					}
				}
			}
		}

		private static void GrassOverrideGrowth(int i, int j, int minI, int maxI, int minJ, int maxJ) {
			if (!WorldGen.InWorld(i, j, 10)) {
				return;
			}
			int num2 = Main.tile[i, j].TileType;
			bool flag7 = false;
			switch (num2) {
				case 32:
					num2 = 23;
					if (!WorldGen.AllowedToSpreadInfections) {
						return;
					}
					break;
				case 352:
					num2 = 199;
					if (!WorldGen.AllowedToSpreadInfections) {
						return;
					}
					break;
				case 477:
					num2 = 2;
					break;
				case 492:
					num2 = 109;
					break;
			}
			int grass = num2;
			int num10 = -1;
			if (num2 == 23 || num2 == 661) {
				grass = 23;
				num10 = 661;
			}
			if (num2 == 199 || num2 == 662) {
				grass = 199;
				num10 = 662;
			}
			for (int num11 = minI; num11 < maxI; num11++) {
				for (int num13 = minJ; num13 < maxJ; num13++) {
					if (!WorldGen.InWorld(num11, num13, 10) || (i == num11 && j == num13) || !Main.tile[num11, num13].HasTile) {
						continue;
					}
					int type2 = Main.tile[num11, num13].TileType;
					TileColorCache color3 = Main.tile[i, j].BlockColorAndCoating();
					if (type2 == 0 || (num10 > -1 && type2 == 59) || ((num2 == 23 || num2 == 661 || num2 == 199 || num2 == 662) && (type2 == 2 || type2 == 109 || type2 == 477 || type2 == 492))) {
						if (WorldGen.AllowedToSpreadInfections && (num2 == 23 || num2 == 199 || num2 == 661 || num2 == 662)) {
							WorldGen.SpreadGrass(num11, num13, ModContent.TileType<CreamGrass>(), grass, repeat: false, color3);
							WorldGen.SpreadGrass(num11, num13, ModContent.TileType<CreamGrassMowed>(), grass, repeat: false, color3);
							if (num10 > -1) {
								WorldGen.SpreadGrass(num11, num13, 60, num10, repeat: false, color3);
							}
						}
						if (Main.tile[num11, num13].TileType == num2 || (num10 > -1 && Main.tile[num11, num13].TileType == num10)) {
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

		public override void AnimateTile()
		{
			TheConfectionRebirth.instance.sherbertStyleTimer += 0.2f;
			if (TheConfectionRebirth.instance.sherbertStyleTimer > 1.00f)
			{
				TheConfectionRebirth.instance.sherbertStyleTimer = 0;
				TheConfectionRebirth.instance.sherbertStyle++;
				if (TheConfectionRebirth.instance.sherbertStyle > 12)
				{
					TheConfectionRebirth.instance.sherbertStyle = 0;
				}
			}
			Color[] sherbColors = new Color[13] {
				new Color(213, 105, 89),
				new Color(213, 136, 89),
				new Color(213, 167, 89),
				new Color(213, 198, 89),
				new Color(198, 213, 89),
				new Color(136, 213, 89),
				new Color(89, 213, 136),
				new Color(89, 198, 213),
				new Color(89, 167, 213),
				new Color(89, 136, 213),
				new Color(105, 89, 213),
				new Color(167, 89, 213),
				new Color(213, 89, 213)
			};
			int previousStyle = TheConfectionRebirth.instance.sherbertStyle;
			if (previousStyle <= 0)
				previousStyle = 12;
			else
				previousStyle--;
			Color newColor = sherbColors[TheConfectionRebirth.instance.sherbertStyle];
			Color previousColor = sherbColors[previousStyle];
			float lerpAmount = TheConfectionRebirth.instance.sherbertStyleTimer;
			byte r = (byte)MathHelper.Lerp(previousColor.R, newColor.R, lerpAmount);
			byte g = (byte)MathHelper.Lerp(previousColor.G, newColor.G, lerpAmount);
			byte b = (byte)MathHelper.Lerp(previousColor.B, newColor.B, lerpAmount);
			TheConfectionRebirth.SherbR = r;
			TheConfectionRebirth.SherbG = g;
			TheConfectionRebirth.SherbB = b;
		}
	}
}
