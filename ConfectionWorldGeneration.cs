using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.WorldBuilding;
using TheConfectionRebirth.ModSupport;
using TheConfectionRebirth.Tiles;
using TheConfectionRebirth.Walls;

namespace TheConfectionRebirth {
	public enum HallowOptions
	{
		Random,
		Hallow,
		Confection,
	}

	public class ConfectionWorldGeneration : ModSystem {

		public static bool confectionorHallow;

		public static int totalCandy;

		public static int totalCandy2;

		public static byte tCandy;

		public HallowOptions SelectedHallowOption { get; set; } = HallowOptions.Random;

		public static int confectionTree;

		public override void Load()
		{
			On_WorldGen.GERunner += GERunnerEditer;
			On_WorldGen.ConvertSkyIslands += On_WorldGen_ConvertSkyIslands;
			On_WorldGen.hardUpdateWorld += On_WorldGen_hardUpdateWorld;
			On_WorldGen.UpdateWorld_OvergroundTile += On_WorldGen_UpdateWorld_OvergroundTile;
			On_WorldGen.UpdateWorld_UndergroundTile += On_WorldGen_UpdateWorld_UndergroundTile;
			On_WorldGen.SpreadDesertWalls += On_WorldGen_SpreadDesertWalls;
		}

		public override void Unload()
		{
			On_WorldGen.GERunner -= GERunnerEditer;
			On_WorldGen.ConvertSkyIslands -= On_WorldGen_ConvertSkyIslands;
			On_WorldGen.hardUpdateWorld -= On_WorldGen_hardUpdateWorld;
			On_WorldGen.UpdateWorld_OvergroundTile -= On_WorldGen_UpdateWorld_OvergroundTile;
			On_WorldGen.UpdateWorld_UndergroundTile -= On_WorldGen_UpdateWorld_UndergroundTile;
			On_WorldGen.SpreadDesertWalls -= On_WorldGen_SpreadDesertWalls;
		}

		public override void OnWorldLoad()
		{
			confectionorHallow = false;
			confectionTree = 0;
		}

		public override void OnWorldUnload() 
		{
			confectionorHallow = false;
			confectionTree = 0;
		}

		public override void SaveWorldData(TagCompound tag) 
		{
			tag["TheConfectionRebirth:confectionorHallow"] = confectionorHallow;
			tag["TheConfectionRebirth:confectionTree"] = confectionTree;
		}

		public override void SaveWorldHeader(TagCompound tag)
		{
			tag["HasConfection"] = confectionorHallow;
		}

		public override void ClearWorld()
		{
			totalCandy = 0;
			totalCandy2 = 0;
			tCandy = 0;
		}

		public override void LoadWorldData(TagCompound tag)
		{
			confectionTree = tag.GetInt("TheConfectionRebirth:confectionTree");
			confectionorHallow = tag.GetBool("TheConfectionRebirth:confectionorHallow");
			//Love terraria hardcoding how % are caculated, re-caculated here because the current caculations earlier in the file saving are so early they cant caculate modded tiles (as modded tiles haddent loaded at that point)
			for (int x = 0; x < Main.maxTilesX; x++)
			{
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					int type = Main.tile[x, j].TileType;
					if (type != -1)
					{
						if (j <= Main.worldSurface)
						{
							if (j <= Main.worldSurface)
							{
								WorldGen.tileCounts[type] += 1 * 5;
							}
							else
							{
								int num4 = (int)(Main.worldSurface - (double)j + 1.0);
								int num5 = 1 - num4;
								WorldGen.tileCounts[type] += num4 * 5 + num5;
							}
						}
						else
						{
							WorldGen.tileCounts[type] += 1;
						}
					}
				}
			}
			WorldGen.AddUpAlignmentCounts(true);

			//World Converter (1.4.3 => 1.4.4)
			//Also contains some explinations 
			if (!Main.dedServ) //Make sure that we are not on a server so we dont infinatly get stuck on Syncing Mods
			{
				try
				{
					string twld = Path.ChangeExtension(Main.worldPathName, ".twld"); //gets the world we are updating
					var tag2 = TagIO.FromStream(new MemoryStream(File.ReadAllBytes(twld))); //We read the nbt data of the world .twld file
					if (tag2.ContainsKey("modData"))
					{ //We look for modData here and v there 
						foreach (TagCompound modDataTag in tag2.GetList<TagCompound>("modData"))
						{
							if (modDataTag.Get<string>("mod") == "AltLibrary" && modDataTag.Get<string>("name") == "WorldBiomeManager")
							{ //Here we take two paths, one for if altlib is enabled (imposter mod the original wont return) or if the mod is unloaded 
								TagCompound dataTag = modDataTag.Get<TagCompound>("data");

								if (dataTag.Get<string>("AltLibrary:WorldHallow") == "TheConfectionRebirth/ConfectionBiome")
								{ //Look for the correct string that WorldHallow is saved under
									confectionorHallow = true; //Convert world by giving it the tag
									ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("Altlib save!, converting world!"); //Announce converting
								}
								else
								{
									ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("non-Altlib save, unable to convert world"); //Announce that the world doesn't have altlib/it didn't work
								}
								break;
							}
							if (modDataTag.Get<string>("mod") == "ModLoader")
							{
								ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("Didn't find altlib, Attempting to look in unloaded mods"); //Didn't find altlib so we look in unloaded and announce 
								TagCompound dataTag = modDataTag.Get<TagCompound>("data"); //we look for the first tmod data
								if (dataTag.ContainsKey("list"))
								{ //find a list called list
									ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("Found List inside unloaded mods!"); //anounce we have found the list since list can be tricky sometimes
									foreach (TagCompound unloadedList in dataTag.GetList<TagCompound>("list"))
									{ //same here as above ^

										if (unloadedList.Get<string>("mod") == "AltLibrary" && unloadedList.Get<string>("name") == "WorldBiomeManager")
										{ //Look for altlib inside of list
											ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("Found Altlib under unloaded mods!"); //announce that altlib has been found inside tmod's unloaded data
											TagCompound dataTag2 = (TagCompound)unloadedList["data"]; //We look for the data entry list under list

											if (dataTag2.Get<string>("AltLibrary:WorldHallow") == "TheConfectionRebirth/ConfectionBiome")
											{ //same as the lines previously when altlib was enabled
												confectionorHallow = true;
												ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("Altlib save!, converting world!");
											}
											else
											{
												ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("non-Altlib save, unable to convert world");
											}
											break;
										}
									}
								}
								break;
							}
						}
					}
				}
				catch
				{
					ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("Could not get the world file, you are either joining a server or world directory is false!");
				}
			}
			else
			{
				ModContent.GetInstance<TheConfectionRebirth>().Logger.Debug("Could not get the world file, you are either joining a server or world directory is false!");
			}
		}

		public override void NetSend(BinaryWriter writer) 
		{
			writer.Write(confectionorHallow);
			writer.Write(tCandy);
			writer.Write(confectionTree);
		}

		public override void NetReceive(BinaryReader reader) 
		{
			confectionorHallow = reader.ReadBoolean();
			tCandy = reader.ReadByte();
			confectionTree = reader.ReadInt32();
		}

