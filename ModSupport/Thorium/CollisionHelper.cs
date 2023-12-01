using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace TheConfectionRebirth.ModSupport.Thorium;

internal static class CollisionHelper {
	public static bool CanHitLine(this Entity start, Entity end) {
		return CanHitLine(Utils.ToTileCoordinates(start.Center), Utils.ToTileCoordinates(end.Center));
	}

	public static bool CanHitLine(Vector2 start, Vector2 end) {
		return CanHitLine(Utils.ToTileCoordinates(start), Utils.ToTileCoordinates(end));
	}

	public static bool CanHitLine(Point start, Point end) {
		if (!WorldGen.InWorld(start.X, start.Y, 0) || !WorldGen.InWorld(end.X, end.Y, 0) || WorldGen.SolidTile3(Framing.GetTileSafely(start))) {
			return false;
		}

		int distX = Math.Abs(end.X - start.X);
		int distY = Math.Abs(end.Y - start.Y);
		int sign_x = (end.X - start.X > 0) ? 1 : (-1);
		int sign_y = (end.Y - start.Y > 0) ? 1 : (-1);
		int ix = 0;
		int iy = 0;

		while (ix < distX || iy < distY) {
			int xyDiff = ((1 + 2 * ix) * distY).CompareTo((1 + 2 * iy) * distX);
			if (xyDiff == 0) {
				if (WorldGen.SolidTile3(Framing.GetTileSafely(start.X + sign_x, start.Y)) || WorldGen.SolidTile3(Framing.GetTileSafely(start.X, start.Y + sign_y))) {
					return false;
				}

				start.X += sign_x;
				start.Y += sign_y;
				ix++;
				iy++;
			}
			else if (xyDiff < 0) {
				start.X += sign_x;
				ix++;
			}
			else {
				start.Y += sign_y;
				iy++;
			}

			if (WorldGen.SolidTile3(Framing.GetTileSafely(start))) {
				return false;
			}
		}

		return true;
	}
}
