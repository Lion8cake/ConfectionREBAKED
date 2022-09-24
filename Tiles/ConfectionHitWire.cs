using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace TheConfectionRebirth.Tiles
{
    public class ConfectionHitWire
    {
        public static void HitWire(int type, int i, int j, int tileX, int tileY)
        {
	        int x = i - Main.tile[i, j].TileFrameX / 18 % tileX;
	        int y = j - Main.tile[i, j].TileFrameY / 18 % tileY;
        	for (int m = x; m < x + tileX; m++)
        	{
        		for (int n = y; n < y + tileY; n++)
        		{
        			/*if (Main.tile[m, n] == null)
        			{
        				Main.tile[m, n] = new Tile();
        			}*/
        			if (Main.tile[m, n].HasTile && Main.tile[m, n].TileType == type)
        			{
        				if (Main.tile[m, n].TileFrameX < 18 * tileX)
        				{
        					Main.tile[m, n].TileFrameX += (short)(18 * tileX);
        				}
        				else
        				{
        					Main.tile[m, n].TileFrameX -= (short)(18 * tileX);
        				}
        			}
        		}
        	}
        	if (!Wiring.running)
        	{
        		return;
        	}
        	for (int k = 0; k < tileX; k++)
        	{
        		for (int l = 0; l < tileY; l++)
        		{
        			Wiring.SkipWire(x + k, y + l);
        		}
        	}
        }
    }
}