using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.Graphics;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class SherbetWall : ModWall
    {
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = ModContent.DustType<SherbetDust>();
			AddMapEntry(new Color(98, 39, 32));
		}

		public override bool PreDraw(int x, int y, SpriteBatch spriteBatch)
		{
			Tilemap _tileArray = (Tilemap)typeof(WallDrawing).GetField("_tileArray", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Main.instance.WallsRenderer);
			Tile tile = Framing.GetTileSafely(x, y);
			int wallType = tile.WallType;
			int[] wallBlend = Main.wallBlend;
			VertexColors vertices;
			int num21 = (int)(120f * (1f - Main.gfxQuality) + 40f * Main.gfxQuality);
			int num13 = (int)((float)num21 * 0.4f);
			int num14 = (int)((float)num21 * 0.35f);
			int num15 = (int)((float)num21 * 0.3f);
			Vector2 vector = new((float)Main.offScreenRange, (float)Main.offScreenRange);
			if (Main.drawToScreen)
			{
				vector = Vector2.Zero;
			}
			Rectangle frame = new Rectangle(0, 0, 32, 32);
			Color color = Lighting.GetColor(x, y);
			if (tile.IsWallFullbright)
			{
				color = Color.White;
			}
			if (color.R == 0 && color.G == 0 && color.B == 0 && y < Main.UnderworldLayer)
			{
				return false;
			}
			Main.instance.LoadWall(wallType);
			frame.X = tile.WallFrameX;
			frame.Y = tile.WallFrameY + Main.wallFrame[wallType] * 180;
			if (Lighting.NotRetro && !Main.wallLight[wallType] && !WorldGen.SolidTile(tile))
			{
				Texture2D tileDrawTexture = (Texture2D)typeof(WallDrawing).GetMethod("GetTileDrawTexture", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(Main.instance.WallsRenderer, new object[] { tile, x, y });
				vertices.TopRightColor = (vertices.TopLeftColor = (vertices.BottomRightColor = (vertices.BottomLeftColor = new Color((int)(byte)TheConfectionRebirth.SherbR, (int)(byte)TheConfectionRebirth.SherbG, (int)(byte)TheConfectionRebirth.SherbB))));
				Main.tileBatch.Draw(tileDrawTexture, new Vector2((float)(x * 16 - (int)Main.screenPosition.X - 8), (float)(y * 16 - (int)Main.screenPosition.Y - 8)) + vector, frame, vertices, Vector2.Zero, 1f, (SpriteEffects)0);
			}
			else
			{
				Color color2 = new Color(TheConfectionRebirth.SherbR, TheConfectionRebirth.SherbG, TheConfectionRebirth.SherbB);
				Texture2D tileDrawTexture2 = (Texture2D)typeof(WallDrawing).GetMethod("GetTileDrawTexture", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(Main.instance.WallsRenderer, new object[] { tile, x, y });
				spriteBatch.Draw(tileDrawTexture2, new Vector2((float)(x * 16 - (int)Main.screenPosition.X - 8), (float)(y * 16 - (int)Main.screenPosition.Y - 8)) + vector, (Rectangle?)frame, color2, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
			}
			if (color.R > num13 || color.G > num14 || color.B > num15)
			{
				bool num22 = _tileArray[x - 1, y].WallType > 0 && wallBlend[_tileArray[x - 1, y].WallType] != wallBlend[tile.WallType];
				bool flag = _tileArray[x + 1, y].WallType > 0 && wallBlend[_tileArray[x + 1, y].WallType] != wallBlend[tile.WallType];
				bool flag2 = _tileArray[x, y - 1].WallType > 0 && wallBlend[_tileArray[x, y - 1].WallType] != wallBlend[tile.WallType];
				bool flag3 = _tileArray[x, y + 1].WallType > 0 && wallBlend[_tileArray[x, y + 1].WallType] != wallBlend[tile.WallType];
				if (num22)
				{
					spriteBatch.Draw(TextureAssets.WallOutline.Value, new Vector2((float)(x * 16 - (int)Main.screenPosition.X), (float)(y * 16 - (int)Main.screenPosition.Y)) + vector, (Rectangle?)new Rectangle(0, 0, 2, 16), color, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
				}
				if (flag)
				{
					spriteBatch.Draw(TextureAssets.WallOutline.Value, new Vector2((float)(x * 16 - (int)Main.screenPosition.X + 14), (float)(y * 16 - (int)Main.screenPosition.Y)) + vector, (Rectangle?)new Rectangle(14, 0, 2, 16), color, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
				}
				if (flag2)
				{
					spriteBatch.Draw(TextureAssets.WallOutline.Value, new Vector2((float)(x * 16 - (int)Main.screenPosition.X), (float)(y * 16 - (int)Main.screenPosition.Y)) + vector, (Rectangle?)new Rectangle(0, 0, 16, 2), color, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
				}
				if (flag3)
				{
					spriteBatch.Draw(TextureAssets.WallOutline.Value, new Vector2((float)(x * 16 - (int)Main.screenPosition.X), (float)(y * 16 - (int)Main.screenPosition.Y + 14)) + vector, (Rectangle?)new Rectangle(0, 14, 16, 2), color, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
				}
			}
			return false;
		}
	}
}