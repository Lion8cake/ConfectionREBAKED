using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheConfectionRebirth.Walls;
using TheConfectionRebirth.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace TheConfectionRebirth.Projectiles
{
    public class ConvertConfection : ModSystem
    {
	
		public static void ConvertFromConfection(int x, int y, ConvertType convert, bool tileframe = true)
		{
			Tile tile = Main.tile[x, y];
			int type = tile.TileType;
			int wallType = tile.WallType;
			ModContent.GetInstance<global::TheConfectionRebirth.TheConfectionRebirth>();
			if (!WorldGen.InWorld(x, y, 1))
			{
				return;
			}
			if (Main.tile[x, y] != null)
			{
				if (wallType == ModContent.WallType<CookieWall>())
				{
					Main.tile[x, y].WallType = 2;
				}
				else if (wallType == ModContent.WallType<CreamWall>())
				{
					Main.tile[x, y].WallType = 40;
				}
				else if (wallType == ModContent.WallType<CreamGrassWall>())
				{
					switch (convert)
					{
					case ConvertType.Corrupt:
						Main.tile[x, y].WallType = 69;
						break;
					case ConvertType.Crimson:
						Main.tile[x, y].WallType = 81;
						break;
					case ConvertType.Hallow:
						Main.tile[x, y].WallType = 70;
						break;
					case ConvertType.Pure:
						Main.tile[x, y].WallType = 63;
						break;
					}
				}
				else if (wallType == ModContent.WallType<BlueIceWall>())
				{
					Main.tile[x, y].WallType = 71;
				}
				else if (wallType == ModContent.WallType<CreamstoneWall>())
				{
					switch (convert)
					{
					case ConvertType.Corrupt:
						Main.tile[x, y].WallType = 3;
						break;
					case ConvertType.Crimson:
						Main.tile[x, y].WallType = 83;
						break;
					case ConvertType.Hallow:
						Main.tile[x, y].WallType = 28;
						break;
					case ConvertType.Pure:
						Main.tile[x, y].WallType = 1;
						break;
					}
				}
			}
			if (Main.tile[x, y] != null)
			{
				if (type == ModContent.TileType<CookieBlock>())
				{
					tile.TileType = 0;
				}
				else if (type == ModContent.TileType<CreamBlock>())
				{
					tile.TileType = 147;
				}
				else if (type == ModContent.TileType<PinkFairyFloss>())
				{
					tile.TileType = 189;
				}
				else if (type == ModContent.TileType<PurpleFairyFloss>())
				{
					tile.TileType = 196;
				}
				else if (type == ModContent.TileType<BlueFairyFloss>())
				{
					tile.TileType = 460;
				}
				else if (type == ModContent.TileType<CreamGrass>())
				{
					SetTileFromConvert(x, y, convert, 23, 199, 109, 2);
				}
				else if (type == ModContent.TileType<Creamstone>())
				{
					SetTileFromConvert(x, y, convert, 25, 203, 117, 1);
				}
				else if (type == ModContent.TileType<Creamsand>())
				{
					SetTileFromConvert(x, y, convert, 112, 234, 116, 53);
				}
				else if (type == ModContent.TileType<Creamsandstone>())
				{
					SetTileFromConvert(x, y, convert, 400, 401, 403, 396);
				}
				else if (type == ModContent.TileType<HardenedCreamsand>())
				{
					SetTileFromConvert(x, y, convert, 398, 399, 402, 397);
				}
				else if (type == ModContent.TileType<BlueIce>())
				{
					SetTileFromConvert(x, y, convert, 163, 200, 164, 161);
				}
			}
			if (tileframe)
			{
				if (Main.netMode == 0)
				{
					WorldGen.SquareTileFrame(x, y);
				}
				else if (Main.netMode == 2)
				{
					NetMessage.SendTileSquare(-1, x, y, 1);
				}
			}
		}
		
		public static void SetTileFromConvert(int x, int y, ConvertType convert, ushort corrupt, ushort crimson, ushort hallow, ushort pure)
		{
			switch (convert)
			{
			case ConvertType.Corrupt:
				if (corrupt != ushort.MaxValue)
				{
					Main.tile[x, y].TileType = corrupt;
					WorldGen.SquareTileFrame(x, y);
				}
				break;
			case ConvertType.Crimson:
				if (crimson != ushort.MaxValue)
				{
					Main.tile[x, y].TileType = crimson;
					WorldGen.SquareTileFrame(x, y);
				}
				break;
			case ConvertType.Hallow:
				if (hallow != ushort.MaxValue)
				{
					Main.tile[x, y].TileType = hallow;
					WorldGen.SquareTileFrame(x, y);
				}
				break;
			case ConvertType.Pure:
				if (pure != ushort.MaxValue)
				{
					Main.tile[x, y].TileType = pure;
					WorldGen.SquareTileFrame(x, y);
				}
				break;
			}
		}
    }
}