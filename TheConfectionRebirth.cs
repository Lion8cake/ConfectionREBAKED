using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;
using Terraria.ID;
using MonoMod.Cil;
using System.Reflection;
using Terraria.ModLoader.Config;
using Terraria.Utilities;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.Map;
using TheConfectionRebirth.Tiles.Trees;
using Microsoft.Xna.Framework.Graphics;

namespace TheConfectionRebirth
{
	public class TheConfectionRebirth : Mod
	{
		//Edit the following
		//Player.cs
		//MowGrassTile //Needs lawnmower grass to be done, this should be the FINAL thing to do with that grass
		//SmartCursorHelper.cs
		//Step_LawnMower
		//WorldGen.cs
		//PlantCheck (done) (i think)

		public override void Load() {
			ConfectionWindUtilities.Load();

			On_Player.PlaceThing_Tiles_PlaceIt_KillGrassForSolids += KillConjoinedGrass_PlaceThing;
			On_Player.DoesPickTargetTransformOnKill += PickaxeKillTile;
			IL_Liquid.DelWater += BurnGrass;
			IL_WorldGen.PlantCheck += PlantTileFrameIL;
			On_Player.DoBootsEffect_PlaceFlowersOnTile += FlowerBootsEdit;
			On_WorldGen.IsFitToPlaceFlowerIn += Flowerplacement;
			On_WorldGen.PlaceTile += PlaceTile;
			IL_WorldGen.TileFrame += VineTileFrame;
			IL_MapHelper.CreateMapTile += CactusMapColor;
			On_WorldGen.PlaceLilyPad += LilyPadPreventer;
			On_WorldGen.CheckCatTail += CheckCattail;
		}

		public override void Unload() {
			ConfectionWindUtilities.Unload();

			On_Player.PlaceThing_Tiles_PlaceIt_KillGrassForSolids -= KillConjoinedGrass_PlaceThing;
			On_Player.DoesPickTargetTransformOnKill -= PickaxeKillTile;
			IL_Liquid.DelWater -= BurnGrass;
			IL_WorldGen.PlantCheck -= PlantTileFrameIL;
			On_Player.DoBootsEffect_PlaceFlowersOnTile -= FlowerBootsEdit;
			On_WorldGen.IsFitToPlaceFlowerIn -= Flowerplacement;
			On_WorldGen.PlaceTile -= PlaceTile;
			IL_WorldGen.TileFrame -= VineTileFrame;
			IL_MapHelper.CreateMapTile -= CactusMapColor;
			On_WorldGen.PlaceLilyPad -= LilyPadPreventer;
		}

