using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Terraria.UI;

namespace TheConfectionRebirth.Biomes
{
	public class BestiaryBackgroundOverlay : IBestiaryInfoElement, IBestiaryBackgroundOverlayAndColorProvider
	{
		private Asset<Texture2D> BestiaryMapOverlayAsset;

		private Color? BestiaryMapOverlayColor;

		public float DisplayPriority { get; set; }

		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}

		public Color? GetBackgroundOverlayColor()
		{
			return BestiaryMapOverlayColor;
		}

		public Asset<Texture2D> GetBackgroundOverlayImage()
		{
			if (BestiaryMapOverlayAsset == null)
			{
				return null;
			}

			return BestiaryMapOverlayAsset;
		}

		public BestiaryBackgroundOverlay(Asset<Texture2D> mapOverlay = null, Color? mapOverlayColor = null)
		{
			BestiaryMapOverlayAsset = mapOverlay;
			BestiaryMapOverlayColor = mapOverlayColor;
		}
	}
}
