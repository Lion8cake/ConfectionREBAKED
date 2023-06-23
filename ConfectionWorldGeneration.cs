using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.WorldBuilding;
using TheConfectionRebirth.Tiles;
using TheConfectionRebirth.Walls;

namespace TheConfectionRebirth {
	public enum HallowOptions {
		Random,
		Hallow,
		Confection,
	}

	public class ConfectionWorldGeneration : ModSystem {
		public HallowOptions SelectedHallowOption { get; set; } = HallowOptions.Random;
		public static bool confectionorHallow;
		public static int confectionBG;

		public override void OnWorldLoad() {
			confectionorHallow = false;
			confectionBG = 0;
		}

		public override void OnWorldUnload() {
			confectionorHallow = false;
			confectionBG = 0;
			totalCandy = 0;
			totalCandy2 = 0;
			tCandy = 0;
		}

		public override void SaveWorldData(TagCompound tag) {
			if (confectionorHallow) {
				tag["TheConfectionRebirth:confectionorHallow"] = true;
			}
			tag["TheConfectionRebirth:confectionBG"] = confectionBG;

			// Update config cache values on save world
			ConfectionConfig config = ModContent.GetInstance<ConfectionConfig>();
			Dictionary<string, ConfectionConfig.WorldDataValues> tempDict = config.GetWorldData();
			ConfectionConfig.WorldDataValues worldData;

			worldData.confection = confectionorHallow;

			string path = Path.ChangeExtension(Main.worldPathName, ".twld");
			tempDict[path] = worldData;
			config.SetWorldData(tempDict);

			ConfectionConfig.Save(config);
		}

		public override void LoadWorldData(TagCompound tag) {
			confectionorHallow = tag.ContainsKey("TheConfectionRebirth:confectionorHallow");
			confectionBG = tag.GetInt("TheConfectionRebirth:confectionBG");
			for (var i = 0; i < Main.tile.Width; ++i)
				for (var j = 0; j < Main.tile.Height; ++j)
					if (ConfectCountCollection.Contains(Main.tile[i, j].TileType))
						totalCandy2++;
		}

		public override void NetSend(BinaryWriter writer) {
			var flags = new BitsByte();
			flags[0] = confectionorHallow;
			writer.Write(flags);

			writer.Write(confectionBG);

			writer.Write(tCandy);
		}

		public override void NetReceive(BinaryReader reader) {
			BitsByte flags = reader.ReadByte();
			confectionorHallow = flags[0];

			confectionBG = reader.ReadInt32();

			tCandy = reader.ReadByte();
		}

		public override void PreWorldGen() {
			confectionorHallow = SelectedHallowOption switch {
				HallowOptions.Random => Main.rand.NextBool(),
				HallowOptions.Hallow => false,
				HallowOptions.Confection => true,
				_ => throw new ArgumentOutOfRangeException(),
			};
			confectionBG = Main.rand.Next(4);
		}

		public override void ModifyHardmodeTasks(List<GenPass> list) {
			if (confectionorHallow) {
				int index2 = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Good"));
				if (index2 != -1) {
					list.Insert(index2 + 1, new PassLegacy("Hardmode Good", new WorldGenLegacyMethod(Confection)));
					list.RemoveAt(index2);
				}
				int index3 = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Evil"));
				if (index3 != -1) {
					list.RemoveAt(index3);
				}
			}
		}

