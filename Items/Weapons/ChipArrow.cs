using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
	public class ChipArrow : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Cunks of Chocolate bounce around after impact.");
			SacrificeTotal = 1;
		}

		public override void SetDefaults() {
			Item.damage = 10;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true; 
			Item.knockBack = 1.5f;
			Item.value = 80;
			Item.rare = ItemRarityID.Orange;
			Item.shoot = ModContent.ProjectileType<Projectiles.ChipArrow>();
			Item.shootSpeed = 7f;
			Item.ammo = AmmoID.Arrow;
		}

		public override void AddRecipes() {
			CreateRecipe(25)
				.AddIngredient(ItemID.WoodenArrow, 25)
				.AddIngredient(ModContent.ItemType<CookieDough>(), 2)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
