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
			IL_WorldGen.CheckCatTail += CheckCattailEdit;
			IL_WorldGen.PlaceCatTail += PlaceCattailEdit;
			On_WorldGen.GrowCatTail += GrowCattailEdit;
			//IL_TileDrawing.DrawMultiTileGrassInWind += MultiTileWindEdit;
			//IL_TileDrawing.DrawMultiTileGrass += IL_TileDrawing_DrawMultiTileGrass;
			//IL_TileDrawing.DrawMultiTileGrassInWind += IL_TileDrawing_DrawMultiTileGrassInWind;
		}

		private void IL_TileDrawing_DrawMultiTileGrassInWind(ILContext il) {
			ILCursor c = new(il);
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdloca(10),
				i => i.MatchCall<Tile>("get_type"),
				i => i.MatchLdindU2(),
				i => i.MatchStloc(11));
			c.EmitLdloc(3); //type
			c.EmitLdloca(6); //flag
			c.EmitLdloca(2); //num
			c.EmitLdarg(3); //topLeftX
			c.EmitLdarg(4); //topLeftY
			c.EmitLdarg(5); //sizeX
			c.EmitDelegate((int type, ref bool flag, ref float num, int topLeftX, int topLeftY, int sizeX) => {
				if (type == ModContent.TileType<CreamCattails>()) {
					flag = WorldGen.InAPlaceWithWind(topLeftX, topLeftY, sizeX, 1);
					num = 0.07f;
				}
			});
		}

		private void IL_TileDrawing_DrawMultiTileGrass(ILContext il) {
			ILCursor c = new(il);
			c.GotoNext(
				MoveType.After,
				i => i.MatchLdsflda<Main>("tile"),
				i => i.MatchLdloc(5),
				i => i.MatchLdloc(6),
				i => i.MatchCall<Tilemap>("get_Item"),
				i => i.MatchStloc(9)
				);
			c.EmitLdloc(9); //tile
			c.EmitLdloc(5); //x
			c.EmitLdloca(6); //num3
			c.EmitLdloca(7); //sizeX
			c.EmitLdloca(8); //num4
			c.EmitDelegate((Tile tile, int x, ref int num3, ref int sizeX, ref int num4) => {
				if (tile != null && tile.HasTile) { 
					if (Main.tile[x, num3].TileType == ModContent.TileType<CreamCattails>()) {
						sizeX = 1;
						num4 = ClimbCreamCatTail(x, num3);
						num3 -= num4 - 1;
					}
				}
			});
		}
		#region ProofOfConcept?
		/*
		private void On_TileDrawing_DrawMultiTileGrassInWind(On_TileDrawing.orig_DrawMultiTileGrassInWind orig, TileDrawing self, Vector2 screenPosition, Vector2 offSet, int topLeftX, int topLeftY, int sizeX, int sizeY) {
			float windCycle = GetWindCycle(topLeftX, topLeftY, _sunflowerWindCounter);
			new Vector2((float)(sizeX * 16) * 0.5f, (float)(sizeY * 16));
			Vector2 vector = new Vector2((float)(topLeftX * 16 - (int)screenPosition.X) + (float)sizeX * 16f * 0.5f, (float)(topLeftY * 16 - (int)screenPosition.Y + 16 * sizeY)) + offSet;
			float num = 0.07f;
			int type = Main.tile[topLeftX, topLeftY].type;
			Texture2D texture2D = null;
			Color color = Color.Transparent;
			bool flag = InAPlaceWithWind(topLeftX, topLeftY, sizeX, sizeY);
			switch (type) {
				case 27:
					texture2D = TextureAssets.Flames[14].Value;
					color = Color.White;
					break;
				case 519:
					flag = InAPlaceWithWind(topLeftX, topLeftY, sizeX, 1);
					break;
				default:
					num = 0.15f;
					break;
				case 521:
				case 522:
				case 523:
				case 524:
				case 525:
				case 526:
				case 527:
					num = 0f;
					flag = false;
					break;
			}
			Vector2 vector3 = default(Vector2);
			for (int i = topLeftX; i < topLeftX + sizeX; i++) {
				for (int j = topLeftY; j < topLeftY + sizeY; j++) {
					Tile tile = Main.tile[i, j];
					ushort type2 = tile.type;
					if (type == ModContent.TileType<CreamCattails>()) {
						flag = InAPlaceWithWind(topLeftX, topLeftY, sizeX, 1);
						num = 0.07f;
					}
					if (type2 != type || !IsVisible(tile)) {
						continue;
					}
					Math.Abs(((float)(i - topLeftX) + 0.5f) / (float)sizeX - 0.5f);
					short tileFrameX = tile.frameX;
					short tileFrameY = tile.frameY;
					float num2 = 1f - (float)(j - topLeftY + 1) / (float)sizeY;
					if (num2 == 0f) {
						num2 = 0.1f;
					}
					if (!flag) {
						num2 = 0f;
					}
					GetTileDrawData(i, j, tile, type2, ref tileFrameX, ref tileFrameY, out var tileWidth, out var tileHeight, out var tileTop, out var halfBrickHeight, out var addFrX, out var addFrY, out var tileSpriteEffect, out var _, out var _, out var _);
					bool flag2 = _rand.Next(4) == 0;
					Color tileLight = Lighting.GetColor(i, j);
					DrawAnimatedTile_AdjustForVisionChangers(i, j, tile, type2, tileFrameX, tileFrameY, ref tileLight, flag2);
					tileLight = DrawTiles_GetLightOverride(j, i, tile, type2, tileFrameX, tileFrameY, tileLight);
					if (_isActiveAndNotPaused && flag2) {
						DrawTiles_EmitParticles(j, i, tile, type2, tileFrameX, tileFrameY, tileLight);
					}
					Vector2 vector2 = new Vector2((float)(i * 16 - (int)screenPosition.X), (float)(j * 16 - (int)screenPosition.Y + tileTop)) + offSet;
					if (tile.type == 493 && tile.frameY == 0) {
						if (Main.WindForVisuals >= 0f) {
							tileSpriteEffect = (SpriteEffects)(tileSpriteEffect ^ 1);
						}
						if (!((Enum)tileSpriteEffect).HasFlag((Enum)(object)(SpriteEffects)1)) {
							vector2.X -= 6f;
						}
						else {
							vector2.X += 6f;
						}
					}
					((Vector2)(ref vector3))..ctor(windCycle * 1f, Math.Abs(windCycle) * 2f * num2);
					Vector2 origin = vector - vector2;
					Texture2D tileDrawTexture = GetTileDrawTexture(tile, i, j);
					if (tileDrawTexture != null) {
						Main.spriteBatch.Draw(tileDrawTexture, vector + new Vector2(0f, vector3.Y), (Rectangle?)new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), tileLight, windCycle * num * num2, origin, 1f, tileSpriteEffect, 0f);
						if (texture2D != null) {
							Main.spriteBatch.Draw(texture2D, vector + new Vector2(0f, vector3.Y), (Rectangle?)new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), color, windCycle * num * num2, origin, 1f, tileSpriteEffect, 0f);
						}
					}
				}
			}
		}

		private void On_TileDrawing_DrawMultiTileGrass(On_TileDrawing.orig_DrawMultiTileGrass orig, TileDrawing self) {
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 zero = Vector2.Zero;
			int num = 4;
			int num2 = _specialsCount[num];
			for (int i = 0; i < num2; i++) {
				Point val = _specialPositions[num][i];
				int x = val.X;
				int num3 = val.Y;
				int sizeX = 1;
				int num4 = 1;
				Tile tile = Main.tile[x, num3];
				if (tile != null && tile.HasTile) { //
					if (Main.tile[x, num3].TileType == ModContent.TileType<CreamCattails>()) {
						sizeX = 1;
						num4 = ClimbCreamCatTail(x, num3);
						num3 -= num4 - 1;
					}
				} //
				if (tile != null && tile.active()) {
					switch (Main.tile[x, num3].type) {
						case 27:
							sizeX = 2;
							num4 = 5;
							break;
						case 236:
						case 238:
							sizeX = (num4 = 2);
							break;
						case 233:
							sizeX = ((Main.tile[x, num3].frameY != 0) ? 2 : 3);
							num4 = 2;
							break;
						case 530:
						case 651:
							sizeX = 3;
							num4 = 2;
							break;
						case 485:
						case 490:
						case 521:
						case 522:
						case 523:
						case 524:
						case 525:
						case 526:
						case 527:
						case 652:
							sizeX = 2;
							num4 = 2;
							break;
						case 489:
							sizeX = 2;
							num4 = 3;
							break;
						case 493:
							sizeX = 1;
							num4 = 2;
							break;
						case 519:
							sizeX = 1;
							num4 = ClimbCatTail(x, num3);
							num3 -= num4 - 1;
							break;
					}
					DrawMultiTileGrassInWind(unscaledPosition, zero, x, num3, sizeX, num4);
				}
			}
		}
		*/
		#endregion

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
			//IL_TileDrawing.DrawMultiTileGrassInWind -= MultiTileWindEdit;
		}

		private void MultiTileWindEdit(MonoMod.Cil.ILContext il) {
			ILCursor c = new(il);
			c.EmitLdarg0(); //self
			c.EmitLdarga(3); //topLeftX
			c.EmitLdarga(4); //topLeftY
			c.EmitLdarga(1); //screenPosition
			c.EmitLdarga(2); //offset
			c.EmitLdarga(5); //sizeX
			c.EmitLdarga(6); //size Y
			c.EmitDelegate((TileDrawing self, ref int topLeftX, ref int topLeftY, ref Vector2 screenPosition, ref Vector2 offSet, ref int sizeX, ref int sizeY) =>
			{
				Texture2D dud = null;
				Color dud2 = Color.White;
				bool dud3 = false;
				float dud4 = 0;
				SetMultiGrassDrawingSettings(self, ref screenPosition, ref offSet, ref topLeftX, ref topLeftY, ref sizeX, ref sizeY, ref dud3, ref dud4, ref dud, ref dud2); //Adds the sizing of the tile
			});

			c.GotoNext(
				MoveType.After,
				i => i.MatchLdloca(10),
				i => i.MatchCall<Tile>("get_type"),
				i => i.MatchLdindU2(),
				i => i.MatchStloc(11));
			c.EmitLdarg0(); //self
			c.EmitLdarga(3); //topLeftX
			c.EmitLdarga(4); //topLeftY
			c.EmitLdloca(6); //flag
			c.EmitLdloca(2); //num
			c.EmitDelegate((TileDrawing self, ref int topLeftX, ref int topLeftY, ref bool flag, ref float num) => 
			{
				int dud = 0;
				Texture2D dud2 = null;
				Color dud3 = Color.White;
				Vector2 dud4 = Vector2.Zero;
				SetMultiGrassDrawingSettings(self, ref dud4, ref dud4, ref topLeftX, ref topLeftY, ref dud, ref dud, ref flag, ref num, ref dud2, ref dud3); //Adds extra content (glowmasks, color, ect)
			});

			c.GotoNext(
				MoveType.After,
				i => i.MatchLdarg0(),
				i => i.MatchLdloc(10),
				i => i.MatchLdloc(8),
				i => i.MatchLdloc(9),
				i => i.MatchCall<TileDrawing>("GetTileDrawTexture"),
				i => i.MatchStloc(27)
				); //Goes to before the Main.Spritebatch Drawing and injects, when I mean right before, I mean the next instruction is calling Main.spritebatch, ya know
			c.EmitLdarg0(); //self
			c.EmitLdarga(3); //topLeftX
			c.EmitLdarga(4); //topLeftY
			c.EmitLdloca(4); //val2
			c.EmitLdloca(5); //color
			c.EmitDelegate((TileDrawing self, ref int topLeftX, ref int topLeftY, ref Texture2D val2, ref Color color) => //Should note that the val2 is the extra texture thats only used by sunflower's """"flames""""
			{
				int dud = 0;
				Vector2 dud2 = Vector2.Zero;
				bool dud3 = false;
				float dud4 = 0.0f;
				SetMultiGrassDrawingSettings(self, ref dud2, ref dud2, ref topLeftX, ref topLeftY, ref dud, ref dud, ref dud3, ref dud4, ref val2, ref color); //Adds extra content (glowmasks, color, ect)
			});
		}

		public static void SetMultiGrassDrawingSettings(TileDrawing self, ref Vector2 screenPosition, ref Vector2 offSet, ref int topLeftX, ref int topLeftY, ref int sizeX, ref int sizeY, ref bool Wind, ref float WindAmount, ref Texture2D overlayTexture, ref Color overlayColor) {
			if (Main.tile[sizeX, sizeY].TileType == ModContent.TileType<CreamCattails>()) {
				sizeX = 1;
				sizeY = ClimbCreamCatTail(topLeftX, topLeftY);
				topLeftY -= sizeY - 1;
				Wind = WorldGen.InAPlaceWithWind(topLeftX, topLeftY, sizeX, 1);
				WindAmount = 0.07f;
			}
		}

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
				if (type == ModContent.TileType<CreamGrass>()) {
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
			c.EmitDelegate((int type, ref int num5) => {
				if (type == ModContent.TileType<CreamGrass>()) { 
					num5 = ModContent.TileType<CreamCattails>(); 
				}
			});
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
			c.EmitDelegate((int x, int num2, bool flag, ref int num, ref int num5) => {
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
			c.EmitDelegate((int x, int k) => {
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
