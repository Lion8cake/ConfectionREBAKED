using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ModLoader;
using Terraria.UI;

namespace TheConfectionRebirth 
{
	//Massive thanks to the owner of polarities for helping me with this utility system for wind, single handedly introduced me to the whole wind system and tile drawing functions
	public static class ConfectionReflectionUtilities {
		public static void Load() {
			_addSpecialPointSpecialPositions = typeof(TileDrawing).GetField("_specialPositions", BindingFlags.NonPublic | BindingFlags.Instance);
			_addSpecialPointSpecialsCount = typeof(TileDrawing).GetField("_specialsCount", BindingFlags.NonPublic | BindingFlags.Instance);
			_addVineRootPositions = typeof(TileDrawing).GetField("_vineRootsPositions", BindingFlags.NonPublic | BindingFlags.Instance);

			_addCards = typeof(AchievementAdvisor).GetField("_cards", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

			_addIsLoading = typeof(ModLoader).GetField("isLoading", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

			_addBGScale = typeof(Main).GetField("bgScale", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			_addScreenOff = typeof(Main).GetField("screenOff", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			_addBGParallax = typeof(Main).GetField("bgParallax", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			_addBGTopY = typeof(Main).GetField("bgTopY", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			_addSCAdj = typeof(Main).GetField("scAdj", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			_addBGWidthScaled = typeof(Main).GetField("bgWidthScaled", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			_addBGStartX = typeof(Main).GetField("bgStartX", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			_addBGLoops = typeof(Main).GetField("bgLoops", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			_addColorOfSurfaceBackgroundsModified = typeof(Main).GetField("ColorOfSurfaceBackgroundsModified", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		public static void Unload() {
			_addSpecialPointSpecialPositions = null;
			_addSpecialPointSpecialsCount = null;
			_addVineRootPositions = null;
			_addCards = null;
			_addIsLoading = null;
			_addBGScale = null;
			_addScreenOff = null;
			_addBGParallax = null;
			_addBGTopY = null;
			_addSCAdj = null;
			_addBGWidthScaled = null;
			_addBGStartX = null;
			_addBGLoops = null;
			_addColorOfSurfaceBackgroundsModified = null;
		}

		public static FieldInfo _addSpecialPointSpecialPositions;
		public static FieldInfo _addSpecialPointSpecialsCount;
		public static FieldInfo _addVineRootPositions;

		public static FieldInfo _addCards;

		public static FieldInfo _addIsLoading;

		public static FieldInfo _addBGScale;
		public static FieldInfo _addScreenOff;
		public static FieldInfo _addBGParallax;
		public static FieldInfo _addBGTopY;
		public static FieldInfo _addSCAdj;
		public static FieldInfo _addBGWidthScaled;
		public static FieldInfo _addBGStartX;
		public static FieldInfo _addBGLoops;
		public static FieldInfo _addColorOfSurfaceBackgroundsModified;

		public static void AddSpecialPoint(this Terraria.GameContent.Drawing.TileDrawing tileDrawing, int x, int y, int type) {
			if (_addSpecialPointSpecialPositions.GetValue(tileDrawing) is Point[][] _specialPositions) {
				if (_addSpecialPointSpecialsCount.GetValue(tileDrawing) is int[] _specialsCount) {
					_specialPositions[type][_specialsCount[type]++] = new Point(x, y);
				}
			}
		}

		public static void CrawlToTopOfVineAndAddSpecialPoint(this Terraria.GameContent.Drawing.TileDrawing tileDrawing, int j, int i) {
			if (_addVineRootPositions.GetValue(tileDrawing) is List<Point> _vineRootsPositions) {
				int y = j;
				for (int num = j - 1; num > 0; num--) {
					Tile tile = Main.tile[i, num];
					if (WorldGen.SolidTile(i, num) || !tile.HasTile) {
						y = num + 1;
						break;
					}
				}
				Point item = new(i, y);
				if (!_vineRootsPositions.Contains(item)) {
					_vineRootsPositions.Add(item);
					Main.instance.TilesRenderer.AddSpecialPoint(i, y, 6);
				}
			}
		}

		public static bool GetIsLoading()
		{
			if (_addIsLoading != null && _addIsLoading.GetValue(null) is bool isLoading)
			{
				return isLoading;
			}
			return true;
		}

		public static List<AchievementAdvisorCard> GetCards(this AchievementAdvisor self)
		{
			if (_addCards.GetValue(self) is List<AchievementAdvisorCard> _cards)
			{
				return _cards;
			}
			return null;
		}

		public static void SetCards(this AchievementAdvisor self, List<AchievementAdvisorCard> _cards)
		{
			if (_addCards.GetValue(self) is List<AchievementAdvisorCard> _)
			{
				_addCards.SetValue(self, _cards);
			}
		}

		public static float GetBGScale(this Main self)
		{
			if (_addBGScale.GetValue(null) is float bgScale)
			{
				return bgScale;
			}
			return 1f;
		}

		public static float GetScreenOff(this Main self)
		{
			if (_addScreenOff.GetValue(self) is float screenOff)
			{
				return screenOff;
			}
			return 1f;
		}

		public static double GetBGParallax(this Main self)
		{
			if (_addBGParallax.GetValue(self) is double bgParallax)
			{
				return bgParallax;
			}
			return 1.0;
		}

		public static int GetBGTopY(this Main self)
		{
			if (_addBGTopY.GetValue(self) is int bgTopY)
			{
				return bgTopY;
			}
			return 1;
		}

		public static float GetSCAdj(this Main self)
		{
			if (_addSCAdj.GetValue(self) is float scAdj)
			{
				return scAdj;
			}
			return 1f;
		}

		public static int GetBGWidthScaled(this Main self)
		{
			if (_addBGWidthScaled.GetValue(null) is int bgWidthScaled)
			{
				return bgWidthScaled;
			}
			return 1;
		}

		public static int GetBGStartX(this Main self)
		{
			if (_addBGStartX.GetValue(self) is int bgStartX)
			{
				return bgStartX;
			}
			return 1;
		}

		public static int GetBGLoops(this Main self)
		{
			if (_addBGLoops.GetValue(self) is int bgLoops)
			{
				return bgLoops;
			}
			return 1;
		}

		public static Color GetColorOFSurfaceBackgroundsModified(this Main self)
		{
			if (_addColorOfSurfaceBackgroundsModified.GetValue(null) is Color ColorOfSurfaceBackgroundsModified)
			{
				return ColorOfSurfaceBackgroundsModified;
			}
			return Color.White;
		}
	}
}
