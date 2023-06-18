using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using TheConfectionRebirth.Tiles;
using TheConfectionRebirth.Walls;
using System.IO;
using Terraria.GameContent.Biomes;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using Terraria.IO;
using TheConfectionRebirth.Hooks;

namespace TheConfectionRebirth
{
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
		}

		public override void NetSend(BinaryWriter writer) {
			var flags = new BitsByte();
			flags[0] = confectionorHallow;
			writer.Write(flags);
		}

		public override void NetReceive(BinaryReader reader) {
			BitsByte flags = reader.ReadByte();
			confectionorHallow = flags[0];
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
							else if (TileID.Sets.Crimson[Main.tile[i, j].TileType] || TileID.Sets.Corrupt[Main.tile[i, j].TileType]) {
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
							else if (TileID.Sets.Crimson[Main.tile[k, l].TileType] || TileID.Sets.Corrupt[Main.tile[k, l].TileType]) {
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
				if (TileID.Sets.Crimson[tile.TileType]) {
					num12 = (ushort)(192 + WorldGen.genRand.Next(4));
				}
				else if (TileID.Sets.Corrupt[tile.TileType]) {
					num12 = (ushort)(188 + WorldGen.genRand.Next(4));
				}
				else if (TileID.Sets.Hallow[tile.TileType]) {
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
	}
}
