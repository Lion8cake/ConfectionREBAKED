using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
	public class SacchariteLash : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() 
		{
			Item.DefaultToWhip(ModContent.ProjectileType<Projectiles.SacchariteLash>(), 58, 2, 4);

			Item.shootSpeed = 4;
			Item.SetShopValues(ItemRarityColor.Pink5, Item.sellPrice(0, 4, 60));
		}

		public override bool MeleePrefix() 
		{
			return true;
		}
	}
}
