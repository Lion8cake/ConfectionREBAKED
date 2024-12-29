using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Terraria.UI;

namespace TheConfectionRebirth.Biomes
{
	public class BestiaryBackground : IBestiaryInfoElement, IBestiaryBackgroundImagePathAndColorProvider
	{
		private Asset<Texture2D> BestiaryMapAsset;

		private Color? BestiaryMapColor;

		public float OrderPriority { get; set; }

		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}

		public Color? GetBackgroundColor()
		{
			return BestiaryMapColor;
		}

		public Asset<Texture2D> GetBackgroundImage()
		{
			return BestiaryMapAsset;
		}

		public BestiaryBackground(Asset<Texture2D> mapTexture = null, Color? mapColor = null)
		{
			BestiaryMapAsset = mapTexture;
			BestiaryMapColor = mapColor;
		}
	}
}
