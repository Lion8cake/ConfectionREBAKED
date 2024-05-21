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
	public class CreamSeaOats : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileID.Sets.TileCutIgnore.Regrowth[Type] = true;
			TileID.Sets.ReplaceTileBreakUp[Type] = true;
			TileID.Sets.SlowlyDiesInWater[Type] = true;
			TileID.Sets.SwaysInWindBasic[Type] = true;
			TileID.Sets.DrawFlipMode[Type] = 1;
			TileID.Sets.IgnoredByGrowingSaplings[Type] = true;

			AddMapEntry(new Color(200, 170, 108));
			DustType = ModContent.DustType<CreamGrassDust>();
			HitSound = SoundID.Grass;
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak) {
			if (!WorldGen.SolidTileAllowBottomSlope(i, j + 1)) {
				WorldGen.KillTile(i, j);
				return false;
			}
			Tile tile16 = Main.tile[i, j + 1];
			_ = Main.tile[i, j].TileFrameY / 34;
			if (tile16 == null || !tile16.HasTile || (tile16.TileType >= 0 && !TileID.Sets.Conversion.Sand[tile16.TileType])) {
				WorldGen.KillTile(i, j);
			}
			return false;
		}

		public override void RandomUpdate(int i, int j) {
			if (Main.tile[i, j].HasUnactuatedTile) {
				if (i >= Main.worldSurface) {
					if (ConfectionWorldGeneration.CheckSeaOat(i, j) && WorldGen.genRand.NextBool(20)) {
						ConfectionWorldGeneration.GrowSeaOat(i, j);
					}
				}
			}
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) {
			height = 32;
			offsetY = -14;
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) {
			if (i % 2 == 0) {
				spriteEffects = (SpriteEffects)1;
			}
		}
	}
}
