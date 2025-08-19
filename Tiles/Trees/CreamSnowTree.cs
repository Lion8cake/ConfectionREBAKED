using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Drawing;
using System;
using Terraria.GameContent;
using System.Reflection;
using System.Collections.Generic;
using static Terraria.WorldGen;
using static Terraria.GameContent.TilePaintSystemV2;
using ReLogic.Content;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles.Trees
{
    public class CreamSnowTree : ModTile
    {
		public static GrowTreeSettings Tree_CreamSnow = new GrowTreeSettings
		{
			GroundTest = CreamsnowTreeGroundTest,
			WallTest = DefaultTreeWallTest,
			TreeHeightMax = 12,
			TreeHeightMin = 7,
			TreeTileType = (ushort)ModContent.TileType<CreamSnowTree>(),
			TreeTopPaddingNeeded = 4,
			SaplingTileType = (ushort)ModContent.TileType<CreamSnowSapling>()
		};

		public static TreePaintingSettings TreeCreamSnow = new TreePaintingSettings
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
			Main.tileBlockLight[Type] = false;
			Main.tileLavaDeath[Type] = false;
			TileID.Sets.IsATreeTrunk[Type] = true;
            TileID.Sets.IsShakeable[Type] = true;
            TileID.Sets.GetsDestroyedForMeteors[Type] = true;
            TileID.Sets.GetsCheckedForLeaves[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(151, 107, 75), name);
			DustType = ModContent.DustType<CreamwoodDust>();
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
				asset = ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamSnowTree_Tops");
				asset.Wait?.Invoke();
				PrepareTextureIfNecessary(asset.Value);
			}

			public override void PrepareShader() {
				PrepareShader(Key.PaintColor, TreeCreamSnow);
			}
		}

		public class TreeBranchTargetHolder : ARenderTargetHolder {
			public TreeFoliageVariantKey Key;

			public override void Prepare() {
				Asset<Texture2D> asset = null;
				asset = ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamSnowTree_Branches");
				if (asset == null) {
					asset = TextureAssets.TreeBranch[0];
				}
				asset.Wait?.Invoke();
				PrepareTextureIfNecessary(asset.Value);
			}

			public override void PrepareShader() {
				PrepareShader(Key.PaintColor, TreeCreamSnow);
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
			treeFoliageVariantKey.TextureIndex = confectionTreeVariation;
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
						CheckTree(i, j);
						/*CheckTreeWithSettings(i, j, new CheckTreeSettings
						{
							IsGroundValid = CreamwoodTreeGroundTest
						});*/
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
			CreamTree.KillTile_GetTreeDrops(i, j, tileCache, ref bonusWood, ref dropItem, ref secondaryItem);
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
					CreamTree.ShakeTree(i, j);
				}
			}
		}

		public override void NearbyEffects(int i, int j, bool closer) {
			GetTreeBottom(i, j, out var x, out var y);
			Tile tilebelow = Main.tile[x, y + 1];
			Tile tilecurrent = Main.tile[x, y];
			if (tilebelow.TileType != ModContent.TileType<CreamBlock>() && tilecurrent.TileType != ModContent.TileType<CreamBlock>()) {
				Main.tile[i, j].TileType = TileID.Trees;
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);
			DrawTrees(i, j, spriteBatch);
			spriteBatch.End();
			spriteBatch.Begin(); //No params as PostDraw doesn't use spritebatch with params
		}

		public static bool CreamsnowTreeGroundTest(int tileType)
		{
			if (tileType < 0)
			{
				return false;
			}
			if (tileType == ModContent.TileType<CreamBlock>())
			{
				return true;
			}
			return false;
		}

		public static bool GetCreamsnowTreeFoliageData(int i, int j, int xoffset, ref int treeFrame, out int floorY, out int topTextureFrameWidth, out int topTextureFrameHeight)
		{
			int num = i + xoffset;
			CreamSnowTreeTextureFrame(i, ref treeFrame, out topTextureFrameWidth, out topTextureFrameHeight);
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

		private static void CreamSnowTreeTextureFrame(int i, ref int treeFrame, out int topTextureFrameWidth, out int topTextureFrameHeight) {
			topTextureFrameWidth = 80;
			topTextureFrameHeight = 80;
		}

		private void DrawTrees(int k, int l, SpriteBatch spriteBatch)
		{
			double _treeWindCounter = (double)typeof(TileDrawing).GetField("_treeWindCounter", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilesRenderer);
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen) {
				zero = Vector2.Zero;
			}
			float num15 = 0.08f;
			float num16 = 0.06f;
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
							if (!GetCreamsnowTreeFoliageData(x, y, num5, ref treeFrame, out int floorY3, out int topTextureFrameWidth3, out int topTextureFrameHeight3))
							{
								return;
							}
							CreamTree.EmitCreamLeaves(x, y, grassPosX, floorY3);
							byte tileColor3 = tile.TileColor;
							Texture2D treeTopTexture = GetTreeTopTexture(Type, 0, tileColor3);
							Vector2 vector = new Vector2((float)(x * 16 - (int)unscaledPosition.X + 8), (float)(y * 16 - (int)unscaledPosition.Y + 16)) + zero;
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
							spriteBatch.Draw(treeTopTexture, vector, (Rectangle?)new Rectangle(treeFrame * (topTextureFrameWidth3 + 2), 0, topTextureFrameWidth3, topTextureFrameHeight3), color6, num7 * num15, new Vector2((float)(topTextureFrameWidth3 / 2), (float)topTextureFrameHeight3), 1f, (SpriteEffects)0, 0f);
							break;
						}
					case 44:
						{
							int num21 = x;
							int num2 = 1;
							if (!GetCreamsnowTreeFoliageData(x, y, num2, ref treeFrame, out int floorY2, out _, out _))
							{
								return;
							}
							CreamTree.EmitCreamLeaves(x, y, num21 + num2, floorY2);
							byte tileColor2 = tile.TileColor;
							Texture2D treeBranchTexture2 = GetTreeBranchTexture(Type, 0, tileColor2);
							Vector2 position2 = new Vector2((float)(x * 16), (float)(y * 16)) - unscaledPosition.Floor() + zero + new Vector2(16f, 12f);
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
							spriteBatch.Draw(treeBranchTexture2, position2, (Rectangle?)new Rectangle(0, treeFrame * 42, 40, 40), color4, num4 * num16, new Vector2(40f, 24f), 1f, (SpriteEffects)0, 0f);
							break;
						}
					case 66:
						{
							int num17 = x;
							int num18 = -1;
							if (!GetCreamsnowTreeFoliageData(x, y, num18, ref treeFrame, out int floorY, out _, out _))
							{
								return;
							}
							CreamTree.EmitCreamLeaves(x, y, num17 + num18, floorY);
							byte tileColor = tile.TileColor;
							Texture2D treeBranchTexture = GetTreeBranchTexture(Type, 0, tileColor);
							Vector2 position = new Vector2((float)(x * 16), (float)(y * 16)) - unscaledPosition.Floor() + zero + new Vector2(0f, 18f);
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
							spriteBatch.Draw(treeBranchTexture, position, (Rectangle?)new Rectangle(42, treeFrame * 42, 40, 40), color2, num20 * num16, new Vector2(0f, 30f), 1f, (SpriteEffects)0, 0f);
							break;
						}
				}
			}
		}
	}
}