		public override void PreWorldGen() 
		{
			confectionorHallow = SelectedHallowOption switch
			{
				HallowOptions.Random => Main.rand.NextBool(),
				HallowOptions.Hallow => false,
				HallowOptions.Confection => true,
				_ => throw new ArgumentOutOfRangeException(),
			};
			confectionTree = Main.rand.Next(3);
		}

		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
		{
			ConfectionModCalling.UpdateFargoBoBW();
			if (ConfectionModCalling.FargoBoBW || Main.drunkWorld)
			{
				int index = tasks.FindIndex(genpass => genpass.Name.Equals("Dungeon"));
				if (index != -1)
				{
					tasks.Insert(index + 1, new PassLegacy("Confection Biome Chest", new WorldGenLegacyMethod(ConfectionChest)));
				}
			}
			else
			{
				if (confectionorHallow)
				{
					int index2 = tasks.FindIndex(genpass => genpass.Name.Equals("Dungeon"));
					if (index2 != -1)
					{
						tasks.Insert(index2 + 1, new PassLegacy("Confection Biome Chest", new WorldGenLegacyMethod(ConfectionChest)));
						tasks.Insert(index2 + 2, new PassLegacy("Hallow Chest removal", new WorldGenLegacyMethod(HallowChestRemoval)));
					}
				}
			}
		}

		public override void ModifyHardmodeTasks(List<GenPass> list)
		{
			ConfectionModCalling.UpdateFargoBoBW();
			if (confectionorHallow || ConfectionModCalling.FargoBoBW || Main.drunkWorld)
			{
				int index4 = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Good Remix"));
				if (index4 != -1)
				{
					list.Insert(index4 + 1, new PassLegacy("Hardmode Good Remix", new WorldGenLegacyMethod(ConfectionRemix))); //Usure if finished
					if (confectionorHallow && !(Main.drunkWorld || ConfectionModCalling.FargoBoBW))
						list.RemoveAt(index4);
				}
			}
			if (ConfectionModCalling.FargoBoBW || Main.drunkWorld)
			{
				int index2 = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Good"));
				if (index2 != -1)
				{
					list.Insert(index2 + 1, new PassLegacy("Hardmode Good", new WorldGenLegacyMethod(ConfectionDrunkInner)));
					list.RemoveAt(index2);
				}
				int index3 = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Evil"));
				if (index3 != -1)
				{
					list.Insert(index3 + 1, new PassLegacy("Hardmode Evil", new WorldGenLegacyMethod(ConfectionDrunkOuter)));
					list.RemoveAt(index3);
				}
			}
			int index = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Walls"));
			if (index != -1)
			{
				list.Insert(index + 1, new PassLegacy("Confection Walls", new WorldGenLegacyMethod(ConfectionWalls)));
			}
			if (confectionorHallow || ((Main.drunkWorld || ConfectionModCalling.FargoBoBW) && WorldGen.genRand.NextBool(2)))
			{
				int index5 = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Announcement"));
				if (index5 != -1)
				{
					list.Insert(index5 + 1, new PassLegacy("Hardmode Announcement", new WorldGenLegacyMethod(AnnounceConfection)));
					list.RemoveAt(index5);
				}
			}
		}

		#region BiomeChest
		private static void ConfectionChest(GenerationProgress progres, GameConfiguration configurations)
		{
			for (int num79 = 0; num79 < 1; num79++)
			{
				bool flag5 = false;
				while (!flag5)
				{
					int num80 = WorldGen.genRand.Next(GenVars.dMinX, GenVars.dMaxX);
					int num81 = WorldGen.genRand.Next((int)Main.worldSurface, GenVars.dMaxY);
					if (!Main.wallDungeon[Main.tile[num80, num81].WallType] || Main.tile[num80, num81].HasTile)
					{
						continue;
					}
					ushort chestTileType = (ushort)ModContent.TileType<Tiles.ConfectionBiomeChestTile>();
					int contain = 0;
					int style2 = 0;
					if (num79 == 0)
					{
						style2 = 1;
						contain = ModContent.ItemType<Items.Weapons.PopRocket>();
					}
					flag5 = WorldGen.AddBuriedChest(num80, num81, contain, notNearOtherChests: false, style2, trySlope: false, chestTileType);
				}
			}
		}

		private static void HallowChestRemoval(GenerationProgress progres, GameConfiguration configurations)
		{
			if (!Main.drunkWorld)
			{
				for (int index = 0; index < Main.maxChests; index++)
				{
					if (Main.chest[index] != null)
					{
						int X = Main.chest[index].x;
						int Y = Main.chest[index].y;
						if (Main.tile[X, Y].TileType == TileID.Containers && Main.wallDungeon[Main.tile[X, Y].WallType] && (Main.tile[X, Y].TileFrameX == 26 * (18 * 2) || Main.tile[X, Y].TileFrameX == 26.5 * (18 * 2)))
						{
							Chest.DestroyChestDirect(X, Y, index);
							WorldGen.KillTile(X, Y, false, false, true);
						}
					}
				}
			}
		}
		#endregion

		#region DrunkWorldgen
		public static void HardmodeCalulations(out double num, out double num2, out int num3, out int num4, out int num5, out int num6)
		{
			if (Main.rand == null)
			{
				Main.rand = new UnifiedRandom((int)DateTime.Now.Ticks);
			}
			num = (double)WorldGen.genRand.Next(300, 400) * 0.001;
			num2 = (double)WorldGen.genRand.Next(200, 300) * 0.001;
			num3 = (int)((double)(Main.maxTilesX) * num);
			num4 = (int)((double)(Main.maxTilesX) * (1.0 - num));
			num5 = 1;
			if (GenVars.dungeonX > Main.maxTilesX / 2)
			{
				num4 = (int)((double)(Main.maxTilesX) * num);
				num3 = (int)((double)(Main.maxTilesX) * (1.0 - num));
				num5 = -1;
			}
			num6 = 1;
			if (GenVars.dungeonX > Main.maxTilesX / 2)
			{
				num6 = -1;
			}
			if (num6 < 0)
			{
				if (num4 < num3)
				{
					num4 = (int)((double)(Main.maxTilesX) * num2);
				}
				else
				{
					num3 = (int)((double)(Main.maxTilesX) * num2);
				}
			}
			else if (num4 > num3)
			{
				num4 = (int)((double)(Main.maxTilesX) * (1.0 - num2));
			}
			else
			{
				num3 = (int)((double)(Main.maxTilesX) * (1.0 - num2));
			}
		}

		private static void ConfectionDrunkInner(GenerationProgress progres, GameConfiguration configurations)
		{
			HardmodeCalulations(out double num, out double num2, out int num3, out int num4, out int num5, out int num6);
			if (!Main.remixWorld)
			{
				WorldGen.GERunner(num3, 0, 3 * num5, 5.0);
				WorldGen.GERunner(num4, 0, 3 * -num5, 5.0, good: false);
			}
		}

		private static void ConfectionDrunkOuter(GenerationProgress progres, GameConfiguration configurations)
		{
			HardmodeCalulations(out double num, out double num2, out int num3, out int num4, out int num5, out int num6);
			if (!Main.remixWorld)
			{
				confectionorHallow = !confectionorHallow;
				WorldGen.GERunner(num4, 0, 3 * num5, 5.0);
				confectionorHallow = !confectionorHallow;

				WorldGen.crimson = !WorldGen.crimson;
				WorldGen.GERunner(num3, 0, 3 * -num5, 5.0, good: false);
				WorldGen.crimson = !WorldGen.crimson;
			}
		}
		#endregion

