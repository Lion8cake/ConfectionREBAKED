using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;
using static Terraria.GameContent.TilePaintSystemV2;

namespace TheConfectionRebirth.Tiles.Trees {
	public class CreamPalmTree : ModTile {
		private static TreePaintingSettings PalmTreeCream = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0.5f,
			SpecialGroupMaximumHueValue = 11f / 18f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		public override void SetStaticDefaults()
		{
			Main.tileAxe[Type] = true; //Cuttable with an axe
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true; //Breaks when touching lava
			TileID.Sets.IsShakeable[Type] = true;
			TileID.Sets.GetsCheckedForLeaves[Type] = true;
			TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
			TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
			TileID.Sets.PreventsSandfall[Type] = true;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(151, 107, 75), name);
			DustType = ModContent.DustType<CreamwoodDust>();
		}

		public Texture2D GetTreeTopTexture(int tileType, int treeTextureStyle, byte tileColor)
		{
			Texture2D texture2D = TryGetTreeTopAndRequestIfNotReady(ConfectionWorldGeneration.confectionTree, treeTextureStyle, tileColor);
			if (texture2D == null)
			{
				texture2D = (Texture2D)ModContent.Request<Texture2D>(Texture + "_Tops");
			}
			return texture2D;
		}

		public class TreeTopRenderTargetHolder : ARenderTargetHolder
		{
			public TreeFoliageVariantKey Key;

			public override void Prepare()
			{
				Asset<Texture2D> asset = null;
				if (Key.TextureIndex == 0)
				{
					asset = ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamPalmTree_Tops");
				}
				else if (Key.TextureIndex == 1)
				{
					asset = ModContent.Request<Texture2D>("TheConfectionRebirth/Tiles/Trees/CreamPalmOasisTree_Tops");
				}
				if (asset == null)
				{
					asset = TextureAssets.TreeTop[0];
				}
				asset.Wait?.Invoke();
				PrepareTextureIfNecessary(asset.Value);
			}

			public override void PrepareShader()
			{
				PrepareShader(Key.PaintColor, PalmTreeCream);
			}
		}

		private Dictionary<TreeFoliageVariantKey, TreeTopRenderTargetHolder> _treeTopRenders = new Dictionary<TreeFoliageVariantKey, TreeTopRenderTargetHolder>();

		public Texture2D TryGetTreeTopAndRequestIfNotReady(int OasisOrNot, int treeTopStyle, int paintColor)
		{
			TreeFoliageVariantKey treeFoliageVariantKey = default(TreeFoliageVariantKey);
			treeFoliageVariantKey.TextureStyle = treeTopStyle;
			treeFoliageVariantKey.PaintColor = paintColor;
			treeFoliageVariantKey.TextureIndex = OasisOrNot;
			TreeFoliageVariantKey lookupKey = treeFoliageVariantKey;
			if (_treeTopRenders.TryGetValue(lookupKey, out var value) && value.IsReady)
			{
				return (Texture2D)(object)value.Target;
			}
			RequestTreeTop(ref lookupKey);
			return null;
		}

