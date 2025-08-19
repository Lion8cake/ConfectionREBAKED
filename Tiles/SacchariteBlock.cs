using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TheConfectionRebirth.Dusts;
using System;

namespace TheConfectionRebirth.Tiles
{
    public class SacchariteBlock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileLighted[Type] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<Creamstone>()] = true;
            AddMapEntry(new Color(32, 174, 221));
            DustType = ModContent.DustType<SacchariteCrystals>();
            HitSound = SoundID.Item27;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void RandomUpdate(int i, int j)
        {
            if (j > Main.rockLayer)
            {
				int num2 = i;
				int num3 = j;
				int num4 = 0;
				Tile tile = Main.tile[num2 + 1, num3];
				if (tile.HasTile) {
					tile = Main.tile[num2 + 1, num3];
					if (tile.TileType == Type) {
						num4++;
					}
				}
				tile = Main.tile[num2 - 1, num3];
				if (tile.HasTile) {
					tile = Main.tile[num2 - 1, num3];
					if (tile.TileType == Type) {
						num4++;
					}
				}
				tile = Main.tile[num2, num3 + 1];
				if (tile.HasTile) {
					tile = Main.tile[num2, num3 + 1];
					if (tile.TileType == Type) {
						num4++;
					}
				}
				tile = Main.tile[num2, num3 - 1];
				if (tile.HasTile) {
					tile = Main.tile[num2, num3 - 1];
					if (tile.TileType == Type) {
						num4++;
					}
				}
				if (num4 >= 3) {
					tile = Main.tile[i, j];
					if (tile.TileType != ModContent.TileType<Creamstone>() && tile.TileType != ModContent.TileType<HardenedCreamsand>() && tile.TileType != ModContent.TileType<Creamsandstone>() && tile.TileType != ModContent.TileType<BlueIce>()) {
						return;
					}
				}
				switch (WorldGen.genRand.Next(4)) {
					case 0:
						num3--;
						break;
					case 1:
						num3++;
						break;
					case 2:
						num2--;
						break;
					case 3:
						num2++;
						break;
				}
				tile = Main.tile[num2, num3];
				if (tile.HasTile) {
					return;
				}
				num4 = 0;
				tile = Main.tile[num2 + 1, num3];
				if (tile.HasTile) {
					tile = Main.tile[num2 + 1, num3];
					if (tile.TileType == Type) {
						num4++;
					}
				}
				tile = Main.tile[num2 - 1, num3];
				if (tile.HasTile) {
					tile = Main.tile[num2 - 1, num3];
					if (tile.TileType == Type) {
						num4++;
					}
				}
				tile = Main.tile[num2, num3 + 1];
				if (tile.HasTile) {
					tile = Main.tile[num2, num3 + 1];
					if (tile.TileType == Type) {
						num4++;
					}
				}
				tile = Main.tile[num2, num3 - 1];
				if (tile.HasTile) {
					tile = Main.tile[num2, num3 - 1];
					if (tile.TileType == Type) {
						num4++;
					}
				}
				if (num4 >= 2) {
					return;
				}
				int num5 = 7;
				int num9 = num2 - num5;
				int num6 = num2 + num5;
				int num7 = num3 - num5;
				int num8 = num3 + num5;
				bool flag = false;
				for (int k = num9; k < num6; k++) {
					for (int l = num7; l < num8; l++) {
						if (Math.Abs(k - num2) * 2 + Math.Abs(l - num3) >= 9) {
							continue;
						}
						tile = Main.tile[k, l];
						if (!tile.HasTile) {
							continue;
						}
						tile = Main.tile[k, l];
						if (tile.TileType != ModContent.TileType<Creamstone>() && tile.TileType != ModContent.TileType<HardenedCreamsand>() && tile.TileType != ModContent.TileType<Creamsandstone>() && tile.TileType != ModContent.TileType<BlueIce>()) {
							continue;
						}
						tile = Main.tile[k, l - 1];
						if (!tile.HasTile) {
							continue;
						}
						tile = Main.tile[k, l - 1];
						if (tile.TileType == Type) {
							tile = Main.tile[k, l - 1];
							if (tile.LiquidAmount == 0) {
								flag = true;
								break;
							}
						}
					}
				}
				if (flag) {
					tile = Main.tile[num2, num3];
					if (WorldGen.genRand.NextBool(8)) {
						tile.TileType = (ushort)ModContent.TileType<EnchantedSacchariteBlock>();
					}
					else {
						tile.TileType = Type;
					}
					tile = Main.tile[num2, num3];
					tile.HasTile = true;
					tile = Main.tile[num2, num3];
					tile.CopyPaintAndCoating(Main.tile[i, j]);
					WorldGen.SquareTileFrame(num2, num3);
					if (Main.netMode == 2) {
						NetMessage.SendTileSquare(-1, num2, num3, 1);
					}
				}
			}
        }
    }
}