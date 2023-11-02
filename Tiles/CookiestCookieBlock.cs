using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using TheConfectionRebirth.Pets.CookiestPet;


namespace TheConfectionRebirth.Tiles
{
	public class CookiestCookieBlock : CookieBlock
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			RegisterItemDrop(ModContent.ItemType<CookiestBlock>());
		}

		public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor) {
			sightColor = new Color(210, 196, 145);
			return true;
		}
	}
}
