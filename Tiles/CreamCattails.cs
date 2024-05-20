using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheConfectionRebirth.Dusts;
using static Mono.CompilerServices.SymbolWriter.CodeBlockEntry;

namespace TheConfectionRebirth.Tiles
{
	public class CreamCattails : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileLighted[Type] = true;

			TileID.Sets.TileCutIgnore.Regrowth[Type] = true;

			AddMapEntry(new Color(235, 207, 150));
			DustType = ModContent.DustType<CreamGrassDust>();
			HitSound = SoundID.Grass;
		}

		public override void RandomUpdate(int i, int j) {
			ConfectionWorldGeneration.CheckCreamCatTail(i, j);
			if (Main.tile[i, j].HasTile && WorldGen.genRand.NextBool(8)) {
				WorldGen.GrowCatTail(i, j);
				ConfectionWorldGeneration.CheckCreamCatTail(i, j);
			}
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak) {
			ConfectionWorldGeneration.CheckCreamCatTail(i, j);
			return false;
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) {
			spriteBatch.End();
			spriteBatch.Begin(0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);
			bool intoRenderTargets = true;
			bool flag = intoRenderTargets || Main.LightingEveryFrame;

			if (Main.tile[i, j].TileFrameX / 18 <= 4 && flag) {
				DrawMultiTileGrass(i, j, spriteBatch);
			}
			spriteBatch.End();
			spriteBatch.Begin(); //No params as PostDraw doesn't use spritebatch with params
			return false;
		}

		private void DrawMultiTileGrass(int x, int num3, SpriteBatch unused) {
			Vector2 unscaledPosition = Main.Camera.UnscaledPosition;
			Vector2 zero = Vector2.Zero;
			int sizeX = 1;
			int num4 = 1;
			Tile tile = Main.tile[x, num3];
			if (tile != null && tile.HasTile) {
				sizeX = 1;
				num4 = TheConfectionRebirth.ClimbCreamCatTail(x, num3);
				num3 -= num4 - 1;
				DrawMultiTileGrassInWind(unscaledPosition, zero, x, num3, sizeX, num4);
			}
		}

		private void DrawMultiTileGrassInWind(Vector2 screenPosition, Vector2 offSet, int topLeftX, int topLeftY, int sizeX, int sizeY) {
			double _sunflowerWindCounter = (double)typeof(TileDrawing).GetField("_sunflowerWindCounter", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilesRenderer);
			float windCycle = Main.instance.TilesRenderer.GetWindCycle(topLeftX, topLeftY, _sunflowerWindCounter);
			new Vector2((float)(sizeX * 16) * 0.5f, (float)(sizeY * 16));
			int PositioningFix = CaptureManager.Instance.IsCapturing ? 0 : 192; //Fix to the positioning to the tiles being 192 pixels to the top and left
			offSet = new(PositioningFix, PositioningFix);
			Vector2 vector = new Vector2((float)(topLeftX * 16 - (int)screenPosition.X) + (float)sizeX * 16f * 0.5f, (float)(topLeftY * 16 - (int)screenPosition.Y + 16 * sizeY)) + offSet;
			float num = 0.07f;
			int type = Main.tile[topLeftX, topLeftY].TileType;
			Texture2D texture2D = null;
			Color color = Color.Transparent;
			bool flag = WorldGen.InAPlaceWithWind(topLeftX, topLeftY, sizeX, sizeY);
			if (type == ModContent.TileType<CreamCattails>()) {
				flag = WorldGen.InAPlaceWithWind(topLeftX, topLeftY, sizeX, 1);
			}
			Vector2 vector3 = default(Vector2);
			for (int i = topLeftX; i < topLeftX + sizeX; i++) {
				for (int j = topLeftY; j < topLeftY + sizeY; j++) {
					Tile tile = Main.tile[i, j];
					ushort type2 = tile.TileType;
					if (type2 != type || !TileDrawing.IsVisible(tile)) {
						continue;
					}
					Math.Abs(((float)(i - topLeftX) + 0.5f) / (float)sizeX - 0.5f);
					short tileFrameX = tile.TileFrameX;
					short tileFrameY = tile.TileFrameY;
					float num2 = 1f - (float)(j - topLeftY + 1) / (float)sizeY;
					if (num2 == 0f) {
						num2 = 0.1f;
					}
					if (!flag) {
						num2 = 0f;
					}
					Main.instance.TilesRenderer.GetTileDrawData(i, j, tile, type2, ref tileFrameX, ref tileFrameY, out var tileWidth, out var tileHeight, out var tileTop, out var halfBrickHeight, out var addFrX, out var addFrY, out var tileSpriteEffect, out var _, out var _, out var _);
					bool flag2 = Main.rand.NextBool(4);
					Color tileLight = Lighting.GetColor(i, j);
					//As far as I know cattails dont use these, so they'll keep as comments until needed (watch relogic add particle effects to cattails)
					/*DrawAnimatedTile_AdjustForVisionChangers(i, j, tile, type2, tileFrameX, tileFrameY, ref tileLight, flag2);
					tileLight = DrawTiles_GetLightOverride(j, i, tile, type2, tileFrameX, tileFrameY, tileLight);
					if (_isActiveAndNotPaused && flag2) {
						DrawTiles_EmitParticles(j, i, tile, type2, tileFrameX, tileFrameY, tileLight);
					}*/
					Vector2 vector2 = new Vector2((float)(i * 16 - (int)screenPosition.X), (float)(j * 16 - (int)screenPosition.Y + tileTop)) + offSet;
					vector3 = new(windCycle * 1f, Math.Abs(windCycle) * 2f * num2);
					Vector2 origin = vector - vector2;
					Texture2D tileDrawTexture = Main.instance.TilesRenderer.GetTileDrawTexture(tile, i, j);
					if (tileDrawTexture != null) {
						Main.spriteBatch.Draw(tileDrawTexture, vector + new Vector2(0f, vector3.Y), (Rectangle?)new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), tileLight, windCycle * num * num2, origin, 1f, tileSpriteEffect, 0f);
						if (texture2D != null) {
							Main.spriteBatch.Draw(texture2D, vector + new Vector2(0f, vector3.Y), (Rectangle?)new Rectangle(tileFrameX + addFrX, tileFrameY + addFrY, tileWidth, tileHeight - halfBrickHeight), color, windCycle * num * num2, origin, 1f, tileSpriteEffect, 0f);
						}
					}
				}
			}
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) {
			offsetY = 2;
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) {
			if (i % 2 == 0) {
				spriteEffects = (SpriteEffects)1;
			}
		}
	}


	public class CattailGen : GlobalTile {
		public override void RandomUpdate(int i, int j, int type) {
			if (j >= Main.worldSurface) {
				if (Main.tile[i, j].LiquidAmount > 32) {
					if (WorldGen.genRand.NextBool(600)) {
						WorldGen.PlaceTile(i, j, ModContent.TileType<CreamCattails>(), mute: true);
						if (Main.netMode == NetmodeID.Server) {
							NetMessage.SendTileSquare(-1, i, j);
						}
					}
				}
			}
		}
	}
}
