using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Tiles;

namespace TheConfectionRebirth
{
	public static class ConfectionUtils
	{
		public static void Merge(int tile, int tile2)
		{
			Main.tileMerge[tile][tile2] = true;
			Main.tileMerge[tile2][tile] = true;

			if (tile2 == ModContent.TileType<CookieBlock>())
			{
				Merge(tile, ModContent.TileType<CookiestCookieBlock>());
			}
			else if (tile == ModContent.TileType<CookieBlock>())
			{
				Merge(ModContent.TileType<CookiestCookieBlock>(), tile2);
			}
		}
	}
}
