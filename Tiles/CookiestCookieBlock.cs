using Terraria.ModLoader;
using TheConfectionRebirth.Pets.CookiestPet;

namespace TheConfectionRebirth.Tiles
{
	public class CookiestCookieBlock : CookieBlock
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemDrop = ModContent.ItemType<CookiestBlock>();
		}
	}
}