		#region ConfectionRemixWorldgen
		private static void ConfectionRemix(GenerationProgress progres, GameConfiguration configurations)
		{
			WorldGen.IsGeneratingHardMode = true;
			WorldGen.TryProtectingSpawnedItems();
			if (Main.rand == null)
			{
				Main.rand = new UnifiedRandom((int)DateTime.Now.Ticks);
			}
			bool flag = (Main.drunkWorld || ConfectionModCalling.FargoBoBW) ? GenVars.dungeonX > Main.maxTilesX / 2 : GenVars.dungeonX < Main.maxTilesX / 2;
			if (Main.remixWorld)
			{
				int num7 = Main.maxTilesX / 7;
				int num8 = Main.maxTilesX / 14;
				if (flag)
				{
					for (int i = Main.maxTilesX - num7 - num8; i < Main.maxTilesX; i++)
					{
						for (int j = (int)Main.worldSurface + WorldGen.genRand.Next(-1, 2); j < Main.maxTilesY - 10; j++)
						{
							if (i > Main.maxTilesX - num7)
							{
								ConfectionConvert(i, j, 1);
							}
							else if (TileID.Sets.Crimson[Main.tile[i, j].TileType] || TileID.Sets.Corrupt[Main.tile[i, j].TileType])
							{
								ConfectionConvert(i, j, 1);
							}
						}
					}
				}
				else
				{
					for (int k = 0; k < num7 + num8; k++)
					{
						for (int l = (int)Main.worldSurface + WorldGen.genRand.Next(-1, 2); l < Main.maxTilesY - 10; l++)
						{
							if (k < num7)
							{
								ConfectionConvert(k, l, 1);
							}
							else if (TileID.Sets.Crimson[Main.tile[k, l].TileType] || TileID.Sets.Corrupt[Main.tile[k, l].TileType])
							{
								ConfectionConvert(k, l, 1);
							}
						}
					}
				}
			}
			double num9 = (double)Main.maxTilesX / 4200.0;
			int num10 = (int)(25.0 * num9);
			ShapeData shapeData = new ShapeData();
			int num11 = 0;
			while (num10 > 0)
			{
				if (++num11 % 15000 == 0)
				{
					num10--;
				}
				Point point = WorldGen.RandomWorldPoint((int)Main.worldSurface - 100, 1, 190, 1);
				Tile tile = Main.tile[point.X, point.Y];
				Tile tile2 = Main.tile[point.X, point.Y - 1];
				ushort num12 = 0;
				if (TileID.Sets.Crimson[tile.TileType])
				{
					num12 = (ushort)(192 + WorldGen.genRand.Next(4));
				}
				else if (TileID.Sets.Corrupt[tile.TileType])
				{
					num12 = (ushort)(188 + WorldGen.genRand.Next(4));
				}
				else if (TileID.Sets.Hallow[tile.TileType])
				{
					num12 = (ushort)(200 + WorldGen.genRand.Next(4));
				}
				if (tile.HasTile && num12 != 0 && !tile2.HasTile)
				{
					bool flag2 = WorldUtils.Gen(new Point(point.X, point.Y - 1), new ShapeFloodFill(1000), Actions.Chain(new Modifiers.IsNotSolid(), new Modifiers.OnlyWalls(0, 54, 55, 56, 57, 58, 59, 61, 185, 212, 213, 214, 215, 2, 196, 197, 198, 199, 15, 40, 71, 64, 204, 205, 206, 207, 208, 209, 210, 211, 71), new Actions.Blank().Output(shapeData)));
					if (shapeData.Count > 50 && flag2)
					{
						WorldUtils.Gen(new Point(point.X, point.Y), new ModShapes.OuterOutline(shapeData, useDiagonals: true, useInterior: true), new Actions.PlaceWall(num12));
						num10--;
					}
					shapeData.Clear();
				}
			}
			if (Main.netMode == 2)
			{
				Netplay.ResetSections();
			}
			WorldGen.UndoSpawnedItemProtection();
			WorldGen.IsGeneratingHardMode = false;
		}
		#endregion

