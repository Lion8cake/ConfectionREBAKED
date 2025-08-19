using Terraria.Enums;
using Terraria.ModLoader;
using Terraria;
using TheConfectionRebirth.Projectiles;
using TheConfectionRebirth.Buffs;

namespace TheConfectionRebirth.Items
{
	public class CookiestBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToVanitypet(ModContent.ProjectileType<CookiestCookieBlock>(), ModContent.BuffType<CookiestBlockBuff>());
			Item.width = 16;
			Item.height = 16;
			Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(0, 0, 10));
		}
	}
}