		private static void Confection(GenerationProgress progres, GameConfiguration configurations) {
			Main.NewText("Hallow Generation is happening");
			if (Main.rand == null) {
				Main.rand = new UnifiedRandom((int)DateTime.Now.Ticks);
			}
			double num = (double)WorldGen.genRand.Next(300, 400) * 0.001;
			double num2 = (double)WorldGen.genRand.Next(200, 300) * 0.001;
			int num3 = (int)((double)Main.maxTilesX * num);
			int num4 = (int)((double)Main.maxTilesX * (1.0 - num));
			int num5 = 1;
			if (WorldGen.genRand.Next(2) == 0) {
				num4 = (int)((double)Main.maxTilesX * num);
				num3 = (int)((double)Main.maxTilesX * (1.0 - num));
				num5 = -1;
			}
			int num6 = 1;
			if (GenVars.dungeonX < Main.maxTilesX / 2) {
				num6 = -1;
			}
			if (num6 < 0) {
				if (num4 < num3) {
					num4 = (int)((double)Main.maxTilesX * num2);
				}
				else {
					num3 = (int)((double)Main.maxTilesX * num2);
				}
			}
			else if (num4 > num3) {
				num4 = (int)((double)Main.maxTilesX * (1.0 - num2));
			}
			else {
				num3 = (int)((double)Main.maxTilesX * (1.0 - num2));
			}
			if (Main.remixWorld) {
				int num7 = Main.maxTilesX / 7;
				int num8 = Main.maxTilesX / 14;
				if (Main.dungeonX < Main.maxTilesX / 2) {
					for (int i = Main.maxTilesX - num7 - num8; i < Main.maxTilesX; i++) {
						for (int j = (int)Main.worldSurface + WorldGen.genRand.Next(-1, 2); j < Main.maxTilesY - 10; j++) {
							if (i > Main.maxTilesX - num7) {
								WorldGen.Convert(i, j, 2, 1);
							}
						}
					}
				}
				else {
					for (int k = 0; k < num7 + num8; k++) {
						for (int l = (int)Main.worldSurface + WorldGen.genRand.Next(-1, 2); l < Main.maxTilesY - 10; l++) {
							if (k < num7) {
								WorldGen.Convert(k, l, 2, 1);
							}
						}
					}
				}
			}
			else {
				WorldGen.GERunner(num3, 0, 3 * num5, 5.0);
				WorldGen.GERunner(num4, 0, 3 * -num5, 5.0, good: false);
			}
			double num9 = (double)Main.maxTilesX / 4200.0;
			int num10 = (int)(25.0 * num9);
			ShapeData shapeData = new ShapeData();
			int num11 = 0;
			while (num10 > 0) {
				if (++num11 % 15000 == 0) {
					num10--;
				}
				Point point = WorldGen.RandomWorldPoint((int)Main.worldSurface - 100, 1, 190, 1);
				Tile tile = Main.tile[point.X, point.Y];
				Tile tile2 = Main.tile[point.X, point.Y - 1];
				ushort num12 = 0;
				if (TileID.Sets.Hallow[tile.TileType]) {
					num12 = (ushort)(200 + WorldGen.genRand.Next(4));
				}
				if (tile.HasTile && num12 != 0 && !tile2.HasTile) {
					bool flag = WorldUtils.Gen(new Point(point.X, point.Y - 1), new ShapeFloodFill(1000), Actions.Chain(new Modifiers.IsNotSolid(), new Modifiers.OnlyWalls(0, 54, 55, 56, 57, 58, 59, 61, 185, 212, 213, 214, 215, 2, 196, 197, 198, 199, 15, 40, 71, 64, 204, 205, 206, 207, 208, 209, 210, 211, 71), new Actions.Blank().Output(shapeData)));
					if (shapeData.Count > 50 && flag) {
						WorldUtils.Gen(new Point(point.X, point.Y), new ModShapes.OuterOutline(shapeData, useDiagonals: true, useInterior: true), new Actions.PlaceWall(num12));
						num10--;
					}
					shapeData.Clear();
				}
			}
			if (Main.netMode == 0) {
				Main.NewText(Lang.misc[15].Value, 50, byte.MaxValue, 130);
			}
			AchievementsHelper.NotifyProgressionEvent(9);
			if (Main.netMode == 2) {
				Netplay.ResetSections();
			}
		}

		public static int totalCandy;
		public static int totalCandy2;
		public static byte tCandy;
		public static int[] NewTileCounts = new int[TileLoader.TileCount];

		public override void Load() {
			On_WorldGen.CountTiles += On_WorldGen_CountTiles;
			On_WorldGen.AddUpAlignmentCounts += On_WorldGen_AddUpAlignmentCounts;
			On_WorldGen.hardUpdateWorld += On_WorldGen_hardUpdateWorld;
			On_WorldGen.UpdateWorld_OvergroundTile += On_WorldGen_UpdateWorld_OvergroundTile;
			On_WorldGen.UpdateWorld_UndergroundTile += On_WorldGen_UpdateWorld_UndergroundTile;
			On_WorldGen.SpreadDesertWalls += On_WorldGen_SpreadDesertWalls;
		}

		public override void Unload() {
			On_WorldGen.CountTiles -= On_WorldGen_CountTiles;
			On_WorldGen.AddUpAlignmentCounts -= On_WorldGen_AddUpAlignmentCounts;
			On_WorldGen.hardUpdateWorld -= On_WorldGen_hardUpdateWorld;
			On_WorldGen.UpdateWorld_OvergroundTile -= On_WorldGen_UpdateWorld_OvergroundTile;
			On_WorldGen.UpdateWorld_UndergroundTile -= On_WorldGen_UpdateWorld_UndergroundTile;
			On_WorldGen.SpreadDesertWalls -= On_WorldGen_SpreadDesertWalls;
		}

