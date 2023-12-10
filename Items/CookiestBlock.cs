using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

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
			Item.DefaultToVanitypet(ModContent.ProjectileType<Projectiles.CookiestBlockPro>(), ModContent.BuffType<Buffs.CookiestBlockBuff>());
			Item.width = 16;
			Item.height = 16;
			Item.SetShopValues(ItemRarityColor.Orange3, Item.buyPrice(0, 0, 25, 0));
		}
	}
}
