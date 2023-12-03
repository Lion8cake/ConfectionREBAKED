using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles
{
    public class CreamGrass : ModTile
    {
        public override void SetStaticDefaults()
        {
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamGrass").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CookieBlock").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("Creamstone").Type] = true;
            TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamGrassMowed").Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileBlockLight[Type] = true;
			TileID.Sets.Grass[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			TileID.Sets.ForcedDirtMerging[Type] = true;
			TileID.Sets.Conversion.MergesWithDirtInASpecialWay[Type] = true;
			TileID.Sets.Conversion.Grass[Type] = true;
			AddMapEntry(new Color(235, 207, 150));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.CookieBlock>());
		}

		public override void RandomUpdate(int i, int j) {
			int minI = i - 1;
			int maxI = i + 2;
			int minJ = j - 1;
			int maxJ = j + 2;

			if (!WorldGen.InWorld(i, j, 10)) {
				return;
			}
			if (j > Main.rockLayer) {
				int type = Main.tile[i, j].TileType;
				int num12 = -1;
				int num19 = type;
				int num20 = -1;
				int num = ModContent.TileType<CookieBlock>();
				int num18 = ModContent.TileType<CreamGrass_Foliage>();
				if (WorldGen.genRand.Next(12) == 0) {
					num18 = ModContent.TileType<YumDrop>();
				}
				int maxValue = 2;
				bool flag = false;
				if (num18 != -1 && !Main.tile[i, minJ].HasTile && WorldGen.genRand.Next(maxValue) == 0 && Main.tile[i, minJ].LiquidType < LiquidID.Water) {
					flag = true;
					if (WorldGen.PlaceTile(i, minJ, num18, mute: true)) {
						Main.tile[i, minJ].CopyPaintAndCoating(Main.tile[i, j]);
					}
					if (Main.netMode == 2 && Main.tile[i, minJ].HasTile) {
						NetMessage.SendTileSquare(-1, i, minJ);
					}
				}
				if (num != -1) {
					bool flag3 = false;
					TileColorCache color = Main.tile[i, j].BlockColorAndCoating();
					for (int k = minI; k < maxI; k++) {
						for (int l = minJ; l < maxJ; l++) {
							if (!WorldGen.InWorld(k, l, 10) || (i == k && j == l) || !Main.tile[k, l].HasTile) {
								continue;
							}
							if (Main.tile[k, l].TileType == num) {
								WorldGen.SpreadGrass(k, l, num, num19, repeat: false, color);
								if (Main.tile[k, l].TileType == num19) {
									WorldGen.SquareTileFrame(k, l);
									flag3 = true;
								}
							}
							else if (num12 > -1 && num20 > -1 && Main.tile[k, l].TileType == num12) {
								WorldGen.SpreadGrass(k, l, num12, num20, repeat: false, color);
								if (Main.tile[k, l].TileType == num20) {
									WorldGen.SquareTileFrame(k, l);
									flag3 = true;
								}
							}
						}
					}
					if (Main.netMode == 2 && flag3) {
						NetMessage.SendTileSquare(-1, i, j, 3);
					}
				}
			}
			else {
				int num2 = Main.tile[i, j].TileType;

				if (!Main.tile[i, minJ].HasTile && WorldGen.genRand.Next(10) == 0 && Main.tile[i, minJ].LiquidType < LiquidID.Water) {
					int placedgrass = ModContent.TileType<CreamGrass_Foliage>();
					if (WorldGen.genRand.Next(12) == 0) {
						placedgrass = ModContent.TileType<YumDrop>();
					}
					WorldGen.PlaceTile(i, minJ, placedgrass, mute: true);
					if (Main.tile[i, minJ].HasTile) {
						Main.tile[i, minJ].CopyPaintAndCoating(Main.tile[i, j]);
					}
					if (Main.netMode == 2 && Main.tile[i, minJ].HasTile) {
						NetMessage.SendTileSquare(-1, i, minJ);
					}
				}
				TileColorCache color2 = Main.tile[i, j].BlockColorAndCoating();
				bool flag6 = false;
				for (int num3 = minI; num3 < maxI; num3++) {
					for (int num4 = minJ; num4 < maxJ; num4++) {
						if ((i != num3 || j != num4) && Main.tile[num3, num4].HasTile && Main.tile[num3, num4].TileType == ModContent.TileType<CookieBlock>()) {
							WorldGen.SpreadGrass(num3, num4, ModContent.TileType<CookieBlock>(), num2, repeat: false, color2);
							if (Main.tile[num3, num4].TileType == num2) {
								WorldGen.SquareTileFrame(num3, num4);
								flag6 = true;
							}
						}
					}
				}
				if (Main.netMode == 2 && flag6) {
					NetMessage.SendTileSquare(-1, i, j, 3);
				}
			}

			Tile tile = Main.tile[i, j];
			if (GrowMoreVines(i, j)) {
				int maxValue6 = 70;
				tile = Main.tile[i, j];
				if (tile.TileType == ModContent.TileType<CreamVines>()) {
					maxValue6 = 7;
				}
				if (WorldGen.genRand.Next(maxValue6) == 0) {
					tile = Main.tile[i, j + 1];
					if (!tile.HasTile) {
						tile = Main.tile[i, j + 1];
						if (tile.LiquidType != LiquidID.Lava) {
							bool flag3 = false;
							for (int num41 = j; num41 > j - 10; num41--) {
								tile = Main.tile[i, num41];
								if (tile.BottomSlope) {
									flag3 = false;
									break;
								}
								tile = Main.tile[i, num41];
								if (tile.HasTile) {
									tile = Main.tile[i, num41];
									if (tile.TileType == Type) {
										tile = Main.tile[i, num41];
										if (!tile.BottomSlope) {
											flag3 = true;
											break;
										}
									}
								}
							}
							if (flag3) {
								int num42 = j + 1;
								tile = Main.tile[i, num42];
								tile.TileType = (ushort)ModContent.TileType<CreamVines>();
								tile = Main.tile[i, num42];
								tile.HasTile = true;
								tile = Main.tile[i, num42];
								tile.CopyPaintAndCoating(Main.tile[i, j]);
								WorldGen.SquareTileFrame(i, num42);
								if (Main.netMode == 2) {
									NetMessage.SendTileSquare(-1, i, num42);
								}
							}
						}
					}
				}
			}
		}

		public static bool GrowMoreVines(int x, int y) {
			if (!WorldGen.InWorld(x, y, 30)) {
				return false;
			}
			int num = 4;
			int num2 = 6;
			int num3 = 10;
			int num4 = 60;
			int num5 = 0;
			if (Main.tile[x, y].TileType == 528) {
				num4 /= 5;
			}
			for (int i = x - num; i <= x + num; i++) {
				for (int j = y - num2; j <= y + num3; j++) {
					if (TileID.Sets.IsVine[Main.tile[i, j].TileType]) {
						num5++;
						if (j > y && Collision.CanHitLine(new Vector2((float)(x * 16), (float)(y * 16)), 1, 1, new Vector2((float)(i * 16), (float)(j * 16)), 1, 1)) {
							num5 = ((Main.tile[i, j].TileType != 528) ? (num5 + (j - y) * 2) : (num5 + (j - y) * 20));
						}
						if (num5 > num4) {
							return false;
						}
					}
				}
			}
			return true;
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail && !effectOnly)
            {
                Main.tile[i, j].TileType = (ushort)ModContent.TileType<CookieBlock>();
            }
        }

		public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor) {
			sightColor = new Color(210, 196, 145);
			return true;
		}
	}
}