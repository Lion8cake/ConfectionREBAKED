using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TheConfectionRebirth.Tiles;
using static System.Net.WebRequestMethods;
using static Terraria.GameContent.Animations.IL_Actions.Sprites;

namespace TheConfectionRebirth {
	public class ConfectionWorldGeneration : ModSystem {

		public static int confectionTree;
		public override void OnWorldLoad() {
			confectionTree = 0;
		}

		public override void OnWorldUnload() {
			confectionTree = 0;
		}

		public override void SaveWorldData(TagCompound tag) {
			tag["TheConfectionRebirth:confectionTree"] = confectionTree;
		}

		public override void LoadWorldData(TagCompound tag) {
			confectionTree = tag.GetInt("TheConfectionRebirth:confectionTree");
		}

		public override void NetSend(BinaryWriter writer) {
			writer.Write(confectionTree);
		}

		public override void NetReceive(BinaryReader reader) {
			confectionTree = reader.ReadInt32();
		}

		public override void PreWorldGen() {
			confectionTree = Main.rand.Next(3);
		}

		public static void ConfectionConvert(int i, int j, int size = 4) {
			for (int k = i - size; k <= i + size; k++) {
				for (int l = j - size; l <= j + size; l++) {
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size)) {
						int type = Main.tile[k, l].TileType;
						int wall = Main.tile[k, l].WallType;

						#region Walls
						/*if (WallID.Sets.Conversion.Stone[wall] && wall != ModContent.WallType<CreamstoneWall>() && wall != WallID.RocksUnsafe1 && wall != WallID.RocksUnsafe2 && wall != WallID.RocksUnsafe3 && wall != WallID.RocksUnsafe4 && wall != WallID.CorruptionUnsafe1 && wall != WallID.CorruptionUnsafe2 && wall != WallID.CorruptionUnsafe3 && wall != WallID.CorruptionUnsafe4 && wall != WallID.CrimsonUnsafe1 && wall != WallID.CrimsonUnsafe2 && wall != WallID.CrimsonUnsafe3 && wall != WallID.CrimsonUnsafe4 && wall != WallID.HallowUnsafe1 && wall != WallID.HallowUnsafe2 && wall != WallID.HallowUnsafe3 && wall != WallID.HallowUnsafe4) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<CreamstoneWall>();
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (WallID.Sets.Conversion.Snow[wall] && wall != ModContent.WallType<CreamWall>()) 
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<CreamWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (WallID.Sets.Conversion.Dirt[wall] && wall != ModContent.WallType<CookieWall>()) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<CookieWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (WallID.Sets.Conversion.HardenedSand[wall] && wall != ModContent.WallType<CreamsandstoneWall>()) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<CreamsandstoneWall>();
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (WallID.Sets.Conversion.Sandstone[wall] && wall != ModContent.WallType<HardenedCreamsandWall>()) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<HardenedCreamsandWall>();
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (WallID.Sets.Conversion.Grass[wall] && wall != ModContent.WallType<CreamGrassWall>()) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<CreamGrassWall>();
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (WallID.Sets.Conversion.Ice[wall] && wall != ModContent.WallType<BlueIceWall>()) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<BlueIceWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}
						else if (wall == WallID.Cloud && wall != ModContent.WallType<PinkFairyFlossWall>())
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<PinkFairyFlossWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}
						else if ((wall == 59 || wall == 261) && wall != ModContent.WallType<CookieStonedWall>()) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<CookieStonedWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}
						else if ((wall == WallID.RocksUnsafe1 || wall == WallID.CorruptionUnsafe1 || wall == WallID.CrimsonUnsafe1 || wall == WallID.HallowUnsafe1) && wall != ModContent.WallType<Creamstone2Wall>()) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<Creamstone2Wall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}
						else if ((wall == WallID.RocksUnsafe2 || wall == WallID.CorruptionUnsafe2 || wall == WallID.CrimsonUnsafe2 || wall == WallID.HallowUnsafe2) && wall != ModContent.WallType<Creamstone3Wall>()) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<Creamstone3Wall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}
						else if ((wall == WallID.RocksUnsafe3 || wall == WallID.CorruptionUnsafe3 || wall == WallID.CrimsonUnsafe3 || wall == WallID.HallowUnsafe3) && wall != ModContent.WallType<Creamstone4Wall>()) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<Creamstone4Wall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}
						else if ((wall == WallID.RocksUnsafe4 || wall == WallID.CorruptionUnsafe4 || wall == WallID.CrimsonUnsafe4 || wall == WallID.HallowUnsafe4) && wall != ModContent.WallType<Creamstone5Wall>()) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<Creamstone5Wall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}*/
						#endregion

						#region TileIDConversions
						if (TileID.Sets.Conversion.Stone[type] && type != ModContent.TileType<Creamstone>()) {
							WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(k, l, ModContent.TileType<Creamstone>());
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<Creamstone>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (TileID.Sets.Conversion.Sand[type] && type != ModContent.TileType<Creamsand>()) {
							WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(k, l, ModContent.TileType<Creamsand>());
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<Creamsand>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (TileID.Sets.Conversion.Grass[type] && type != ModContent.TileType<CreamGrass>() && type != ModContent.TileType<CreamGrassMowed>()) {
							WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(k, l, ModContent.TileType<CreamGrass>());
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamGrass>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (TileID.Sets.Conversion.Ice[type] && type != ModContent.TileType<BlueIce>()) {
							WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(k, l, ModContent.TileType<BlueIce>());
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<BlueIce>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (TileID.Sets.Conversion.Sandstone[type] && type != ModContent.TileType<Creamsandstone>()) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<Creamsandstone>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (TileID.Sets.Conversion.HardenedSand[type] && type != ModContent.TileType<HardenedCreamsand>()) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<HardenedCreamsand>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (TileID.Sets.Conversion.Dirt[type] && type != ModContent.TileType<CookieBlock>()) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CookieBlock>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (TileID.Sets.Conversion.Snow[type] && type != ModContent.TileType<CreamBlock>()) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamBlock>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (TileID.Sets.Conversion.GolfGrass[type] && type != ModContent.TileType<CreamGrassMowed>()) {
							WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(k, l, ModContent.TileType<CreamGrassMowed>());
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamGrassMowed>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (TileID.Sets.Conversion.Thorn[type]) {
							WorldGen.KillTile(k, l);
							if (Main.netMode == NetmodeID.MultiplayerClient) {
								NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, k, l);
							}
						}
						#endregion

						#region ManualTileConverting
						/*else if (Main.tile[k, l].TileType == TileID.Ruby) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneRuby>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.Sapphire) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneSaphire>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.Diamond) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneDiamond>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.Emerald) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneEmerald>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.Amethyst) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneAmethyst>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.Topaz) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneTopaz>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else */
						if (Main.tile[k, l].TileType == TileID.Cloud) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<PinkFairyFloss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.RainCloud) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<PurpleFairyFloss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.SnowCloud) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<BlueFairyFloss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						/*else if (Main.tile[k, l].TileType == TileID.ArgonMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<ArgonCreamMoss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.BlueMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<BlueCreamMoss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.BrownMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<BrownCreamMoss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.GreenMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<GreenCreamMoss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.KryptonMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<KryptonCreamMoss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.LavaMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<LavaCreamMoss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.PurpleMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<PurpleCreamMoss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.RedMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<RedCreamMoss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.XenonMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<XenomCreamMoss>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}*/

						if (type == TileID.Mud && (Main.tile[k - 1, l].TileType == ModContent.TileType<CreamGrass>() || Main.tile[k + 1, l].TileType == ModContent.TileType<CreamGrass>() || Main.tile[k, l - 1].TileType == ModContent.TileType<CreamGrass>() || Main.tile[k, l + 1].TileType == ModContent.TileType<CreamGrass>())) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CookieBlock>();
							WorldGen.SquareTileFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l);
						}
						#endregion
					}
				}
			}
		}

		//Stalac Checks, unfinished, cattail checks, finished, sea oats check, finished, oasis plants finished
		//TODO: convert mormal stalacs to these
		public static bool GrowMoreVines(int x, int y) {
			return (bool)typeof(WorldGen).GetMethod("GrowMoreVines", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(null, new object[] { x, y });
		}

		public static bool OasisPlantWaterCheck(int x, int y, bool boost = false) {
			return (bool)typeof(WorldGen).GetMethod("OasisPlantWaterCheck", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(null, new object[] { x, y, boost });
		}

		public static bool GrowSeaOat(int x, int y) {
			return (bool)typeof(WorldGen).GetMethod("GrowSeaOat", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(null, new object[] { x, y });
		}

		public static bool CheckSeaOat(int x, int y) {
			return (bool)typeof(WorldGen).GetMethod("CheckSeaOat", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(null, new object[] { x, y });
		}

		public static bool SeaOatWaterCheck(int x, int y) {
			return (bool)typeof(WorldGen).GetMethod("SeaOatWaterCheck", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(null, new object[] { x, y });
		}

		public static bool PlantSeaOat(int x, int y) {
			return (bool)typeof(WorldGen).GetMethod("PlantSeaOat", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(null, new object[] { x, y });
		}

		public static void GrowCreamCatTail(int x, int j) {
			if (Main.netMode == 1) {
				return;
			}
			int num = j;
			Tile tile;
			while (true) {
				tile = Main.tile[x, num];
				if (tile.LiquidAmount <= 0 || num <= 50) {
					break;
				}
				num--;
			}
			num++;
			int i = num;
			while (true) {
				tile = Main.tile[x, i];
				if (tile.HasTile) {
					bool[] tileSolid = Main.tileSolid;
					tile = Main.tile[x, i];
					if (tileSolid[tile.TileType]) {
						bool[] tileSolidTop = Main.tileSolidTop;
						tile = Main.tile[x, i];
						if (!tileSolidTop[tile.TileType]) {
							break;
						}
					}
				}
				if (i >= Main.maxTilesY - 50) {
					break;
				}
				i++;
			}
			num = i - 1;
			while (true) {
				tile = Main.tile[x, num];
				if (!tile.HasTile) {
					break;
				}
				tile = Main.tile[x, num];
				if (tile.TileType != ModContent.TileType<CreamCattails>()) {
					break;
				}
				num--;
			}
			num++;
			tile = Main.tile[x, num];
			if (tile.TileFrameX == 90) {
				tile = Main.tile[x, num - 1];
				if (tile.HasTile) {
					bool[] tileCut = Main.tileCut;
					tile = Main.tile[x, num - 1];
					if (tileCut[tile.TileType]) {
						WorldGen.KillTile(x, num - 1);
						if (Main.netMode == 2) {
							NetMessage.SendData(17, -1, -1, null, 0, x, num - 1);
						}
					}
				}
			}
			tile = Main.tile[x, num - 1];
			if (tile.HasTile) {
				return;
			}
			tile = Main.tile[x, num];
			if (tile.TileFrameX == 0) {
				tile = Main.tile[x, num];
				tile.TileFrameX = 18;
				WorldGen.SquareTileFrame(x, num);
				if (Main.netMode == 2) {
					NetMessage.SendTileSquare(-1, x, num);
				}
			}
			else {
				tile = Main.tile[x, num];
				if (tile.TileFrameX == 18) {
					tile = Main.tile[x, num];
					tile.TileFrameX = (short)(18 * WorldGen.genRand.Next(2, 5));
					tile = Main.tile[x, num - 1];
					tile.HasTile = true;
					tile = Main.tile[x, num - 1];
					tile.TileType = (ushort)ModContent.TileType<CreamCattails>();
					tile = Main.tile[x, num - 1];
					tile.TileFrameX = 90;
					tile = Main.tile[x, num - 1];
					ref short frameY = ref tile.TileFrameY;
					tile = Main.tile[x, num];
					frameY = tile.TileFrameY;
					tile = Main.tile[x, num - 1];
					tile.IsHalfBlock = false;
					tile = Main.tile[x, num - 1];
					tile.Slope = 0;
					tile = Main.tile[x, num - 1];
					tile.CopyPaintAndCoating(Main.tile[x, num]);
					WorldGen.SquareTileFrame(x, num);
					if (Main.netMode == 2) {
						NetMessage.SendTileSquare(-1, x, num);
					}
				}
				else {
					tile = Main.tile[x, num];
					if (tile.TileFrameX == 90) {
						tile = Main.tile[x, num - 1];
						if (tile.LiquidAmount == 0) {
							tile = Main.tile[x, num - 2];
							if (!tile.HasTile) {
								tile = Main.tile[x, num];
								if (tile.LiquidAmount <= 0) {
									tile = Main.tile[x, num + 1];
									if (tile.LiquidAmount <= 0) {
										tile = Main.tile[x, num + 2];
										if (tile.LiquidAmount <= 0) {
											goto IL_0482;
										}
									}
								}
								if (WorldGen.genRand.Next(3) == 0) {
									tile = Main.tile[x, num];
									tile.TileFrameX = 108;
									tile = Main.tile[x, num - 1];
									tile.HasTile = true;
									tile = Main.tile[x, num - 1];
									tile.TileType = (ushort)ModContent.TileType<CreamCattails>();
									tile = Main.tile[x, num - 1];
									tile.TileFrameX = 90;
									tile = Main.tile[x, num - 1];
									ref short frameY2 = ref tile.TileFrameY;
									tile = Main.tile[x, num];
									frameY2 = tile.TileFrameY;
									tile = Main.tile[x, num - 1];
									tile.IsHalfBlock = false;
									tile = Main.tile[x, num - 1];
									tile.Slope = 0;
									tile = Main.tile[x, num - 1];
									tile.CopyPaintAndCoating(Main.tile[x, num]);
									WorldGen.SquareTileFrame(x, num);
									goto IL_0670;
								}
							}
							goto IL_0482;
						}
						tile = Main.tile[x, num];
						tile.TileFrameX = 108;
						tile = Main.tile[x, num - 1];
						tile.HasTile = true;
						tile = Main.tile[x, num - 1];
						tile.TileType = (ushort)ModContent.TileType<CreamCattails>();
						tile = Main.tile[x, num - 1];
						tile.TileFrameX = 90;
						tile = Main.tile[x, num - 1];
						ref short frameY3 = ref tile.TileFrameY;
						tile = Main.tile[x, num];
						frameY3 = tile.TileFrameY;
						tile = Main.tile[x, num - 1];
						tile.IsHalfBlock = false;
						tile = Main.tile[x, num - 1];
						tile.Slope = 0;
						tile = Main.tile[x, num - 1];
						tile.CopyPaintAndCoating(Main.tile[x, num]);
						WorldGen.SquareTileFrame(x, num);
					}
				}
			}
			goto IL_0670;
		IL_0670:
			WorldGen.SquareTileFrame(x, num - 1, resetFrame: false);
			if (Main.netMode == 2) {
				NetMessage.SendTileSquare(-1, x, num - 1, 1, 2);
			}
			return;
		IL_0482:
			int num2 = WorldGen.genRand.Next(3);
			tile = Main.tile[x, num];
			tile.TileFrameX = (short)(126 + num2 * 18);
			tile = Main.tile[x, num - 1];
			tile.HasTile = true;
			tile = Main.tile[x, num - 1];
			tile.TileType = (ushort)ModContent.TileType<CreamCattails>();
			tile = Main.tile[x, num - 1];
			tile.TileFrameX = (short)(180 + num2 * 18);
			tile = Main.tile[x, num - 1];
			ref short frameY4 = ref tile.TileFrameY;
			tile = Main.tile[x, num];
			frameY4 = tile.TileFrameY;
			tile = Main.tile[x, num - 1];
			tile.IsHalfBlock = false;
			tile = Main.tile[x, num - 1];
			tile.Slope = 0;
			tile = Main.tile[x, num - 1];
			tile.CopyPaintAndCoating(Main.tile[x, num]);
			WorldGen.SquareTileFrame(x, num);
			goto IL_0670;
		}

		public static void CheckCreamCatTail(int x, int j) {
			if (Main.tile[x, j] == null) {
				return;
			}
			int num = j;
			bool flag = false;
			int num2 = num;
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
			if (Main.tile[x, num] == null) {
				return;
			}
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
			if (Main.tile[x, num] == null) {
				return;
			}
			int num3 = num;
			int num4 = 8;//catTailDistance;
			if (num2 - num3 > num4) {
				flag = true;
			}
			int type = Main.tile[x, num2].TileType;
			int num5 = -1;
			if (type == ModContent.TileType<CreamGrass>() || type == ModContent.TileType<CreamGrassMowed>() || type == ModContent.TileType<Creamsand>()) {
				num5 = ModContent.TileType<CreamCattails>();
			}
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
			if (Main.tile[x, num2 - 1] != null && Main.tile[x, num2 - 1].LiquidAmount < 127 && WorldGen.genRand.Next(4) == 0) {
				flag = true;
			}
			if (Main.tile[x, num] != null && Main.tile[x, num].TileFrameX >= 180 && Main.tile[x, num].LiquidAmount > 127 && WorldGen.genRand.Next(4) == 0) {
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
			if (flag) {
				int num7 = num3;
				if (num < num3) {
					num7 = num;
				}
				num7 -= 4;
				for (int i = num7; i <= num2; i++) {
					if (Main.tile[x, i] != null && Main.tile[x, i].HasTile && (Main.tile[x, i].TileType == 519 || Main.tile[x, i].TileType == ModContent.TileType<CreamCattails>())) {
						WorldGen.KillTile(x, i);
						if (Main.netMode == 2) {
							NetMessage.SendData(17, -1, -1, null, 0, x, i);
						}
						WorldGen.SquareTileFrame(x, i);
					}
				}
			}
			else {
				for (int k = num; k < num2; k++) {
					if (Main.tile[x, k] != null && Main.tile[x, k].HasTile && (Main.tile[x, k].TileType == 519 || Main.tile[x, k].TileType == ModContent.TileType<CreamCattails>())) {
						if (num5 == ModContent.TileType<CreamCattails>()) {
							Main.tile[x, k].TileType = (ushort)ModContent.TileType<CreamCattails>();
							Main.tile[x, k].TileFrameY = 0;
						}
						else {
							Main.tile[x, k].TileType = 519;
							Main.tile[x, k].TileFrameY = (short)num5;
						}
						if (Main.netMode == 2) {
							NetMessage.SendTileSquare(-1, x, num);
						}
					}
				}
			}
		}

		public static void PlaceTight(int x, int y) {
			if (Main.tile[x, y].LiquidType != LiquidID.Shimmer) {
				PlaceUncheckedStalactite(x, y, WorldGen.genRand.NextBool(2), WorldGen.genRand.Next(3));
				if (Main.tile[x, y].TileType == ModContent.TileType<BlueIceStalactite>() || Main.tile[x, y].TileType == ModContent.TileType<CreamstoneStalactite>()) {
					CheckTight(x, y);
				}
			}
		}

		public static void PlaceUncheckedStalactite(int x, int y, bool preferSmall, int variation) {
			ushort type;
			variation = Utils.Clamp(variation, 0, 2);
			if (WorldGen.SolidTile(x, y - 1) && !Main.tile[x, y].HasTile && !Main.tile[x, y + 1].HasTile) {
				if (Main.tile[x, y - 1].TileType == ModContent.TileType<BlueIce>()) {
					type = (ushort)ModContent.TileType<BlueIceStalactite>();
					if (preferSmall) {
						int num12 = variation * 18;
						Tile tile = Main.tile[x, y];
						tile.TileType = type;
						tile.HasTile = true;
						tile.TileFrameX = (short)num12;
						tile.TileFrameY = 72;
					}
					else {
						int num15 = variation * 18;
						Tile tile = Main.tile[x, y];
						tile.TileType = type;
						tile.HasTile = true;
						tile.TileFrameX = (short)num15;
						tile.TileFrameY = 0;
						Tile tile2 = Main.tile[x, y + 1];
						tile2.TileType = type;
						tile2.HasTile = true;
						tile2.TileFrameX = (short)num15;
						tile2.TileFrameY = 18;
					}
				}
				if (Main.tile[x, y - 1].TileType == ModContent.TileType<Creamstone>() || Main.tile[x, y - 1].TileType == ModContent.TileType<Creamsandstone>() || Main.tile[x, y - 1].TileType == ModContent.TileType<HardenedCreamsand>()) {
					type = (ushort)ModContent.TileType<CreamstoneStalactite>();
					if (preferSmall) {
						int num16 = 54 + variation * 18;
						Tile tile = Main.tile[x, y];
						tile.TileType = type;
						tile.HasTile = true;
						tile.TileFrameX = (short)num16;
						tile.TileFrameY = 72;
					}
					else {
						int num17 = 54 + variation * 18;
						Tile tile = Main.tile[x, y];
						tile.TileType = type;
						tile.HasTile = true;
						tile.TileFrameX = (short)num17;
						tile.TileFrameY = 0;
						Tile tile2 = Main.tile[x, y + 1];
						tile2.TileType = type;
						tile2.HasTile = true;
						tile2.TileFrameX = (short)num17;
						tile2.TileFrameY = 18;
					}
				}
			}
			else {
				if (Main.tile[x, y + 1].TileType == ModContent.TileType<Creamstone>() || Main.tile[x, y + 1].TileType == ModContent.TileType<Creamsandstone>() || Main.tile[x, y + 1].TileType == ModContent.TileType<HardenedCreamsand>()) {
					type = (ushort)ModContent.TileType<CreamstoneStalactite>();
					if (preferSmall) {
						int num5 = 54 + variation * 18;
						Tile tile = Main.tile[x, y];
						tile.TileType = type;
						tile.HasTile = true;
						tile.TileFrameX = (short)num5;
						tile.TileFrameY = 90;
					}
					else {
						int num6 = 54 + variation * 18;
						Tile tile = Main.tile[x, y - 1];
						tile.TileType = type;
						tile.HasTile = true;
						tile.TileFrameX = (short)num6;
						tile.TileFrameY = 36;
						Tile tile2 = Main.tile[x, y];
						tile2.TileType = type;
						tile2.HasTile = true;
						tile2.TileFrameX = (short)num6;
						tile2.TileFrameY = 54;
					}
				}
			}
		}

		public static void CheckTight(int x, int j) {
			if (Main.tile[x, j] == null) {
				return;
			}
			int num = j;
			if (Main.tile[x, num].TileFrameY == 72) {
				bool flag = false;
				if (!WorldGen.SolidTile(x, num - 1)) {
					flag = true;
				}
				if (!flag && !UpdateStalagtiteStyle(x, num)) {
					flag = true;
				}
				if (flag) {
					WorldGen.destroyObject = true;
					if (Main.tile[x, num].TileType == Main.tile[x, j].TileType) {
						WorldGen.KillTile(x, num);
					}
					WorldGen.destroyObject = false;
				}
				return;
			}
			if (Main.tile[x, num].TileFrameY == 90) {
				bool flag2 = false;
				if (!WorldGen.SolidTile(x, num + 1)) {
					flag2 = true;
				}
				if (!flag2 && !UpdateStalagtiteStyle(x, num)) {
					flag2 = true;
				}
				if (flag2) {
					WorldGen.destroyObject = true;
					if (Main.tile[x, num].TileType == Main.tile[x, j].TileType) {
						WorldGen.KillTile(x, num);
					}
					WorldGen.destroyObject = false;
				}
				return;
			}
			if (Main.tile[x, num].TileFrameY >= 36) {
				bool flag3 = false;
				if (!WorldGen.SolidTile(x, num + 2)) {
					flag3 = true;
				}
				if (Main.tile[x, num + 1].TileType != Main.tile[x, num].TileType) {
					flag3 = true;
				}
				if (Main.tile[x, num + 1].TileFrameX != Main.tile[x, num].TileFrameX) {
					flag3 = true;
				}
				if (!flag3 && !UpdateStalagtiteStyle(x, num)) {
					flag3 = true;
				}
				if (flag3) {
					WorldGen.destroyObject = true;
					if (Main.tile[x, num].TileType == Main.tile[x, j].TileType) {
						WorldGen.KillTile(x, num);
					}
					if (Main.tile[x, num + 1].TileType == Main.tile[x, j].TileType) {
						WorldGen.KillTile(x, num + 1);
					}
					WorldGen.destroyObject = false;
				}
				return;
			}
			if (Main.tile[x, num].TileFrameY == 18) {
				num--;
			}
			bool flag4 = false;
			if (!WorldGen.SolidTile(x, num - 1)) {
				flag4 = true;
			}
			if (Main.tile[x, num + 1].TileType != Main.tile[x, num].TileType) {
				flag4 = true;
			}
			if (Main.tile[x, num + 1].TileFrameX != Main.tile[x, num].TileFrameX) {
				flag4 = true;
			}
			if (!flag4 && !UpdateStalagtiteStyle(x, num)) {
				flag4 = true;
			}
			if (flag4) {
				WorldGen.destroyObject = true;
				if (Main.tile[x, num].TileType == Main.tile[x, j].TileType) {
					WorldGen.KillTile(x, num);
				}
				if (Main.tile[x, num + 1].TileType == Main.tile[x, j].TileType) {
					WorldGen.KillTile(x, num + 1);
				}
				WorldGen.destroyObject = false;
			}
		}

		public static bool UpdateStalagtiteStyle(int x, int j) {
			if (Main.netMode == 1) {
				return true;
			}
			if (Main.tile[x, j] == null) {
				return true;
			}
			GetStalagtiteStyle(x, j, out var type, out var fail);
			if (fail) {
				return false;
			}
			GetDesiredStalagtiteStyle(x, j, out var fail2, out var desiredType, out var height, out var y);
			if (fail2) {
				return false;
			}
			if (type != desiredType) {
				int num = WorldGen.genRand.Next(3) * 18;
				ushort num2;
				if (desiredType == 0)
					num2 = (ushort)ModContent.TileType<BlueIceStalactite>();
				else if (desiredType == 1)
					num2 = (ushort)ModContent.TileType<CreamstoneStalactite>();
				else if (desiredType == 2)
					num2 = TileID.Stalactite;
				else
					num2 = TileID.Stalactite;
				for (int i = y; i < y + height; i++) {
					Main.tile[x, i].TileFrameX = (short)num;
					Main.tile[x, i].TileType = num2;
				}
				if (Main.netMode == 2) {
					NetMessage.SendTileSquare(-1, x, y, 1, 2);
				}
			}
			return true;
		}

		private static void GetStalagtiteStyle(int x, int y, out int type, out bool fail) {
			type = 0;
			fail = false;
			if (Main.tile[x, y].TileType == ModContent.TileType<BlueIceStalactite>())
				type = 0;
			else if (Main.tile[x, y].TileType == ModContent.TileType<CreamstoneStalactite>())
				type = 1;
			else if (Main.tile[x, y].TileType == TileID.Stalactite)
				type = 2;
			else
				fail = true;
		}

		private static void GetDesiredStalagtiteStyle(int x, int j, out bool fail, out int desiredStyle, out int height, out int y) {
			fail = false;
			desiredStyle = 0;
			height = 1;
			y = j;
			if (Main.tile[x, y].TileFrameY == 72) {
				desiredStyle = Main.tile[x, y - 1].TileType;
			}
			else if (Main.tile[x, y].TileFrameY == 90) {
				desiredStyle = Main.tile[x, y + 1].TileType;
			}
			else if (Main.tile[x, y].TileFrameY >= 36) {
				if (Main.tile[x, y].TileFrameY == 54) {
					y--;
				}
				height = 2;
				desiredStyle = Main.tile[x, y + 2].TileType;
			}
			else {
				if (Main.tile[x, y].TileFrameY == 18) {
					y--;
				}
				height = 2;
				desiredStyle = Main.tile[x, y - 1].TileType;
			}
			if (desiredStyle == ModContent.TileType<Creamstone>() || desiredStyle == ModContent.TileType<Creamsandstone>() || desiredStyle == ModContent.TileType<HardenedCreamsand>()) {
				desiredStyle = 1;
			}
			else if (desiredStyle == ModContent.TileType<BlueIce>()) {
				desiredStyle = 0;
			}
			else {
				desiredStyle = 2;
				//fail = true;
			}
		}
	}
}