		#region SpreadingDetours
		private void On_WorldGen_SpreadDesertWalls(On_WorldGen.orig_SpreadDesertWalls orig, int wallDist, int i, int j) {
			orig.Invoke(wallDist, i, j);
			if (WorldGen.InWorld(i, j, 10) || (WallID.Sets.Conversion.Sandstone[Main.tile[i, j].WallType] && (Main.tile[i, j].HasTile || TileID.Sets.Conversion.Sandstone[Main.tile[i, j].TileType]) && WallID.Sets.Conversion.HardenedSand[Main.tile[i, j].WallType])) {
				bool num = false;
				int wall = Main.tile[i, j].WallType;
				int type = Main.tile[i, j].TileType;
				if (
				(wall == ModContent.WallType<CreamGrassWall>() || wall == ModContent.WallType<CreamstoneWall>() || wall == ModContent.WallType<Creamstone2Wall>() || wall == ModContent.WallType<Creamstone3Wall>() || wall == ModContent.WallType<Creamstone4Wall>() || wall == ModContent.WallType<Creamstone5Wall>() || wall == ModContent.WallType<BlueIceWall>() || wall == ModContent.WallType<HardenedCreamsandWall>() || wall == ModContent.WallType<CreamsandstoneWall>() ||
				((wall == ModContent.WallType<CookieWall>() || wall == ModContent.WallType<BlueFairyFlossWall>() || wall == ModContent.WallType<CreamWall>() || wall == ModContent.WallType<PinkFairyFlossWall>() || wall == ModContent.WallType<PurpleFairyFlossWall>()) && ModContent.GetInstance<ConfectionServerConfig>().CookieSpread))
				||
				(type == ModContent.TileType<CreamGrass>() || type == ModContent.TileType<CreamGrass_Foliage>() || type == ModContent.TileType<CreamVines>() || type == ModContent.TileType<Creamsand>() || type == ModContent.TileType<Creamstone>() || type == ModContent.TileType<BlueIce>() || type == ModContent.TileType<HardenedCreamsand>() || type == ModContent.TileType<Creamsandstone>() || type == ModContent.TileType<CreamGrassMowed>() || (
				(type == ModContent.TileType<CookieBlock>() || type == ModContent.TileType<CreamBlock>() || type == ModContent.TileType<PinkFairyFloss>() || type == ModContent.TileType<PurpleFairyFloss>() || type == ModContent.TileType<BlueFairyFloss>() || type == ModContent.TileType<CookiestCookieBlock>() || type == ModContent.TileType<CreamstoneAmethyst>() || type == ModContent.TileType<CreamstoneTopaz>() || type == ModContent.TileType<CreamstoneSaphire>() || type == ModContent.TileType<CreamstoneEmerald>() || type == ModContent.TileType<CreamstoneRuby>() || type == ModContent.TileType<CreamstoneDiamond>() ||
				type == ModContent.TileType<ArgonCreamMoss>() || type == ModContent.TileType<BlueCreamMoss>() || type == ModContent.TileType<BrownCreamMoss>() || type == ModContent.TileType<GreenCreamMoss>() || type == ModContent.TileType<KryptonCreamMoss>() || type == ModContent.TileType<LavaCreamMoss>() || type == ModContent.TileType<PurpleCreamMoss>() || type == ModContent.TileType<RedCreamMoss>() || type == ModContent.TileType<XenomCreamMoss>()) && ModContent.GetInstance<ConfectionServerConfig>().CookieSpread))) {
					num = true;
				}
				if (num == false) {
					return;
				}
				int num2 = i + WorldGen.genRand.Next(-2, 3);
				int num3 = j + WorldGen.genRand.Next(-2, 3);
				bool flag = false;
				if (WallID.Sets.Conversion.PureSand[Main.tile[num2, num3].WallType]) {
					if (num == true) {
						for (int num4 = i - wallDist; num4 < i + wallDist; num4++) {
							for (int num5 = j - wallDist; num5 < j + wallDist; num5++) {
								int type2 = Main.tile[num4, num5].TileType;
								if (Main.tile[num4, num5].HasTile && (type2 == ModContent.TileType<CreamGrass>() || type2 == ModContent.TileType<CreamGrass_Foliage>() || type2 == ModContent.TileType<CreamVines>() || type2 == ModContent.TileType<Creamsand>() || type2 == ModContent.TileType<Creamstone>() || type2 == ModContent.TileType<BlueIce>() || type2 == ModContent.TileType<HardenedCreamsand>() || type2 == ModContent.TileType<Creamsandstone>() || type2 == ModContent.TileType<CreamGrassMowed>() || (
									(type2 == ModContent.TileType<CookieBlock>() || type2 == ModContent.TileType<CreamBlock>() || type2 == ModContent.TileType<PinkFairyFloss>() || type2 == ModContent.TileType<PurpleFairyFloss>() || type2 == ModContent.TileType<BlueFairyFloss>() || type2 == ModContent.TileType<CookiestCookieBlock>() || type2 == ModContent.TileType<CreamstoneAmethyst>() || type2 == ModContent.TileType<CreamstoneTopaz>() || type2 == ModContent.TileType<CreamstoneSaphire>() || type2 == ModContent.TileType<CreamstoneEmerald>() || type2 == ModContent.TileType<CreamstoneRuby>() || type2 == ModContent.TileType<CreamstoneDiamond>() ||
									type2 == ModContent.TileType<ArgonCreamMoss>() || type2 == ModContent.TileType<BlueCreamMoss>() || type2 == ModContent.TileType<BrownCreamMoss>() || type2 == ModContent.TileType<GreenCreamMoss>() || type2 == ModContent.TileType<KryptonCreamMoss>() || type2 == ModContent.TileType<LavaCreamMoss>() || type2 == ModContent.TileType<PurpleCreamMoss>() || type2 == ModContent.TileType<RedCreamMoss>() || type2 == ModContent.TileType<XenomCreamMoss>()) && ModContent.GetInstance<ConfectionServerConfig>().CookieSpread))) {
									flag = true;
									break;
								}
							}
							if (flag) {
								break;
							}
						}
					}
				}
				if (!flag) {
					return;
				}
				ushort? num6 = null;
				if (WallID.Sets.Conversion.Sandstone[Main.tile[num2, num3].WallType]) {
					if (num == true) {
						num6 = (ushort)ModContent.WallType<HardenedCreamsandWall>();
					}
				}
				if (WallID.Sets.Conversion.HardenedSand[Main.tile[num2, num3].WallType]) {
					if (num == true) {
						num6 = (ushort)ModContent.WallType<CreamsandstoneWall>();
					}
				}
				if (num6.HasValue && Main.tile[num2, num3].WallType != num6.Value) {
					Main.tile[num2, num3].WallType = num6.Value;
					if (Main.netMode == 2) {
						NetMessage.SendTileSquare(-1, num2, num3);
					}
				}
			}
		}