		public void RequestTreeTop(ref TreeFoliageVariantKey lookupKey)
		{
			List<ARenderTargetHolder> _requests = (List<ARenderTargetHolder>)typeof(TilePaintSystemV2).GetField("_requests", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilePaintSystem);
			if (!_treeTopRenders.TryGetValue(lookupKey, out var value))
			{
				value = new TreeTopRenderTargetHolder
				{
					Key = lookupKey
				};
				_treeTopRenders.Add(lookupKey, value);
			}
			if (!value.IsReady)
			{
				_requests.Add(value);
			}
		}

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{
			//if (drawData.tileCache.TileFrameX <= 132 && drawData.tileCache.TileFrameX >= 88)
			//{
				//return;
			//}
			//vector.X += drawData.tileCache.TileFrameY;
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			width = 20;
			height = 20;
			bool isOcean = false;
			if (i >= WorldGen.beachDistance && i <= Main.maxTilesX - WorldGen.beachDistance)
			{
				isOcean = true;
			}
			if (isOcean)
			{
				tileFrameY = 22;
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
						WorldGen.CheckPalmTree(i, j);
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
			if (Main.tenthAnniversaryWorld)
			{
				dropItemStack += WorldGen.genRand.Next(2, 5);
			}
			if (tileCache.TileFrameX <= 132 && tileCache.TileFrameX >= 88)
			{
				secondaryItem = ItemID.Acorn;
			}
			int y;
			for (y = j; !Main.tile[i, y].HasTile || !Main.tileSolid[Main.tile[i, y].TileType]; y++)
			{
			}
			if (Main.tile[i, y].HasTile)
			{
				dropItem = ModContent.ItemType<Items.Placeable.CreamWood>();
			}
			yield return new Item(dropItem, dropItemStack);
			yield return new Item(secondaryItem);
		}

		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			blockDamaged = false;
			Tile tile = Main.tile[i, j];
			Tile tile2 = default(Tile);
			if (j >= 1)
			{
				tile2 = Main.tile[i, j - 1];
			}
			int type;
			if (tile2 != null && tile2.HasTile)
			{
				type = tile2.TileType;
				if (TileID.Sets.IsATreeTrunk[type] && tile.TileType != type && (tile2.TileFrameX != 66 || tile2.TileFrameY < 0 || tile2.TileFrameY > 44) && (tile2.TileFrameX != 88 || tile2.TileFrameY < 66 || tile2.TileFrameY > 110) && tile2.TileFrameY < 198)
				{
					return false;
				}
				if (tile.TileType != type && (tile2.TileFrameX == 66 || tile2.TileFrameX == 220))
				{
					return false;
				}
			}
			return true;
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if (Main.tile[i, j].TileFrameX <= 132 && Main.tile[i, j].TileFrameX >= 88)
			{
				return false;
			}
			return true;
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);
			DrawPalmTrees(i, j, spriteBatch);
			spriteBatch.End();
			spriteBatch.Begin(); //No params as PostDraw doesn't use spritebatch with params
		}

		private void DrawPalmTrees(int k, int l, SpriteBatch spriteBatch)
		{
			double _treeWindCounter = (double)typeof(TileDrawing).GetField("_treeWindCounter", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilesRenderer);
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
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

			if (frameX < 88 || frameX > 132)
			{
				return;
			}
			int num8 = 0;
			switch (frameX)
			{
				case 110:
					num8 = 1;
					break;
				case 132:
					num8 = 2;
					break;
			}
			int num9 = 80;
			int num10 = 80;
			int num11 = 32;
			int num13 = 0;
			int textureIndex = 0;
			bool isOcean = false;
			if (x >= WorldGen.beachDistance && x <= Main.maxTilesX - WorldGen.beachDistance)
			{
				isOcean = true;
			}
			if (isOcean)
			{
				textureIndex = 1;
				num9 = 114;
				num10 = 98;
				num11 = 48;
				num13 = 2;
			}

			int frameY2 = Main.tile[x, y].TileFrameY;
			byte tileColor4 = tile.TileColor;
			Texture2D treeTopTexture2 = GetTreeTopTexture(textureIndex, 0, tileColor4);
			Vector2 position3 = new Vector2((float)(x * 16 - (int)unscaledPosition.X - num11 + frameY2 + num9 / 2), (float)(y * 16 - (int)unscaledPosition.Y + 16 + num13)) + zero;
			float num14 = 0f;
			if (!flag)
			{
				num14 = Main.instance.TilesRenderer.GetWindCycle(x, y, _treeWindCounter);
			}
			position3.X += num14 * 2f;
			position3.Y += Math.Abs(num14) * 2f;
			Color color7 = Lighting.GetColor(x, y);
			if (tile.IsTileFullbright)
			{
				color7 = Color.White;
			}
			spriteBatch.Draw(treeTopTexture2, position3, (Rectangle?)new Rectangle(num8 * (num9 + 2), 0, num9, num10), color7, num14 * num15, new Vector2((float)(num9 / 2), (float)num10), 1f, (SpriteEffects)0, 0f);
		}
	}
}
