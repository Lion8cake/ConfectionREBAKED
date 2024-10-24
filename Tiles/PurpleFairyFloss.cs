using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class PurpleFairyFloss : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileNoSunLight[Type] = true;

			TileID.Sets.MergesWithClouds[Type] = true;
			TileID.Sets.Clouds[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			ConfectionIDs.Sets.Confection[Type] = true;

			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;
			AddMapEntry(new Color(210, 90, 250));
			DustType = ModContent.DustType<FairyFlossRainDust>();
		}

		public override bool HasWalkDust() {
			return true;
		}

		public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color) {
			dustType = DustType;
		}
	}
}