		private void CheckCattail(On_WorldGen.orig_CheckCatTail orig, int x, int j) {
			if (Main.tile[x, j] == null) {
				return;
			}
			int num = j;
			bool flag = false;
			int num2 = num;
			while ((!Main.tile[x, num2].HasTile || !Main.tileSolid[Main.tile[x, num2].TileType] || Main.tileSolidTop[Main.tile[x, num2].TileType]) && num2 < Main.maxTilesY - 50) {
				if (Main.tile[x, num2].HasTile && (Main.tile[x, num2].TileType != 519 || Main.tile[x, num2].TileType != ModContent.TileType<CreamCattails>())) {
					flag = true;
				}
				if (!Main.tile[x, num2].HasTile) {
					break;
				}
				num2++;
				if (Main.tile[x, num2] == null) {
					return;
				}
			}
			num = num2 - 1;
			if (Main.tile[x, num] == null) {
				return;
			}
			while (Main.tile[x, num] != null && Main.tile[x, num].LiquidAmount > 0 && num > 50) {
				if ((Main.tile[x, num].HasTile && (Main.tile[x, num].TileType != 519 || Main.tile[x, num].TileType != ModContent.TileType<CreamCattails>())) || Main.tile[x, num].LiquidType != 0) {
					flag = true;
				}
				num--;
				if (Main.tile[x, num] == null) {
					return;
				}
			}
			num++;
			if (Main.tile[x, num] == null) {
				return;
			}
			//CreamcattailCheck2(x, num2, ref num, ref flag);
			int num3 = num;
			int num4 = 8;//WorldGen.catTailDistance;
			if (num2 - num3 > num4) {
				flag = true;
			}
			int type = Main.tile[x, num2].TileType;
			int num5 = -1;
			if (type == ModContent.TileType<CreamGrass>()) { //
				num5 = ModContent.TileType<CreamCattails>(); //
			} //
			switch (type) {
				case 2:
				case 477:
					num5 = 0;
					break;
				case 53:
					num5 = 18;
					break;
				case 199:
				case 234:
				case 662:
					num5 = 54;
					break;
				case 23:
				case 112:
				case 661:
					num5 = 72;
					break;
				case 70:
					num5 = 90;
					break;
			}
			if (!Main.tile[x, num2].HasTile) {
				flag = true;
			}
			if (num5 < 0) {
				flag = true;
			}
			num = num2 - 1;
			if (Main.tile[x, num] != null && !Main.tile[x, num].HasTile) {
				for (int num6 = num; num6 >= num3; num6--) {
					if (Main.tile[x, num6] == null) {
						return;
					}
					if (Main.tile[x, num6].HasTile && Main.tile[x, num6].TileType == 519) {
						num = num6;
						break;
					}
				}
			}
			while (Main.tile[x, num] != null && Main.tile[x, num].HasTile && Main.tile[x, num].TileType == 519) {
				num--;
			}
			num++;
			//if (Main.tile[x, num2].TileType == ModContent.TileType<CreamCattails>()) { //
			//CreamcattailCheck(x, num2, ref num, ref flag);
			//}
			CreamcattailCheck3(x, num2, ref num, ref flag);
			if (flag) {
				int num7 = num3;
				if (num < num3) {
					num7 = num;
				}
				num7 -= 4;
				for (int i = num7; i <= num2; i++) {
					if (Main.tile[x, i] != null && Main.tile[x, i].HasTile && Main.tile[x, i].TileType == ModContent.TileType<CreamCattails>()) {
						WorldGen.KillTile(x, i);
						if (Main.netMode == NetmodeID.Server) {
							NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, x, i);
						}
						WorldGen.SquareTileFrame(x, i);
					}
				}
			}
			else {
				if (num5 == ModContent.TileType<CreamCattails>()) {
					for (int k = num; k < num2; k++) {
						if (Main.tile[x, k] != null && Main.tile[x, k].HasTile) {
							Main.tile[x, k].TileType = (ushort)num5;
							if (Main.netMode == NetmodeID.Server) {
								NetMessage.SendTileSquare(-1, x, num);
							}
						}
					}
					//return;
				}
			} //

