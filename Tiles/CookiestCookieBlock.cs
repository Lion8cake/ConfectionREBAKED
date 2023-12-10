using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Tiles
{
	public class CookiestCookieBlock : CookieBlock
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			RegisterItemDrop(ModContent.ItemType<Items.CookiestBlock>());
		}

		public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor) {
			sightColor = new Color(210, 196, 145);
			return true;
		}
	}
}
