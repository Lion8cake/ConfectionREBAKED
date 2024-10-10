using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Liquid;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Items;
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
			AddMapEntry(new Color(182, 141, 86));
			DustType = ModContent.DustType<CreamwoodDust>();
		}

		private int GetPalmTreeType(int tileX, int tileY)
		{
			int i;
			for (i = tileY; Main.tile[tileX, i].HasTile && Main.tile[tileX, i].TileType == Type; i++)
			{
			}
			return GetPalmTreeVariant(tileX, i);
		}

		private int GetPalmTreeVariant(int x, int y)
		{
			int num = -1;
			if (Main.tile[x, y].HasTile && Main.tile[x, y].TileType == ModContent.TileType<Creamsand>())
			{
				num = 0;
			}
			if (WorldGen.IsPalmOasisTree(x))
			{
				num += 1;
			}
			return num;
		}

		public Texture2D GetTreeTopTexture(int tileType, int treeTextureStyle, byte tileColor)
		{
			Texture2D texture2D = TryGetTreeTopAndRequestIfNotReady(tileType, treeTextureStyle, tileColor);
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

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			width = 20;
			height = 20;
			if (GetPalmTreeType(i, j) == 1)
			{
				tileFrameY = 22;
			}
			else
			{
				tileFrameY = 0;
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
			ThreadLocal<TileDrawInfo> _currentTileDrawInfo = (ThreadLocal<TileDrawInfo>)typeof(TileDrawing).GetField("_currentTileDrawInfo", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilesRenderer);
			DrawSingleTile(_currentTileDrawInfo.Value, Main.Camera.UnscaledPosition, new Vector2(Main.offScreenRange, Main.offScreenRange), i, j);
			return false;
		}

		#region trunk rendering
		private void DrawSingleTile(TileDrawInfo drawData, Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY)
		{
			drawData.tileCache = Main.tile[tileX, tileY];
			drawData.typeCache = drawData.tileCache.TileType;
			drawData.tileFrameX = drawData.tileCache.TileFrameX;
			drawData.tileFrameY = drawData.tileCache.TileFrameY;
			drawData.tileLight = Lighting.GetColor(tileX, tileY);
			Main.instance.TilesRenderer.GetTileDrawData(tileX, tileY, drawData.tileCache, drawData.typeCache, ref drawData.tileFrameX, ref drawData.tileFrameY, out drawData.tileWidth, out drawData.tileHeight, out drawData.tileTop, out drawData.halfBrickHeight, out drawData.addFrX, out drawData.addFrY, out drawData.tileSpriteEffect, out drawData.glowTexture, out drawData.glowSourceRect, out drawData.glowColor);
			drawData.drawTexture = Main.instance.TilesRenderer.GetTileDrawTexture(drawData.tileCache, tileX, tileY);
			Rectangle empty = Rectangle.Empty;
			Color highlightColor = Color.Transparent;
			bool flag = false;
			if (drawData.tileLight.R >= 1 || drawData.tileLight.G >= 1 || drawData.tileLight.B >= 1)
			{
				flag = true;
			}
			if (drawData.tileCache.WallType > 0 && (drawData.tileCache.WallType == 318 || drawData.tileCache.IsWallFullbright))
			{
				flag = true;
			}
			flag &= TileDrawing.IsVisible(drawData.tileCache);
			Rectangle rectangle = new(drawData.tileFrameX + drawData.addFrX, drawData.tileFrameY + drawData.addFrY, drawData.tileWidth, drawData.tileHeight - drawData.halfBrickHeight);
			Vector2 vector = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - ((float)drawData.tileWidth - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + drawData.tileTop + drawData.halfBrickHeight)) + screenOffset;
			if (!flag)
			{
				return;
			}
			drawData.colorTint = Color.White;
			drawData.finalColor = GetFinalLight(drawData.tileCache, drawData.typeCache, drawData.tileLight, drawData.colorTint);

			if (drawData.tileCache.TileFrameX <= 132 && drawData.tileCache.TileFrameX >= 88)
			{
				return;
			}
			vector.X += drawData.tileCache.TileFrameY;

			DrawBasicTile(screenPosition, screenOffset, tileX, tileY, drawData, rectangle, vector);
		}

		private void DrawBasicTile(Vector2 screenPosition, Vector2 screenOffset, int tileX, int tileY, TileDrawInfo drawData, Rectangle normalTileRect, Vector2 normalTilePosition)
		{
			typeof(TileDrawing).GetMethod("DrawBasicTile", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(Main.instance.TilesRenderer, new object[] { screenPosition, screenOffset, tileX, tileY, drawData, normalTileRect, normalTilePosition });
		}

		private static Color GetFinalLight(Tile tileCache, ushort typeCache, Color tileLight, Color tint)
		{
			int num = (int)((float)(tileLight.R * tint.R) / 255f);
			int num2 = (int)((float)(tileLight.G * tint.G) / 255f);
			int num3 = (int)((float)(tileLight.B * tint.B) / 255f);
			if (num > 255)
			{
				num = 255;
			}
			if (num2 > 255)
			{
				num2 = 255;
			}
			if (num3 > 255)
			{
				num3 = 255;
			}
			num3 <<= 16;
			num2 <<= 8;
			tileLight.PackedValue = (uint)(num | num2 | num3) | 0xFF000000u;
			if (tileCache.IsTileFullbright)
			{
				tileLight = Color.White;
			}
			if (tileCache.IsActuated)
			{
				tileLight = ActColor(tileLight, tileCache);
			}
			else if (ShouldTileShine(typeCache, tileCache.TileFrameX))
			{
				tileLight = Main.shine(tileLight, typeCache);
			}
			return tileLight;
		}

		private static bool ShouldTileShine(ushort type, short frameX)
		{
			return (bool)typeof(TileDrawing).GetMethod("ShouldTileShine", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(Main.instance.TilesRenderer, new object[] { type, frameX });
		}

		private static Color ActColor(Color oldColor, Tile tile)
		{
			if (!tile.IsActuated)
			{
				return oldColor;
			}
			double num = 0.4;
			return new Color((int)(byte)(num * (double)(int)oldColor.R), (int)(byte)(num * (double)(int)oldColor.G), (int)(byte)(num * (double)(int)oldColor.B), (int)oldColor.A);
		}
		#endregion

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

		public override void NearbyEffects(int i, int j, bool closer)
		{
			GetTreeBottom(i, j, out var x, out var y);
			Tile tilebelow = Main.tile[x, y + 1];
			Tile tilecurrent = Main.tile[x, y];
			if (tilebelow.TileType != ModContent.TileType<Creamsand>() && tilecurrent.TileType != ModContent.TileType<Creamsand>())
			{
				Main.tile[i, j].TileType = TileID.PalmTree;
			}
		}

		public void GetTreeBottom(int i, int j, out int x, out int y)
		{
			x = i;
			y = j;
			Tile tileSafely = Framing.GetTileSafely(x, y);
			while (y < Main.maxTilesY - 50 && (!tileSafely.HasTile || tileSafely.TileType == Type))
			{
				y++;
				tileSafely = Framing.GetTileSafely(x, y);
			}
		}

		public static bool IsTileALeafyTreeTop(int i, int j)
		{
			Tile tileSafely = Framing.GetTileSafely(i, j);
			if (tileSafely.HasTile && TileID.Sets.GetsCheckedForLeaves[tileSafely.TileType])
			{
				if (tileSafely.TileFrameX >= 88)
				{
					return true;
				}
			}
			return false;
		}

		private void ShakeTree(int i, int j)
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
			if (Main.getGoodWorld && WorldGen.genRand.NextBool(17))
			{
				Projectile.NewProjectile(new EntitySource_ShakeTree(x, y), x * 16, y * 16, (float)Main.rand.Next(-100, 101) * 0.002f, 0f, ProjectileID.Bomb, 0, 0f, Main.myPlayer, 16f, 16f);
			}
			else if (WorldGen.genRand.NextBool(7))
			{
				Item.NewItem(new EntitySource_ShakeTree(x, y), x * 16, y * 16, 16, 16, ItemID.Acorn, WorldGen.genRand.Next(1, 3));
			}
			else if (WorldGen.genRand.NextBool(35) && Main.halloween)
			{
				Item.NewItem(new EntitySource_ShakeTree(x, y), x * 16, y * 16, 16, 16, ItemID.RottenEgg, WorldGen.genRand.Next(1, 3));
			}
			else if (WorldGen.genRand.NextBool(12))
			{
				Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, ModContent.ItemType<Items.Placeable.CreamWood>(), WorldGen.genRand.Next(1, 4));
			}
			else if (WorldGen.genRand.NextBool(20))
			{
				int type = ItemID.CopperCoin;
				int num2 = WorldGen.genRand.Next(50, 100);
				if (WorldGen.genRand.NextBool(30))
				{
					type = ItemID.GoldCoin;
					num2 = 1;
					if (WorldGen.genRand.NextBool(5))
					{
						num2++;
					}
					if (WorldGen.genRand.NextBool(10))
					{
						num2++;
					}
				}
				else if (WorldGen.genRand.NextBool(10))
				{
					type = ItemID.SilverCoin;
					num2 = WorldGen.genRand.Next(1, 21);
					if (WorldGen.genRand.NextBool(3))
					{
						num2 += WorldGen.genRand.Next(1, 21);
					}
					if (WorldGen.genRand.NextBool(4))
					{
						num2 += WorldGen.genRand.Next(1, 21);
					}
				}
				Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, type, num2);
			}
			else if (WorldGen.genRand.NextBool(15))
			{
				int type2 = Main.rand.NextFromList(new short[4] { (short)ModContent.NPCType<NPCs.Pip>(), (short)ModContent.NPCType<NPCs.Birdnana>(), NPCID.Squirrel, NPCID.SquirrelRed });
				if (Player.GetClosestRollLuck(x, y, NPC.goldCritterChance) == 0f)
				{
					type2 = ((!WorldGen.genRand.NextBool(2)) ? NPCID.SquirrelGold : NPCID.GoldBird);
				}
				NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16, y * 16, type2);
			}
			else if (WorldGen.genRand.NextBool(50) && !Main.dayTime)
			{
				int type3 = Main.rand.NextFromList(new short[3] { NPCID.FairyCritterPink, NPCID.FairyCritterGreen, NPCID.FairyCritterBlue });
				if (Main.tenthAnniversaryWorld && !Main.rand.NextBool(4))
				{
					type3 = NPCID.FairyCritterPink;
				}
				NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16, y * 16, type3);
			}
			else if (WorldGen.genRand.NextBool(50))
			{
				Point point;
				for (int l = 0; l < 5; l++)
				{
					point = new(x + Main.rand.Next(-2, 2), y - 1 + Main.rand.Next(-2, 2));
					int type4 = ((Player.GetClosestRollLuck(x, y, NPC.goldCritterChance) != 0f) ? Main.rand.NextFromList(new short[2] { (short)ModContent.NPCType<NPCs.Pip>(), (short)ModContent.NPCType<NPCs.Birdnana>() }) : NPCID.GoldBird);
					NPC obj3 = Main.npc[NPC.NewNPC(new EntitySource_ShakeTree(x, y), point.X * 16, point.Y * 16, type4)];
					obj3.velocity = Main.rand.NextVector2CircularEdge(3f, 3f);
					obj3.netUpdate = true;
				}
			}
			else if (WorldGen.genRand.NextBool(20) && !WorldGen.IsPalmOasisTree(x))
			{
				NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16, y * 16, NPCID.Seagull2);
			}
			else if (WorldGen.genRand.NextBool(20) && !Main.raining && !NPC.TooWindyForButterflies && Main.dayTime)
			{
				int type5 = ModContent.NPCType<NPCs.GrumbleBee>();
				if (Player.GetClosestRollLuck(x, y, NPC.goldCritterChance) == 0f)
				{
					type5 = NPCID.GoldButterfly;
				}
				NPC.NewNPC(new EntitySource_ShakeTree(x, y), x * 16, y * 16, type5);
			}
			else if (WorldGen.genRand.NextBool(12) && !WorldGen.IsPalmOasisTree(x))
			{
				int secondaryItemStack = ((!WorldGen.genRand.NextBool(2)) ? ItemID.Coconut : ItemID.Banana);
				Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, secondaryItemStack);
			}
			else if (WorldGen.genRand.NextBool(12))
			{
				int secondaryItemStack = ((!WorldGen.genRand.NextBool(2)) ? ModContent.ItemType<Cherimoya>() : ModContent.ItemType<CocoaBeans>());
				Item.NewItem(WorldGen.GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, secondaryItemStack);
			}
			if (Main.netMode == NetmodeID.Server)
			{
				NetMessage.SendData(MessageID.SpecialFX, -1, -1, null, 1, x, y, 1f, ModContent.GoreType<CreamTreeLeaf>());
			}
			if (Main.netMode == NetmodeID.Server)
			{
				NetMessage.SendData(MessageID.SpecialFX, -1, -1, null, 1, x, y, 1f, ModContent.GoreType<CreamTreeLeaf>());
			}
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				WorldGen.TreeGrowFX(x, y, 1, ModContent.GoreType<CreamTreeLeaf>(), hitTree: true);
			}
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
			int textureIndex = GetPalmTreeType(x, y);
			if (textureIndex == 1)
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

	public class CreamPalmTreeSquirelHook : GlobalProjectile
	{
		public override bool? GrappleCanLatchOnTo(Projectile projectile, Player player, int x, int y)
		{
			if (projectile.type == ProjectileID.SquirrelHook && Main.tile[x, y].TileType == ModContent.TileType<CreamPalmTree>())
				return true;
			else
				return null;
		}

		public override void Load()
		{
			On_WorldGen.GetTileVisualHitbox += HookVisualDisplacement;
		}

		public override void Unload()
		{
			On_WorldGen.GetTileVisualHitbox -= HookVisualDisplacement;
		}

		private Rectangle? HookVisualDisplacement(On_WorldGen.orig_GetTileVisualHitbox orig, int x, int y)
		{
			Rectangle? rect = orig.Invoke(x, y);
			Rectangle value;
			if (rect != null)
			{
				value = (Rectangle)rect;
			}
			else
			{
				return null;
			}
            Tile tile = Main.tile[x, y];
			if (tile == null || !tile.HasUnactuatedTile)
			{
				return null;
			}
			if (tile.TileType == ModContent.TileType<CreamPalmTree>())
			{
				value.X += tile.TileFrameY;
			}
			return value;
		}
	}
}
