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
    public class CreamSapling : ModTile
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
            TileObjectData.newTile.AnchorValidTiles = new[] { ModContent.TileType<CreamGrass>(), ModContent.TileType<CreamGrassMowed>() };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawFlipHorizontal = true;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.RandomStyleRange = 3;
            TileObjectData.newTile.StyleMultiplier = 3;

            TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
            TileObjectData.newSubTile.AnchorValidTiles = new int[] { ModContent.TileType<Creamsand>() };
            TileObjectData.addSubTile(1);

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

            if (tile.TileFrameX < 54)
            {
				tile = Main.tile[i, j];
				if (tile.HasUnactuatedTile) {
					if (j > Main.rockLayer) {
						if (WorldGen.genRand.NextBool(5)) {
							AttemptToGrowCreamTreeFromSapling(i, j);
						}
					}
					else {
						if (WorldGen.genRand.NextBool(20)) {
							AttemptToGrowCreamTreeFromSapling(i, j);
						}
					}
				}
				growSucess = false;
			}
            else
            {
                growSucess = GrowPalmTree(i, j);
            }

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

		public static bool GrowPalmTree(int i, int y)
		{
			int num = y;
			if (!WorldGen.InWorld(i, y))
			{
				return false;
			}
			while (TileID.Sets.TreeSapling[Main.tile[i, num].TileType])
			{
				num++;
				if (Main.tile[i, num] == null)
				{
					return false;
				}
			}
			Tile tile = Main.tile[i, num];
			Tile tile2 = Main.tile[i, num - 1];
			byte color = 0;
			if (Main.tenthAnniversaryWorld && !WorldGen.gen)
			{
				color = (byte)WorldGen.genRand.Next(1, 13);
			}
			if (!tile.HasTile || tile.IsHalfBlock || tile.Slope != 0)
			{
				return false;
			}
			if (tile2.WallType != 0 || tile2.LiquidAmount != 0)
			{
				return false;
			}
			bool vanillaCanGrow = true;
			if (tile.TileType != 53 && tile.TileType != 234 && tile.TileType != 116 && tile.TileType != 112)
			{
				vanillaCanGrow = false;
			}
			if (!vanillaCanGrow && !TileLoader.CanGrowModPalmTree(tile.TileType))
			{
				return false;
			}
			if (!WorldGen.EmptyTileCheck(i, i, num - 2, num - 1, 20))
			{
				return false;
			}
			if (!WorldGen.EmptyTileCheck(i - 1, i + 1, num - 30, num - 3, 20))
			{
				return false;
			}
			int num2 = WorldGen.genRand.Next(10, 21);
			int num3 = WorldGen.genRand.Next(-8, 9);
			num3 *= 2;
			short num4 = 0;
			for (int j = 0; j < num2; j++)
			{
				tile = Main.tile[i, num - 1 - j];
				if (j == 0)
				{
					tile.HasTile = true;
					tile.TileType = (ushort)ModContent.TileType<CreamPalmTree>();
					tile.TileFrameX = 66;
					tile.TileFrameY = 0;
					tile.TileColor = color;
					continue;
				}
				if (j == num2 - 1)
				{
					tile.HasTile = true;
					tile.TileType = (ushort)ModContent.TileType<CreamPalmTree>();
					tile.TileFrameX = (short)(22 * WorldGen.genRand.Next(4, 7));
					tile.TileFrameY = num4;
					tile.TileColor = color;
					continue;
				}
				if (num4 != num3)
				{
					double num5 = (double)j / (double)num2;
					if (!(num5 < 0.25))
					{
						if ((!(num5 < 0.5) || !WorldGen.genRand.NextBool(13)) && (!(num5 < 0.7) || !WorldGen.genRand.NextBool(9)) && num5 < 0.95)
						{
							WorldGen.genRand.Next(5);
						}
						short num6 = (short)Math.Sign(num3);
						num4 += (short)(num6 * 2);
					}
				}
				tile.HasTile = true;
				tile.TileType = (ushort)ModContent.TileType<CreamPalmTree>();
				tile.TileFrameX = (short)(22 * WorldGen.genRand.Next(0, 3));
				tile.TileFrameY = num4;
				tile.TileColor = color;
			}
			WorldGen.RangeFrame(i - 2, num - num2 - 1, i + 2, num + 1);
			NetMessage.SendTileSquare(-1, i, num - num2, 1, num2);
			return true;
		}

		public static bool AttemptToGrowCreamTreeFromSapling(int x, int y)
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
			bool flag = CreamTree.GrowModdedTreeWithSettings(x, y, CreamTree.Tree_Cream);
			if (flag && WorldGen.PlayerLOS(x, y))
			{
				GrowCreamTreeFXCheck(x, y);
			}
			return flag;
		}

		public static void GrowCreamTreeFXCheck(int x, int y)
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
