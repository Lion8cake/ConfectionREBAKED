using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class CreamGrassMowed : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;			

			TileID.Sets.Conversion.MergesWithDirtInASpecialWay[Type] = true;
			TileID.Sets.Conversion.Grass[Type] = true;
			TileID.Sets.Conversion.GolfGrass[Type] = true;
			TileID.Sets.AllBlocksWithSmoothBordersToResolveHalfBlockIssue[Type] = true;
			TileID.Sets.CanBeDugByShovel[Type] = true;
			TileID.Sets.ResetsHalfBrickPlacementAttempt[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			TileID.Sets.SpreadOverground[Type] = true;
			TileID.Sets.Grass[Type] = true;
			ConfectionIDs.Sets.Confection[Type] = true;
			ConfectionIDs.Sets.ConfectionBiomeSight[Type] = true;

			Main.tileMerge[Type][ModContent.TileType<Creamstone>()] = true;
			Main.tileMerge[Type][ModContent.TileType<CookieBlock>()] = true;

			AddMapEntry(new Color(235, 207, 150));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.CookieBlock>());
			DustType = ModContent.DustType<CreamGrassDust>();
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem) {
			if (fail) {
				Main.tile[i, j].TileType = (ushort)ModContent.TileType<CookieBlock>();
			}
		}

		public override void RandomUpdate(int i, int j) {
			if (i > Main.worldSurface) {
				if (Main.tile[i, j].HasUnactuatedTile) {
					int num = i - 1;
					int num11 = i + 2;
					int num22 = j - 1;
					int num33 = j + 2;
					CreamGrass.CreamGrassGrowth(i, j, num, num11, num22, num33);
				}
			}
		}
	}
}
