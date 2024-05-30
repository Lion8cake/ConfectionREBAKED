using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class PinkFairyFloss : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileNoSunLight[Type] = true;
			Main.tileSolid[Type] = true;

			TileID.Sets.Clouds[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;

			Main.tileMerge[Type][TileID.Cloud] = true;
			Main.tileMerge[TileID.Cloud][Type] = true;

			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;
			AddMapEntry(new Color(253, 142, 250));
			DustType = ModContent.DustType<FairyFlossDust>();
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak) {
			for (int type = 0; type < TileLoader.TileCount; type++) {
				if (TileID.Sets.MergesWithClouds[type]) {
					Main.tileMerge[Type][type] = true;
					Main.tileMerge[type][Type] = true;
				}
			}
			return true;
		}

		public override bool HasWalkDust() {
			return true;
		}

		public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color) {
			dustType = DustType;
		}
	}
}
