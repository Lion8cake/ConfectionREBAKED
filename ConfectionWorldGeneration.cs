using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;

namespace TheConfectionRebirth {
	public class ConfectionWorldGeneration : ModSystem {

		//Stalac Checks, unfinished
		//TODO: convert mormal stalacs to these
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
