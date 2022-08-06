using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
	public class SacchariteArrow : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Shatters apon impact.");
		}

		public override void SetDefaults() {
			Item.damage = 6;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true; 
			Item.knockBack = 1.5f;
			Item.value = 80;
			Item.rare = ItemRarityID.Orange;
			Item.shoot = ModContent.ProjectileType<Projectiles.SacchariteArrow>();
			Item.shootSpeed = 7f;
			Item.ammo = AmmoID.Arrow;
		}

		public override void AddRecipes() {
			Recipe recipe = CreateRecipe(25);
			recipe.AddIngredient(40, 25);
			recipe.AddIngredient(ModContent.ItemType<Placeable.Saccharite>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
