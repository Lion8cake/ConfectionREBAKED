using TheConfectionRebirth.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheConfectionRebirth.Tiles
{
	public class CreamSnowSapling : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileFrameImportant[base.Type] = true;
			Main.tileNoAttach[base.Type] = true;
			Main.tileLavaDeath[base.Type] = true;
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[2] { 16, 18 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.AnchorValidTiles = new[] { ModContent.TileType<CreamBlock>() };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.DrawFlipHorizontal = true;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.newTile.RandomStyleRange = 3;
			TileObjectData.addTile(base.Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Cream Sapling");
			AddMapEntry(new Color(231, 52, 101), name);
			AdjTiles = new int[] { TileID.Saplings };
			TileID.Sets.CommonSapling[Type] = true;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public override void RandomUpdate(int i, int j)
		{
			if (WorldGen.genRand.NextBool(20))
			{
				bool isPlayerNear = WorldGen.PlayerLOS(i, j);
				if (WorldGen.GrowTree(i, j) && isPlayerNear)
				{
					WorldGen.TreeGrowFXCheck(i, j);
				}
			}
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects) {
			if (i % 2 == 1)
				effects = SpriteEffects.FlipHorizontally;
		}
	}
}