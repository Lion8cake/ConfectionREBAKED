using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;
using TheConfectionRebirth.Pets.BirdnanaLightPet;

namespace TheConfectionRebirth.Pets.CookiestPet
{
	public class CookiestBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			SacrificeTotal = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToVanitypet(ModContent.ProjectileType<CookiestBlockPro>(), ModContent.BuffType<CookiestBlockBuff>());
			Item.width = 16;
			Item.height = 16;
			Item.SetShopValues(ItemRarityColor.Orange3, Item.buyPrice(0, 0, 25, 0));
		}
	}
}
