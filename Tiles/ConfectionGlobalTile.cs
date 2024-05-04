using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles {
	public class ConfectionGlobalTile : GlobalTile {
		public override void RandomUpdate(int i, int j, int type) {
			if (ConfectionIDs.Sets.CanGrowSaccharite[type]) {
				Tile Blockpos = Main.tile[i, j];
				if (j > Main.rockLayer) {
					for (int NearSaccX = i + 4; NearSaccX > i - 4; --NearSaccX) {
						for (int NearSaccY = j + 4; NearSaccY > j - 4; --NearSaccY) {
							if (Main.tile[NearSaccX, NearSaccY].TileType == ModContent.TileType<SacchariteBlock>()) {
								return;
							}
						}
					}
					if (WorldGen.genRand.NextBool(40) && !Blockpos.IsHalfBlock && !Blockpos.BottomSlope && !Blockpos.LeftSlope && !Blockpos.RightSlope && !Blockpos.TopSlope) {
						if (!Main.tile[i + 1, j].HasTile && Main.tile[i + 1, j].LiquidAmount == 0) {
							WorldGen.PlaceTile(i + 1, j, ModContent.TileType<SacchariteBlock>(), mute: true);
						}
						else if (!Main.tile[i, j - 1].HasTile && Main.tile[i, j - 1].LiquidAmount == 0) {
							WorldGen.PlaceTile(i, j - 1, ModContent.TileType<SacchariteBlock>(), mute: true);
						}
						else if (!Main.tile[i, j + 1].HasTile && Main.tile[i, j + 1].LiquidAmount == 0) {
							WorldGen.PlaceTile(i, j + 1, ModContent.TileType<SacchariteBlock>(), mute: true);
						}
						else if (!Main.tile[i - 1, j].HasTile && Main.tile[i - 1, j].LiquidAmount == 0) {
							WorldGen.PlaceTile(i - 1, j, ModContent.TileType<SacchariteBlock>(), mute: true);
						}
					}
				}
			}
		}

		public override bool? IsTileBiomeSightable(int i, int j, int type, ref Color sightColor) {
			if (ConfectionIDs.Sets.ConfectionBiomeSight[type]) {
				sightColor = new Color(210, 196, 145);
				return true;
			}
			else
				return null;
		}
	}
}
