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
using Terraria.GameContent.Drawing;
using Terraria.GameContent;
using Mono.Cecil.Cil;

namespace TheConfectionRebirth
{
	public class TheConfectionRebirth : Mod
	{
		//Edit the following
		//WorldGen.cs
		//PlantCheck (done) (i think) - crimson mushrooms dont convert to yumdrops and vise versa for some dogshit reason - doesnt convert some purity grass correctly
		//TileFrame - Vines dont properly convert - works only if fps is lower than 30

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
			IL_WorldGen.CheckCatTail += CheckCattailEdit;
			IL_WorldGen.PlaceCatTail += PlaceCattailEdit;
			On_WorldGen.GrowCatTail += GrowCattailEdit;
			IL_WorldGen.CheckLilyPad += CheckLilyPadEdit;
			IL_WorldGen.PlaceLilyPad += PlaceLilyPadEdit;
			On_TileDrawing.DrawSingleTile += LilyPadDrawingPreventer;
			On_Liquid.DelWater += LilyPadCheck;
			On_Main.DrawTileInWater += LilyPadDrawing;
			IL_WorldGen.PlantSeaOat += PlantSeaOatEdit;
			IL_WorldGen.PlaceOasisPlant += PlaceOasisPlant;
			On_TileDrawing.DrawMultiTileGrassInWind += MultiTileGrassDetour;
			On_WorldGen.PlaceOasisPlant += PlantOasisPlantEdit;
			On_Player.MowGrassTile += LAWWWWNNNNMOOOWWWWWWAAAAAA;
			On_SmartCursorHelper.Step_LawnMower += SMARTLAWWWWWWNNNNNMOWWWWAAAAASSSSS;
			IL_NPC.SpawnNPC += LawnSpawnPrevention;
			On_SmartCursorHelper.Step_GrassSeeds += CreamBeansSmartCursor;
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
			IL_WorldGen.CheckCatTail -= CheckCattailEdit;
			IL_WorldGen.PlaceCatTail -= PlaceCattailEdit;
			On_WorldGen.GrowCatTail -= GrowCattailEdit;
			IL_WorldGen.CheckLilyPad -= CheckLilyPadEdit;
			IL_WorldGen.PlaceLilyPad -= PlaceLilyPadEdit;
			On_TileDrawing.DrawSingleTile -= LilyPadDrawingPreventer;
			On_Liquid.DelWater -= LilyPadCheck;
			On_Main.DrawTileInWater -= LilyPadDrawing;
			IL_WorldGen.PlantSeaOat -= PlantSeaOatEdit;
			IL_WorldGen.PlaceOasisPlant -= PlaceOasisPlant;
			On_TileDrawing.DrawMultiTileGrassInWind -= MultiTileGrassDetour; 
			On_WorldGen.PlaceOasisPlant -= PlantOasisPlantEdit;
			On_Player.MowGrassTile -= LAWWWWNNNNMOOOWWWWWWAAAAAA;
			On_SmartCursorHelper.Step_LawnMower -= SMARTLAWWWWWWNNNNNMOWWWWAAAAASSSSS;
			IL_NPC.SpawnNPC -= LawnSpawnPrevention;
			On_SmartCursorHelper.Step_GrassSeeds -= CreamBeansSmartCursor;
		}

