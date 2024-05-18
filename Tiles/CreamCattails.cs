using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class CreamCattails : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileLighted[Type] = true;

			TileID.Sets.TileCutIgnore.Regrowth[Type] = true;

			AddMapEntry(new Color(200, 170, 108)); //120 110 100
			DustType = ModContent.DustType<CreamGrassDust>();
			HitSound = SoundID.Grass;
		}

		public override void RandomUpdate(int i, int j) {
			ConfectionWorldGeneration.CheckCreamCatTail(i, j);
			if (Main.tile[i, j].HasTile && WorldGen.genRand.NextBool(8)) {
				WorldGen.GrowCatTail(i, j);
				ConfectionWorldGeneration.CheckCreamCatTail(i, j);
			}
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak) {
			ConfectionWorldGeneration.CheckCreamCatTail(i, j);
			return false;
		}
	}

	public class CattailGen : GlobalTile {
		public override void RandomUpdate(int i, int j, int type) {
			if (j >= Main.worldSurface) {
				if (Main.tile[i, j].LiquidAmount > 32) {
					if (WorldGen.genRand.NextBool(600)) {
						WorldGen.PlaceTile(i, j, ModContent.TileType<CreamCattails>(), mute: true);
						if (Main.netMode == NetmodeID.Server) {
							NetMessage.SendTileSquare(-1, i, j);
						}
					}
				}
			}
		}
	}
}
