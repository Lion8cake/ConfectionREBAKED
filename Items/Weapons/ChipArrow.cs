using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
	public class ChipArrow : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Cunks of Chocolate bounce around after impact.");
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
			Recipe recipe = CreateRecipe(25);
			recipe.AddIngredient(40, 25);
			recipe.AddIngredient(ModContent.ItemType<CookieDough>(), 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