		private void CreamBeansSmartCursor(On_SmartCursorHelper.orig_Step_GrassSeeds orig, object providedInfo, ref int focusedX, ref int focusedY) {
			orig.Invoke(providedInfo, ref focusedX, ref focusedY);
			var SmartCursorUsageInfo = typeof(SmartCursorHelper).GetNestedType("SmartCursorUsageInfo", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
			Item item = (Item)SmartCursorUsageInfo.GetField("item", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int reachableStartX = (int)SmartCursorUsageInfo.GetField("reachableStartX", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int reachableEndX = (int)SmartCursorUsageInfo.GetField("reachableEndX", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int reachableStartY = (int)SmartCursorUsageInfo.GetField("reachableStartY", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int reachableEndY = (int)SmartCursorUsageInfo.GetField("reachableEndY", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			Vector2 mouse = (Vector2)SmartCursorUsageInfo.GetField("mouse", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			List<Tuple<int, int>> _targets = (List<Tuple<int, int>>)typeof(SmartCursorHelper).GetField("_targets", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).GetValue(null);
			if (focusedX > -1 || focusedY > -1) {
				return;
			}
			int type = item.type;
			if ((type < 0 || !ItemID.Sets.GrassSeeds[type]) && type != ModContent.ItemType<Items.Placeable.CreamBeans>()) {
				return;
			}
			_targets.Clear();
			for (int i = reachableStartX; i <= reachableEndX; i++) {
				for (int j = reachableStartY; j <= reachableEndY; j++) {
					Tile tile = Main.tile[i, j];
					bool flag = !Main.tile[i - 1, j].HasTile || !Main.tile[i, j + 1].HasTile || !Main.tile[i + 1, j].HasTile || !Main.tile[i, j - 1].HasTile;
					bool flag2 = !Main.tile[i - 1, j - 1].HasTile || !Main.tile[i - 1, j + 1].HasTile || !Main.tile[i + 1, j + 1].HasTile || !Main.tile[i + 1, j - 1].HasTile;
					if (tile.HasTile && !tile.IsActuated && (flag || flag2)) {
						bool flag3 = false;
						if (type == ModContent.ItemType<Items.Placeable.CreamBeans>()) {
							flag3 = tile.TileType == ModContent.TileType<Tiles.CookieBlock>() || tile.TileType == TileID.Dirt;
						}
						if (flag3) {
							_targets.Add(new Tuple<int, int>(i, j));
						}
					}
				}
			}
			if (_targets.Count > 0) {
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++) {
					float num2 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, mouse);
					if (num == -1f || num2 < num) {
						num = num2;
						tuple = _targets[k];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, reachableStartX, reachableStartY, reachableEndX, reachableEndY)) {
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		#region LAAAAAAWWWWWWNNNNNMOWWWWWWAAAAAAA!!!!!
		private void LawnSpawnPrevention(ILContext il) {
			ILCursor c = new(il);
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdloca(9),
				i => i.MatchLdloc(77),
				i => i.MatchStfld<NPCSpawnInfo>("PlayerFloorY"));
			c.EmitLdloc(6); //num35
			c.EmitLdloca(2); // ref flag12
			c.EmitDelegate((int num35, ref bool flag12) => {
				if (num35 == ModContent.TileType<CreamGrassMowed>() && !Main.bloodMoon && !Main.eclipse && Main.invasionType <= 0 && !Main.pumpkinMoon && !Main.snowMoon && !Main.slimeRain && Main.rand.Next(100) < 10) {
					flag12 = false;
				}
			});
		}

		private void SMARTLAWWWWWWNNNNNMOWWWWAAAAASSSSS(On_SmartCursorHelper.orig_Step_LawnMower orig, object providedInfo, ref int fX, ref int fY) {
			orig.Invoke(providedInfo, ref fX, ref fY);
			var SmartCursorUsageInfo = typeof(SmartCursorHelper).GetNestedType("SmartCursorUsageInfo", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
			Item item = (Item)SmartCursorUsageInfo.GetField("item", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int screenTargetX = (int)SmartCursorUsageInfo.GetField("screenTargetX", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int screenTargetY = (int)SmartCursorUsageInfo.GetField("screenTargetY", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int reachableStartX = (int)SmartCursorUsageInfo.GetField("reachableStartX", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int reachableEndX = (int)SmartCursorUsageInfo.GetField("reachableEndX", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int reachableStartY = (int)SmartCursorUsageInfo.GetField("reachableStartY", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int reachableEndY = (int)SmartCursorUsageInfo.GetField("reachableEndY", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			Vector2 mouse = (Vector2)SmartCursorUsageInfo.GetField("mouse", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			List<Tuple<int, int>> _targets = (List<Tuple<int, int>>)typeof(SmartCursorHelper).GetField("_targets", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).GetValue(null);
			_ = screenTargetX;
			_ = screenTargetY;
			if (item.type != 4049 || fX != -1 || fY != -1) {
				return;
			}
			_targets.Clear();
			for (int i = reachableStartX; i <= reachableEndX; i++) {
				for (int j = reachableStartY; j <= reachableEndY; j++) {
					Tile tile = Main.tile[i, j];
					if (tile.HasTile && (tile.TileType == ModContent.TileType<CreamGrass>())) {
						_targets.Add(new Tuple<int, int>(i, j));
					}
				}
			}
			if (_targets.Count > 0) {
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++) {
					float num2 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, mouse);
					if (num == -1f || num2 < num) {
						num = num2;
						tuple = _targets[k];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, reachableStartX, reachableStartY, reachableEndX, reachableEndY)) {
					fX = tuple.Item1;
					fY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private void LAWWWWNNNNMOOOWWWWWWAAAAAA(On_Player.orig_MowGrassTile orig, Player self, Vector2 thePos) {
			orig.Invoke(self, thePos);
			Point point = thePos.ToTileCoordinates();
			Tile tile = Main.tile[point.X, point.Y];
			ushort num = 0;
			if (tile.TileType == ModContent.TileType<CreamGrass>()) {
				num = (ushort)ModContent.TileType<CreamGrassMowed>();
			}
			if (num != 0) {
				int num2 = WorldGen.KillTile_GetTileDustAmount(fail: true, tile, point.X, point.Y);
				for (int i = 0; i < num2; i++) {
					WorldGen.KillTile_MakeTileDust(point.X, point.Y, tile);
				}
				tile.TileType = num;
				if (Main.netMode == 1) {
					NetMessage.SendTileSquare(-1, point.X, point.Y);
				}
			}

		}
		#endregion

		#region OasisPlants

		private void PlantOasisPlantEdit(On_WorldGen.orig_PlaceOasisPlant orig, int X, int Y, ushort type) {
			if (type == 530) {
				if (Main.tile[X, Y + 1].TileType == ModContent.TileType<Creamsand>()) {
					type = (ushort)ModContent.TileType<CreamOasisPlants>();
				}
			}
			orig.Invoke(X, Y, type);
		}

		private void MultiTileGrassDetour(On_TileDrawing.orig_DrawMultiTileGrassInWind orig, TileDrawing self, Vector2 screenPosition, Vector2 offSet, int topLeftX, int topLeftY, int sizeX, int sizeY) {
			if (Main.tile[topLeftX, topLeftY].TileType == ModContent.TileType<CreamOasisPlants>()) {
				sizeX = 3;
				sizeY = 2;
			}
			orig.Invoke(self, screenPosition, offSet, topLeftX, topLeftY, sizeX, sizeY);
		}
		#endregion

		#region SeaOats
		private void PlaceOasisPlant(ILContext il) {
			ILCursor c = new(il);
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdcI4(529),
				i => i.MatchBeq(out _),
				i => i.MatchLdcI4(0),
				i => i.MatchStloc2());
			c.EmitLdloc(3); //i
			c.EmitLdloc(4); //j
			c.EmitLdloca(2); //ref flag
			c.EmitDelegate((int i, int j, ref bool flag) => {
				if (Main.tile[i, j].TileType == ModContent.TileType<CreamSeaOats>() && !flag) {
					flag = true;
				}
			});
		}

		private void PlantSeaOatEdit(ILContext il) {
			ILCursor c = new(il);
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdcI4(529),
				i => i.MatchStindI2(),
				i => i.MatchLdsflda<Main>("tile"),
				i => i.MatchLdarg0(),
				i => i.MatchLdarg1(),
				i => i.MatchCall<Tilemap>("get_Item"),
				i => i.MatchStloc1());
			c.EmitLdarg0(); //x
			c.EmitLdarg1(); //y
			c.EmitDelegate((int x, int y) => {
				if (Main.tile[x, y + 1].TileType == ModContent.TileType<Creamsand>()) {
					Main.tile[x, y].TileType = (ushort)ModContent.TileType<CreamSeaOats>();
				}
			});
		}
		#endregion

		#region LilyPads
		private void LilyPadDrawing(On_Main.orig_DrawTileInWater orig, Vector2 drawOffset, int x, int y) {
			orig.Invoke(drawOffset, x, y);
			if (Main.tile[x, y] != null && Main.tile[x, y].HasTile && Main.tile[x, y].TileType == ModContent.TileType<CreamLilyPads>()) {
				Main.instance.LoadTiles(Main.tile[x, y].TileType);
				Tile tile = Main.tile[x, y];
				int num = tile.LiquidAmount / 16;
				num -= 3;
				if (WorldGen.SolidTile(x, y - 1) && num > 8) {
					num = 8;
				}
				Rectangle value = new((int)tile.TileFrameX, (int)tile.TileFrameY, 16, 16);
				Main.spriteBatch.Draw(TextureAssets.Tile[tile.TileType].Value, new Vector2((float)(x * 16), (float)(y * 16 - num)) + drawOffset, (Rectangle?)value, Lighting.GetColor(x, y), 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
			}
		}

		private void LilyPadCheck(On_Liquid.orig_DelWater orig, int l) {
			orig.Invoke(l);
			int num = Main.liquid[l].x;
			int num2 = Main.liquid[l].y;
			Tile tile4 = Main.tile[num, num2];
			if (!Main.tileAlch[tile4.TileType] && tile4.TileType == ModContent.TileType<CreamLilyPads>()) {
				if (Liquid.quickFall) {
					WorldGen.CheckLilyPad(num, num2);
				}
				else if (Main.tile[num, num2 + 1].LiquidAmount < byte.MaxValue || Main.tile[num, num2 - 1].LiquidAmount > 0) {
					WorldGen.SquareTileFrame(num, num2);
				}
				else {
					WorldGen.CheckLilyPad(num, num2);
				}
			}
		}

		private void LilyPadDrawingPreventer(On_TileDrawing.orig_DrawSingleTile orig, TileDrawing self, Terraria.DataStructures.TileDrawInfo drawData, bool solidLayer, int waterStyleOverride, Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY) {
			drawData.tileCache = Main.tile[tileX, tileY]; //Doesnt quite work yet, probably something to do with lilypads being drawn elsewhere (not inside of TileDrawing), probs use vs to look for any instance of LilyPad or 518
			drawData.typeCache = drawData.tileCache.TileType;
			drawData.tileFrameX = drawData.tileCache.TileFrameX;
			drawData.tileFrameY = drawData.tileCache.TileFrameY;
			drawData.tileLight = Lighting.GetColor(tileX, tileY);
			if (drawData.tileCache.LiquidAmount > 0 && drawData.tileCache.TileType == ModContent.TileType<CreamLilyPads>()) {
				return;
			}
			orig.Invoke(self, drawData, solidLayer, waterStyleOverride, screenPosition, screenOffset, tileX, tileY);
		}

		private void PlaceLilyPadEdit(ILContext il) {
			ILCursor c = new(il);
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdcI4(5),
				i => i.MatchStloc(1),
				i => i.MatchLdcI4(0),
				i => i.MatchStloc(2));
			c.EmitLdarg(0); //x
			c.EmitLdloc1(); //num2
			c.EmitLdloc0(); //num
			c.EmitLdloca(2); //ref num3
			c.EmitDelegate((int x, int num2, int num, ref int num3) => {
				for (int i = x - num2; i <= x + num2; i++) {
					for (int k = num - num2; k <= num + num2; k++) {
						if (Main.tile[i, k].HasTile && Main.tile[i, k].TileType == ModContent.TileType<CreamLilyPads>()) {
							num3++;
						}
					}
				}
			});
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdindU2(),
				i => i.MatchStloc(5),
				i => i.MatchLdcI4(-1), 
				i => i.MatchStloc(6));
			c.EmitLdloc(5); //type
			c.EmitLdloca(6); //ref num5
			c.EmitDelegate((int type, ref int num5) => {
				if (type == ModContent.TileType<CreamGrass>() || type == ModContent.TileType<CreamGrassMowed>() || type == ModContent.TileType<Creamsand>()) {
					num5 = ModContent.TileType<CreamLilyPads>();
				}
			});
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdloca(7),
				i => i.MatchCall<Tile>("get_frameY"),
				i => i.MatchLdloc(6),
				i => i.MatchConvI2(),
				i => i.MatchStindI2());
			c.EmitLdarg(0); //x
			c.EmitLdloc(0); //num
			c.EmitLdloc(6); //num5
			c.EmitDelegate((int x, int num, int num5) => {
				if (num5 == ModContent.TileType<CreamLilyPads>()) {
					Main.tile[x, num].TileType = (ushort)num5;
					Main.tile[x, num].TileFrameY = 0;
				}
			});
		}

		private void CheckLilyPadEdit(ILContext il) {
			ILCursor c = new(il);
			ILLabel IL_0000 = c.DefineLabel();
			c.GotoNext(
				MoveType.After,
				i => i.MatchStloc(1),
				i => i.MatchLdcI4(-1), //also known as Ldc.i4.m1
				i => i.MatchStloc(2));
			c.EmitLdloc(1); //type
			c.EmitLdloca(2); //ref num2
			c.EmitLdarg0(); //x
			c.EmitLdarg1(); //y
			c.EmitLdloca(3); //ref tile
			c.EmitDelegate((int type, ref int num2, int x, int y, ref Tile tile) => { //we inject this to change the num2 to our tile when under a certain tile type (we use the TileID for creamlilypads as num2 for the same reasons as CheckCattail())
				if (type == ModContent.TileType<CreamGrass>() || type == ModContent.TileType<Creamsand>()) {
					num2 = -1;
					int num3 = ModContent.TileType<CreamLilyPads>();
					tile = Main.tile[x, y];
					if (num3 != tile.TileType) {
						tile.TileType = (ushort)num3;
						tile.TileFrameY = 0;
						if (Main.netMode == 2) {
							NetMessage.SendTileSquare(-1, x, y);
						}
					}
					tile = Main.tile[x, y - 1];
					if (tile.LiquidType > 0) {
						if (!tile.HasTile) {
							tile.HasTile = true;
							tile.TileType = (ushort)ModContent.TileType<CreamLilyPads>();
							ref short frameX = ref tile.TileFrameX;
							tile = Main.tile[x, y];
							frameX = tile.TileFrameX;
							tile = Main.tile[x, y - 1];
							ref short frameY = ref tile.TileFrameY;
							tile = Main.tile[x, y];
							frameY = tile.TileFrameY;
							tile = Main.tile[x, y - 1];
							tile.IsHalfBlock = false;
							tile.Slope = 0;
							tile = Main.tile[x, y];
							tile.HasTile = false;
							tile.TileType = 0;
							WorldGen.SquareTileFrame(x, y - 1, resetFrame: false);
							if (Main.netMode == 2) {
								NetMessage.SendTileSquare(-1, x, y - 1, 1, 2);
							}
							return;
						}
					}
					tile = Main.tile[x, y];
					if (tile.LiquidAmount != 0) {
						return;
					}
					Tile tileSafely = Framing.GetTileSafely(x, y + 1);
					if (!tileSafely.HasTile) {
						tile = Main.tile[x, y + 1];
						tile.HasTile = true;
						tile.TileType = (ushort)ModContent.TileType<CreamLilyPads>();
						ref short frameX2 = ref tile.TileFrameX;
						tile = Main.tile[x, y];
						frameX2 = tile.TileFrameX;
						tile = Main.tile[x, y + 1];
						ref short frameY2 = ref tile.TileFrameY;
						tile = Main.tile[x, y];
						frameY2 = tile.TileFrameY;
						tile = Main.tile[x, y + 1];
						tile.IsHalfBlock = false;
						tile.Slope = 0;
						tile = Main.tile[x, y];
						tile.HasTile = false;
						tile = Main.tile[x, y];
						tile.TileType = 0;
						WorldGen.SquareTileFrame(x, y + 1, resetFrame: false);
						if (Main.netMode == 2) {
							NetMessage.SendTileSquare(-1, x, y, 1, 2);
						}
					}
					else if (tileSafely.HasTile && !TileID.Sets.Platforms[tileSafely.TileType] && (!Main.tileSolid[tileSafely.TileType] || Main.tileSolidTop[tileSafely.TileType])) {
						WorldGen.KillTile(x, y);
						if (Main.netMode == 2) {
							NetMessage.SendData(17, -1, -1, null, 0, x, y);
						}
					}
				}
			});
			c.EmitLdloc(1); //type
			c.EmitDelegate((int type) => {
				return type == ModContent.TileType<CreamGrass>() || type == ModContent.TileType<Creamsand>() || type == ModContent.TileType<CreamGrassMowed>();
			});
			c.EmitBrfalse(IL_0000);
			c.EmitRet(); //return
			c.MarkLabel(IL_0000);

			c.GotoNext(
				MoveType.After,
				i => i.MatchLdloca(3),
				i => i.MatchCall<Tile>("get_frameY"),
				i => i.MatchLdloc(2),
				i => i.MatchConvI2(),
				i => i.MatchStindI2());
			c.EmitLdloca(3); //ref Tile
			c.EmitDelegate((ref Tile tile) => { //set the TileType to lilyPad since so the conversion between our and their tiles dont result in unintended consiquences
				tile.TileType = TileID.LilyPad;
			});
		}
		#endregion

		#region Cattails
		public static int ClimbCreamCatTail(int originx, int originy) {
			int num = 0;
			int num2 = originy;
			while (num2 > 10) {
				Tile tile = Main.tile[originx, num2];
				if (!tile.HasTile || tile.TileType != ModContent.TileType<CreamCattails>()) {
					break;
				}
				if (tile.TileFrameX >= 180) {
					num++;
					break;
				}
				num2--;
				num++;
			}
			return num;
		}

		private void GrowCattailEdit(On_WorldGen.orig_GrowCatTail orig, int x, int j) {
			ConfectionWorldGeneration.GrowCreamCatTail(x, j);
			orig.Invoke(x, j);
		}

		private void PlaceCattailEdit(ILContext il) {
			ILCursor c = new(il);
			ILLabel IL_0000 = c.DefineLabel();
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdcI4(7),
				i => i.MatchStloc(2),
				i => i.MatchLdcI4(0),
				i => i.MatchStloc(3));
			c.EmitLdarg(0); //x
			c.EmitLdloc2(); //num2
			c.EmitLdloc0(); //num
			c.EmitLdloca(3); //ref num3
			c.EmitDelegate((int x, int num2, int num, ref int num3) => {
				for (int i = x - num2; i <= x + num2; i++) { //
					for (int k = num - num2; k <= num + num2; k++) {
						if (Main.tile[i, k].HasTile && Main.tile[i, k].TileType == ModContent.TileType<CreamCattails>()) {
							num3++;
							break;
						}
					}
				}
			});
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdloc1(),
				i => i.MatchRet(),
				i => i.MatchLdcI4(-1), //also known as m1
				i => i.MatchStloc(7));
			c.EmitLdloc(6); //type
			c.EmitLdloca(7); //ref num5
			c.EmitDelegate((int type, ref int num5) => {
				if (type == ModContent.TileType<CreamGrass>() || type == ModContent.TileType<CreamGrassMowed>() || type == ModContent.TileType<Creamsand>()) {
					num5 = ModContent.TileType<CreamCattails>();
				}
			});
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdloc1(),
				i => i.MatchRet(),
				i => i.MatchLdloc(4),
				i => i.MatchLdcI4(1),
				i => i.MatchSub(),
				i => i.MatchStloc0());
			c.EmitLdloc(7); //num5
			c.EmitDelegate((int num5) => {
				return num5 == ModContent.TileType<CreamCattails>();
			});
			c.EmitBrfalse(IL_0000);
			c.EmitLdloc(7); //num5
			c.EmitLdarg(0); //x
			c.EmitLdloc0(); //num
			c.EmitDelegate((int num5, int x, int num) => {
				Tile tile2 = Main.tile[x, num];
				tile2.HasTile = true;
				Main.tile[x, num].TileType = (ushort)num5;
				Main.tile[x, num].TileFrameX = 0;
				Main.tile[x, num].TileFrameY = 0;
				tile2.IsHalfBlock = false;
				tile2.Slope = 0;
				Main.tile[x, num].CopyPaintAndCoating(Main.tile[x, num + 1]);
				WorldGen.SquareTileFrame(x, num);

			});
			//return new Point(x, num);
			c.EmitLdarg0(); //x
			c.EmitLdloc0(); //num
			c.EmitNewobj(typeof(Point).GetConstructor([typeof(int), typeof(int)])); //new Point()
			c.EmitRet(); //return
			c.MarkLabel(IL_0000); //thanks fox for figuring out a solution for me for the in if bound return :3
		}

		private void CheckCattailEdit(ILContext il) {
			ILCursor c = new(il);
			c.GotoNext(
				MoveType.After,
				i => i.MatchStloc(5),
				i => i.MatchLdcI4(-1), //also known as Ldc.i4.m1
				i => i.MatchStloc(6));
			c.EmitLdloc(5); //type
			c.EmitLdloca(6); //ref num5
			c.EmitDelegate((int type, ref int num5) => { //injects this delegate just before the switch to set num5 to the TileFrameY
				if (type == ModContent.TileType<CreamGrass>() || type == ModContent.TileType<CreamGrassMowed>() || type == ModContent.TileType<Creamsand>()) { 
					num5 = ModContent.TileType<CreamCattails>(); 
				}
			}); //num5 is usually used for tileframeY of the cattail type (normal, desert, hallow(unused), corruption, crimson, mushroom)
				//but here we use the TileID so that we dont clash with other mods
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdcI4(519),
				i => i.MatchBeq(out _),
				i => i.MatchLdloc0(),
				i => i.MatchLdcI4(1), 
				i => i.MatchAdd(), 
				i => i.MatchStloc0());
			c.EmitLdarg0(); //x
			c.EmitLdloc2(); //num2
			c.EmitLdloc1(); //flag
			c.EmitLdloca(0); //ref num
			c.EmitLdloca(6); //ref num5
			c.EmitDelegate((int x, int num2, bool flag, ref int num, ref int num5) => { //to make injection easier, we insert this after the incrimenting of num,
																						//this converts the tile if num5 is the creamcattail ID
				if (Main.tile[x, num2].TileType == ModContent.TileType<CreamCattails>()) {
					CreamcattailCheck(x, num2, ref num, ref flag);
				}
				if (!flag) {
					if (num5 == ModContent.TileType<CreamCattails>()) {
						for (int k = num; k < num2; k++) {
							if (Main.tile[x, k] != null && Main.tile[x, k].HasTile) {
								Main.tile[x, k].TileType = (ushort)num5;
								Main.tile[x, k].TileFrameY = 0;
								if (Main.netMode == NetmodeID.Server) {
									NetMessage.SendTileSquare(-1, x, num);
								}
							}
						}
						//return; //commented out since returns dont work in IL edits, id have to call a ret instruction here 
					}
				}
			});
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdloc(6),
				i => i.MatchConvI2(),
				i => i.MatchStindI2());
			c.EmitLdarg1(); //x
			c.EmitLdloc(11); //k
			c.EmitDelegate((int x, int k) => { //Adds tiletype to make sure we arent placing a CreamCattail instead
				Main.tile[x, k].TileType = TileID.Cattail;
			});
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
		#endregion

		private bool PlaceTile(On_WorldGen.orig_PlaceTile orig, int i, int j, int Type, bool mute, bool forced, int plr, int style) {
			int num = Type;
			if (i >= 0 && j >= 0 && i < Main.maxTilesX && j < Main.maxTilesY) {
				Tile tile = Main.tile[i, j];
				if (forced || Collision.EmptyTile(i, j) || !Main.tileSolid[num] || num == ModContent.TileType<CreamGrass>() && (tile.TileType == 0 || tile.TileType == ModContent.TileType<CookieBlock>()) && tile.HasTile) {
					if (num == ModContent.TileType<CreamGrass>() && ((tile.TileType != 0 && tile.TileType != ModContent.TileType<CookieBlock>()) || !tile.HasTile)) {
						return false;
					}
				}
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
					if (tile.LiquidAmount > 0 || tile.CheckingLiquid) {
						int num5 = num; 
						if (!TileID.Sets.Torch[num]) {
							if (num5 <= ModContent.TileType<CreamSeaOats>()) {
								if (num5 == ModContent.TileType<CreamSeaOats>()) {
									return false;
								}
							}
						}
					}
					else if (num == ModContent.TileType<CreamLilyPads>()) {
						WorldGen.PlaceLilyPad(i, j);
					}
					else if (num == ModContent.TileType<CreamCattails>()) {
						WorldGen.PlaceCatTail(i, j);
					}
					else if (num == ModContent.TileType<CreamSeaOats>()) {
						ConfectionWorldGeneration.PlantSeaOat(i, j);
					}
				}
			}
			return orig.Invoke(i, j, Type, mute, forced, plr, style);
		}

		#region CactusMapColor
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
					GetCactusType(i, j, tile.TileFrameX, tile.TileFrameY, out var sandType);
					if (Main.tile[i, j].TileType == TileID.Cactus && TileLoader.CanGrowModCactus(sandType) && sandType != 0 && sandType == ModContent.TileType<Creamsand>()) {
						num5 = MapHelper.tileLookup[ModContent.TileType<SprinkleCactusDudTile>()];
					}
				}
			});
		}

		public static void GetCactusType(int tileX, int tileY, int frameX, int frameY, out int type) {
			type = 0;
			int num = tileX;
			if (frameX == 36) {
				num--;
			}
			if (frameX == 54) {
				num++;
			}
			if (frameX == 108) {
				num = ((frameY != 18) ? (num + 1) : (num - 1));
			}
			int num2 = tileY;
			bool flag = false;
			Tile tile = Main.tile[num, num2];
			if (tile == null) {
				return;
			}
			if (tile.TileType == 80 && tile.HasTile) {
				flag = true;
			}
			while (tile != null && (!tile.HasTile || !Main.tileSolid[tile.TileType] || !flag)) {
				if (tile.TileType == 80 && tile.HasTile) {
					flag = true;
				}
				num2++;
				if (num2 > tileY + 20) {
					break;
				}
				if (num2 <= Main.maxTilesY)
					tile = Main.tile[num, num2];
			}
			type = tile.TileType;
		}
		#endregion

		private void VineTileFrame(ILContext il) {
			var cursor = new ILCursor(il); //Code thats USED here (none commented out) is made by Ghasttear1

			// Add vine condition for conversion
			cursor.GotoNext(MoveType.Before, i => i.MatchStloc(121));
			cursor.Emit(OpCodes.Ldloc, 84); // up
			cursor.EmitDelegate((ushort origValue, int up) =>
			{
				if (up == ModContent.TileType<CreamVines>() || up == ModContent.TileType<CreamGrass>()) {
					return (ushort)ModContent.TileType<CreamVines>();
				}
				return origValue;
			});

			// Add vine condition for kill
			cursor.GotoNext(MoveType.Before, i => i.MatchStloc(122));
			cursor.Emit(OpCodes.Ldloc, 3); // num
			cursor.Emit(OpCodes.Ldloc, 84); // up
			cursor.EmitDelegate((bool origValue, int num, int up) =>
			{
				if (num == ModContent.TileType<CreamVines>() && up != ModContent.TileType<CreamGrass>()) {
					return true;
				}
				return origValue;
			});
			/*ILCursor c = new(il);
			ILLabel IL_0000 = c.DefineLabel();
			ILLabel IL_80ab = null;
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdcI4(52),
				i => i.MatchStloc(121),
				i => i.MatchLdloc(120),
				i => i.MatchBrfalse(out IL_80ab));
			if (IL_80ab == null) {
				ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("Vine Tile framing conversion could not be found");
				return;
			}
			c.Prev.Operand = IL_0000;
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdcI4(382),
				i => i.MatchStloc(121));
			c.MarkLabel(IL_0000);
			c.EmitLdloc(84); //up
			c.EmitLdloca(121); //ref num37
			c.EmitDelegate((int up, ref ushort num37) => {
				if (up == ModContent.TileType<CreamGrass>() || up == ModContent.TileType<CreamVines>()) {
					num37 = (ushort)ModContent.TileType<CreamVines>();
				}
			});
			MonoModHooks.DumpIL(this, il);*/
			/*c.GotoNext(
				MoveType.After,
				i => i.MatchLdcI4(0),
				i => i.MatchStloc(121));
			c.EmitLdloc(84); //up
			c.EmitLdloca(121); //ref num37
			c.EmitDelegate((int up, ref ushort num37) => {
				if (up == ModContent.TileType<CreamGrass>() || up == ModContent.TileType<CreamVines>()) {
					num37 = (ushort)ModContent.TileType<CreamVines>();
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
			//MonoModHooks.DumpIL(this, il);*/
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
			if (!tile.HasTile && tile.LiquidAmount == 0 && Main.tile[X, Y + 1] != null && WorldGen.SolidTile(X, Y + 1)) {
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
				if (tile5.TileType == ModContent.TileType<CreamGrass>() || tile5.TileType == ModContent.TileType<CreamGrassMowed>()) { //Turns Creamgrass or golf creamgrass into Cookie block when lava is near
					tile5.TileType = (ushort)ModContent.TileType<CookieBlock>();
					WorldGen.SquareTileFrame(i, j);
					if (Main.netMode == NetmodeID.Server) {
						NetMessage.SendTileSquare(-1, num, num2, 3);
					}
				}
			});
		}

		private bool PickaxeKillTile(On_Player.orig_DoesPickTargetTransformOnKill orig, Player self, HitTile hitCounter, int damage, int x, int y, int pickPower, int bufferIndex, Tile tileTarget) {
			if (hitCounter.AddDamage(bufferIndex, damage, updateAmount: false) >= 100 && (tileTarget.TileType == ModContent.TileType<CreamGrass>() || tileTarget.TileType == ModContent.TileType<CreamGrassMowed>())) {
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
					if (!tile.HasTile || self.inventory[self.selectedItem].createTile == tile.TileType || (tile.TileType != ModContent.TileType<CreamGrass>() && tile.TileType != ModContent.TileType<CreamGrassMowed>())) {
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