		#region CovertToConfectionWorldGen
		public static void ConfectGERunner(int i, int j, double speedX = 0.0, double speedY = 0.0, bool good = true)
		{
			GERunnerCalulations(out Vector2D val, out int num4, out int num2, out Vector2D val2, out bool flag, i, j, speedX, speedY);
			bool flag2 = true;
			while (flag2)
			{
				int num5 = (int)(val.X - num4 * 0.5);
				int num6 = (int)(val.X + num4 * 0.5);
				int num7 = (int)(val.Y - num4 * 0.5);
				int num8 = (int)(val.Y + num4 * 0.5);
				if (num5 < 0)
				{
					num5 = 0;
				}
				if (num6 > Main.maxTilesX)
				{
					num6 = Main.maxTilesX;
				}
				if (num7 < 0)
				{
					num7 = 0;
				}
				if (num8 > Main.maxTilesY - 5)
				{
					num8 = Main.maxTilesY - 5;
				}
				for (int m = num5; m < num6; m++)
				{
					for (int n = num7; n < num8; n++)
					{
						if (!(Math.Abs((double)m - val.X) + Math.Abs((double)n - val.Y) < (double)num2 * 0.5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015)))
						{
							continue;
						}
						if (good)
						{
							if (Main.tile[m, n].WallType == WallID.GrassUnsafe || Main.tile[m, n].WallType == WallID.FlowerUnsafe || Main.tile[m, n].WallType == WallID.Grass || Main.tile[m, n].WallType == WallID.Flower || Main.tile[m, n].WallType == WallID.CorruptGrassUnsafe || Main.tile[m, n].WallType == WallID.CrimsonGrassUnsafe)
							{
								Main.tile[m, n].WallType = (ushort)ModContent.WallType<CreamGrassWall>();
							}
							else if (Main.tile[m, n].WallType == WallID.HardenedSand)
							{
								Main.tile[m, n].WallType = (ushort)ModContent.WallType<HardenedCreamsandWall>();
							}
							else if (Main.tile[m, n].WallType == WallID.Sandstone)
							{
								Main.tile[m, n].WallType = (ushort)ModContent.WallType<CreamsandstoneWall>();
							}
							else if (Main.tile[m, n].WallType == WallID.EbonstoneUnsafe || Main.tile[m, n].WallType == WallID.CrimstoneUnsafe)
							{
								Main.tile[m, n].WallType = (ushort)ModContent.WallType<CreamstoneWall>();
							}
							else if (ModContent.GetInstance<ConfectionServerConfig>().CookieSpread != "No Spread")
							{
								if (Main.tile[m, n].WallType == WallID.Dirt || Main.tile[m, n].WallType == WallID.DirtUnsafe)
								{
									Main.tile[m, n].WallType = (ushort)ModContent.WallType<CookieWall>();
								}
								else if (Main.tile[m, n].WallType == WallID.Cave6Unsafe)
								{
									Main.tile[m, n].WallType = (ushort)ModContent.WallType<CookieStonedWall>();
								}
								else if (Main.tile[m, n].WallType == WallID.Cloud)
								{
									Main.tile[m, n].WallType = (ushort)ModContent.WallType<PinkFairyFlossWall>();
								}
								else if (Main.tile[m, n].WallType == WallID.IceUnsafe)
								{
									Main.tile[m, n].WallType = (ushort)ModContent.WallType<BlueIceWall>();
								}
								else if (Main.tile[m, n].WallType == WallID.SnowWallUnsafe)
								{
									Main.tile[m, n].WallType = (ushort)ModContent.WallType<CreamWall>();
								}
								else if (Main.tile[m, n].WallType == WallID.Cave4Unsafe)
								{
									Main.tile[m, n].WallType = (ushort)ModContent.WallType<BlueCreamyMossyWall>();
								}
								else if (Main.tile[m, n].WallType == WallID.Cave2Unsafe)
								{
									Main.tile[m, n].WallType = (ushort)ModContent.WallType<BrownCreamyMossyWall>();
								}
								else if (Main.tile[m, n].WallType == WallID.CaveUnsafe)
								{
									Main.tile[m, n].WallType = (ushort)ModContent.WallType<GreenCreamyMossyWall>();
								}
								else if (Main.tile[m, n].WallType == WallID.Cave5Unsafe)
								{
									Main.tile[m, n].WallType = (ushort)ModContent.WallType<PurpleCreamyMossyWall>();
								}
								else if (Main.tile[m, n].WallType == WallID.Cave3Unsafe)
								{
									Main.tile[m, n].WallType = (ushort)ModContent.WallType<RedCreamyMossyWall>();
								}
								else if (Main.tile[m, n].WallType == WallID.AmethystUnsafe)
								{
									Main.tile[m, n].WallType = (ushort)ModContent.WallType<CreamstoneAmethystWall>();
								}
								else if (Main.tile[m, n].WallType == WallID.TopazUnsafe)
								{
									Main.tile[m, n].WallType = (ushort)ModContent.WallType<CreamstoneTopazWall>();
								}
								else if (Main.tile[m, n].WallType == WallID.SapphireUnsafe)
								{
									Main.tile[m, n].WallType = (ushort)ModContent.WallType<CreamstoneSapphireWall>();
								}
								else if (Main.tile[m, n].WallType == WallID.EmeraldUnsafe)
								{
									Main.tile[m, n].WallType = (ushort)ModContent.WallType<CreamstoneEmeraldWall>();
								}
								else if (Main.tile[m, n].WallType == WallID.RubyUnsafe)
								{
									Main.tile[m, n].WallType = (ushort)ModContent.WallType<CreamstoneRubyWall>();
								}
								else if (Main.tile[m, n].WallType == WallID.DiamondUnsafe)
								{
									Main.tile[m, n].WallType = (ushort)ModContent.WallType<CreamstoneDiamondWall>();
								}
							}
							if (flag && Main.tile[m, n].TileType == TileID.Hive)
							{
								Main.tile[m, n].TileType = (ushort)ModContent.TileType<Creamstone>();
								WorldGen.SquareTileFrame(m, n);
							}
							else if (flag && Main.tile[m, n].TileType == TileID.CrispyHoneyBlock)
							{
								Main.tile[m, n].TileType = (ushort)ModContent.TileType<HardenedCreamsand>();
								WorldGen.SquareTileFrame(m, n);
							}
							else if (Main.tile[m, n].TileType == TileID.Grass)
							{
								Main.tile[m, n].TileType = (ushort)ModContent.TileType<CreamGrass>();
								WorldGen.SquareTileFrame(m, n);
							}
							else if (Main.tile[m, n].TileType == TileID.Stone || Main.tile[m, n].TileType == TileID.Ebonstone || Main.tile[m, n].TileType == TileID.Crimstone)
							{
								Main.tile[m, n].TileType = (ushort)ModContent.TileType<Creamstone>();
								WorldGen.SquareTileFrame(m, n);
							}
							else if (Main.tile[m, n].TileType == TileID.Sand || Main.tile[m, n].TileType == TileID.Silt || Main.tile[m, n].TileType == TileID.Ebonsand || Main.tile[m, n].TileType == TileID.Crimsand)
							{
								Main.tile[m, n].TileType = (ushort)ModContent.TileType<Creamsand>();
								WorldGen.SquareTileFrame(m, n);
							}
							else if (Main.tile[m, n].TileType == TileID.CorruptJungleGrass || Main.tile[m, n].TileType == TileID.CrimsonJungleGrass)
							{
								Main.tile[m, n].TileType = TileID.JungleGrass;
								WorldGen.SquareTileFrame(m, n);
							}
							else if (Main.tile[m, n].TileType == TileID.CorruptGrass || Main.tile[m, n].TileType == TileID.CrimsonGrass)
							{
								Main.tile[m, n].TileType = (ushort)ModContent.TileType<CreamGrass>();
								WorldGen.SquareTileFrame(m, n);
							}
							else if (Main.tile[m, n].TileType == TileID.IceBlock || Main.tile[m, n].TileType == TileID.CorruptIce || Main.tile[m, n].TileType == TileID.FleshIce)
							{
								Main.tile[m, n].TileType = (ushort)ModContent.TileType<BlueIce>();
								WorldGen.SquareTileFrame(m, n);
							}
							else if (Main.tile[m, n].TileType == TileID.Sandstone)
							{
								Main.tile[m, n].TileType = (ushort)ModContent.TileType<HardenedCreamsand>();
								WorldGen.SquareTileFrame(m, n);
							}
							else if (Main.tile[m, n].TileType == TileID.HardenedSand)
							{
								Main.tile[m, n].TileType = (ushort)ModContent.TileType<Creamsandstone>();
								WorldGen.SquareTileFrame(m, n);
							}
							else if (ConfectionIDs.Sets.ConvertsToConfection[Main.tile[m, n].TileType] >= 0)
							{
								Main.tile[m, n].TileType = (ushort)ConfectionIDs.Sets.ConvertsToConfection[Main.tile[m, n].TileType];
								WorldGen.SquareTileFrame(m, n);
							}
							else if (ModContent.GetInstance<ConfectionServerConfig>().CookieSpread != "No Spread")
							{
								if (Main.tile[m, n].TileType == TileID.Dirt)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<CookieBlock>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.DirtiestBlock)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<CookiestCookieBlock>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.SnowBlock)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<CreamBlock>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.Cloud || Main.tile[m, n].TileType == TileID.LesionBlock || Main.tile[m, n].TileType == TileID.FleshBlock)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<PinkFairyFloss>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.RainCloud)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<PurpleFairyFloss>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.SnowCloud)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<BlueFairyFloss>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.Amethyst)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<CreamstoneAmethyst>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.Topaz)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<CreamstoneTopaz>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.Sapphire)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<CreamstoneSaphire>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.Emerald)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<CreamstoneEmerald>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.Ruby)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<CreamstoneRuby>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.Diamond)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<CreamstoneDiamond>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.GreenMoss)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<CreamstoneMossGreen>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.BrownMoss)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<CreamstoneMossBrown>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.RedMoss)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<CreamstoneMossRed>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.BlueMoss)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<CreamstoneMossBlue>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.PurpleMoss)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<CreamstoneMossPurple>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.LavaMoss)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<CreamstoneMossLava>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.KryptonMoss)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<CreamstoneMossKrypton>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.XenonMoss)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<CreamstoneMossXenon>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.ArgonMoss)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<CreamstoneMossArgon>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.VioletMoss)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<CreamstoneMossNeon>();
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == TileID.RainbowMoss)
								{
									Main.tile[m, n].TileType = (ushort)ModContent.TileType<CreamstoneMossHelium>();
									WorldGen.SquareTileFrame(m, n);
								}
							}
						}
					}
				}
				val += val2;
				val2.X += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
				if (val2.X > speedX + 1.0)
				{
					val2.X = speedX + 1.0;
				}
				if (val2.X < speedX - 1.0)
				{
					val2.X = speedX - 1.0;
				}
				if (val.X < (double)(-num2) || val.Y < (double)(-num2) || val.X > (double)(Main.maxTilesX + num2) || val.Y > (double)(Main.maxTilesY + num2))
				{
					flag2 = false;
				}
			}
		}
		#endregion

		#region GERunner Edits and other
		private void GERunnerEditer(On_WorldGen.orig_GERunner orig, int i, int j, double speedX, double speedY, bool good)
		{
			if (good && confectionorHallow)
			{
				ConfectGERunner(i, j, speedX, speedY, good);
				return;
			}
			else
			{
				GERunnerEdits(i, j, speedX, speedY, good);
			}
			orig.Invoke(i, j, speedX, speedY, good);
		}

		private static void GERunnerCalulations(out Vector2D val, out int num4, out int num2, out Vector2D val2, out bool flag, int i, int j, double speedX = 0.0, double speedY = 0.0)
		{
			int num = 0;
			for (int k = 20; k < Main.maxTilesX - 20; k++)
			{
				for (int l = 20; l < Main.maxTilesY - 20; l++)
				{
					if (Main.tile[k, l].HasTile && Main.tile[k, l].TileType == TileID.Hive)
					{
						num++;
					}
				}
			}
			flag = false;
			if (num > 200000)
			{
				flag = true;
			}
			num2 = WorldGen.genRand.Next(200, 250);
			double num3 = (double)Main.maxTilesX / 4200.0;
			num2 = (int)((double)num2 * num3);
			num4 = num2;
			val = default(Vector2D);
			val.X = i;
			val.Y = j;
			val2 = default(Vector2D);
			val2.X = (double)WorldGen.genRand.Next(-10, 11) * 0.1;
			val2.Y = (double)WorldGen.genRand.Next(-10, 11) * 0.1;
			if (speedX != 0.0 || speedY != 0.0)
			{
				val2.X = speedX;
				val2.Y = speedY;
			}
		}

		public static void GERunnerEdits(int i, int j, double speedX = 0.0, double speedY = 0.0, bool good = true)
		{
			GERunnerCalulations(out Vector2D val, out int num4, out int num2, out Vector2D val2, out bool flag, i, j, speedX, speedY);
			bool flag2 = true;
			while (flag2)
			{
				int num5 = (int)(val.X - num4 * 0.5);
				int num6 = (int)(val.X + num4 * 0.5);
				int num7 = (int)(val.Y - num4 * 0.5);
				int num8 = (int)(val.Y + num4 * 0.5);
				if (num5 < 0)
				{
					num5 = 0;
				}
				if (num6 > Main.maxTilesX)
				{
					num6 = Main.maxTilesX;
				}
				if (num7 < 0)
				{
					num7 = 0;
				}
				if (num8 > Main.maxTilesY - 5)
				{
					num8 = Main.maxTilesY - 5;
				}
				for (int m = num5; m < num6; m++)
				{
					for (int n = num7; n < num8; n++)
					{
						if (!(Math.Abs((double)m - val.X) + Math.Abs((double)n - val.Y) < (double)num2 * 0.5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015)))
						{
							continue;
						}
						if (good)
						{
							if (Main.tile[m, n].TileType == ModContent.TileType<CreamGrass>())
							{
								Main.tile[m, n].TileType = TileID.HallowedGrass;
								WorldGen.SquareTileFrame(m, n);
							}
							else if (Main.tile[m, n].TileType == ModContent.TileType<Creamstone>())
							{
								Main.tile[m, n].TileType = TileID.Pearlstone;
								WorldGen.SquareTileFrame(m, n);
							}
							else if (Main.tile[m, n].TileType == ModContent.TileType<Creamsand>())
							{
								Main.tile[m, n].TileType = TileID.Pearlsand;
								WorldGen.SquareTileFrame(m, n);
							}
							else if (Main.tile[m, n].TileType == ModContent.TileType<BlueIce>())
							{
								Main.tile[m, n].TileType = TileID.HallowedIce;
								WorldGen.SquareTileFrame(m, n);
							}
						}
						else if (WorldGen.crimson)
						{
							if (Main.tile[m, n].TileType == ModContent.TileType<CreamGrass>())
							{
								Main.tile[m, n].TileType = TileID.CrimsonGrass;
								WorldGen.SquareTileFrame(m, n);
							}
							else if (Main.tile[m, n].TileType == ModContent.TileType<Creamstone>())
							{
								Main.tile[m, n].TileType = TileID.Crimstone;
								WorldGen.SquareTileFrame(m, n);
							}
							else if (Main.tile[m, n].TileType == ModContent.TileType<Creamsand>())
							{
								Main.tile[m, n].TileType = TileID.Crimsand;
								WorldGen.SquareTileFrame(m, n);
							}
							else if (Main.tile[m, n].TileType == ModContent.TileType<BlueIce>())
							{
								Main.tile[m, n].TileType = TileID.FleshIce;
								WorldGen.SquareTileFrame(m, n);
							}
						}
						else
						{
							if (Main.tile[m, n].TileType == ModContent.TileType<CreamGrass>())
							{
								Main.tile[m, n].TileType = TileID.CorruptGrass;
								WorldGen.SquareTileFrame(m, n);
							}
							else if (Main.tile[m, n].TileType == ModContent.TileType<Creamstone>())
							{
								Main.tile[m, n].TileType = TileID.Ebonstone;
								WorldGen.SquareTileFrame(m, n);
							}
							else if (Main.tile[m, n].TileType == ModContent.TileType<Creamsand>())
							{
								Main.tile[m, n].TileType = TileID.Ebonsand;
								WorldGen.SquareTileFrame(m, n);
							}
							else if (Main.tile[m, n].TileType == ModContent.TileType<BlueIce>())
							{
								Main.tile[m, n].TileType = TileID.CorruptIce;
								WorldGen.SquareTileFrame(m, n);
							}
						}
					}
				}
				val += val2;
				val2.X += (double)WorldGen.genRand.Next(-10, 11) * 0.05;
				if (val2.X > speedX + 1.0)
				{
					val2.X = speedX + 1.0;
				}
				if (val2.X < speedX - 1.0)
				{
					val2.X = speedX - 1.0;
				}
				if (val.X < (double)(-num2) || val.Y < (double)(-num2) || val.X > (double)(Main.maxTilesX + num2) || val.Y > (double)(Main.maxTilesY + num2))
				{
					flag2 = false;
				}
			}
		}
		#endregion

		#region Confection Walls
		private static void ConfectionWalls(GenerationProgress progres, GameConfiguration configurations)
		{
			double num13 = (double)Main.maxTilesX / 4200.0;
			int num10 = (int)(25.0 * num13);
			ShapeData shapeData = new ShapeData();
			int num11 = 0;
			while (num10 > 0)
			{
				if (++num11 % 15000 == 0)
				{
					num10--;
				}
				Point point = WorldGen.RandomWorldPoint((int)Main.worldSurface - 100, 1, 190, 1);
				Tile tile = Main.tile[point.X, point.Y];
				Tile tile2 = Main.tile[point.X, point.Y - 1];
				ushort num12 = 0;
				if (ConfectionIDs.Sets.Confection[tile.TileType])
				{
					int randNum = WorldGen.genRand.Next(4);
					if (randNum == 0)
					{
						num12 = (ushort)(ModContent.WallType<Creamstone2Wall>());
					}
					else if (randNum == 1)
					{ 
						num12 = (ushort)(ModContent.WallType<Creamstone3Wall>()); 
					}
					else if (randNum == 2)
					{ 
						num12 = (ushort)(ModContent.WallType<Creamstone4Wall>()); 
					}
					else
					{
						num12 = (ushort)(ModContent.WallType<Creamstone5Wall>());
					}
				}
				if (tile.HasTile && num12 != 0 && !tile2.HasTile)
				{
					bool flag = WorldUtils.Gen(new Point(point.X, point.Y - 1), new ShapeFloodFill(1000), Actions.Chain(new Modifiers.IsNotSolid(), new Modifiers.OnlyWalls(0, 54, 55, 56, 57, 58, 59, 61, 185, 212, 213, 214, 215, 2, 196, 197, 198, 199, 15, 40, 71, 64, 204, 205, 206, 207, 208, 209, 210, 211, 71), new Actions.Blank().Output(shapeData)));
					if (shapeData.Count > 50 && flag)
					{
						WorldUtils.Gen(new Point(point.X, point.Y), new ModShapes.OuterOutline(shapeData, useDiagonals: true, useInterior: true), new Actions.PlaceWall(num12));
						num10--;
					}
					shapeData.Clear();
				}
			}
		}
		#endregion

		#region Confection Hardmode Announcement
		private static void AnnounceConfection(GenerationProgress progres, GameConfiguration configurations)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				Main.NewText(Language.GetTextValue("Mods.TheConfectionRebirth.HardmodeGeneration.Confection"), 50, 255, 130);
			}
			else if (Main.netMode == NetmodeID.Server)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Language.GetTextValue("Mods.TheConfectionRebirth.HardmodeGeneration.Confection")), new Color(50, 255, 130));
			}
			AchievementsHelper.NotifyProgressionEvent(9);
		}
		#endregion

		#region ConfectionSkyIslands
		private void On_WorldGen_ConvertSkyIslands(On_WorldGen.orig_ConvertSkyIslands orig, int convertType, bool growTrees)
		{
			if (confectionorHallow)
			{
				int num = 0;
				for (int i = 20; (double)i < Main.worldSurface; i++)
				{
					for (int j = 20; j < Main.maxTilesX - 20; j++)
					{
						Tile tile = Main.tile[j, i];
						if (tile.HasTile && TileID.Sets.Clouds[tile.TileType])
						{
							num = i;
							break;
						}
					}
				}
				for (int k = 20; k <= Main.maxTilesX - 20; k++)
				{
					for (int l = 20; l < num; l++)
					{
						Tile tile2 = Main.tile[k, l];
						Tile tile3 = Main.tile[k, l - 1];
						if (tile2.HasTile && (tile2.TileType == 2 || tile2.TileType == 0 || tile2.TileType == TileID.Cloud || tile2.TileType == TileID.RainCloud || tile2.TileType == TileID.SnowCloud))
						{
							if (tile3.TileType == 596 || tile3.TileType == 616)
							{
								WorldGen.KillTile(k, l - 1);
							}
							ConfectionConvert(k, l, 1);
							ushort type = tile3.TileType;
							if ((uint)(type - 82) <= 1u || (uint)(type - 185) <= 2u || type == 227)
							{
								WorldGen.KillTile(k, l - 1);
							}
							if (growTrees && WorldGen._genRand.NextBool(3))
							{
								WorldGen.GrowTree(k, l);
							}
						}
					}
				}
				for (int n = 0; n < Main.maxTilesX; n++)
				{
					for (int m = 0; m < Main.maxTilesY; m++)
					{
						if (Main.tile[n, m].HasTile && Main.tile[n, m].TileType == TileID.WaterFountain)
						{
							Tile tile = Main.tile[n, m];
							tile.HasTile = false;
						}
					}
				}
			}
			else
			{
				orig.Invoke(convertType, growTrees);
			}
		}
		#endregion

		#region Creamstone Moss Rendering
		public override void PostDrawTiles()
		{
			Main.spriteBatch.Begin(0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			int scrMinX = (int)Main.screenPosition.X / 16;
			int scrMinY = (int)Main.screenPosition.Y / 16;
			int scrWid = Main.screenWidth / 16;
			int scrHei = Main.screenHeight / 16;
			int scrMaxX = scrMinX + scrWid;
			int scrMaxY = scrMinY + scrHei;
			for (int i = scrMinX; i < scrMaxX; i++)
			{
				for (int j = scrMinY; j < scrMaxY; j++)
				{
					if (i > 0 && i < Main.maxTilesX && j > 0 && j < Main.maxTilesY)
					{
						Tile tile = Main.tile[i, j];
						if (tile.HasTile && ConfectionIDs.Sets.IsTileCreamMoss[tile.TileType] != null)
						{
							Vector2 tileFrameNumber = new Vector2(tile.TileFrameX / 18, tile.TileFrameY / 18);
							if (tileFrameNumber.X >= 14 && tileFrameNumber.X < 16 && tileFrameNumber.Y >= 5 && tileFrameNumber.Y < 9)
							{
								continue;
							}
							Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
							Vector2 mossPos = new Vector2((i * 16) - 4, (j * 16) - 4) - unscaledPosition;
							Vector2 frameNumber = new Vector2(0, 0);
							Rectangle frame;
							Vector2 origin = Vector2.Zero;
							Color color2 = (Color)ConfectionIDs.Sets.IsTileCreamMoss[tile.TileType];
							Color color = Lighting.GetColorClamped(i, j, color2);
							if (tile.IsTileFullbright || color2.A == 0)
							{
								color = color2;
								color.A = 255;
							}
							if (tile.TileType == ModContent.TileType<CreamstoneMossHelium>())
							{
								color = Main.DiscoColor;
							}
							if (tile.IsActuated)
							{
								color = ActColor(color, tile);
							}
							if (tile.IsHalfBlock)
							{
								if (tileFrameNumber.X >= 9 && tileFrameNumber.X < 12 && tileFrameNumber.Y == 3)
								{
									frameNumber = new Vector2(15, 8);
								}
								else if ((tileFrameNumber.Y == 3 && (tileFrameNumber.X == 0 || tileFrameNumber.X == 2 || tileFrameNumber.X == 4)) || (tileFrameNumber.X == 9 && tileFrameNumber.Y >= 0 && tileFrameNumber.Y < 3))
								{
									frameNumber = new Vector2(14, 7);
								}
								else if ((tileFrameNumber.Y == 3 && (tileFrameNumber.X == 1 || tileFrameNumber.X == 3 || tileFrameNumber.X == 5)) || (tileFrameNumber.X == 12 && tileFrameNumber.Y >= 0 && tileFrameNumber.Y < 3))
								{
									frameNumber = new Vector2(15, 7);
								}
								else
								{
									frameNumber = new Vector2(14, 8);
								}
							}
							else if (tile.Slope != SlopeType.Solid)
							{
								switch (tile.Slope)
								{
									case SlopeType.SlopeDownLeft:
										frameNumber = new Vector2(14, 5);
										break;
									case SlopeType.SlopeDownRight:
										frameNumber = new Vector2(15, 5);
										break;
									case SlopeType.SlopeUpLeft:
										frameNumber = new Vector2(14, 6);
										break;
									case SlopeType.SlopeUpRight:
										frameNumber = new Vector2(15, 6);
										break;
								}
							}
							else
							{
								frameNumber = tileFrameNumber;
							}
							frame = new Rectangle((int)(frameNumber.X * 26), (int)(frameNumber.Y * 34), 26, 34);
							Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/CreamstoneMoss"), mossPos, frame, color, 0f, origin, 1f, (SpriteEffects)0, 1f);
						}
					}
				}
			}
			Main.spriteBatch.End();
		}
		#endregion

		#region Actuation Color
		//For some reason tml privates Tile.actColor and due to the size is much easier to copy the code and put it into a public static method
		public static Color ActColor(Color oldColor, Tile tile)
		{
			if (!tile.IsActuated)
			{
				return oldColor;
			}
			double num = 0.4;
			return new Color((int)(byte)(num * (double)(int)oldColor.R), (int)(byte)(num * (double)(int)oldColor.G), (int)(byte)(num * (double)(int)oldColor.B), (int)oldColor.A);
		}
		#endregion

		#region Conversion - Confection
		public static void ConfectionConvert(int i, int j, int size = 4) {
			for (int k = i - size; k <= i + size; k++) {
				for (int l = j - size; l <= j + size; l++) {
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size)) {
						int type = Main.tile[k, l].TileType;
						int wall = Main.tile[k, l].WallType;

						#region Walls
						if (WallID.Sets.Conversion.Stone[wall] && wall != ModContent.WallType<CreamstoneWall>()) {
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
						else if (WallID.Sets.Conversion.NewWall1[wall] && wall != ModContent.WallType<Creamstone2Wall>())
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<Creamstone2Wall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}
						else if (WallID.Sets.Conversion.NewWall2[wall] && wall != ModContent.WallType<Creamstone3Wall>())
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<Creamstone3Wall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}
						else if (WallID.Sets.Conversion.NewWall3[wall] && wall != ModContent.WallType<Creamstone4Wall>())
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<Creamstone4Wall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}
						else if (WallID.Sets.Conversion.NewWall4[wall] && wall != ModContent.WallType<Creamstone5Wall>())
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<Creamstone5Wall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}
						else if (wall == WallID.Cloud)
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<PinkFairyFlossWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}
						else if (wall == WallID.Cave6Unsafe || wall == WallID.Cave6Echo) {
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<CookieStonedWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
							break;
						}
						else if (wall == WallID.Cave4Unsafe || wall == WallID.Cave4Echo)
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<BlueCreamyMossyWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == WallID.Cave2Unsafe || wall == WallID.Cave2Echo)
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<BrownCreamyMossyWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == WallID.CaveUnsafe || wall == WallID.Cave1Echo)
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<GreenCreamyMossyWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == WallID.Cave5Unsafe || wall == WallID.Cave5Echo)
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<PurpleCreamyMossyWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == WallID.Cave3Unsafe || wall == WallID.Cave3Echo)
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<RedCreamyMossyWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == WallID.AmethystUnsafe || wall == WallID.AmethystEcho)
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<CreamstoneAmethystWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == WallID.TopazUnsafe || wall == WallID.TopazEcho)
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<CreamstoneTopazWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == WallID.SapphireUnsafe || wall == WallID.SapphireEcho)
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<CreamstoneSapphireWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == WallID.EmeraldUnsafe || wall == WallID.EmeraldEcho)
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<CreamstoneEmeraldWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == WallID.RubyUnsafe || wall == WallID.RubyEcho)
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<CreamstoneRubyWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (wall == WallID.DiamondUnsafe || wall == WallID.DiamondEcho)
						{
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<CreamstoneDiamondWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
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
						else if (Main.tile[k, l].TileType == TileID.Ruby) {
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
						else if (Main.tile[k, l].TileType == TileID.Cloud) {
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
						else if (Main.tile[k, l].TileType == TileID.GreenMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneMossGreen>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.BrownMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneMossBrown>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.RedMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneMossRed>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.BlueMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneMossBlue>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.PurpleMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneMossPurple>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.LavaMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneMossLava>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.KryptonMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneMossKrypton>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.XenonMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneMossXenon>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.ArgonMoss) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneMossArgon>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.VioletMoss)
						{
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneMossNeon>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (Main.tile[k, l].TileType == TileID.RainbowMoss)
						{
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<CreamstoneMossHelium>();
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}

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
		#endregion

		#region SpreadingDetours
		private void On_WorldGen_SpreadDesertWalls(On_WorldGen.orig_SpreadDesertWalls orig, int wallDist, int i, int j)
		{
			orig.Invoke(wallDist, i, j);
			if (WorldGen.InWorld(i, j, 10) || (WallID.Sets.Conversion.Sandstone[Main.tile[i, j].WallType] && (Main.tile[i, j].HasTile || TileID.Sets.Conversion.Sandstone[Main.tile[i, j].TileType]) && WallID.Sets.Conversion.HardenedSand[Main.tile[i, j].WallType]))
			{
				bool num = false;
				int wall = Main.tile[i, j].WallType;
				int type = Main.tile[i, j].TileType;
				if (ConfectionIDs.Sets.IsNaturalConfectionWall[wall] || (ConfectionIDs.Sets.IsExtraConfectionWall[wall] && ModContent.GetInstance<ConfectionServerConfig>().CookieSpread == "Full Spread") || ConfectionIDs.Sets.IsNaturalConfectionTile[type] || (ConfectionIDs.Sets.IsExtraConfectionTile[type] && ModContent.GetInstance<ConfectionServerConfig>().CookieSpread == "Full Spread"))
				{
					num = true;
				}
				if (num == false)
				{
					return;
				}
				int num2 = i + WorldGen.genRand.Next(-2, 3);
				int num3 = j + WorldGen.genRand.Next(-2, 3);
				bool flag = false;
				if (WallID.Sets.Conversion.PureSand[Main.tile[num2, num3].WallType])
				{
					if (num == true)
					{
						for (int num4 = i - wallDist; num4 < i + wallDist; num4++)
						{
							for (int num5 = j - wallDist; num5 < j + wallDist; num5++)
							{
								int type2 = Main.tile[num4, num5].TileType;
								if (Main.tile[num4, num5].HasTile && (ConfectionIDs.Sets.IsNaturalConfectionTile[type2] || (ConfectionIDs.Sets.IsExtraConfectionTile[type2] && ModContent.GetInstance<ConfectionServerConfig>().CookieSpread == "Full Spread")))
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								break;
							}
						}
					}
				}
				if (!flag)
				{
					return;
				}
				ushort? num6 = null;
				if (WallID.Sets.Conversion.Sandstone[Main.tile[num2, num3].WallType])
				{
					if (num == true)
					{
						num6 = (ushort)ModContent.WallType<HardenedCreamsandWall>();
					}
				}
				if (WallID.Sets.Conversion.HardenedSand[Main.tile[num2, num3].WallType])
				{
					if (num == true)
					{
						num6 = (ushort)ModContent.WallType<CreamsandstoneWall>();
					}
				}
				if (num6.HasValue && Main.tile[num2, num3].WallType != num6.Value)
				{
					Main.tile[num2, num3].WallType = num6.Value;
					if (Main.netMode == 2)
					{
						NetMessage.SendTileSquare(-1, num2, num3);
					}
				}
			}
		}

		private void On_WorldGen_UpdateWorld_OvergroundTile(On_WorldGen.orig_UpdateWorld_OvergroundTile orig, int i, int j, bool checkNPCSpawns, int wallDist)
		{
			orig.Invoke(i, j, checkNPCSpawns, wallDist);
			WallSpread(i, j, wallDist);
		}

		private void On_WorldGen_UpdateWorld_UndergroundTile(On_WorldGen.orig_UpdateWorld_UndergroundTile orig, int i, int j, bool checkNPCSpawns, int wallDist)
		{
			orig.Invoke(i, j, checkNPCSpawns, wallDist);
			WallSpread(i, j, wallDist);
		}

		private void WallSpread(int i, int j, int wallDist)
		{
			if (WorldGen.AllowedToSpreadInfections)
			{
				if (Main.tile[i, j].WallType == ModContent.WallType<CreamGrassWall>() || (Main.tile[i, j].TileType == ModContent.TileType<CreamGrass>() && Main.tile[i, j].HasTile))
				{
					int num30 = i + WorldGen.genRand.Next(-2, 3);
					int num31 = j + WorldGen.genRand.Next(-2, 3);
					if ((WorldGen.InWorld(num30, num31, 10) && Main.tile[num30, num31].WallType == 63) || Main.tile[num30, num31].WallType == 65 || Main.tile[num30, num31].WallType == 66 || Main.tile[num30, num31].WallType == 68)
					{
						bool flag4 = false;
						for (int num32 = i - wallDist; num32 < i + wallDist; num32++)
						{
							for (int num33 = j - wallDist; num33 < j + wallDist; num33++)
							{
								if (Main.tile[num32, num33].HasTile)
								{
									int type6 = Main.tile[num32, num33].TileType;
									if (type6 == ModContent.TileType<CreamGrass>() || type6 == ModContent.TileType<CreamGrass_Foliage>() || type6 == ModContent.TileType<CreamVines>() || type6 == ModContent.TileType<Creamsand>() || type6 == ModContent.TileType<Creamstone>() || type6 == ModContent.TileType<BlueIce>() || type6 == ModContent.TileType<HardenedCreamsand>() || type6 == ModContent.TileType<Creamsandstone>() || type6 == ModContent.TileType<CreamGrassMowed>())
									{
										flag4 = true;
										break;
									}
								}
							}
						}
						if (flag4)
						{
							Main.tile[num30, num31].WallType = (ushort)ModContent.WallType<CreamGrassWall>();
							if (Main.netMode == 2)
							{
								NetMessage.SendTileSquare(-1, num30, num31);
							}
						}
					}
				}
				if ((Main.tile[i, j].WallType == ModContent.WallType<CookieWall>() || (Main.tile[i, j].TileType == ModContent.TileType<CookieBlock>() && Main.tile[i, j].HasTile)) && ModContent.GetInstance<ConfectionServerConfig>().CookieSpread == "Full Spread")
				{
					int num30 = i + WorldGen.genRand.Next(-2, 3);
					int num31 = j + WorldGen.genRand.Next(-2, 3);
					if (Main.tile[num30, num31].WallType == 2 || Main.tile[num30, num31].WallType == 16)
					{
						bool flag4 = false;
						for (int num32 = i - wallDist; num32 < i + wallDist; num32++)
						{
							for (int num33 = j - wallDist; num33 < j + wallDist; num33++)
							{
								if (Main.tile[num32, num33].HasTile)
								{
									int type6 = Main.tile[num32, num33].TileType;
									if (type6 == ModContent.TileType<CreamGrass>() || type6 == ModContent.TileType<CreamGrass_Foliage>() || type6 == ModContent.TileType<CreamVines>() || type6 == ModContent.TileType<Creamsand>() || type6 == ModContent.TileType<Creamstone>() || type6 == ModContent.TileType<BlueIce>() || type6 == ModContent.TileType<HardenedCreamsand>() || type6 == ModContent.TileType<Creamsandstone>() || type6 == ModContent.TileType<CreamGrassMowed>())
									{
										flag4 = true;
										break;
									}
								}
							}
						}
						if (flag4)
						{
							Main.tile[num30, num31].WallType = (ushort)ModContent.WallType<CookieWall>();
							if (Main.netMode == 2)
							{
								NetMessage.SendTileSquare(-1, num30, num31);
							}
						}
					}
				}
			}
		}

		private void On_WorldGen_hardUpdateWorld(On_WorldGen.orig_hardUpdateWorld orig, int i, int j)
		{
			orig.Invoke(i, j);
			if (Main.hardMode && !Main.tile[i, j].IsActuated)
			{
				int type = Main.tile[i, j].TileType;
				if ((NPC.downedPlantBoss && WorldGen.genRand.NextBool(2)) || WorldGen.AllowedToSpreadInfections)
				{
					if (ConfectionIDs.Sets.IsNaturalConfectionTile[type] || (ConfectionIDs.Sets.IsExtraConfectionTile[type] && ModContent.GetInstance<ConfectionServerConfig>().CookieSpread == "Full Spread"))
					{
						bool flag4 = true;
						while (flag4)
						{
							flag4 = false;
							int num15 = i + WorldGen.genRand.Next(-3, 4);
							int num16 = j + WorldGen.genRand.Next(-3, 4);
							if (!WorldGen.InWorld(num15, num16, 10) || WorldGen.CountNearBlocksTypes(num15, num16, 2, 1, 27) > 0)
							{
								continue;
							}
							if (Main.tile[num15, num16].TileType == 2)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag4 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamGrass>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == 477)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag4 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamGrassMowed>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == 1 || Main.tileMoss[Main.tile[num15, num16].TileType])
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag4 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<Creamstone>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == 53)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag4 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<Creamsand>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == 396)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag4 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<Creamsandstone>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == 397)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag4 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<HardenedCreamsand>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == 161)
							{
								if (WorldGen.genRand.NextBool(2))
								{
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

			if (Main.hardMode && !Main.tile[i, j].IsActuated)
			{
				int type = Main.tile[i, j].TileType;
				if ((NPC.downedPlantBoss && WorldGen.genRand.NextBool(2)) || WorldGen.AllowedToSpreadInfections)
				{
					if ((ConfectionIDs.Sets.IsNaturalConfectionTile[type] || (ConfectionIDs.Sets.IsExtraConfectionTile[type]) && ModContent.GetInstance<ConfectionServerConfig>().CookieSpread == "Full Spread") && ModContent.GetInstance<ConfectionServerConfig>().CookieSpread != "No Spread")
					{
						bool flag40 = true;
						while (flag40)
						{
							flag40 = false;
							int num15 = i + WorldGen.genRand.Next(-3, 4);
							int num16 = j + WorldGen.genRand.Next(-3, 4);
							if (!WorldGen.InWorld(num15, num16, 10) || WorldGen.CountNearBlocksTypes(num15, num16, 2, 1, 27) > 0)
							{
								continue;
							}
							if (Main.tile[num15, num16].TileType == TileID.Dirt)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CookieBlock>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.SnowBlock)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamBlock>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.Cloud)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<PinkFairyFloss>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.RainCloud)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<PurpleFairyFloss>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.SnowCloud)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<BlueFairyFloss>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							if (Main.tile[num15, num16].TileType == TileID.Amethyst)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneAmethyst>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.Topaz)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneTopaz>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.Sapphire)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneSaphire>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.Emerald)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneEmerald>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.Ruby)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneRuby>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.Diamond)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneDiamond>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.DirtiestBlock)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CookiestCookieBlock>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.GreenMoss)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneMossGreen>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.BrownMoss)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneMossBrown>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.RedMoss)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneMossRed>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.BlueMoss)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneMossBlue>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.PurpleMoss)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneMossPurple>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.LavaMoss)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneMossLava>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.KryptonMoss)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneMossKrypton>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.XenonMoss)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneMossXenon>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.ArgonMoss)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneMossArgon>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.VioletMoss)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneMossNeon>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
							else if (Main.tile[num15, num16].TileType == TileID.RainbowMoss)
							{
								if (WorldGen.genRand.NextBool(2))
								{
									flag40 = true;
								}
								Main.tile[num15, num16].TileType = (ushort)ModContent.TileType<CreamstoneMossHelium>();
								WorldGen.SquareTileFrame(num15, num16);
								NetMessage.SendTileSquare(-1, num15, num16);
							}
						}
					}
				}
			}
		}
		#endregion

		#region Reflections and other System type methods for tiles
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
		#endregion
	}
}