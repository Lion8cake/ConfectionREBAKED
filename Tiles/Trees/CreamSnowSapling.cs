using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles.Trees
{
    public class CreamSnowSapling : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorValidTiles = new[] { ModContent.TileType<CreamBlock>() };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawFlipHorizontal = true;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.RandomStyleRange = 3;
            TileObjectData.newTile.StyleMultiplier = 3;

            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(151, 107, 75), name);

            TileID.Sets.TreeSapling[Type] = true;
            TileID.Sets.CommonSapling[Type] = true;
            TileID.Sets.SwaysInWindBasic[Type] = true;
			TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);

			DustType = ModContent.DustType<CreamwoodDust>();

            AdjTiles = new int[] { TileID.Saplings };
        }

		public override void RandomUpdate(int i, int j)
		{
			if (!WorldGen.genRand.NextBool(20))
			{
				return;
			}

			Tile tile = Framing.GetTileSafely(i, j);
			bool growSucess;

			tile = Main.tile[i, j];
			if (tile.HasUnactuatedTile)
			{
				if (j > Main.rockLayer)
				{
					if (WorldGen.genRand.NextBool(5))
					{
						AttemptToGrowCreamSnowTreeFromSapling(i, j);
					}
				}
				else
				{
					if (WorldGen.genRand.NextBool(20))
					{
						AttemptToGrowCreamSnowTreeFromSapling(i, j);
					}
				}
			}
			growSucess = false;

			bool isPlayerNear = WorldGen.PlayerLOS(i, j);

			if (growSucess && isPlayerNear)
			{
				WorldGen.TreeGrowFXCheck(i, j);
			}
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects)
		{
			if (i % 2 == 1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
		}

		public static bool AttemptToGrowCreamSnowTreeFromSapling(int x, int y)
		{
			if (Main.netMode == 1)
			{
				return false;
			}
			if (!WorldGen.InWorld(x, y, 2))
			{
				return false;
			}
			Tile tile = Main.tile[x, y];
			if (tile == null || !tile.HasTile)
			{
				return false;
			}
			bool flag = CreamTree.GrowModdedTreeWithSettings(x, y, CreamSnowTree.Tree_CreamSnow);
			if (flag && WorldGen.PlayerLOS(x, y))
			{
				GrowCreamSnowTreeFXCheck(x, y);
			}
			return flag;
		}

		public static void GrowCreamSnowTreeFXCheck(int x, int y)
		{
			int treeHeight = 1;
			for (int num = -1; num > -100; num--)
			{
				Tile tile = Main.tile[x, y + num];
				if (!tile.HasTile || !TileID.Sets.GetsCheckedForLeaves[tile.TileType])
				{
					break;
				}
				treeHeight++;
			}
			for (int i = 1; i < 5; i++)
			{
				Tile tile2 = Main.tile[x, y + i];
				if (tile2.HasTile && TileID.Sets.GetsCheckedForLeaves[tile2.TileType])
				{
					treeHeight++;
					continue;
				}
				break;
			}
			if (treeHeight > 0)
			{
				if (Main.netMode == 2)
				{
					NetMessage.SendData(MessageID.SpecialFX, -1, -1, null, 1, x, y, treeHeight, ModContent.GoreType<CreamTreeLeaf>());
				}
				if (Main.netMode == 0)
				{
					WorldGen.TreeGrowFX(x, y, treeHeight, ModContent.GoreType<CreamTreeLeaf>());
				}
			}
		}
	}
}