			if (Main.tile[x, num2 - 1] != null && Main.tile[x, num2 - 1].LiquidAmount < 127 && WorldGen.genRand.NextBool(4)) {
				flag = true;
			}
			if (Main.tile[x, num] != null && Main.tile[x, num].TileFrameX >= 180 && Main.tile[x, num].LiquidAmount > 127 && WorldGen.genRand.NextBool(4)) {
				flag = true;
			}
			if (Main.tile[x, num] != null && Main.tile[x, num2 - 1] != null && Main.tile[x, num].TileFrameX > 18) {
				if (Main.tile[x, num2 - 1].TileFrameX < 36 || Main.tile[x, num2 - 1].TileFrameX > 72) {
					flag = true;
				}
				else if (Main.tile[x, num].TileFrameX < 90) {
					flag = true;
				}
				else if (Main.tile[x, num].TileFrameX >= 108 && Main.tile[x, num].TileFrameX <= 162) {
					Main.tile[x, num].TileFrameX = 90;
				}
			}
			if (num2 > num + 4 && Main.tile[x, num + 4] != null && Main.tile[x, num + 3] != null && Main.tile[x, num + 4].LiquidAmount == 0 && Main.tile[x, num + 3].TileType == 519) {
				flag = true;
			}
			if (flag) {
				int num7 = num3;
				if (num < num3) {
					num7 = num;
				}
				num7 -= 4;
				for (int i = num7; i <= num2; i++) {
					if (Main.tile[x, i] != null && Main.tile[x, i].HasTile && Main.tile[x, i].TileType == 519) {
						WorldGen.KillTile(x, i);
						if (Main.netMode == NetmodeID.Server) {
							NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, x, i);
						}
						WorldGen.SquareTileFrame(x, i);
					}
				}
			}
			else {
				if (num5 == Main.tile[x, num].TileFrameY) {
					return;
				}
				for (int k = num; k < num2; k++) {
					if (Main.tile[x, k] != null && Main.tile[x, k].HasTile && Main.tile[x, k].TileType == 519) {
						Main.tile[x, k].TileFrameY = (short)num5;
						Main.tile[x, k].TileType = 519; //
						if (Main.netMode == NetmodeID.Server) {
							NetMessage.SendTileSquare(-1, x, num);
						}
					}
				}
			}
		}

		private void CreamcattailCheck2(int x, int y, ref int num, ref bool flag) {
			int num2 = y;
			while ((!Main.tile[x, num2].HasTile || !Main.tileSolid[Main.tile[x, num2].TileType] || Main.tileSolidTop[Main.tile[x, num2].TileType]) && num2 < Main.maxTilesY - 50) {
				if (Main.tile[x, num2].HasTile && Main.tile[x, num2].TileType != ModContent.TileType<CreamCattails>()) {
					flag = true;
				}
				if (!Main.tile[x, num2].HasTile) {
					break;
				}
				num2++;
				if (Main.tile[x, num2] == null) {
					return;
				}
			}
			num = num2 - 1;
			while (Main.tile[x, num] != null && Main.tile[x, num].LiquidAmount > 0 && num > 50) {
				if ((Main.tile[x, num].HasTile && Main.tile[x, num].TileType != ModContent.TileType<CreamCattails>()) || Main.tile[x, num].LiquidType != 0) {
					flag = true;
				}
				num--;
				if (Main.tile[x, num] == null) {
					return;
				}
			}
			num++;
		}

		private void CreamcattailCheck(int x, int y, ref int num, ref bool flag) {
			int num2 = y;
			while ((!Main.tile[x, num2].HasTile || !Main.tileSolid[Main.tile[x, num2].TileType] || Main.tileSolidTop[Main.tile[x, num2].TileType]) && num2 < Main.maxTilesY - 50) {
				if (Main.tile[x, num2].HasTile && Main.tile[x, num2].TileType != ModContent.TileType<CreamCattails>()) {
					flag = true;
				}
				if (!Main.tile[x, num2].HasTile) {
					break;
				}
				num2++;
				if (Main.tile[x, num2] == null) {
					return;
				}
			}
			num = num2 - 1;
			while (Main.tile[x, num] != null && Main.tile[x, num].LiquidAmount > 0 && num > 50) {
				if ((Main.tile[x, num].HasTile && Main.tile[x, num].TileType != ModContent.TileType<CreamCattails>()) || Main.tile[x, num].LiquidType != 0) {
					flag = true;
				}
				num--;
				if (Main.tile[x, num] == null) {
					return;
				}
			}
			num++;
			int num3 = num;
			int num4 = 8;//WorldGen.catTailDistance;
			if (num2 - num3 > num4) {
				flag = true;
			}
			if (!Main.tile[x, num2].HasTile) {
				flag = true;
			}
			num = num2 - 1;
			if (Main.tile[x, num] != null && !Main.tile[x, num].HasTile) {
				for (int num6 = num; num6 >= num3; num6--) {
					if (Main.tile[x, num6] == null) {
						return;
					}
					if (Main.tile[x, num6].HasTile && Main.tile[x, num6].TileType == ModContent.TileType<CreamCattails>()) {
						num = num6;
						break;
					}
				}
			}
			while (Main.tile[x, num] != null && Main.tile[x, num].HasTile && Main.tile[x, num].TileType == ModContent.TileType<CreamCattails>()) {
				num--;
			}
			num++;
			if (Main.tile[x, num2 - 1] != null && Main.tile[x, num2 - 1].LiquidAmount < 127 && WorldGen.genRand.NextBool(4)) {
				flag = true;
			}
			if (Main.tile[x, num] != null && Main.tile[x, num].TileFrameX >= 180 && Main.tile[x, num].LiquidAmount > 127 && WorldGen.genRand.NextBool(4)) {
				flag = true;
			}
			if (Main.tile[x, num] != null && Main.tile[x, num2 - 1] != null && Main.tile[x, num].TileFrameX > 18) {
				if (Main.tile[x, num2 - 1].TileFrameX < 36 || Main.tile[x, num2 - 1].TileFrameX > 72) {
					flag = true;
				}
				else if (Main.tile[x, num].TileFrameX < 90) {
					flag = true;
				}
				else if (Main.tile[x, num].TileFrameX >= 108 && Main.tile[x, num].TileFrameX <= 162) {
					Main.tile[x, num].TileFrameX = 90;
				}
			}
			if (num2 > num + 4 && Main.tile[x, num + 4] != null && Main.tile[x, num + 3] != null && Main.tile[x, num + 4].LiquidAmount == 0 && Main.tile[x, num + 3].TileType == ModContent.TileType<CreamCattails>()) {
				flag = true;
			}
		}

		private void CreamcattailCheck3(int x, int num2, ref int num, ref bool flag) {
			if (Main.tile[x, num2 - 1] != null && Main.tile[x, num2 - 1].LiquidAmount < 127 && WorldGen.genRand.NextBool(4)) {
				flag = true;
			}
			if (Main.tile[x, num] != null && Main.tile[x, num].TileFrameX >= 180 && Main.tile[x, num].LiquidAmount > 127 && WorldGen.genRand.NextBool(4)) {
				flag = true;
			}
			if (Main.tile[x, num] != null && Main.tile[x, num2 - 1] != null && Main.tile[x, num].TileFrameX > 18) {
				if (Main.tile[x, num2 - 1].TileFrameX < 36 || Main.tile[x, num2 - 1].TileFrameX > 72) {
					flag = true;
				}
				else if (Main.tile[x, num].TileFrameX < 90) {
					flag = true;
				}
				else if (Main.tile[x, num].TileFrameX >= 108 && Main.tile[x, num].TileFrameX <= 162) {
					Main.tile[x, num].TileFrameX = 90;
				}
			}
			if (num2 > num + 4 && Main.tile[x, num + 4] != null && Main.tile[x, num + 3] != null && Main.tile[x, num + 4].LiquidAmount == 0 && Main.tile[x, num + 3].TileType == ModContent.TileType<CreamCattails>()) {
				flag = true;
			}
		}

		private bool LilyPadPreventer(On_WorldGen.orig_PlaceLilyPad orig, int x, int j) {
			int num = j;
			while (Main.tile[x, num].LiquidAmount > 0 && num > 50) {
				num--;
			}
			num++;
			int l;
			for (l = num; (!Main.tile[x, l].HasTile || !Main.tileSolid[Main.tile[x, l].TileType] || Main.tileSolidTop[Main.tile[x, l].TileType]) && l < Main.maxTilesY - 50; l++) {
				if (Main.tile[x, l].HasTile && Main.tile[x, l].TileType == ModContent.TileType<CreamCattails>()) {
					return false;
				}
			}
			return orig.Invoke(x, j);
		}

		private void CactusMapColor(ILContext il) {
			ILCursor c = new(il);
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdsfld("Terraria.Map.MapHelper", "tileLookup"),
				i => i.MatchLdloc(7),
				i => i.MatchLdelemU2(),
				i => i.MatchStloc3());
			c.EmitLdarg0(); //i (aka X)
			c.EmitLdarg1(); //j (aka Y)
			c.EmitLdloca(3); //num5
			c.EmitDelegate((int i, int j, ref int num5) => {
				Tile tile = Main.tile[i, j];
				if (tile != null) { //somehow still out of bounds
					WorldGen.GetCactusType(i, j, tile.TileFrameX, tile.TileFrameY, out var sandType);
					if (Main.tile[i, j].TileType == TileID.Cactus && TileLoader.CanGrowModCactus(sandType) && sandType == ModContent.TileType<Creamsand>()) {
						num5 = MapHelper.tileLookup[ModContent.TileType<SprinkleCactusDudTile>()];
					}
				}
			});
		}

		private void VineTileFrame(ILContext il) {
			ILCursor c = new(il);
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdcI4(0),
				i => i.MatchStloc(121));
			c.EmitLdloc(84); //up
			c.EmitLdloca(121); //ref num37
			c.EmitDelegate((int up, ref int num37) => {
				bool numCream = up == ModContent.TileType<CreamGrass>() || up == ModContent.TileType<CreamVines>();
				if (numCream) {
					num37 = ModContent.TileType<CreamVines>();
				}
			});
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdcI4(0),
				i => i.MatchStloc(122));
			c.EmitLdloc3(); //num
			c.EmitLdloc(84); //up
			c.EmitLdloca(122); //ref flag5
			c.EmitDelegate((int num, int up, ref bool flag5) => {
				if (num == ModContent.TileType<CreamVines>() && up != ModContent.TileType<CreamGrass>()) {
					flag5 = true;
				}
			});
		}

		private bool PlaceTile(On_WorldGen.orig_PlaceTile orig, int i, int j, int Type, bool mute, bool forced, int plr, int style) {
			int num = Type;
			if (i >= 0 && j >= 0 && i < Main.maxTilesX && j < Main.maxTilesY) {
				Tile tile = Main.tile[i, j];
				if (forced || Collision.EmptyTile(i, j) || !Main.tileSolid[num]) {
					if (num == ModContent.TileType<CreamGrass_Foliage>()) {
						if (WorldGen.IsFitToPlaceFlowerIn(i, j, num)) {
							if (tile.WallType >= 0 && WallID.Sets.AllowsPlantsToGrow[tile.WallType] && Main.tile[i, j + 1].WallType >= 0 && Main.tile[i, j + 1].WallType < WallLoader.WallCount && WallID.Sets.AllowsPlantsToGrow[Main.tile[i, j + 1].WallType]) {
								if (WorldGen.genRand.NextBool(50) || WorldGen.genRand.NextBool(40)) {
									tile.HasTile = true;
									tile.TileType = (ushort)num;
									tile.TileFrameX = 144;
								}
								else if (WorldGen.genRand.NextBool(35) || (Main.tile[i, j].WallType >= 63 && Main.tile[i, j].WallType <= 70)) {
									tile.HasTile = true;
									tile.TileType = (ushort)num;
									int num3 = WorldGen.genRand.NextFromList<int>(6, 7, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
									tile.TileFrameX = (short)(num3 * 18);
								}
								else {
									tile.HasTile = true;
									tile.TileType = (ushort)num;
									tile.TileFrameX = (short)(WorldGen.genRand.Next(6) * 18);
								}
							}
						}
					}
					else if (num == ModContent.TileType<CreamCattails>()) {
						WorldGen.PlaceCatTail(i, j);
					}
				}
			}
			return orig.Invoke(i, j, Type, mute, forced, plr, style);
		}

		private bool Flowerplacement(On_WorldGen.orig_IsFitToPlaceFlowerIn orig, int x, int y, int typeAttemptedToPlace) {
			if (y < 1 || y > Main.maxTilesY - 1) {
				return false;
			}
			Tile tile = Main.tile[x, y + 1];
			if (tile.HasTile && tile.Slope == 0 && !tile.IsHalfBlock) {
				if ((tile.TileType != ModContent.TileType<CreamGrass>() && tile.TileType != ModContent.TileType<CreamGrassMowed>()) || typeAttemptedToPlace != ModContent.TileType<CreamGrass_Foliage>()) {
					return false;
				}
				return true;
			}
			return orig.Invoke(x, y, typeAttemptedToPlace);
		}

		private bool FlowerBootsEdit(On_Player.orig_DoBootsEffect_PlaceFlowersOnTile orig, Player self, int X, int Y) {
			Tile tile = Main.tile[X, Y];
			if (tile == null) {
				return false;
			}
			if (!tile.HasTile && tile.LiquidType == 0 && Main.tile[X, Y + 1] != null && WorldGen.SolidTile(X, Y + 1)) {
				tile.TileFrameY = 0;
				tile.Slope = 0;
				tile.IsHalfBlock = false;
				if (Main.tile[X, Y + 1].TileType == ModContent.TileType<CreamGrass>() || Main.tile[X, Y + 1].TileType == ModContent.TileType<CreamGrassMowed>()) {
					int[] ShortgrassArray = new int[] { 6, 7, 10, 15, 16, 17 };
					//if (Main.rand.Next(2) == 0) {
					tile.HasTile = true;
					tile.TileType = (ushort)ModContent.TileType<CreamGrass_Foliage>();
					tile.TileFrameX = (short)(18 * ShortgrassArray[Main.rand.Next(6)]);
					tile.CopyPaintAndCoating(Main.tile[X, Y + 1]);
					while (tile.TileFrameX == 90) {
						tile.TileFrameX = (short)(18 * Main.rand.Next(4, 7));
					}
					/*}
					else {
						tile.active(active: true);
						tile.type = 113;
						tile.frameX = (short)(18 * Main.rand.Next(2, 8));
						tile.CopyPaintAndCoating(Main.tile[X, Y + 1]);
						while (tile.frameX == 90) {
							tile.frameX = (short)(18 * Main.rand.Next(2, 8));
						}
					}*/
					if (Main.netMode == NetmodeID.MultiplayerClient) {
						NetMessage.SendTileSquare(-1, X, Y);
					}
					return true;
				}
			}
			return orig.Invoke(self, X, Y);
		}

		private void PlantTileFrameIL(ILContext il) {
			try {
				ILCursor c = new(il);
				ILLabel IL_0433 = c.DefineLabel();
				c.GotoNext(
					MoveType.Before,
					i => i.MatchLdcI4(0),
					i => i.MatchStloc2(),
					i => i.MatchLdloc1(),
					i => i.MatchLdcI4(3),
					i => i.MatchBeq(out _)
					);
				c.MarkLabel(IL_0433);
				c.GotoPrev(
					MoveType.Before,
					i => i.MatchLdloca(5),
					i => i.MatchCall<Tile>("get_type"),
					i => i.MatchPop(),
					i => i.MatchLdloc1(),
					i => i.MatchLdcI4(3),
					i => i.MatchBneUn(out _)
					);
				c.GotoPrev(
					MoveType.Before,
					i => i.MatchLdloca(5),
					i => i.MatchCall<Tile>("nactive"),
					i => i.MatchBrfalse(out _)
					);
				if (IL_0433 == null) {
					ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("Plant Check's main tile framing IlLable could not be found");
					return;
				}
				c.EmitLdloc1(); //num2
				c.EmitLdloc0(); //num
				c.EmitDelegate((int num2, int num) => {
					return (!(num2 != ModContent.TileType<CreamGrass_Foliage>() || num == ModContent.TileType<CreamGrass>() || num == ModContent.TileType<CreamGrassMowed>()));
				});
				c.EmitBrtrue(IL_0433);

				c.GotoNext(
					MoveType.After,
					i => i.MatchLdcI4(0),
					i => i.MatchStloc2()
					);
				c.EmitLdloc0(); //num
				c.EmitLdloca(1); //num2
				c.EmitLdloca(2); //flag
				c.EmitLdloca(5); //tile
				c.EmitLdarg0(); //x
				c.EmitLdarg1(); //y
				c.EmitDelegate((int num, ref int num2, ref bool flag, ref Tile tile, int x, int y) => {
					if (num2 == ModContent.TileType<CreamGrass_Foliage>()) {
						tile = Main.tile[x, y]; //The last use of the tile variable uses the coords [x + 1, y + 1], so we reset it here
						flag = tile.TileFrameX == 144; //This is supposed to convert crimson mushrooms to yumdrops but doesnt????
					}
					if (num == ModContent.TileType<CreamGrass>() || num == ModContent.TileType<CreamGrassMowed>()) {
						num2 = ModContent.TileType<CreamGrass_Foliage>();
					}
				});
			}
			catch (Exception) {
			}
		}

		private void BurnGrass(ILContext il) {
			ILCursor c = new(il);
			if (!c.TryGotoNext(
				MoveType.After,
				i => i.MatchLdloca(10),
				i => i.MatchCall<Tile>("active"), //Gets the if statement checking if the tile at tile5 is active
				i => i.MatchBrfalse(out _))) //aka this places this BEFORE the check for normal grasses (normal, hallowed, corruption, (anything that uses dirt)
			{
				ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("The Confection REBAKED: lava tile burning instructions not found");
				return;
			}
			c.EmitLdloca(10); //ref tile5
			c.EmitLdloc(8); //i
			c.EmitLdloc(9); //j
			c.EmitLdloc(0); //num
			c.EmitLdloc(1); //num2
			c.EmitDelegate((ref Tile tile5, int i, int j, int num, int num2) => {
				if (tile5.TileType == ModContent.TileType<CreamGrass>()) { //Turns Creamgrass into Cookie block when lava is near
					tile5.TileType = (ushort)ModContent.TileType<CookieBlock>();
					WorldGen.SquareTileFrame(i, j);
					if (Main.netMode == NetmodeID.Server) {
						NetMessage.SendTileSquare(-1, num, num2, 3);
					}
				}
			});
		}

		private bool PickaxeKillTile(On_Player.orig_DoesPickTargetTransformOnKill orig, Player self, HitTile hitCounter, int damage, int x, int y, int pickPower, int bufferIndex, Tile tileTarget) {
			if (hitCounter.AddDamage(bufferIndex, damage, updateAmount: false) >= 100 && tileTarget.TileType == ModContent.TileType<CreamGrass>()) {
				return true;
			}
			else {
				return orig.Invoke(self, hitCounter, damage, x, y, pickPower, bufferIndex, tileTarget);
			}
		}

		private void KillConjoinedGrass_PlaceThing(On_Player.orig_PlaceThing_Tiles_PlaceIt_KillGrassForSolids orig, Player self) {
			orig.Invoke(self);
			for (int i = Player.tileTargetX - 1; i <= Player.tileTargetX + 1; i++) {
				for (int j = Player.tileTargetY - 1; j <= Player.tileTargetY + 1; j++) {
					Tile tile = Main.tile[i, j];
					if (!tile.HasTile || self.inventory[self.selectedItem].createTile == tile.TileType || tile.TileType != ModContent.TileType<CreamGrass>()) {
						continue;
					}
					bool flag = true;
					for (int k = i - 1; k <= i + 1; k++) {
						for (int l = j - 1; l <= j + 1; l++) {
							if (!WorldGen.SolidTile(k, l)) {
								flag = false;
							}
						}
					}
					if (flag) {
						WorldGen.KillTile(i, j, fail: true);
						if (Main.netMode == NetmodeID.MultiplayerClient) {
							NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 1f);
						}
					}
				}
			}
		}
	}

	public static class ConfectionWindUtilities {
		public static void Load() {
			_addSpecialPointSpecialPositions = typeof(Terraria.GameContent.Drawing.TileDrawing).GetField("_specialPositions", BindingFlags.NonPublic | BindingFlags.Instance);
			_addSpecialPointSpecialsCount = typeof(Terraria.GameContent.Drawing.TileDrawing).GetField("_specialsCount", BindingFlags.NonPublic | BindingFlags.Instance);
			_addVineRootPositions = typeof(Terraria.GameContent.Drawing.TileDrawing).GetField("_vineRootsPositions", BindingFlags.NonPublic | BindingFlags.Instance);
		}

		public static void Unload() {
			_addSpecialPointSpecialPositions = null;
			_addSpecialPointSpecialsCount = null;
			_addVineRootPositions = null;
		}

		public static FieldInfo _addSpecialPointSpecialPositions;
		public static FieldInfo _addSpecialPointSpecialsCount;
		public static FieldInfo _addVineRootPositions;

		public static void AddSpecialPoint(this Terraria.GameContent.Drawing.TileDrawing tileDrawing, int x, int y, int type) {
			if (_addSpecialPointSpecialPositions.GetValue(tileDrawing) is Point[][] _specialPositions) {
				if (_addSpecialPointSpecialsCount.GetValue(tileDrawing) is int[] _specialsCount) {
					_specialPositions[type][_specialsCount[type]++] = new Point(x, y);
				}
			}
		}

		public static void CrawlToTopOfVineAndAddSpecialPoint(this Terraria.GameContent.Drawing.TileDrawing tileDrawing, int j, int i) {
			if (_addVineRootPositions.GetValue(tileDrawing) is List<Point> _vineRootsPositions) {
				int y = j;
				for (int num = j - 1; num > 0; num--) {
					Tile tile = Main.tile[i, num];
					if (WorldGen.SolidTile(i, num) || !tile.HasTile) {
						y = num + 1;
						break;
					}
				}
				Point item = new(i, y);
				if (!_vineRootsPositions.Contains(item)) {
					_vineRootsPositions.Add(item);
					Main.instance.TilesRenderer.AddSpecialPoint(i, y, 6);
				}
			}
		}
	}
}
