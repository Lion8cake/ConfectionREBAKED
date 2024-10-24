using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class BlueIce : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileBlockLight[Type] = true;

			TileID.Sets.Conversion.Ice[Type] = true;
			TileID.Sets.IceSkateSlippery[Type] = true;
			TileID.Sets.Ices[Type] = true;
			TileID.Sets.IcesSlush[Type] = true;
			TileID.Sets.IcesSnow[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
			ConfectionIDs.Sets.CanGrowSaccharite[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;
			ConfectionIDs.Sets.Confection[Type] = true;

			Main.tileMerge[Type][ModContent.TileType<SacchariteBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<Creamstone>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CookieBlock>()] = true;
			Main.tileMerge[Type][ModContent.TileType<BlueIceStalactite>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CreamstoneStalactite>()] = true;
			Main.tileMerge[Type][TileID.IceBlock] = true;
			Main.tileMerge[Type][TileID.SnowBlock] = true;
			Main.tileMerge[Type][TileID.FleshIce] = true;
			Main.tileMerge[Type][TileID.CorruptIce] = true;
			Main.tileMerge[Type][TileID.HallowedIce] = true;
			Main.tileMerge[TileID.IceBlock][Type] = true;
			Main.tileMerge[TileID.SnowBlock][Type] = true;
			Main.tileMerge[TileID.FleshIce][Type] = true;
			Main.tileMerge[TileID.CorruptIce][Type] = true;
			Main.tileMerge[TileID.HallowedIce][Type] = true;

			DustType = ModContent.DustType<OrangeIceDust>();
			AddMapEntry(new Color(237, 145, 103));
			HitSound = SoundID.Item50;
		}

		public override void RandomUpdate(int i, int j) { //Generates Salactites
			if (Main.tile[i, j].HasUnactuatedTile) {
				if (Main.rand.NextBool(10) && !Main.tile[i, j + 1].HasTile && !Main.tile[i, j + 2].HasTile) {
					int num48 = i - 3;
					int num5 = i + 4;
					int num6 = 0;
					for (int num7 = num48; num7 < num5; num7++) {
						if (Main.tile[num7, j].TileType == ModContent.TileType<BlueIceStalactite>() && Main.tile[num7, j].HasTile) {
							num6++;
						}
						if (Main.tile[num7, j + 1].TileType == ModContent.TileType<BlueIceStalactite>() && Main.tile[num7, j + 1].HasTile) {
							num6++;
						}
						if (Main.tile[num7, j + 2].TileType == ModContent.TileType<BlueIceStalactite>() && Main.tile[num7, j + 2].HasTile) {
							num6++;
						}
						if (Main.tile[num7, j + 3].TileType == ModContent.TileType<BlueIceStalactite>() && Main.tile[num7, j + 3].HasTile) {
							num6++;
						}
					}
					if (num6 < 2) {
						ConfectionWorldGeneration.PlaceTight(i, j + 1);
						WorldGen.SquareTileFrame(i, j + 1);
						if (Main.netMode == 2 && Main.tile[i, j + 1].HasTile) {
							NetMessage.SendTileSquare(-1, i, j + 1, 1, 2);
						}
					}
				}
			}
		}

		//Add special merging for snow/cream blocks
	}
}
