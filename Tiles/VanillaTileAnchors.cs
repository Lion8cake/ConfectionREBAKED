using ExampleMod.Content.Tiles;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheConfectionRebirth.Tiles.Trees;

namespace TheConfectionRebirth.Tiles
{
	public class vanillaTileAnchors : GlobalTile
	{
		public override void SetStaticDefaults() {
			TileObjectData tileObjectData = TileObjectData.GetTileData(TileID.Sunflower, 0);
			tileObjectData.AnchorValidTiles = tileObjectData.AnchorValidTiles.Append(ModContent.TileType<CreamGrass>()).ToArray();
			tileObjectData.AnchorValidTiles = tileObjectData.AnchorValidTiles.Append(ModContent.TileType<CreamGrassMowed>()).ToArray();
			TileObjectData tileObjectData2 = TileObjectData.GetTileData(TileID.Pumpkins, 0);
			tileObjectData2.AnchorValidTiles = tileObjectData.AnchorValidTiles.Append(ModContent.TileType<CreamGrass>()).ToArray();
			tileObjectData2.AnchorValidTiles = tileObjectData.AnchorValidTiles.Append(ModContent.TileType<CreamGrassMowed>()).ToArray();
			TileObjectData tileObjectData3 = TileObjectData.GetTileData(TileID.FallenLog, 0);
			tileObjectData3.AnchorValidTiles = tileObjectData.AnchorValidTiles.Append(ModContent.TileType<CreamGrass>()).ToArray();
			tileObjectData3.AnchorValidTiles = tileObjectData.AnchorValidTiles.Append(ModContent.TileType<CreamGrassMowed>()).ToArray();
		}

		public override void Unload() {
			TileObjectData tileObjectData = TileObjectData.GetTileData(TileID.Sunflower, 0);
			tileObjectData.AnchorValidTiles = tileObjectData.AnchorValidTiles.Except(new int[] { ModContent.TileType<CreamGrass>(), ModContent.TileType<CreamGrassMowed>() }).ToArray();
			TileObjectData tileObjectData2 = TileObjectData.GetTileData(TileID.Pumpkins, 0);
			tileObjectData2.AnchorValidTiles = tileObjectData.AnchorValidTiles.Except(new int[] { ModContent.TileType<CreamGrass>(), ModContent.TileType<CreamGrassMowed>() }).ToArray();
			TileObjectData tileObjectData3 = TileObjectData.GetTileData(TileID.FallenLog, 0);
			tileObjectData3.AnchorValidTiles = tileObjectData.AnchorValidTiles.Except(new int[] { ModContent.TileType<CreamGrass>(), ModContent.TileType<CreamGrassMowed>() }).ToArray();
		}

		public override void NearbyEffects(int i, int j, int type, bool closer) {
			if (type == TileID.Trees) {
				WorldGen.GetTreeBottom(i, j, out var x, out var y);
				Tile tilebelow = Main.tile[x, y + 1];
				Tile tilecurrent = Main.tile[x, y];
				if (tilebelow.TileType == ModContent.TileType<CreamGrass>() || tilebelow.TileType == ModContent.TileType<CreamGrassMowed>() || tilebelow.TileType == ModContent.TileType<CreamTree>() || tilecurrent.TileType == ModContent.TileType<CreamGrass>() || tilecurrent.TileType == ModContent.TileType<CreamGrassMowed>() || tilecurrent.TileType == ModContent.TileType<CreamTree>()) {
					Main.tile[i, j].TileType = (ushort)ModContent.TileType<CreamTree>();
				}
			}
		}
	}
}