using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace test
{
    internal class Moss : ModSystem
    {
		Texture2D tex;

        public override void Load()
        {
			tex = ModContent.Request<Texture2D>("test/ConfectionMoss", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		}
        private void GetScreenDrawArea(Vector2 screenPosition, Vector2 offSet, out int firstTileX, out int lastTileX, out int firstTileY, out int lastTileY)
		{
			firstTileX = (int)((screenPosition.X - offSet.X) / 16f - 1f);
			lastTileX = (int)((screenPosition.X + (float)Main.screenWidth + offSet.X) / 16f) + 2;
			firstTileY = (int)((screenPosition.Y - offSet.Y) / 16f - 1f);
			lastTileY = (int)((screenPosition.Y + (float)Main.screenHeight + offSet.Y) / 16f) + 5;
			if (firstTileX < 4)
			{
				firstTileX = 4;
			}
			if (lastTileX > Main.maxTilesX - 4)
			{
				lastTileX = Main.maxTilesX - 4;
			}
			if (firstTileY < 4)
			{
				firstTileY = 4;
			}
			if (lastTileY > Main.maxTilesY - 4)
			{
				lastTileY = Main.maxTilesY - 4;
			}
			if (Main.sectionManager.FrameSectionsLeft > 0)
			{
				TimeLogger.DetailedDrawReset();
				WorldGen.SectionTileFrameWithCheck(firstTileX, firstTileY, lastTileX, lastTileY);
				TimeLogger.DetailedDrawTime(5);
			}
		}
		public override void PostDrawTiles()
        {
            Main.spriteBatch.Begin(0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            int num3;
            int num4;
            int num5;
            int num6;
			Vector2 screenPosition = Main.Camera.UnscaledPosition;
			Vector2 screenOffset = Vector2.Zero;
			//Vector2 screenOffset = new((float)Main.offScreenRange, (float)Main.offScreenRange);
			if (Main.drawToScreen)
			{
				screenOffset = Vector2.Zero;
			}
			this.GetScreenDrawArea(screenPosition, screenOffset + (Main.Camera.UnscaledPosition - Main.Camera.ScaledPosition), out num3, out num4, out num5, out num6);
			for (int tileX = num3 - 2; tileX < num4 + 2; tileX++)
            {
                for (int tileY = num5; tileY < num6 + 4; tileY++)
				{
					if (IsMoss(tileX, tileY))
					{
						BitsByte adjacency = new();
						if (HasTile(tileX - 1, tileY - 1))
						{
							adjacency[0] = true;
						}
						if (HasTile(tileX, tileY - 1))
						{
							adjacency[1] = true;
						}
						if (HasTile(tileX + 1, tileY - 1))
						{
							adjacency[2] = true;
						}
						if (HasTile(tileX - 1, tileY))
						{
							adjacency[3] = true;
						}
						if (HasTile(tileX + 1, tileY))
						{
							adjacency[4] = true;
						}
						if (HasTile(tileX - 1, tileY + 1))
						{
							adjacency[5] = true;
						}
						if (HasTile(tileX, tileY + 1))
						{
							adjacency[6] = true;
						}
						if (HasTile(tileX + 1, tileY + 1))
						{
							adjacency[7] = true;
						}
						BitsByte mossAdjacency = new();
						if (IsMoss(tileX - 1, tileY - 1))
						{
							mossAdjacency[0] = true;
						}
						if (IsMoss(tileX, tileY - 1))
						{
							mossAdjacency[1] = true;
						}
						if (IsMoss(tileX + 1, tileY - 1))
						{
							mossAdjacency[2] = true;
						}
						if (IsMoss(tileX - 1, tileY))
						{
							mossAdjacency[3] = true;
						}
						if (IsMoss(tileX + 1, tileY))
						{
							mossAdjacency[4] = true;
						}
						if (IsMoss(tileX - 1, tileY + 1))
						{
							mossAdjacency[5] = true;
						}
						if (IsMoss(tileX, tileY + 1))
						{
							mossAdjacency[6] = true;
						}
						if (IsMoss(tileX + 1, tileY + 1))
						{
							mossAdjacency[7] = true;
						}
						Vector2 normalTilePosition = new Vector2((float)(tileX * 16 - (int)screenPosition.X) - (16 - 16f) / 2f, (float)(tileY * 16 - (int)screenPosition.Y + 0)) + screenOffset;

						Main.spriteBatch.Draw(tex, normalTilePosition + new Vector2(8f, 8f), GetMossTile_FromAdjacency(adjacency, mossAdjacency), Color.White, 0, new Vector2(16, 16), 1, SpriteEffects.None, 0);
					}
				}
            }
            Main.spriteBatch.End();
        }

		Rectangle GetMossTile_FromAdjacency(BitsByte adj, BitsByte mossAdjacency)
		{
			const byte TopLeft = 0;
			const byte Top = 1;
			const byte TopRight = 2;
			const byte Left = 3;
			const byte Right = 4;
			const byte BottomLeft = 5;
			const byte Bottom = 6;
			const byte BottomRight = 7;

			const byte TopLeftFalse = 8;
			const byte TopFalse = 9;
			const byte TopRightFalse = 10;
			const byte LeftFalse = 11;
			const byte RightFalse = 12;
			const byte BottomLeftFalse = 13;
			const byte BottomFalse = 14;
			const byte BottomRightFalse = 15;

			//outer
			if (AdjFilled(adj, Top, Bottom, TopRight, Right, BottomRight, LeftFalse))
			{
				return GetMossTile(0, 0);
			}
			if (AdjFilled(adj, Left, Right, BottomLeft, Bottom, BottomRight, TopFalse))
			{
				return GetMossTile(1, 0);
			}
			if (AdjFilled(adj, TopLeft, Left, BottomLeft, Top, Bottom, RightFalse))
			{
				return GetMossTile(2, 0);
			}
			if (AdjFilled(adj, Left, Right, TopLeft, Top, TopRight, BottomFalse))
			{
				return GetMossTile(3, 0);
			}
			if (AdjFilled(adj, Bottom, Right, BottomRight, TopLeftFalse, TopFalse, LeftFalse))
			{
				return GetMossTile(4, 0);
			}
			if (AdjFilled(adj, Bottom, Left, BottomLeft, TopRightFalse, TopFalse, RightFalse))
			{
				return GetMossTile(5, 0);
			}
			if (AdjFilled(adj, Top, Right, TopRight, BottomLeftFalse, BottomFalse, LeftFalse))
			{
				return GetMossTile(6, 0);
			}
			if (AdjFilled(adj, Top, Left, TopLeft, BottomRightFalse, BottomFalse, RightFalse))
			{
				return GetMossTile(7, 0);
			}

			//inner corners
			if (AdjFilled(adj, TopLeftFalse, Top, TopRight, Left, Right, BottomLeft, Bottom, BottomRight))
			{
				return GetMossTile(8, 0);
			}
			if (AdjFilled(adj, TopLeft, Top, TopRightFalse, Left, Right, BottomLeft, Bottom, BottomRight))
			{
				return GetMossTile(9, 0);
			}
			if (AdjFilled(adj, TopLeft, Top, TopRight, Left, Right, BottomLeftFalse, Bottom, BottomRight))
			{
				return GetMossTile(10, 0);
			}
			if (AdjFilled(adj, TopLeft, Top, TopRight, Left, Right, BottomLeft, Bottom, BottomRightFalse))
			{
				return GetMossTile(11, 0);
			}


			if (AdjFilled(adj, TopFalse, Left, BottomFalse, Right))
			{
				return GetMossTile(8, 3);
			}
			if (AdjFilled(adj, Top, LeftFalse, Bottom, RightFalse))
			{
				return GetMossTile(9, 3);
			}


			if (AdjFilled(adj, Left, Right, BottomLeft, Bottom, BottomRight, Top))
			{
				return GetMossTile(12, 0);
			}
			if (AdjFilled(adj, Left, Right, TopLeft, Top, TopRight, Bottom))
			{
				return GetMossTile(13, 0);
			}
			if (AdjFilled(adj, Top, Bottom, TopRight, Right, BottomRight, Left))
			{
				return GetMossTile(14, 0);
			}
			if (AdjFilled(adj, TopLeft, Left, BottomLeft, Top, Bottom, Right))
			{
				return GetMossTile(15, 0);
			}

			return GetMossTile(0, 0);
		}

		bool AdjFilled(BitsByte adj, params byte[] adjRule)
		{
			for (int x = 0; x < adjRule.Length; x++)
			{
				if (adj[adjRule[x] % 8] == (adjRule[x] / 8 == 1))
					return false;
			}
			return true;
		}
		bool IsMoss(int i, int j)
		{

			Tile tile = Main.tile[i, j];
			return tile.HasTile && tile.TileType == TileID.ArgonMoss;
		}
		bool HasTile(int i, int j)
		{

			Tile tile = Main.tile[i, j];
			return tile.HasTile && Main.tileSolid[tile.TileType];
		}
		Rectangle GetMossTile(int x, int y)
		{
			return new Rectangle(x * 36 + 2, y * 36 + 2, 32, 32);
		}
	}
}
