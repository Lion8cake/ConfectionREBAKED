using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Drawing;
using System;
using Terraria.GameContent;
using Terraria.Utilities;
using Terraria.DataStructures;
using System.Reflection;
using Terraria.GameContent.Skies.CreditsRoll;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Terraria.GameContent.Tile_Entities;
using static Terraria.WorldGen;
using Terraria.Enums;
using System.Threading;
using static Terraria.GameContent.TilePaintSystemV2;
using ReLogic.Content;
using Terraria.GameContent.Liquid;
using static Terraria.GameContent.Drawing.TileDrawing;
using Terraria.Graphics.Capture;
using TheConfectionRebirth.Tiles.Trees;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth;
using TheConfectionRebirth.Items;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TheConfectionRebirth.Tiles.Trees
{
    public class CreamTree : ModTile
    {
		//Plans for this

		//Basically since we need to create our own tree for tree variants we run into a few issues
		//converting normal trees into this tree
		//converting this tree into normal trees
		
		//with converting, this tile will extend off the normal tree tile anchor types and then if this tile detects that the tile thats below it is NOT creamgrass then
		//it will convert to the normal tree type and vise versa for the normal tree. This will HOPEFULLY give a seamless transition between converting trees

		public static GrowTreeSettings Tree_Cream = new GrowTreeSettings
		{
			GroundTest = CreamwoodTreeGroundTest,
			WallTest = DefaultTreeWallTest,
			TreeHeightMax = 12,
			TreeHeightMin = 7,
			TreeTileType = (ushort)ModContent.TileType<CreamTree>(),
			TreeTopPaddingNeeded = 4,
			SaplingTileType = (ushort)ModContent.TileType<CreamSapling>()
		};

		public static TreePaintingSettings TreeCream = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 11f / 72f,
			SpecialGroupMaximumHueValue = 0.25f,
			SpecialGroupMinimumSaturationValue = 0.88f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		public override void SetStaticDefaults()
        {
            Main.tileAxe[Type] = true;
            Main.tileFrameImportant[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileID.Sets.IsATreeTrunk[Type] = true;
            TileID.Sets.IsShakeable[Type] = true;
            TileID.Sets.GetsDestroyedForMeteors[Type] = true;
            TileID.Sets.GetsCheckedForLeaves[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(42, 43, 51), name); //change to normal tree color
			DustType = ModContent.DustType<CreamwoodDust>();
		}

		public static void KillTile_GetTreeDrops(int i, int j, Tile tileCache, ref bool bonusWood, ref int dropItem, ref int secondaryItem) {
			if (tileCache.TileFrameX >= 22 && tileCache.TileFrameY >= 198) {
				if (Main.netMode != 1) {
					if (genRand.NextBool(2)) {
						int k;
						for (k = j; Main.tile[i, k] != null && (!Main.tile[i, k].HasTile || !Main.tileSolid[Main.tile[i, k].TileType] || Main.tileSolidTop[Main.tile[i, k].TileType]); k++) {
						}
						if (Main.tile[i, k] != null) {
							dropItem = 9;
							secondaryItem = 27;
						}
					}
					else {
						dropItem = 9;
					}
				}
			}
			else {
				dropItem = 9;
			}
			if (dropItem != 9) {
				return;
			}
			GetTreeBottom(i, j, out var x, out var y);
			if (Main.tile[x, y].HasTile) {
				dropItem = ModContent.ItemType<Items.Placeable.CreamWood>();
			}
			int num = Player.FindClosest(new Vector2((float)(x * 16), (float)(y * 16)), 16, 16);
			int axe = Main.player[num].inventory[Main.player[num].selectedItem].axe;
			if (genRand.Next(100) < axe || Main.rand.NextBool(3)) {
				bonusWood = true;
			}
		}

		public static bool GrowModdedTreeWithSettings(int checkedX, int checkedY, GrowTreeSettings settings) {
			int i;
			for (i = checkedY; Main.tile[checkedX, i].TileType == settings.SaplingTileType; i++) {
			}
			if (Main.tile[checkedX - 1, i - 1].LiquidAmount != 0 || Main.tile[checkedX, i - 1].LiquidAmount != 0 || Main.tile[checkedX + 1, i - 1].LiquidAmount != 0) {
				return false;
			}
			Tile tile = Main.tile[checkedX, i];
			if (!tile.HasUnactuatedTile || tile.IsHalfBlock || tile.Slope != 0) {
				return false;
			}
			bool flag = settings.WallTest(Main.tile[checkedX, i - 1].WallType);
			if (!settings.GroundTest(tile.TileType) || !flag) {
				return false;
			}
			if ((!Main.tile[checkedX - 1, i].HasTile || !settings.GroundTest(Main.tile[checkedX - 1, i].TileType)) && (!Main.tile[checkedX + 1, i].HasTile || !settings.GroundTest(Main.tile[checkedX + 1, i].TileType))) {
				return false;
			}
			TileColorCache cache = Main.tile[checkedX, i].BlockColorAndCoating();
			if (Main.tenthAnniversaryWorld && !gen && (settings.TreeTileType == 596 || settings.TreeTileType == 616)) {
				cache.Color = (byte)genRand.Next(1, 13);
			}
			int num = 2;
			int num2 = genRand.Next(settings.TreeHeightMin, settings.TreeHeightMax + 1);
			int num3 = num2 + settings.TreeTopPaddingNeeded;
			if (!EmptyTileCheck(checkedX - num, checkedX + num, i - num3, i - 1, 20)) {
				return false;
			}
			bool flag2 = false;
			bool flag3 = false;
			int num4;
			for (int j = i - num2; j < i; j++) {
				Tile tile2 = Main.tile[checkedX, j];
				tile2.TileFrameNumber = (byte)genRand.Next(3);
				tile2.HasTile = true;
				tile2.TileType = settings.TreeTileType;
				tile2.UseBlockColors(cache);
				num4 = genRand.Next(3);
				int num5 = genRand.Next(10);
				if (j == i - 1 || j == i - num2) {
					num5 = 0;
				}
				while (((num5 == 5 || num5 == 7) && flag2) || ((num5 == 6 || num5 == 7) && flag3)) {
					num5 = genRand.Next(10);
				}
				flag2 = false;
				flag3 = false;
				if (num5 == 5 || num5 == 7) {
					flag2 = true;
				}
				if (num5 == 6 || num5 == 7) {
					flag3 = true;
				}
				switch (num5) {
					case 1:
						if (num4 == 0) {
							tile2.TileFrameX = 0;
							tile2.TileFrameY = 66;
						}
						if (num4 == 1) {
							tile2.TileFrameX = 0;
							tile2.TileFrameY = 88;
						}
						if (num4 == 2) {
							tile2.TileFrameX = 0;
							tile2.TileFrameY = 110;
						}
						break;
					case 2:
						if (num4 == 0) {
							tile2.TileFrameX = 22;
							tile2.TileFrameY = 0;
						}
						if (num4 == 1) {
							tile2.TileFrameX = 22;
							tile2.TileFrameY = 22;
						}
						if (num4 == 2) {
							tile2.TileFrameX = 22;
							tile2.TileFrameY = 44;
						}
						break;
					case 3:
						if (num4 == 0) {
							tile2.TileFrameX = 44;
							tile2.TileFrameY = 66;
						}
						if (num4 == 1) {
							tile2.TileFrameX = 44;
							tile2.TileFrameY = 88;
						}
						if (num4 == 2) {
							tile2.TileFrameX = 44;
							tile2.TileFrameY = 110;
						}
						break;
					case 4:
						if (num4 == 0) {
							tile2.TileFrameX = 22;
							tile2.TileFrameY = 66;
						}
						if (num4 == 1) {
							tile2.TileFrameX = 22;
							tile2.TileFrameY = 88;
						}
						if (num4 == 2) {
							tile2.TileFrameX = 22;
							tile2.TileFrameY = 110;
						}
						break;
					case 5:
						if (num4 == 0) {
							tile2.TileFrameX = 88;
							tile2.TileFrameY = 0;
						}
						if (num4 == 1) {
							tile2.TileFrameX = 88;
							tile2.TileFrameY = 22;
						}
						if (num4 == 2) {
							tile2.TileFrameX = 88;
							tile2.TileFrameY = 44;
						}
						break;
					case 6:
						if (num4 == 0) {
							tile2.TileFrameX = 66;
							tile2.TileFrameY = 66;
						}
						if (num4 == 1) {
							tile2.TileFrameX = 66;
							tile2.TileFrameY = 88;
						}
						if (num4 == 2) {
							tile2.TileFrameX = 66;
							tile2.TileFrameY = 110;
						}
						break;
					case 7:
						if (num4 == 0) {
							tile2.TileFrameX = 110;
							tile2.TileFrameY = 66;
						}
						if (num4 == 1) {
							tile2.TileFrameX = 110;
							tile2.TileFrameY = 88;
						}
						if (num4 == 2) {
							tile2.TileFrameX = 110;
							tile2.TileFrameY = 110;
						}
						break;
					default:
						if (num4 == 0) {
							tile2.TileFrameX = 0;
							tile2.TileFrameY = 0;
						}
						if (num4 == 1) {
							tile2.TileFrameX = 0;
							tile2.TileFrameY = 22;
						}
						if (num4 == 2) {
							tile2.TileFrameX = 0;
							tile2.TileFrameY = 44;
						}
						break;
				}
				if (num5 == 5 || num5 == 7) {
					Tile tile3 = Main.tile[checkedX - 1, j];
					tile3.HasTile = true;
					tile3.TileType = settings.TreeTileType;
					tile3.UseBlockColors(cache);
					num4 = genRand.Next(3);
					if (genRand.Next(3) < 2) {
						if (num4 == 0) {
							tile3.TileFrameX = 44;
							tile3.TileFrameY = 198;
						}
						if (num4 == 1) {
							tile3.TileFrameX = 44;
							tile3.TileFrameY = 220;
						}
						if (num4 == 2) {
							tile3.TileFrameX = 44;
							tile3.TileFrameY = 242;
						}
					}
					else {
						if (num4 == 0) {
							tile3.TileFrameX = 66;
							tile3.TileFrameY = 0;
						}
						if (num4 == 1) {
							tile3.TileFrameX = 66;
							tile3.TileFrameY = 22;
						}
						if (num4 == 2) {
							tile3.TileFrameX = 66;
							tile3.TileFrameY = 44;
						}
					}
				}
				if (num5 != 6 && num5 != 7) {
					continue;
				}
				Tile tile4 = Main.tile[checkedX + 1, j];
				tile4.HasTile = true;
				tile4.TileType = settings.TreeTileType;
				tile4.UseBlockColors(cache);
				num4 = genRand.Next(3);
				if (genRand.Next(3) < 2) {
					if (num4 == 0) {
						tile4.TileFrameX = 66;
						tile4.TileFrameY = 198;
					}
					if (num4 == 1) {
						tile4.TileFrameX = 66;
						tile4.TileFrameY = 220;
					}
					if (num4 == 2) {
						tile4.TileFrameX = 66;
						tile4.TileFrameY = 242;
					}
				}
				else {
					if (num4 == 0) {
						tile4.TileFrameX = 88;
						tile4.TileFrameY = 66;
					}
					if (num4 == 1) {
						tile4.TileFrameX = 88;
						tile4.TileFrameY = 88;
					}
					if (num4 == 2) {
						tile4.TileFrameX = 88;
						tile4.TileFrameY = 110;
					}
				}
			}
			bool flag4 = false;
			bool flag5 = false;
			if (Main.tile[checkedX - 1, i].HasUnactuatedTile && !Main.tile[checkedX - 1, i].IsHalfBlock && Main.tile[checkedX - 1, i].Slope == 0 && IsTileTypeFitForTree(Main.tile[checkedX - 1, i].TileType)) {
				flag4 = true;
			}
			if (Main.tile[checkedX + 1, i].HasUnactuatedTile && !Main.tile[checkedX + 1, i].IsHalfBlock && Main.tile[checkedX + 1, i].Slope == 0 && IsTileTypeFitForTree(Main.tile[checkedX + 1, i].TileType)) {
				flag5 = true;
			}
			if (genRand.Next(3) == 0) {
				flag4 = false;
			}
			if (genRand.Next(3) == 0) {
				flag5 = false;
			}
			if (flag5) {
				Tile HasTile1 = Main.tile[checkedX + 1, i - 1];
				HasTile1.HasTile = true;
				Main.tile[checkedX + 1, i - 1].TileType = settings.TreeTileType;
				Main.tile[checkedX + 1, i - 1].UseBlockColors(cache);
				num4 = genRand.Next(3);
				if (num4 == 0) {
					Main.tile[checkedX + 1, i - 1].TileFrameX = 22;
					Main.tile[checkedX + 1, i - 1].TileFrameY = 132;
				}
				if (num4 == 1) {
					Main.tile[checkedX + 1, i - 1].TileFrameX = 22;
					Main.tile[checkedX + 1, i - 1].TileFrameY = 154;
				}
				if (num4 == 2) {
					Main.tile[checkedX + 1, i - 1].TileFrameX = 22;
					Main.tile[checkedX + 1, i - 1].TileFrameY = 176;
				}
			}
			if (flag4) {
				Tile HasTile2 = Main.tile[checkedX - 1, i - 1];
				HasTile2.HasTile = true;
				Main.tile[checkedX - 1, i - 1].TileType = settings.TreeTileType;
				Main.tile[checkedX - 1, i - 1].UseBlockColors(cache);
				num4 = genRand.Next(3);
				if (num4 == 0) {
					Main.tile[checkedX - 1, i - 1].TileFrameX = 44;
					Main.tile[checkedX - 1, i - 1].TileFrameY = 132;
				}
				if (num4 == 1) {
					Main.tile[checkedX - 1, i - 1].TileFrameX = 44;
					Main.tile[checkedX - 1, i - 1].TileFrameY = 154;
				}
				if (num4 == 2) {
					Main.tile[checkedX - 1, i - 1].TileFrameX = 44;
					Main.tile[checkedX - 1, i - 1].TileFrameY = 176;
				}
			}
			num4 = genRand.Next(3);
			if (flag4 && flag5) {
				if (num4 == 0) {
					Main.tile[checkedX, i - 1].TileFrameX = 88;
					Main.tile[checkedX, i - 1].TileFrameY = 132;
				}
				if (num4 == 1) {
					Main.tile[checkedX, i - 1].TileFrameX = 88;
					Main.tile[checkedX, i - 1].TileFrameY = 154;
				}
				if (num4 == 2) {
					Main.tile[checkedX, i - 1].TileFrameX = 88;
					Main.tile[checkedX, i - 1].TileFrameY = 176;
				}
			}
			else if (flag4) {
				if (num4 == 0) {
					Main.tile[checkedX, i - 1].TileFrameX = 0;
					Main.tile[checkedX, i - 1].TileFrameY = 132;
				}
				if (num4 == 1) {
					Main.tile[checkedX, i - 1].TileFrameX = 0;
					Main.tile[checkedX, i - 1].TileFrameY = 154;
				}
				if (num4 == 2) {
					Main.tile[checkedX, i - 1].TileFrameX = 0;
					Main.tile[checkedX, i - 1].TileFrameY = 176;
				}
			}
			else if (flag5) {
				if (num4 == 0) {
					Main.tile[checkedX, i - 1].TileFrameX = 66;
					Main.tile[checkedX, i - 1].TileFrameY = 132;
				}
				if (num4 == 1) {
					Main.tile[checkedX, i - 1].TileFrameX = 66;
					Main.tile[checkedX, i - 1].TileFrameY = 154;
				}
				if (num4 == 2) {
					Main.tile[checkedX, i - 1].TileFrameX = 66;
					Main.tile[checkedX, i - 1].TileFrameY = 176;
				}
			}
			if (genRand.Next(13) != 0) {
				num4 = genRand.Next(3);
				if (num4 == 0) {
					Main.tile[checkedX, i - num2].TileFrameX = 22;
					Main.tile[checkedX, i - num2].TileFrameY = 198;
				}
				if (num4 == 1) {
					Main.tile[checkedX, i - num2].TileFrameX = 22;
					Main.tile[checkedX, i - num2].TileFrameY = 220;
				}
				if (num4 == 2) {
					Main.tile[checkedX, i - num2].TileFrameX = 22;
					Main.tile[checkedX, i - num2].TileFrameY = 242;
				}
			}
			else {
				num4 = genRand.Next(3);
				if (num4 == 0) {
					Main.tile[checkedX, i - num2].TileFrameX = 0;
					Main.tile[checkedX, i - num2].TileFrameY = 198;
				}
				if (num4 == 1) {
					Main.tile[checkedX, i - num2].TileFrameX = 0;
					Main.tile[checkedX, i - num2].TileFrameY = 220;
				}
				if (num4 == 2) {
					Main.tile[checkedX, i - num2].TileFrameX = 0;
					Main.tile[checkedX, i - num2].TileFrameY = 242;
				}
			}
			RangeFrame(checkedX - 2, i - num2 - 1, checkedX + 2, i + 1);
			if (Main.netMode == 2) {
				NetMessage.SendTileSquare(-1, checkedX - 1, i - num2, 3, num2);
			}
			return true;
		}

		public static bool IsTileTypeFitForTree(ushort type) {
			return type == ModContent.TileType<CreamGrass>() || type == ModContent.TileType<CreamGrassMowed>();
		}

		public Texture2D GetTreeTopTexture(int tileType, int treeTextureStyle, byte tileColor) {
			Texture2D texture2D = TryGetTreeTopAndRequestIfNotReady(ConfectionWorldGeneration.confectionTree, treeTextureStyle, tileColor);
			if (texture2D == null) {
				texture2D = (Texture2D)ModContent.Request<Texture2D>(Texture + "_Tops"); //unsure if we need to add the variant code here or not
			}
			return texture2D;
		}

		public Texture2D GetTreeBranchTexture(int tileType, int treeTextureStyle, byte tileColor) {
			Texture2D texture2D = TryGetTreeBranchAndRequestIfNotReady(ConfectionWorldGeneration.confectionTree, treeTextureStyle, tileColor);
			if (texture2D == null) {
				texture2D = (Texture2D)ModContent.Request<Texture2D>(Texture + "_Branches");
			}
			return texture2D;
		}

		public class TreeTopRenderTargetHolder : ARenderTargetHolder {
			public TreeFoliageVariantKey Key;

			public override void Prepare() {
				Asset<Texture2D> asset = null;
				if (Key.TextureIndex == 0) {
						asset = ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamTree_Tops");
				}
				else if (Key.TextureIndex == 1) {
					asset = ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamTree_2_Tops");
				}
				else if (Key.TextureIndex == 2) {
					asset = ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamTree_3_Tops");
				}
				if (asset == null) {
					asset = TextureAssets.TreeTop[0];
				}
				asset.Wait?.Invoke();
				PrepareTextureIfNecessary(asset.Value);
			}

			public override void PrepareShader() {
				PrepareShader(Key.PaintColor, TreeCream);
			}
		}

		public class TreeBranchTargetHolder : ARenderTargetHolder {
			public TreeFoliageVariantKey Key;

			public override void Prepare() {
				Asset<Texture2D> asset = null;
				if (Key.TextureIndex == 0) {
					asset = ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamTree_Branches");
				}
				else if (Key.TextureIndex == 1) {
					asset = ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamTree_2_Branches");
				}
				else if (Key.TextureIndex == 2) {
					asset = ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamTree_3_Branches");
				}
				if (asset == null) {
					asset = TextureAssets.TreeBranch[0];
				}
				asset.Wait?.Invoke();
				PrepareTextureIfNecessary(asset.Value);
			}

			public override void PrepareShader() {
				PrepareShader(Key.PaintColor, TreeCream);
			}
		}

		private Dictionary<TreeFoliageVariantKey, TreeBranchTargetHolder> _treeBranchRenders = new Dictionary<TreeFoliageVariantKey, TreeBranchTargetHolder>();

		private Dictionary<TreeFoliageVariantKey, TreeTopRenderTargetHolder> _treeTopRenders = new Dictionary<TreeFoliageVariantKey, TreeTopRenderTargetHolder>();

		public void RequestTreeTop(ref TreeFoliageVariantKey lookupKey) {
			List<ARenderTargetHolder> _requests = (List<ARenderTargetHolder>)typeof(TilePaintSystemV2).GetField("_requests", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilePaintSystem);
			if (!_treeTopRenders.TryGetValue(lookupKey, out var value)) {
				value = new TreeTopRenderTargetHolder {
					Key = lookupKey
				};
				_treeTopRenders.Add(lookupKey, value);
			}
			if (!value.IsReady) {
				_requests.Add(value);
			}
		}

		public void RequestTreeBranch(ref TreeFoliageVariantKey lookupKey) {
			List<ARenderTargetHolder> _requests = (List<ARenderTargetHolder>)typeof(TilePaintSystemV2).GetField("_requests", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilePaintSystem);
			if (!_treeBranchRenders.TryGetValue(lookupKey, out var value)) {
				value = new TreeBranchTargetHolder {
					Key = lookupKey
				};
				_treeBranchRenders.Add(lookupKey, value);
			}
			if (!value.IsReady) {
				_requests.Add(value);
			}
		}

		public Texture2D TryGetTreeTopAndRequestIfNotReady(int confectionTreeVariation, int treeTopStyle, int paintColor) {
			TreeFoliageVariantKey treeFoliageVariantKey = default(TreeFoliageVariantKey);
			treeFoliageVariantKey.TextureStyle = treeTopStyle;
			treeFoliageVariantKey.PaintColor = paintColor;
			treeFoliageVariantKey.TextureIndex = confectionTreeVariation;
			TreeFoliageVariantKey lookupKey = treeFoliageVariantKey;
			if (_treeTopRenders.TryGetValue(lookupKey, out var value) && value.IsReady) {
				return (Texture2D)(object)value.Target;
			}
			RequestTreeTop(ref lookupKey);
			return null;
		}

		public Texture2D TryGetTreeBranchAndRequestIfNotReady(int confectionTreeVariation, int treeTopStyle, int paintColor) {
			TreeFoliageVariantKey treeFoliageVariantKey = default(TreeFoliageVariantKey);
			treeFoliageVariantKey.TextureStyle = treeTopStyle;
			treeFoliageVariantKey.PaintColor = paintColor;
			treeFoliageVariantKey.TextureIndex = confectionTreeVariation; //We use the tiletype as the index instead of the actual tree index since we can't insert our own index
			TreeFoliageVariantKey lookupKey = treeFoliageVariantKey;
			if (_treeBranchRenders.TryGetValue(lookupKey, out var value) && value.IsReady) {
				return (Texture2D)(object)value.Target;
			}
			RequestTreeBranch(ref lookupKey);
			return null;
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			width = 20;
			height = 20;
			if (ConfectionWorldGeneration.confectionTree > 0) {
				if (tileFrameX < 176) {
					tileFrameX = (short)(tileFrameX + 176);
				}
			}
			else {
				if (tileFrameX >= 176) {
					tileFrameX = (short)(tileFrameX - 176);
				}
			}
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			Tile tile = Main.tile[i, j];
			if (i > 5 && j > 5 && i < Main.maxTilesX - 5 && j < Main.maxTilesY - 5 && Main.tile[i, j] != null)
			{
				if (tile.HasTile)
				{
					if (Main.tileFrameImportant[Type])
					{
						CheckTreeWithSettings(i, j, new CheckTreeSettings
						{
							IsGroundValid = CreamwoodTreeGroundTest
						});
					}
				}
			} 
			return false;
		}

		public override IEnumerable<Item> GetItemDrops(int i, int j)
		{
			int dropItem = ItemID.None;
			int dropItemStack = 1;
			int secondaryItem = ItemID.None;
			Tile tileCache = Main.tile[i, j];
			bool bonusWood = false;
			KillTile_GetTreeDrops(i, j, tileCache, ref bonusWood, ref dropItem, ref secondaryItem);
			if (bonusWood)
			{
				dropItemStack++;
			}
			yield return new Item(dropItem, dropItemStack);
			yield return new Item(secondaryItem);
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			Tile tile = Main.tile[i, j];
			if (fail)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient && TileID.Sets.IsShakeable[tile.TileType])
				{
					ShakeTree(i, j);
				}
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			spriteBatch.End();
			spriteBatch.Begin(0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);
			DrawTrees(i, j);
			spriteBatch.End();
			spriteBatch.Begin(); //No params as PostDraw doesn't use spritebatch with params
		}

		private static void ShakeTree(int i, int j)
		{
			FieldInfo numTreeShakesReflect = typeof(WorldGen).GetField("numTreeShakes", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance);
			int numTreeShakes = (int)numTreeShakesReflect.GetValue(null);
			int maxTreeShakes = (int)typeof(WorldGen).GetField("maxTreeShakes", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(null);
			int[] treeShakeX = (int[])typeof(WorldGen).GetField("treeShakeX", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(null);
			int[] treeShakeY = (int[])typeof(WorldGen).GetField("treeShakeY", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(null);
			if (numTreeShakes == maxTreeShakes)
			{
				return;
			}
			GetTreeBottom(i, j, out var x, out var y);
			for (int k = 0; k < numTreeShakes; k++)
			{
				if (treeShakeX[k] == x && treeShakeY[k] == y)
				{
					return;
				}
			}
			treeShakeX[numTreeShakes] = x;
			treeShakeY[numTreeShakes] = y;
			numTreeShakesReflect.SetValue(null, ++numTreeShakes);
			y--;
			while (y > 10 && Main.tile[x, y].HasTile && TileID.Sets.IsShakeable[Main.tile[x, y].TileType])
			{
				y--;
			}
			y++;
			if (!IsTileALeafyTreeTop(x, y) || Collision.SolidTiles(x - 2, x + 2, y - 2, y + 2))
			{
				return;
			}

			if (Main.getGoodWorld && genRand.Next(17) == 0)
			{
				Projectile.NewProjectile(new EntitySource_ShakeTree(x, y), x * 16, y * 16, (float)Main.rand.Next(-100, 101) * 0.002f, 0f, ProjectileID.Bomb, 0, 0f, Main.myPlayer, 16f, 16f);
			}
			else if (genRand.Next(35) == 0 && Main.halloween)
			{
				Item.NewItem(new EntitySource_ShakeTree(x, y), x * 16, y * 16, 16, 16, ItemID.RottenEgg, genRand.Next(1, 3));
			}
			else if (genRand.Next(12) == 0)
			{
				Item.NewItem(GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, ModContent.ItemType<Items.Placeable.CreamWood>(), genRand.Next(1, 4));
			}
			else if (genRand.Next(20) == 0)
			{
				int type = ItemID.CopperCoin;
				int num2 = genRand.Next(50, 100);
				if (genRand.Next(30) == 0)
				{
					type = ItemID.GoldCoin;
					num2 = 1;
					if (genRand.Next(5) == 0)
					{
						num2++;
					}
					if (genRand.Next(10) == 0)
					{
						num2++;
					}
				}
				else if (genRand.Next(10) == 0)
				{
					type = ItemID.SilverCoin;
					num2 = genRand.Next(1, 21);
					if (genRand.Next(3) == 0)
					{
						num2 += genRand.Next(1, 21);
					}
					if (genRand.Next(4) == 0)
					{
						num2 += genRand.Next(1, 21);
					}
				}
				Item.NewItem(GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, type, num2);
			}
			else if (genRand.Next(20) == 0 && y > Main.maxTilesY - 250)
			{
				//Should probs look into the shake code again
				//NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16, y * 16, (WorldGen.genRand.NextBool(3) ? ModContent.NPCType<NPCs.AlbinoRat>() : (WorldGen.genRand.NextBool(2) ? ModContent.NPCType<NPCs.EnchantedNightmareWorm>() : ModContent.NPCType<NPCs.QuartzCrawler>())));
			}
			else if (Main.remixWorld && genRand.Next(20) == 0 && y > Main.maxTilesY - 250)
			{
				Item.NewItem(GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, ItemID.Rope, genRand.Next(20, 41));
			}
			else if (genRand.Next(12) == 0)
			{
				int secondaryItemStack = ((genRand.Next(2) != 0) ? ModContent.ItemType<Cherimoya>() : ItemID.Starfruit); //I think its time to add a secondary fruit
				Item.NewItem(GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, secondaryItemStack);
			}

			if (Main.netMode == 2)
			{
				NetMessage.SendData(112, -1, -1, null, 1, x, y, 1f, ModContent.GoreType<CreamTreeLeaf>());
			}
			if (Main.netMode == 0)
			{
				TreeGrowFX(x, y, 1, ModContent.GoreType<CreamTreeLeaf>(), hitTree: true);
			}
		}

		private static void EmitCreamLeaves(int tilePosX, int tilePosY, int grassPosX, int grassPosY)
		{
			bool _isActiveAndNotPaused = (bool)typeof(TileDrawing).GetField("_isActiveAndNotPaused", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilesRenderer);
			int _leafFrequency = (int)typeof(TileDrawing).GetField("_leafFrequency", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilesRenderer);
			UnifiedRandom _rand = (UnifiedRandom)typeof(TileDrawing).GetField("_rand", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilesRenderer);
			if (!_isActiveAndNotPaused)
			{
				return;
			}
			Tile tile = Main.tile[tilePosX, tilePosY];
			if (tile.LiquidAmount > 0)
			{
				return;
			}
			int num = 0;
			bool flag = (byte)num != 0;
			int num2 = _leafFrequency;
			bool flag2 = tilePosX - grassPosX != 0;
			if (flag)
			{
				num2 /= 2;
			}
			if (!DoesWindBlowAtThisHeight(tilePosY))
			{
				num2 = 10000;
			}
			if (flag2)
			{
				num2 *= 3;
			}
			if (_rand.Next(num2) != 0)
			{
				return;
			}
			int num3 = 2;
			Vector2 vector = new((float)(tilePosX * 16 + 8), (float)(tilePosY * 16 + 8));
			if (flag2)
			{
				int num4 = tilePosX - grassPosX;
				vector.X += num4 * 12;
				int num5 = 0;
				if (tile.TileFrameY == 220)
				{
					num5 = 1;
				}
				else if (tile.TileFrameY == 242)
				{
					num5 = 2;
				}
				if (tile.TileFrameX == 66)
				{
					switch (num5)
					{
						case 0:
							vector += new Vector2(0f, -6f);
							break;
						case 1:
							vector += new Vector2(0f, -6f);
							break;
						case 2:
							vector += new Vector2(0f, 8f);
							break;
					}
				}
				else
				{
					switch (num5)
					{
						case 0:
							vector += new Vector2(0f, 4f);
							break;
						case 1:
							vector += new Vector2(2f, -6f);
							break;
						case 2:
							vector += new Vector2(6f, -6f);
							break;
					}
				}
			}
			else
			{
				vector += new Vector2(-16f, -16f);
				if (flag)
				{
					vector.Y -= Main.rand.Next(0, 28) * 4;
				}
			}
			if (!WorldGen.SolidTile(vector.ToTileCoordinates()))
			{
				Gore.NewGoreDirect(new EntitySource_Misc(""), vector, Utils.RandomVector2(Main.rand, -num3, num3), ModContent.GoreType<CreamTreeLeaf>(), 0.7f + Main.rand.NextFloat() * 0.6f).Frame.CurrentColumn = Main.tile[tilePosX, tilePosY].TileColor;
			}
		}

		public static bool CreamwoodTreeGroundTest(int tileType)
		{
			if (tileType < 0)
			{
				return false;
			}
			if (tileType == ModContent.TileType<CreamGrass>() || tileType == ModContent.TileType<CreamGrassMowed>())
			{
				return true;
			}
			return false;
		}

		public static bool GetCreamwoodTreeFoliageData(int i, int j, int xoffset, ref int treeFrame, out int floorY, out int topTextureFrameWidth, out int topTextureFrameHeight)
		{
			int num = i + xoffset;
			CreamTreeTextureFrame(out topTextureFrameWidth, out topTextureFrameHeight);
			floorY = j;
			for (int k = 0; k < 100; k++)
			{
				floorY = j + k;
				Tile tile2 = Main.tile[num, floorY];
				if (tile2 == null)
				{
					return false;
				}
			}
			return true;
		}

		private static void CreamTreeTextureFrame(out int topTextureFrameWidth, out int topTextureFrameHeight) {
			int variant = ConfectionWorldGeneration.confectionTree;
			if (variant == 0) {
				topTextureFrameWidth = 80;
				topTextureFrameHeight = 80;
			}
			else if (variant == 1) {
				topTextureFrameWidth = 100;
				topTextureFrameHeight = 110;
			}
			else {
				topTextureFrameWidth = 100;
				topTextureFrameHeight = 110;
			}
		}

		private void DrawTrees(int k, int l)
		{
			double _treeWindCounter = (double)typeof(TileDrawing).GetField("_treeWindCounter", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilesRenderer);
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 zero = Vector2.Zero;
			float num15 = 0.08f;
			float num16 = 0.06f;
			int PositioningFix = CaptureManager.Instance.IsCapturing ? 0 : 192; //Fix to the positioning to the Branches and Tops being 192 pixels to the top and left
			int x = k;
			int y = l;
			Tile tile = Main.tile[x, y];
			if (tile == null || !tile.HasTile)
			{
				return;
			}
			short frameX = tile.TileFrameX;
			short frameY = tile.TileFrameY;
			bool flag = tile.WallType > 0;
			if (frameY >= 198 && frameX >= 22)
			{
				int treeFrame = WorldGen.GetTreeFrame(tile);
				switch (frameX)
				{
					case 22:
						{
							int num5 = 0;
							int grassPosX = x + num5;
							if (!GetCreamwoodTreeFoliageData(x, y, num5, ref treeFrame, out int floorY3, out int topTextureFrameWidth3, out int topTextureFrameHeight3))
							{
								return;
							}
							EmitCreamLeaves(x, y, grassPosX, floorY3);
							byte tileColor3 = tile.TileColor;
							Texture2D treeTopTexture = GetTreeTopTexture(Type, 0, tileColor3);
							Vector2 vector = new Vector2((float)(x * 16 - (int)unscaledPosition.X + 8 + PositioningFix), (float)(y * 16 - (int)unscaledPosition.Y + 16 + PositioningFix)) + zero;
							float num7 = 0f;
							if (!flag)
							{
								num7 = Main.instance.TilesRenderer.GetWindCycle(x, y, _treeWindCounter);
							}
							vector.X += num7 * 2f;
							vector.Y += Math.Abs(num7) * 2f;
							Color color6 = Lighting.GetColor(x, y);
							if (tile.IsTileFullbright)
							{
								color6 = Color.White;
							}
							Main.spriteBatch.Draw(treeTopTexture, vector, (Rectangle?)new Rectangle(treeFrame * (topTextureFrameWidth3 + 2), 0, topTextureFrameWidth3, topTextureFrameHeight3), color6, num7 * num15, new Vector2((float)(topTextureFrameWidth3 / 2), (float)topTextureFrameHeight3), 1f, (SpriteEffects)0, 0f);
							break;
						}
					case 44:
						{
							int num21 = x;
							int num2 = 1;
							if (!GetCreamwoodTreeFoliageData(x, y, num2, ref treeFrame, out int floorY2, out _, out _))
							{
								return;
							}
							EmitCreamLeaves(x, y, num21 + num2, floorY2);
							byte tileColor2 = tile.TileColor;
							Texture2D treeBranchTexture2 = GetTreeBranchTexture(Type, 0, tileColor2);
							Vector2 position2 = new Vector2((float)(x * 16) + PositioningFix, (float)(y * 16) + PositioningFix) - unscaledPosition.Floor() + zero + new Vector2(16f, 12f);
							float num4 = 0f;
							if (!flag)
							{
								num4 = Main.instance.TilesRenderer.GetWindCycle(x, y, _treeWindCounter);
							}
							if (num4 > 0f)
							{
								position2.X += num4;
							}
							position2.X += Math.Abs(num4) * 2f;
							Color color4 = Lighting.GetColor(x, y);
							if (tile.IsTileFullbright)
							{
								color4 = Color.White;
							}
							Main.spriteBatch.Draw(treeBranchTexture2, position2, (Rectangle?)new Rectangle(0, treeFrame * 42, 40, 40), color4, num4 * num16, new Vector2(40f, 24f), 1f, (SpriteEffects)0, 0f);
							break;
						}
					case 66:
						{
							int num17 = x;
							int num18 = -1;
							if (!GetCreamwoodTreeFoliageData(x, y, num18, ref treeFrame, out int floorY, out _, out _))
							{
								return;
							}
							EmitCreamLeaves(x, y, num17 + num18, floorY);
							byte tileColor = tile.TileColor;
							Texture2D treeBranchTexture = GetTreeBranchTexture(Type, 0, tileColor);
							Vector2 position = new Vector2((float)(x * 16) + PositioningFix, (float)(y * 16) + PositioningFix) - unscaledPosition.Floor() + zero + new Vector2(0f, 18f);
							float num20 = 0f;
							if (!flag)
							{
								num20 = Main.instance.TilesRenderer.GetWindCycle(x, y, _treeWindCounter);
							}
							if (num20 < 0f)
							{
								position.X += num20;
							}
							position.X -= Math.Abs(num20) * 2f;
							Color color2 = Lighting.GetColor(x, y);
							if (tile.IsTileFullbright)
							{
								color2 = Color.White;
							}
							Main.spriteBatch.Draw(treeBranchTexture, position, (Rectangle?)new Rectangle(42, treeFrame * 42, 40, 40), color2, num20 * num16, new Vector2(0f, 30f), 1f, (SpriteEffects)0, 0f);
							break;
						}
				}
			}
		}
	}
}