using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles
{
    public class CreamGrassMowed : ModTile
    {
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CreamGrass").Type] = true;
			TheConfectionRebirth.tileMerge[Type, Mod.Find<ModTile>("CookieBlock").Type] = true;
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
			TileID.Sets.Conversion.GolfGrass[Type] = true;
			AddMapEntry(new Color(235, 207, 150));
			//SoundType = 0;
			//SoundStyle = 2;
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.CookieBlock>());
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail && !effectOnly)
            {
                Main.tile[i, j].TileType = (ushort)ModContent.TileType<CookieBlock>();
            }
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
				int type = ModContent.TileType<CreamGrass>();
				int num12 = -1;
				int num19 = type;
				int num20 = -1;
				int num = ModContent.TileType<CookieBlock>();
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
		}

		public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor) {
			sightColor = new Color(210, 196, 145);
			return true;
		}
	}
}