		private void On_WorldGen_UpdateWorld_OvergroundTile(On_WorldGen.orig_UpdateWorld_OvergroundTile orig, int i, int j, bool checkNPCSpawns, int wallDist) {
			orig.Invoke(i, j, checkNPCSpawns, wallDist);
			if (WorldGen.AllowedToSpreadInfections) {
				if (Main.tile[i, j].WallType == ModContent.WallType<CreamGrassWall>() || (Main.tile[i, j].TileType == ModContent.TileType<CreamGrass>() && Main.tile[i, j].HasTile)) {
					int num30 = i + WorldGen.genRand.Next(-2, 3);
					int num31 = j + WorldGen.genRand.Next(-2, 3);
					if ((WorldGen.InWorld(num30, num31, 10) && Main.tile[num30, num31].WallType == 63) || Main.tile[num30, num31].WallType == 65 || Main.tile[num30, num31].WallType == 66 || Main.tile[num30, num31].WallType == 68) {
						bool flag4 = false;
						for (int num32 = i - wallDist; num32 < i + wallDist; num32++) {
							for (int num33 = j - wallDist; num33 < j + wallDist; num33++) {
								if (Main.tile[num32, num33].HasTile) {
									int type6 = Main.tile[num32, num33].TileType;
									if (type6 == ModContent.TileType<CreamGrass>() || type6 == ModContent.TileType<CreamGrass_Foliage>() || type6 == ModContent.TileType<CreamVines>() || type6 == ModContent.TileType<Creamsand>() || type6 == ModContent.TileType<Creamstone>() || type6 == ModContent.TileType<BlueIce>() || type6 == ModContent.TileType<HardenedCreamsand>() || type6 == ModContent.TileType<Creamsandstone>() || type6 == ModContent.TileType<CreamGrassMowed>()) {
										flag4 = true;
										break;
									}
								}
							}
						}
						if (flag4) {
							Main.tile[num30, num31].WallType = (ushort)ModContent.WallType<CreamGrassWall>();
							if (Main.netMode == 2) {
								NetMessage.SendTileSquare(-1, num30, num31);
							}
						}
					}
				}
				if ((Main.tile[i, j].WallType == ModContent.WallType<CookieWall>() || (Main.tile[i, j].TileType == ModContent.TileType<CookieBlock>() && Main.tile[i, j].HasTile)) && ModContent.GetInstance<ConfectionServerConfig>().CookieSpread) {
					int num30 = i + WorldGen.genRand.Next(-2, 3);
					int num31 = j + WorldGen.genRand.Next(-2, 3);
					if (Main.tile[num30, num31].WallType == 2 || Main.tile[num30, num31].WallType == 16) {
						bool flag4 = false;
						for (int num32 = i - wallDist; num32 < i + wallDist; num32++) {
							for (int num33 = j - wallDist; num33 < j + wallDist; num33++) {
								if (Main.tile[num32, num33].HasTile) {
									int type6 = Main.tile[num32, num33].TileType;
									if (type6 == ModContent.TileType<CreamGrass>() || type6 == ModContent.TileType<CreamGrass_Foliage>() || type6 == ModContent.TileType<CreamVines>() || type6 == ModContent.TileType<Creamsand>() || type6 == ModContent.TileType<Creamstone>() || type6 == ModContent.TileType<BlueIce>() || type6 == ModContent.TileType<HardenedCreamsand>() || type6 == ModContent.TileType<Creamsandstone>() || type6 == ModContent.TileType<CreamGrassMowed>()) {
										flag4 = true;
										break;
									}
								}
							}
						}
						if (flag4) {
							Main.tile[num30, num31].WallType = (ushort)ModContent.WallType<CookieWall>();
							if (Main.netMode == 2) {
								NetMessage.SendTileSquare(-1, num30, num31);
							}
						}
					}
				}
			}
		}

