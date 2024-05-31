using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class BlueFairyFloss : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileNoSunLight[Type] = true;

			TileID.Sets.MergesWithClouds[Type] = true;
			TileID.Sets.Clouds[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;

			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;
			AddMapEntry(new Color(78, 191, 252));
			DustType = ModContent.DustType<FairyFlossSnowDust>();
		}

		public override bool HasWalkDust() {
			return true;
		}

		public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color) {
			dustType = DustType;
		}
	}
}
