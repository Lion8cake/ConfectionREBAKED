using ExampleMod.Content.Tiles;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

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
	}
}