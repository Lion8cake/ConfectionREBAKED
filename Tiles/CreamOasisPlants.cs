using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class CreamOasisPlants : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileID.Sets.TileCutIgnore.Regrowth[Type] = true;
			TileID.Sets.ReplaceTileBreakUp[Type] = true;
			TileID.Sets.SlowlyDiesInWater[Type] = true;
			TileID.Sets.BreakableWhenPlacing[Type] = true;
			TileID.Sets.IsMultitile[Type] = true;
			TileID.Sets.IgnoredByGrowingSaplings[Type] = true;

			AddMapEntry(new Color(200, 170, 108)); //may need to change
			DustType = ModContent.DustType<CreamGrassDust>();
			HitSound = SoundID.Grass;
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) {
			bool intoRenderTargets = true;
			bool flag = intoRenderTargets || Main.LightingEveryFrame;

			if (Main.tile[i, j].TileFrameX < 270) {
				if (Main.tile[i, j].TileFrameX % 54 == 0 && Main.tile[i, j].TileFrameY == 0 && flag) {
					Main.instance.TilesRenderer.AddSpecialPoint(j, i, 4);
				}
			}
			return true;
		}

		public override void RandomUpdate(int i, int j) {
			bool[] sand = TileID.Sets.Conversion.Sand;
			Tile tile = Main.tile[i, j];
			if (!sand[tile.TileType]) {
				tile = Main.tile[i, j];
				if (!ConfectionWorldGeneration.OasisPlantWaterCheck(i, j, boost: true)) {
					WorldGen.KillTile(i, j);
					if (Main.netMode == 2) {
						NetMessage.SendData(17, -1, -1, null, 0, i, j);
					}
				}
			}
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak) {
			//WorldGen.CheckOasisPlant(i, j, Type);
			return false;
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) {
			offsetY = 2;
		}
	}
}
