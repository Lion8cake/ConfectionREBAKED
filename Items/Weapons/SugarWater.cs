using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Weapons
{
	public class SugarWater : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sugar Water");
			Tooltip.SetDefault("Spreads the confection to some blocks");
		}
	
		public override void SetDefaults()
		{
			Item.useStyle = 1;
			Item.shootSpeed = 14f;
			Item.rare = 3;
			Item.damage = 20;
			Item.shoot = ModContent.ProjectileType<SugarWaterBottle>();
			Item.width = 18;
			Item.height = 20;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.knockBack = 3f;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.value = 200;
		}
		
		public override void AddRecipes() 
			{
				Recipe recipe = CreateRecipe(10);
				recipe.AddIngredient(ModContent.ItemType<Items.Sprinkles>(), 3);
				recipe.AddIngredient(ModContent.ItemType<Items.Placeable.CreamBeans>(), 1);
				recipe.AddIngredient(ItemID.BottledWater, 10);
				recipe.Register();
			}
	}
}
