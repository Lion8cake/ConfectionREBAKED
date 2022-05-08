using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TheConfectionRebirth.Items
{
	public class CreamSolution : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Cream Solution");
			Tooltip.SetDefault("Used by the Clentaminator"
				+ "\nSpreads the Confection");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
		}

		public override void SetDefaults() {
			Item.shoot = ModContent.ProjectileType<Projectiles.CreamSolution>() - ProjectileID.PureSpray;
			Item.ammo = AmmoID.Solution;
			Item.width = 10;
			Item.height = 12;
			Item.value = Item.buyPrice(0, 0, 25, 0);
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 999;
			Item.consumable = true;
		}
	}
}