		private void On_WorldGen_UpdateWorld_UndergroundTile(On_WorldGen.orig_UpdateWorld_UndergroundTile orig, int i, int j, bool checkNPCSpawns, int wallDist) {
			orig.Invoke(i, j, checkNPCSpawns, wallDist);
			if (WorldGen.AllowedToSpreadInfections) {
				if (Main.tile[i, j].WallType == ModContent.WallType<CreamGrassWall>() || (Main.tile[i, j].TileType == ModContent.TileType<CreamGrass>() && Main.tile[i, j].HasTile)) {
					int num30 = i + WorldGen.genRand.Next(-2, 3);
					int num31 = j + WorldGen.genRand.Next(-2, 3);
					if ((WorldGen.InWorld(num30, num31, 10) && Main.tile[num30, num31].WallType == 63) || Main.tile[num30, num31].WallType == 65 || Main.tile[num30, num31].WallType == 66 || Main.tile[num30, num31].WallType == 68) {
						bool flag4 = false;
						for (int num32 = i - wallDist; num32 < i + wallDist; num32++) {
							for (int num33 = j - wallDist; num33 < j + wallDist; num33++) {
								if (Main.tile[num32, num33].HasTile) {
									int type6 = Main.tile[num32, num33].TileType;
									if (type6 == ModContent.TileType<CreamGrass>() || type6 == ModContent.TileType<CreamGrass_Foliage>() || type6 == ModContent.TileType<CreamVines>() || type6 == ModContent.TileType<Creamsand>() || type6 == ModContent.TileType<Creamstone>() || type6 == ModContent.TileType<BlueIce>() || type6 == ModContent.TileType<HardenedCreamsand>() || type6 == ModContent.TileType<Creamsandstone>() || type6 == ModContent.TileType<CreamGrassMowed>()) {
										flag4 = true;
										break;
									}
								}
							}
						}
						if (flag4) {
							Main.tile[num30, num31].WallType = (ushort)ModContent.WallType<CreamGrassWall>();
							if (Main.netMode == 2) {
								NetMessage.SendTileSquare(-1, num30, num31);
							}
						}
					}
				}
				if ((Main.tile[i, j].WallType == ModContent.WallType<CookieWall>() || (Main.tile[i, j].TileType == ModContent.TileType<CookieBlock>() && Main.tile[i, j].HasTile)) && ModContent.GetInstance<ConfectionServerConfig>().CookieSpread) {
					int num30 = i + WorldGen.genRand.Next(-2, 3);
					int num31 = j + WorldGen.genRand.Next(-2, 3);
					if (Main.tile[num30, num31].WallType == 2 || Main.tile[num30, num31].WallType == 16) {
						bool flag4 = false;
						for (int num32 = i - wallDist; num32 < i + wallDist; num32++) {
							for (int num33 = j - wallDist; num33 < j + wallDist; num33++) {
								if (Main.tile[num32, num33].HasTile) {
									int type6 = Main.tile[num32, num33].TileType;
									if (type6 == ModContent.TileType<CreamGrass>() || type6 == ModContent.TileType<CreamGrass_Foliage>() || type6 == ModContent.TileType<CreamVines>() || type6 == ModContent.TileType<Creamsand>() || type6 == ModContent.TileType<Creamstone>() || type6 == ModContent.TileType<BlueIce>() || type6 == ModContent.TileType<HardenedCreamsand>() || type6 == ModContent.TileType<Creamsandstone>() || type6 == ModContent.TileType<CreamGrassMowed>()) {
										flag4 = true;
										break;
									}
								}
							}
						}
						if (flag4) {
							Main.tile[num30, num31].WallType = (ushort)ModContent.WallType<CookieWall>();
							if (Main.netMode == 2) {
								NetMessage.SendTileSquare(-1, num30, num31);
							}
						}
					}
				}
			}
		}

