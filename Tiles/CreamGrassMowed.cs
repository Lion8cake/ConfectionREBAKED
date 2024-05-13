using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class CreamGrassMowed : ModTile //Clone of CreamGrass, double check the values with decompiled vanilla code
	{
		public override void SetStaticDefaults()
		{
			Main.tileBrick[Type] = true;
			Main.tileShine[Type] = 9000;
			Main.tileLighted[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			TileID.Sets.Conversion.MergesWithDirtInASpecialWay[Type] = true;
			TileID.Sets.Conversion.Grass[Type] = true;
			TileID.Sets.CanBeDugByShovel[Type] = true;
			TileID.Sets.ResetsHalfBrickPlacementAttempt[Type] = true;
			TileID.Sets.DoesntPlaceWithTileReplacement[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			TileID.Sets.SpreadOverground[Type] = true;
			TileID.Sets.SpreadUnderground[Type] = true;
			TileID.Sets.Grass[Type] = true;
			TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
			TileID.Sets.GrassSpecial[Type] = true;
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
	}
}
