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

namespace TheConfectionRebirth.TilePostDraws
{
	internal class Moss : ModSystem
	{
		Texture2D tex;
		Dictionary<byte, int> MossAdjacencyRules;
		public override void Load()
		{
			tex = ModContent.Request<Texture2D>("TheConfectionRebirth/TilePostDraws/ConfectionMoss", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			MossAdjacencyRules = new();
			MassAdjBake();
		}
        public override void Unload()
        {
			tex = null;
			MossAdjacencyRules = null;

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
						if (Conencted(tileX, tileY, -1, -1))
						{
							adjacency[0] = true;
						}
						if (Conencted(tileX, tileY, 0, -1))
						{
							adjacency[1] = true;
						}
						if (Conencted(tileX, tileY, 1, -1))
						{
							adjacency[2] = true;
						}
						if (Conencted(tileX, tileY, -1, 0))
						{
							adjacency[3] = true;
						}
						if (Conencted(tileX, tileY, 1, 0))
						{
							adjacency[4] = true;
						}
						if (Conencted(tileX, tileY, -1, 1))
						{
							adjacency[5] = true;
						}
						if (Conencted(tileX, tileY, 0, 1))
						{
							adjacency[6] = true;
						}
						if (Conencted(tileX, tileY, 1, 1))
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

						Main.spriteBatch.Draw(tex, normalTilePosition + new Vector2(8f, 8f), GetMossTile_FromAdjacency(adjacency, mossAdjacency, Main.tile[tileX, tileY]), Color.White, 0, new Vector2(16, 16), 1, SpriteEffects.None, 0);
					}
				}
			}
			Main.spriteBatch.End();
		}

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
		Rectangle GetMossTile_FromAdjacency(BitsByte adj, BitsByte mossAdjacency, Tile tile)
		{
			bool succeed = MossAdjacencyRules.TryGetValue(adj, out int Base);

			return GetMossTile(Base, tile.TileFrameNumber);
		}

		void MassAdjBake()
		{

			AdjBake(0, Top, Bottom, TopRight, Right, BottomRight, LeftFalse);
			AdjBake(1, Left, Right, BottomLeft, Bottom, BottomRight, TopFalse);
			AdjBake(2, TopLeft, Left, BottomLeft, Top, Bottom, RightFalse);
			AdjBake(3, Left, Right, TopLeft, Top, TopRight, BottomFalse);
			AdjBake(4, Bottom, Right, BottomRight, TopFalse, LeftFalse);
			AdjBake(5, Bottom, Left, BottomLeft, TopFalse, RightFalse);
			AdjBake(6, Top, Right, TopRight, BottomFalse, LeftFalse);
			AdjBake(7, Top, Left, TopLeft, BottomFalse, RightFalse);

			//inner corners
			AdjBake(8, TopLeftFalse, Top, TopRight, Left, Right, BottomLeft, Bottom, BottomRight);
			AdjBake(9, TopLeft, Top, TopRightFalse, Left, Right, BottomLeft, Bottom, BottomRight);
			AdjBake(10, TopLeft, Top, TopRight, Left, Right, BottomLeftFalse, Bottom, BottomRight);
			AdjBake(11, TopLeft, Top, TopRight, Left, Right, BottomLeft, Bottom, BottomRightFalse);

			//inner "X"

			AdjBake(12, TopLeftFalse, Top, TopRight, Left, Right, BottomLeft, Bottom, BottomRightFalse);
			AdjBake(13, TopLeft, Top, TopRightFalse, Left, Right, BottomLeftFalse, Bottom, BottomRight);

			//2 wide to 1 wide (flat 2 wide)
			AdjBake(14, Left, Right, BottomLeft, Bottom, BottomRight, Top, TopLeftFalse, TopRightFalse);
			AdjBake(15, Left, Right, TopLeft, Top, TopRight, Bottom, BottomLeftFalse, BottomRightFalse);
			AdjBake(16, Top, Bottom, TopRight, Right, BottomRight, Left, TopLeftFalse, BottomLeftFalse);
			AdjBake(17, TopLeft, Left, BottomLeft, Top, Bottom, Right, TopRightFalse, BottomRightFalse);

			//2 wide to 1 wide (corner 2 wide)
			AdjBake(18, Left, Right, Bottom, BottomRight, TopFalse);
			AdjBake(19, LeftFalse, Right, Bottom, BottomRight, Top);
			AdjBake(20, Left, Right, Bottom, BottomRight, Top, TopLeftFalse);


			AdjBake(21, Left, Right, Bottom, BottomLeft, TopFalse);
			AdjBake(22, Left, RightFalse, Bottom, BottomLeft, Top);
			AdjBake(23, Left, Right, Bottom, BottomLeft, Top, TopRightFalse);


			AdjBake(24, Left, Right, Top, TopRight, BottomFalse);
			AdjBake(25, LeftFalse, Right, Top, TopRight, Bottom);
			AdjBake(26, Left, Right, Top, TopRight, Bottom, BottomLeftFalse);


			AdjBake(27, Left, Right, Top, TopLeft, BottomFalse);
			AdjBake(28, Left, RightFalse, Top, TopLeft, Bottom);
			AdjBake(29, Left, Right, Top, TopLeft, Bottom, BottomRightFalse);

			//1 wide connectors
			AdjBake(30, TopFalse, Left, BottomFalse, Right);
			AdjBake(31, Top, LeftFalse, Bottom, RightFalse);


			AdjBake(32, Top, Left, BottomFalse, RightFalse);
			AdjBake(33, Top, LeftFalse, BottomFalse, Right);
			AdjBake(34, TopFalse, Left, Bottom, RightFalse);
			AdjBake(35, TopFalse, LeftFalse, Bottom, Right);

			//1 wide cross

			AdjBake(36, Top, Left, Bottom, RightFalse, TopLeftFalse, BottomLeftFalse);
			AdjBake(37, Top, LeftFalse, Bottom, Right, TopRightFalse, BottomRightFalse);
			AdjBake(38, TopFalse, Left, Bottom, Right, BottomLeftFalse, BottomRightFalse);
			AdjBake(39, Top, Left, BottomFalse, Right, TopLeftFalse, TopRightFalse);
			AdjBake(40, Top, Left, Bottom, Right, TopLeftFalse, TopRightFalse, BottomLeftFalse, BottomRightFalse);

			//Dots

			AdjBake(41, TopFalse, LeftFalse, BottomFalse, Right);
			AdjBake(42, TopFalse, Left, BottomFalse, RightFalse);
			AdjBake(43, TopFalse, LeftFalse, Bottom, RightFalse);
			AdjBake(44, Top, LeftFalse, BottomFalse, RightFalse);
			AdjBake(45, TopFalse, LeftFalse, BottomFalse, RightFalse);
			AdjBake(46, Top, Left, Bottom, Right);
		}

		void AdjBake(int value, params byte[] adjRule)
		{
			BitsByte bakedByte = new();
			List<byte> IgnoreBytes = new();

			for (int x = 0; x < adjRule.Length; x++)
			{
				int index = adjRule[x] % 8;
				if (adjRule[x] / 8 == 0)
					bakedByte[index] = true;
			}

			for (byte i = 0; i < 8; i++)
			{
				bool ignored = true;
				for (int x = 0; x < adjRule.Length; x++)
				{
					int index = adjRule[x] % 8;
					if (index == i)
						ignored = false;
				}
				if (ignored)
					IgnoreBytes.Add(i);
			}

			int maxPermutations = (int)Math.Pow(2, IgnoreBytes.Count);
			for (BitsByte permutations = 0; permutations < maxPermutations; permutations++) {
				for (int x = 0; x < IgnoreBytes.Count; x++)
				{
					bakedByte[IgnoreBytes[x]] = permutations[x];
				}
				MossAdjacencyRules.TryAdd(bakedByte, value);
			}
			
		}
		bool IsMoss(int i, int j)
		{

			Tile tile = Main.tile[i, j];
			return tile.HasTile && tile.TileType == Mod.Find<ModTile>("CreamMoss").Type;
		}
		bool Conencted(int x, int y, int dx, int dy)
		{
			Tile tile = Main.tile[x, y];
			switch (tile.Slope)
			{
				case SlopeType.SlopeDownLeft:
					if (dx != -1 && dy != 1)
						return false;
					break;
				case SlopeType.SlopeDownRight:
					if (dx != 1 && dy != 1)
						return false;
					break;
				case SlopeType.SlopeUpLeft:
					if (dx != -1 && dy != -1)
						return false;
					break;
				case SlopeType.SlopeUpRight:
					if (dx != 1 && dy != -1)
						return false;
					break;
			}
			if (tile.IsHalfBlock && dy != 1)
			{
				return false;
			}

			Tile tile2 = Main.tile[x + dx, y + dy];
			if(!tile2.HasTile || !Main.tileSolid[tile2.TileType])
				return false;

			switch (tile2.Slope)
			{
				case SlopeType.SlopeDownLeft:
					if (dx != 1 && dy != -1)
						return false;
					break;
				case SlopeType.SlopeDownRight:
					if (dx != -1 && dy != -1)
						return false;
					break;
				case SlopeType.SlopeUpLeft:
					if (dx != 1 && dy != 1)
						return false;
					break;
				case SlopeType.SlopeUpRight:
					if (dx != -1 && dy != 1)
						return false;
					break;
			}
			if (tile2.IsHalfBlock && dy != -1)
			{
				return false;
			}
			return true;

		}
		Rectangle GetMossTile(int x, int y)
		{
			return new Rectangle(x * 36 + 2, y * 36 + 2, 32, 32);
		}
	}
}