		private void On_WorldGen_hardUpdateWorld(On_WorldGen.orig_hardUpdateWorld orig, int i, int j) {
			orig.Invoke(i, j);
			if (Main.hardMode || !Main.tile[i, j].IsActuated) {
				int type = Main.tile[i, j].TileType;
				if ((NPC.downedPlantBoss && WorldGen.genRand.Next(2) == 0) || WorldGen.AllowedToSpreadInfections) {
					if (type == ModContent.TileType<CreamGrass>() || type == ModContent.TileType<CreamGrass_Foliage>() || type == ModContent.TileType<CreamVines>() || type == ModContent.TileType<Creamsand>() || type == ModContent.TileType<Creamstone>() || type == ModContent.TileType<BlueIce>() || type == ModContent.TileType<HardenedCreamsand>() || type == ModContent.TileType<Creamsandstone>() || type == ModContent.TileType<CreamGrassMowed>() || (
						(type == ModContent.TileType<CookieBlock>() || type == ModContent.TileType<CreamBlock>() || type == ModContent.TileType<PinkFairyFloss>() || type == ModContent.TileType<PurpleFairyFloss>() || type == ModContent.TileType<BlueFairyFloss>() || type == ModContent.TileType<CookiestCookieBlock>() || type == ModContent.TileType<CreamstoneAmethyst>() || type == ModContent.TileType<CreamstoneTopaz>() || type == ModContent.TileType<CreamstoneSaphire>() || type == ModContent.TileType<CreamstoneEmerald>() || type == ModContent.TileType<CreamstoneRuby>() || type == ModContent.TileType<CreamstoneDiamond>() ||
						type == ModContent.TileType<ArgonCreamMoss>() || type == ModContent.TileType<BlueCreamMoss>() || type == ModContent.TileType<BrownCreamMoss>() || type == ModContent.TileType<GreenCreamMoss>() || type == ModContent.TileType<KryptonCreamMoss>() || type == ModContent.TileType<LavaCreamMoss>() || type == ModContent.TileType<PurpleCreamMoss>() || type == ModContent.TileType<RedCreamMoss>() || type == ModContent.TileType<XenomCreamMoss>()) && ModContent.GetInstance<ConfectionServerConfig>().CookieSpread)) {
						bool flag4 = true;
						while (flag4) {
							flag4 = false;
							int num15 = i + WorldGen.genRand.Next(-3, 4);
							int num16 = j + WorldGen.genRand.Next(-3, 4);
							if (!WorldGen.InWorld(num15, num16, 10) || WorldGen.CountNearBlocksTypes(num15, num16, 2, 1, 27) > 0) {
								continue;
							}
							if (Main.tile[num15, num16].TileType == 2) {
								if (WorldGen.genRand.Next(2) == 0) {
									flag4 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamGrass>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == 477) {
								if (WorldGen.genRand.Next(2) == 0) {
									flag4 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamGrassMowed>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == 1 || Main.tileMoss[Main.tile[num15, num16].TileType]) {
								if (WorldGen.genRand.Next(2) == 0) {
									flag4 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<Creamstone>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == 53) {
								if (WorldGen.genRand.Next(2) == 0) {
									flag4 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<Creamsand>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == 396) {
								if (WorldGen.genRand.Next(2) == 0) {
									flag4 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<Creamsandstone>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == 397) {
								if (WorldGen.genRand.Next(2) == 0) {
									flag4 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<HardenedCreamsand>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == 161) {
								if (WorldGen.genRand.Next(2) == 0) {
									flag4 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<BlueIce>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
						}
					}
				}
			}

			if (Main.hardMode || !Main.tile[i, j].IsActuated) {
				int type = Main.tile[i, j].TileType;
				if ((NPC.downedPlantBoss && WorldGen.genRand.Next(2) == 0) || WorldGen.AllowedToSpreadInfections) {
					if ((type == ModContent.TileType<CookieBlock>() || type == ModContent.TileType<CreamBlock>() || type == ModContent.TileType<PinkFairyFloss>() || type == ModContent.TileType<PurpleFairyFloss>() || type == ModContent.TileType<BlueFairyFloss>() || type == ModContent.TileType<CookiestCookieBlock>() || type == ModContent.TileType<CreamstoneAmethyst>() || type == ModContent.TileType<CreamstoneTopaz>() || type == ModContent.TileType<CreamstoneSaphire>() || type == ModContent.TileType<CreamstoneEmerald>() || type == ModContent.TileType<CreamstoneRuby>() || type == ModContent.TileType<CreamstoneDiamond>() || type == ModContent.TileType<ArgonCreamMoss>() || type == ModContent.TileType<BlueCreamMoss>() || type == ModContent.TileType<BrownCreamMoss>() || type == ModContent.TileType<GreenCreamMoss>() || type == ModContent.TileType<KryptonCreamMoss>() || type == ModContent.TileType<LavaCreamMoss>() || type == ModContent.TileType<PurpleCreamMoss>() || type == ModContent.TileType<RedCreamMoss>() || type == ModContent.TileType<XenomCreamMoss>()) && ModContent.GetInstance<ConfectionServerConfig>().CookieSpread) {
						bool flag40 = true;
						while (flag40) {
							flag40 = false;
							int num15 = i + WorldGen.genRand.Next(-3, 4);
							int num16 = j + WorldGen.genRand.Next(-3, 4);
							if (!WorldGen.InWorld(num15, num16, 10) || WorldGen.CountNearBlocksTypes(num15, num16, 2, 1, 27) > 0) {
								continue;
							}
							if (Main.tile[num15, num16].TileType == TileID.Dirt) {
								if (WorldGen.genRand.Next(2) == 0) {
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CookieBlock>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.SnowBlock) {
								if (WorldGen.genRand.Next(2) == 0) {
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamBlock>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.Cloud) {
								if (WorldGen.genRand.Next(2) == 0) {
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<PinkFairyFloss>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.RainCloud) {
								if (WorldGen.genRand.Next(2) == 0) {
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<PurpleFairyFloss>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.SnowCloud) {
								if (WorldGen.genRand.Next(2) == 0) {
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<BlueFairyFloss>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							if (Main.tile[num15, num16].TileType == TileID.Amethyst) {
								if (WorldGen.genRand.Next(2) == 0) {
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneAmethyst>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.Topaz) {
								if (WorldGen.genRand.Next(2) == 0) {
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneTopaz>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.Sapphire) {
								if (WorldGen.genRand.Next(2) == 0) {
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneSaphire>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.Emerald) {
								if (WorldGen.genRand.Next(2) == 0) {
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneEmerald>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.Ruby) {
								if (WorldGen.genRand.Next(2) == 0) {
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneRuby>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.Diamond) {
								if (WorldGen.genRand.Next(2) == 0) {
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneDiamond>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.DirtiestBlock) {
								if (WorldGen.genRand.Next(2) == 0) {
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CookiestCookieBlock>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
						}
					}
				}
			}
		}
		#endregion

		#region World%Calculations
		private void On_WorldGen_AddUpAlignmentCounts(On_WorldGen.orig_AddUpAlignmentCounts orig, bool clearCounts) {
			orig.Invoke(clearCounts);
			if (clearCounts) {
				totalCandy2 = 0;
			}
			/*for (int i = 0; i < ConfectCountCollection.Count; i++) {
				totalCandy2 += WorldGen.tileCounts[ConfectCountCollection[i]];
			}*/

			WorldGen.totalSolid2 +=
				WorldGen.tileCounts[ModContent.TileType<Creamstone>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamGrass>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamGrassMowed>()] +
				WorldGen.tileCounts[ModContent.TileType<Creamsand>()] +
				WorldGen.tileCounts[ModContent.TileType<BlueIce>()] +
				WorldGen.tileCounts[ModContent.TileType<Creamsandstone>()] +
				WorldGen.tileCounts[ModContent.TileType<HardenedCreamsand>()] +
				WorldGen.tileCounts[ModContent.TileType<CookieBlock>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamBlock>()] +
				WorldGen.tileCounts[ModContent.TileType<PinkFairyFloss>()] +
				WorldGen.tileCounts[ModContent.TileType<PurpleFairyFloss>()] +
				WorldGen.tileCounts[ModContent.TileType<BlueFairyFloss>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneAmethyst>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneSaphire>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneTopaz>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneRuby>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneDiamond>()] +
				WorldGen.tileCounts[ModContent.TileType<CreamstoneEmerald>()] +
				WorldGen.tileCounts[ModContent.TileType<ArgonCreamMoss>()] +
				WorldGen.tileCounts[ModContent.TileType<BlueCreamMoss>()] +
				WorldGen.tileCounts[ModContent.TileType<BrownCreamMoss>()] +
				WorldGen.tileCounts[ModContent.TileType<GreenCreamMoss>()] +
				WorldGen.tileCounts[ModContent.TileType<KryptonCreamMoss>()] +
				WorldGen.tileCounts[ModContent.TileType<LavaCreamMoss>()] +
				WorldGen.tileCounts[ModContent.TileType<PurpleCreamMoss>()] +
				WorldGen.tileCounts[ModContent.TileType<RedCreamMoss>()] +
				WorldGen.tileCounts[ModContent.TileType<XenomCreamMoss>()];

			Array.Clear(WorldGen.tileCounts, 0, WorldGen.tileCounts.Length);
		}

		private void On_WorldGen_CountTiles(On_WorldGen.orig_CountTiles orig, int X) {
			orig.Invoke(X);
			if (X == 0) {
				totalCandy = totalCandy2;
				tCandy = (byte)Math.Round((double)totalCandy / (double)WorldGen.totalSolid * 100.0);
				if (tCandy == 0 && totalCandy > 0) {
					tCandy = 1;
				}
				if (Main.netMode == 2) {
					NetMessage.SendData(MessageID.TileCounts);
				}
				totalCandy2 = 0;
			}
			ushort num = 0;
			ushort num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			do {
				int num6;
				int num7;
				if (num4 == 0) {
					num6 = 0;
					num5 = (int)(Main.worldSurface + 1.0);
					num7 = 5;
				}
				else {
					num6 = num5;
					num5 = Main.maxTilesY;
					num7 = 1;
				}
				for (int i = num6; i < num5; i++) {
					Tile tile = Main.tile[X, i];
					if (tile == null) {
						Tile TILE = Main.tile[X, i];
						tile = (TILE = new Tile());
					}
					num = tile.TileType;
					if (num != 0 || tile.HasTile) {
						if (num == num2) {
							num3 += num7;
							continue;
						}
						NewTileCounts[num2] += num3;
						num2 = num;
						num3 = num7;
					}
				}
				NewTileCounts[num2] += num3;
				num3 = 0;
				num4++;
			}
			while (num4 < 2);
			WorldGen.AddUpAlignmentCounts();
		}

		public static List<int> ConfectCountCollection;

		public override void PostSetupContent() {
			ConfectCountCollection = new List<int> {
				ModContent.TileType<Creamstone>(),
				ModContent.TileType<CreamGrass>(),
				ModContent.TileType<CreamGrassMowed>(),
				ModContent.TileType<Creamsand>(),
				ModContent.TileType<BlueIce>(),
				ModContent.TileType<Creamsandstone>(),
				ModContent.TileType<HardenedCreamsand>(),
				ModContent.TileType<CookieBlock>(),
				ModContent.TileType<CreamBlock>(),
				ModContent.TileType<PinkFairyFloss>(),
				ModContent.TileType<PurpleFairyFloss>(),
				ModContent.TileType<BlueFairyFloss>(),
				ModContent.TileType<CreamstoneAmethyst>(),
				ModContent.TileType<CreamstoneSaphire>(),
				ModContent.TileType<CreamstoneTopaz>(),
				ModContent.TileType<CreamstoneRuby>(),
				ModContent.TileType<CreamstoneDiamond>(),
				ModContent.TileType<CreamstoneEmerald>(),
				ModContent.TileType<ArgonCreamMoss>(),
				ModContent.TileType<BlueCreamMoss>(),
				ModContent.TileType<BrownCreamMoss>(),
				ModContent.TileType<GreenCreamMoss>(),
				ModContent.TileType<KryptonCreamMoss>(),
				ModContent.TileType<LavaCreamMoss>(),
				ModContent.TileType<PurpleCreamMoss>(),
				ModContent.TileType<RedCreamMoss>(),
				ModContent.TileType<XenomCreamMoss>()
			};
		}
		#endregion
	}
}
