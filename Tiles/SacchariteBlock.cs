using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
	public class SacchariteBlock : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = false;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 4500;
			AddMapEntry(new Color(32, 174, 221));
			TileID.Sets.ChecksForMerge[Type] = true;
			TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<Creamstone>()] = true;
			DustType = ModContent.DustType<SacchariteDust>();
			HitSound = SoundID.Item27;
		}

		public override void RandomUpdate(int i, int j) {
			Tile tileBelow = Framing.GetTileSafely(i, j + 1);
			if (j > Main.rockLayer) {
				if (WorldGen.genRand.NextBool(10) && !tileBelow.HasTile && tileBelow.LiquidType != LiquidID.Lava) {
					bool placeSaccharite = false;
					int yTest = j;
					while (yTest > j - 10) {
						Tile testTile = Framing.GetTileSafely(i, yTest);
						if (testTile.BottomSlope) {
							break;
						}
						placeSaccharite = true;
						break;
					}
					if (placeSaccharite) {
						tileBelow.TileType = Type;
						tileBelow.HasTile = true;
						WorldGen.SquareTileFrame(i, j + 1, true);
						if (Main.netMode == NetmodeID.Server) {
							NetMessage.SendTileSquare(-1, i, j + 1, 3, TileChangeType.None);
						}
					}
					if (placeSaccharite) {
						tileBelow.TileType = Type;
						tileBelow.HasTile = true;
						WorldGen.SquareTileFrame(i, j + 1, true);
						if (Main.netMode == NetmodeID.Server) {
							NetMessage.SendTileSquare(+1, i, j - 1, 3, TileChangeType.None);
						}
					}
					/*if (WorldGen.genRand.NextBool(15)) {
						if (placeSaccharite) {
							tileBelow.TileType = (ushort)ModContent.TileType<EnchantedSacchariteBlock>();
							tileBelow.HasTile = true;
							WorldGen.SquareTileFrame(i, j + 1, true);
							if (Main.netMode == NetmodeID.Server) {
								NetMessage.SendTileSquare(-1, i, j + 1, 3, TileChangeType.None);
							}
						}
						if (placeSaccharite) {
							tileBelow.TileType = (ushort)ModContent.TileType<EnchantedSacchariteBlock>();
							tileBelow.HasTile = true;
							WorldGen.SquareTileFrame(i, j + 1, true);
							if (Main.netMode == NetmodeID.Server) {
								NetMessage.SendTileSquare(+1, i, j - 1, 3, TileChangeType.None);
							}
						}
					}*/
				}
			}
		}
	}
}